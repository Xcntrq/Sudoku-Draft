using UnityEngine;

public class ObserveCell : ICommand, IObserveCommand
{
    private readonly Cell[] _sudoku;
    private readonly ObsPara _op;

    private Cell _cachedCell;

    public ObserveCell(Cell[] sudoku, ObsPara op)
    {
        _sudoku = sudoku;
        _op = op;
        Affected = op.Index;
    }

    public Color BorderColor => Color.red;
    public float Delay => .2f;

    public int Affected { get; private set; }

    public void Do()
    {
        _cachedCell = _sudoku[_op.Index];
        _sudoku[_op.Index] = new Cell { _op.Number };
    }

    public void Undo()
    {
        _sudoku[_op.Index] = _cachedCell;
    }

    public string CommandToString()
    {
        return string.Concat("Observe ", _op.Number, " at ", _op.Index);
    }
}
