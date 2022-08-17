using System.Collections.Generic;
using UnityEngine;

public class PropagateCell : ICommand, IPropagateCommand
{
    private readonly Cell[] _sudoku;
    private readonly int _currNum;
    private readonly int _i;

    public PropagateCell(Cell[] sudoku, int i)
    {
        _i = i;
        _currNum = 0;
        _sudoku = sudoku;
        Affected = new HashSet<int>();
        foreach (var number in _sudoku[_i])
            _currNum = number;
    }

    public Color BorderColor => Color.red;
    public Color InvolvedColor => Color.cyan;
    public Color AffectedColor => Color.magenta;
    public float Delay => .2f;
    public int Index => _i;

    public HashSet<int> Affected { get; private set; }
    public HashSet<int> Involved { get; private set; }

    public void Do()
    {
        Involved = Sudoku.GetInvolved(_i);
        foreach (int i in Involved)
        {
            if (_sudoku[i].Contains(_currNum))
            {
                Affected.Add(i);
                _sudoku[i].Remove(_currNum);
            }
        }
    }

    public void Undo()
    {
        foreach (int i in Affected)
            _sudoku[i].Add(_currNum);
    }

    public string CommandToString()
    {
        return string.Concat("Propagate ", _i);
    }
}
