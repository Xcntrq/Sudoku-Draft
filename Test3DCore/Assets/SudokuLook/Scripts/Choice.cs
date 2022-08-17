using System.Collections.Generic;
using UnityEngine;

public class Choice : ICommand, IChoiceCommand
{
    public Choice(IEnumerable<int> involved)
    {
        Involved = new HashSet<int>(involved);
    }

    public Color CellColor => Color.yellow;
    public float Delay => .5f;

    public HashSet<int> Involved { get; private set; }

    public void Do()
    {

    }

    public void Undo()
    {

    }

    public string CommandToString()
    {
        return string.Empty;
    }
}
