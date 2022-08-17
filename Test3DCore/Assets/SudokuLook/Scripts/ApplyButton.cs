using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ApplyButton : MonoBehaviour
{
    [SerializeField] private InputSudoku _inputSudoku;
    [SerializeField] private SudokuSolver _sudoku;
    [SerializeField] private int _index;

    private List<List<(int, int)>> _preparedSudokus;

    private void Awake()
    {
        BuildPreparedSudokus();
        GetComponentInChildren<Button>().onClick.AddListener(
            () =>
            {
                if (_index == -10)
                {
                    for (int i = 0; i < _inputSudoku._inputFields.Count; i++)
                    {
                        _inputSudoku._inputFields[i].SetTextWithoutNotify(string.Empty);
                    }
                    _sudoku.Flush();
                    return;
                }

                if ((_index >= 0) && (_index <= _preparedSudokus.Count))
                {
                    for (int i = 0; i < _inputSudoku._inputFields.Count; i++)
                    {
                        _inputSudoku._inputFields[i].SetTextWithoutNotify(string.Empty);
                    }

                    for (int i = 0; i < _preparedSudokus[_index].Count; i++)
                    {
                        (int, int) tuple = _preparedSudokus[_index][i];
                        _inputSudoku._inputFields[tuple.Item1].SetTextWithoutNotify(tuple.Item2.ToString());
                    }
                }

                _sudoku.Flush();
                for (int i = 0; i < _inputSudoku._inputFields.Count; i++)
                {
                    int result = 0;
                    try
                    {
                        result = int.Parse(_inputSudoku._inputFields[i].text);
                    }
                    catch (FormatException)
                    {
                        result = -1;
                    }
                    if (result > 0) _sudoku.AddInput(i, result);
                }
                _sudoku.Solve();
            });
    }

    private void BuildPreparedSudokus()
    {
        _preparedSudokus = new List<List<(int, int)>>();

        List<(int, int)> sudoku1 = new List<(int, int)>
        {
            (4, 4),
            (5, 2),
            (6, 6),
            (8, 7),
            (10, 2),
            (14, 8),
            (25, 5),
            (29, 1),
            (32, 5),
            (34, 7),
            (35, 9),
            (36, 8),
            (44, 2),
            (45, 4),
            (47, 7),
            (57, 9),
            (62, 5),
            (63, 6),
            (65, 2),
            (66, 4),
            (68, 3),
            (76, 5),
            (78, 7)
        };

        _preparedSudokus.Add(sudoku1);

        List<(int, int)> sudoku2 = new List<(int, int)>
        {
            (1, 9),
            (11, 5),
            (3, 8),
            (5, 7),
            (22, 2),
            (6, 4),
            (16, 6),
            (36, 6),
            (46, 1),
            (31, 9),
            (39, 2),
            (41, 1),
            (49, 3),
            (33, 2),
            (43, 4),
            (54, 9),
            (64, 7),
            (66, 1),
            (68, 4),
            (77, 3),
            (62, 7),
            (69, 8),
        };

        _preparedSudokus.Add(sudoku2);
    }
}
