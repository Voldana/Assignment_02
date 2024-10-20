using Project.Scripts.Game.Board;

namespace Match3 {
    public class GridObject<T> {
        Grid<GridObject<T>> grid;
        int x;
        int y;
        T gem;
        
        public GridObject(Grid<GridObject<T>> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }

        public void SetValue(T gem) {
            this.gem = gem;
        }
        
        public T GetValue() => gem;
    }
}