using System.Collections.Generic;
using UnityEngine;

public interface IChoiceCommand
{
    public Color CellColor { get; }
    public float Delay { get; }

    public HashSet<int> Involved { get; }
}
