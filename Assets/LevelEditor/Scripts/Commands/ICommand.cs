namespace LevelEditor.Commands
{
    public interface ICommand
    {
        public bool Execute();
        public void Undo();
    }
}
