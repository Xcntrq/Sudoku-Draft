using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OutputSudokuCell : MonoBehaviour
{
    [SerializeField] private AnimationCurve _animationCurve;

    private TextMeshProUGUI[] _texts;
    private GridLayoutGroup _gridLayoutGroup;
    private Image _border;
    private Image _cell;

    private Color _borderColor;
    private Color _cellColor;

    private void Awake()
    {
        _texts = GetComponentsInChildren<TextMeshProUGUI>();
        _border = GetComponentInChildren<Border>().GetComponent<Image>();
        _cell = GetComponent<Image>();
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        GetComponentInChildren<Border>().GetComponent<Image>().color = new Color(0, 0, 0, 0);

        for (int i = 0; i < _texts.Length; i++)
            _texts[i].SetText((i + 1).ToString());

        _borderColor = _border.color;
        _cellColor = _cell.color;
    }

    public void Refresh(HashSet<int> cellContent)
    {
        switch (cellContent.Count)
        {
            case 1:
                _gridLayoutGroup.cellSize = new Vector2(30f, 30f);
                break;
            case 2:
            case 3:
            case 4:
                _gridLayoutGroup.constraintCount = 2;
                _gridLayoutGroup.cellSize = new Vector2(15f, 15f);
                break;
            default:
                _gridLayoutGroup.constraintCount = 3;
                _gridLayoutGroup.cellSize = new Vector2(10f, 10f);
                break;
        }

        foreach (TextMeshProUGUI text in _texts)
            text.transform.parent.gameObject.SetActive(false);

        foreach (int n in cellContent)
            _texts[n - 1].transform.parent.gameObject.SetActive(true);
    }

    public void SetBorderColor(Color? color)
    {
        if (color != null)
            _border.color = (Color)color;
        else
            _border.color = _borderColor;
    }

    public void SetColor(Color? color)
    {
        if (color != null)
            _cell.color = (Color)color;
        else
            _cell.color = _cellColor;
    }

    public void Highlight(Color color, float time)
    {
        StopAllCoroutines();
        GetComponent<Image>().color = color;
        StartCoroutine(RevertColor(1f / time));
    }

    public void Highlight(Color color)
    {
        StopAllCoroutines();
        GetComponent<Image>().color = color;
    }

    private IEnumerator RevertColor(float m)
    {
        float time = 0f;
        while (GetComponent<Image>().color != Color.white)
        {
            float additive = _animationCurve.Evaluate(time * m);
            Color color = GetComponent<Image>().color;
            color.r = Mathf.Clamp01(color.r + additive);
            color.g = Mathf.Clamp01(color.g + additive);
            color.b = Mathf.Clamp01(color.b + additive);
            GetComponent<Image>().color = color;
            yield return new WaitForFixedUpdate();
            time += Time.deltaTime;
        }
    }
}
