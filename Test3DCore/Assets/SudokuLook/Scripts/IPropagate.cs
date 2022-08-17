using System.Collections.Generic;
using UnityEngine;

public interface IPropagateCommand
{
    public Color BorderColor { get; }
    public Color InvolvedColor { get; }
    public Color AffectedColor { get; }
    public float Delay { get; }
    public int Index { get; }

    public HashSet<int> Involved { get; }
    public HashSet<int> Affected { get; }
}
