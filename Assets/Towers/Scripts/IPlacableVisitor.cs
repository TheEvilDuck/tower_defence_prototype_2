namespace Towers
{
    public interface IPlacableVisitor
    {
        public void Visit(Placable placable);
        public void Visit(Tower tower);
        public void Visit(MainBuilding mainBuilding);
        public void Visit(MoneyGiver moneyGiver);
        public void Visit(SlowBomb slowBomb);
        public void Visit(Storage storage);
        public void Visit(Bomb bomb);
    }
}
