using System;
using System.Collections.Generic;
using UnityEngine;

public class OnChangeEventArgs : EventArgs
{
    public int Index { get; private set; }
    public Color? CellColor { get; private set; }
    public Color? BorderColor { get; private set; }
    public HashSet<int> CellContent { get; private set; }

    public OnChangeEventArgs(int index, Color? cellColor, Color? borderColor, HashSet<int> cellContent)
    {
        Index = index;
        CellColor = cellColor;
        BorderColor = borderColor;
        CellContent = cellContent;
    }
}
