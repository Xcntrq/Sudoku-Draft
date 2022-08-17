using UniRandom = UnityEngine.Random;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class SudokuSolver : MonoBehaviour
{
    private HashSet<ObsPara> _inputs;
    private HashSet<int> _awaiting;
    private List<ICommand> _commands;
    private int _observedCount;
    private Cell[] _sudoku;

    public event Action<OnChangeEventArgs> OnChange;

    public void Flush()
    {
        _awaiting = new HashSet<int>();
        _inputs = new HashSet<ObsPara>();
        _observedCount = 0;
        _sudoku = new Cell[81];
        for (int i = 0; i < _sudoku.Length; i++)
        {
            _sudoku[i] = new Cell();
            for (int n = 1; n <= 9; n++)
                _sudoku[i].Add(n);
        }
        ShowSudoku();
    }

    public void AddInput(int i, int n)
    {
        ObsPara op = new ObsPara(i, n);
        _inputs.Add(op);
    }

    public void Solve()
    {
        _commands = ProcessSudoku(_inputs);
        StartCoroutine(ShowSolution());
    }

    public IEnumerator ShowSolution()
    {
        int i;
        for (i = 0; i < _sudoku.Length; i++)
        {
            _sudoku[i] = new Cell();
            for (int n = 1; n <= 9; n++)
                _sudoku[i].Add(n);
        }

        ShowSudoku();

        yield return new WaitForSeconds(.5f);

        OnChangeEventArgs onChangeEventArgs;
        float delay = (_commands[0] as IObserveCommand).Delay;
        Color borderColor = (_commands[0] as IObserveCommand).BorderColor;

        HashSet<int> inputs = new HashSet<int>();
        for (i = 0; i < _inputs.Count; i++)
            inputs.Add((_commands[i] as IObserveCommand).Affected);

        foreach (var input in inputs)
        {
            onChangeEventArgs = new OnChangeEventArgs(input, null, borderColor, _sudoku[input]);
            OnChange?.Invoke(onChangeEventArgs);
        }

        yield return new WaitForSeconds(delay);

        for (i = 0; i < _inputs.Count; i++)
        {
            int ind = (_commands[i] as IObserveCommand).Affected;
            _commands[i].Do();
            onChangeEventArgs = new OnChangeEventArgs(ind, null, borderColor, _sudoku[ind]);
            OnChange?.Invoke(onChangeEventArgs);
        }

        yield return new WaitForSeconds(delay);

        foreach (var input in inputs)
        {
            onChangeEventArgs = new OnChangeEventArgs(input, null, null, _sudoku[input]);
            OnChange?.Invoke(onChangeEventArgs);
        }

        for (; i < _commands.Count; i++)
        {
            //yield return new WaitForSeconds(.1f);

            ICommand command = _commands[i];
            if (command is IChoiceCommand choiceCommand)
            {
                foreach (var ind in choiceCommand.Involved)
                {
                    onChangeEventArgs = new OnChangeEventArgs(ind, choiceCommand.CellColor, null, _sudoku[ind]);
                    OnChange?.Invoke(onChangeEventArgs);
                }

                yield return new WaitForSeconds(choiceCommand.Delay);

                foreach (var ind in choiceCommand.Involved)
                {
                    onChangeEventArgs = new OnChangeEventArgs(ind, null, null, _sudoku[ind]);
                    OnChange?.Invoke(onChangeEventArgs);
                }
            }

            if (command is IObserveCommand observeCommand)
            {
                onChangeEventArgs = new OnChangeEventArgs(observeCommand.Affected, null, observeCommand.BorderColor, _sudoku[observeCommand.Affected]);
                OnChange?.Invoke(onChangeEventArgs);

                yield return new WaitForSeconds(observeCommand.Delay);

                command.Do();
                onChangeEventArgs = new OnChangeEventArgs(observeCommand.Affected, null, observeCommand.BorderColor, _sudoku[observeCommand.Affected]);
                OnChange?.Invoke(onChangeEventArgs);

                yield return new WaitForSeconds(observeCommand.Delay);

                onChangeEventArgs = new OnChangeEventArgs(observeCommand.Affected, null, null, _sudoku[observeCommand.Affected]);
                OnChange?.Invoke(onChangeEventArgs);
            }

            if (command is IPropagateCommand propagateCommand)
            {
                onChangeEventArgs = new OnChangeEventArgs(propagateCommand.Index, null, propagateCommand.BorderColor, _sudoku[propagateCommand.Index]);
                OnChange?.Invoke(onChangeEventArgs);

                yield return new WaitForSeconds(propagateCommand.Delay);

                foreach (var ind in propagateCommand.Involved)
                {
                    onChangeEventArgs = new OnChangeEventArgs(ind, propagateCommand.InvolvedColor, null, _sudoku[ind]);
                    OnChange?.Invoke(onChangeEventArgs);
                }

                yield return new WaitForSeconds(propagateCommand.Delay);

                foreach (var ind in propagateCommand.Involved)
                {
                    onChangeEventArgs = new OnChangeEventArgs(ind, null, null, _sudoku[ind]);
                    OnChange?.Invoke(onChangeEventArgs);
                }

                foreach (var ind in propagateCommand.Affected)
                {
                    onChangeEventArgs = new OnChangeEventArgs(ind, propagateCommand.AffectedColor, null, _sudoku[ind]);
                    OnChange?.Invoke(onChangeEventArgs);
                }

                yield return new WaitForSeconds(propagateCommand.Delay);

                command.Do();

                foreach (var ind in propagateCommand.Affected)
                {
                    onChangeEventArgs = new OnChangeEventArgs(ind, propagateCommand.AffectedColor, null, _sudoku[ind]);
                    OnChange?.Invoke(onChangeEventArgs);
                }

                yield return new WaitForSeconds(propagateCommand.Delay);

                foreach (var ind in propagateCommand.Affected)
                {
                    onChangeEventArgs = new OnChangeEventArgs(ind, null, null, _sudoku[ind]);
                    OnChange?.Invoke(onChangeEventArgs);
                }

                onChangeEventArgs = new OnChangeEventArgs(propagateCommand.Index, null, null, _sudoku[propagateCommand.Index]);
                OnChange?.Invoke(onChangeEventArgs);
            }
        }
    }

    private List<ICommand> ProcessSudoku(HashSet<ObsPara> ops)
    {
        List<ICommand> commands = new List<ICommand>();
        List<int> propagatables = new List<int>();
        int cachedObservedCount = _observedCount;

        foreach (var op in ops)
        {
            ObserveCell command = new ObserveCell(_sudoku, op);
            propagatables.Add(op.Index);
            commands.Add(command);
            _observedCount++;
            command.Do();
        }

        int minEn = int.MaxValue;
        HashSet<int> affected = new HashSet<int>(_awaiting);
        while ((propagatables.Count > 0) && (minEn > 0))
        {
            int r = UniRandom.Range(0, propagatables.Count);
            int i = propagatables[r];
            propagatables.RemoveAt(r);

            PropagateCell command = new PropagateCell(_sudoku, i);
            commands.Add(command);
            command.Do();

            affected.UnionWith(command.Affected);
            foreach (var ind in command.Affected)
            {
                if (_sudoku[ind].Count < minEn)
                    minEn = _sudoku[ind].Count;

                if (_sudoku[ind].Count == 1)
                {
                    propagatables.Add(ind);
                    _observedCount++;
                    if (_observedCount == _sudoku.Length)
                        return commands;
                }
            }
        }

        if (minEn == 0)
        {
            _observedCount = cachedObservedCount;
            for (int i = commands.Count - 1; i >= 0; i--)
            {
                commands[i].Undo();
                commands.RemoveAt(i);
            }
            return commands;
        }

        List<int> observables = new List<int>(affected);
        for (int i = observables.Count - 1; i >= 0; i--)
        {
            if (_sudoku[observables[i]].Count == 1)
                observables.RemoveAt(i);
        }
        _awaiting = new HashSet<int>(observables);

        if (_awaiting.Count == 0)
            return commands;

        minEn = int.MaxValue;
        foreach (var i in _awaiting)
        {
            if (_sudoku[i].Count < minEn)
                minEn = _sudoku[i].Count;
        }

        observables = new List<int>(_awaiting);
        for (int i = observables.Count - 1; i >= 0; i--)
        {
            if (_sudoku[observables[i]].Count != minEn)
                observables.RemoveAt(i);
        }

        Choice choice = new Choice(observables);
        commands.Add(choice);

        List<ICommand> newCommands = new List<ICommand>();
        int random = UniRandom.Range(0, observables.Count);
        int index = observables[random];
        foreach (var n in _sudoku[index])
        {
            HashSet<ObsPara> newOps = new HashSet<ObsPara> { new ObsPara(index, n) };
            newCommands = ProcessSudoku(newOps);
            commands.AddRange(newCommands);
            if (newCommands.Count > 0)
                return commands;
        }

        if (newCommands.Count == 0)
        {
            _observedCount = cachedObservedCount;
            for (int i = commands.Count - 1; i >= 0; i--)
            {
                commands[i].Undo();
                commands.RemoveAt(i);
            }
            return commands;
        }

        return commands;
    }

    public void ShowSudoku()
    {
        for (int i = 0; i < _sudoku.Length; i++)
        {
            OnChangeEventArgs onChangeEventArgs = new OnChangeEventArgs(i, Color.white, new Color(0, 0, 0, 0), _sudoku[i]);
            OnChange?.Invoke(onChangeEventArgs);
        }

        /*
        if (_commands != null)
            return;

        foreach (var command in _commands)
            Debug.Log(command.CommandToString());

        Debug.Log(_observedCount);
        */
    }
}
