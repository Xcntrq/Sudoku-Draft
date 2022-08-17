using System.Collections.Generic;
using UnityEngine;

public class OutputSudoku : MonoBehaviour
{
    [SerializeField] private SudokuSolver _sudokuSolver;
    [SerializeField] private GameObject _pfSector;
    [SerializeField] private GameObject _pfCell;

    public List<OutputSudokuCell> _outputCells;

    private void Awake()
    {
        _outputCells = new List<OutputSudokuCell>();
        for (int b = 0; b <= 2; b++)
        {
            List<GameObject> sectors = new List<GameObject>();

            for (int a = 0; a <= 2; a++)
                sectors.Add(Instantiate(_pfSector, transform, false));

            for (int c = 0; c <= 2; c++)
                for (int d = 0; d <= 2; d++)
                    for (int e = 0; e <= 2; e++)
                        _outputCells.Add(Instantiate(_pfCell, sectors[d].transform, false).GetComponentInChildren<OutputSudokuCell>());
        }

        _sudokuSolver.OnChange += Sudoku_OnChange;
    }

    private void Sudoku_OnChange(OnChangeEventArgs onChangeEventArgs)
    {
        _outputCells[onChangeEventArgs.Index].Refresh(onChangeEventArgs.CellContent);
        _outputCells[onChangeEventArgs.Index].SetColor(onChangeEventArgs.CellColor);
        _outputCells[onChangeEventArgs.Index].SetBorderColor(onChangeEventArgs.BorderColor);
    }
}
