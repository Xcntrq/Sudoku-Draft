using UnityEngine;

public interface IObserveCommand
{
    public Color BorderColor { get; }
    public float Delay { get; }
    public int Affected { get; }
}
