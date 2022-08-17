public interface ICommand
{
    public void Do();

    public void Undo();

    public string CommandToString();
}
