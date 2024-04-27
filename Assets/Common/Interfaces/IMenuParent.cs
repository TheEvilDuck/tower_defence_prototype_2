namespace Common.Interfaces
{
    public interface IMenuParent
    {
        public void Show();
        public void Hide();

        public bool Active {get;}
    }
}
