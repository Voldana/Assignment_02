namespace Project.Scripts.Game.Board
{
    public class GridObject<T>
    {
        private Gem gem;

        public void SetValue(Gem gem)
        {
            this.gem = gem;
        }

        public Gem GetValue() => gem;
    }
}