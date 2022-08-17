using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputSudoku : MonoBehaviour
{
    [SerializeField] private GameObject _pfSector;
    [SerializeField] private GameObject _pfCell;

    public List<TMP_InputField> _inputFields;

    private void Awake()
    {
        _inputFields = new List<TMP_InputField>();
        for (int b = 0; b <= 2; b++)
        {
            List<GameObject> sectors = new List<GameObject>();

            for (int a = 0; a <= 2; a++)
            {
                sectors.Add(Instantiate(_pfSector, transform, false));
            }

            for (int c = 0; c <= 2; c++)
            {
                for (int d = 0; d <= 2; d++)
                {
                    for (int e = 0; e <= 2; e++)
                    {
                        _inputFields.Add(Instantiate(_pfCell, sectors[d].transform, false).GetComponentInChildren<TMP_InputField>());
                    }
                }
            }
        }
    }
}