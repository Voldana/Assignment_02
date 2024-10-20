namespace Project.Scripts.Game.Board
{
    public class GridObject<T>
    {
        private Grid<GridObject<T>> grid;
        private int x;
        private int y;
        private T gem;

        public GridObject(Grid<GridObject<T>> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetValue(T gem)
        {
            this.gem = gem;
        }

        public T GetValue() => gem;
    }
}