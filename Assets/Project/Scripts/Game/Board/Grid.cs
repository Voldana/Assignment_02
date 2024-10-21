using UnityEngine;
using Zenject;

namespace Project.Scripts.Game.Board
{
    public class Grid<T>
    {
        private readonly LevelSetting levelSetting;
        private readonly float cellSize;
        private readonly Vector3 origin;
        private readonly T[,] gridArray;

        private readonly CoordinateConverter coordinateConverter;

        public static Grid<T> VerticalGrid(LevelSetting setting, float cellSize, Vector3 origin)
        {
            return new Grid<T>(setting, cellSize, origin, new VerticalConverter());
        }

        [Inject]
        public Grid(LevelSetting levelSetting, float cellSize, Vector3 origin,
            CoordinateConverter coordinateConverter)
        {
            this.levelSetting = levelSetting;
            this.cellSize = cellSize;
            this.origin = origin;
            this.coordinateConverter = coordinateConverter ?? new VerticalConverter();

            gridArray = new T[levelSetting.width, levelSetting.height];
        }

        public void SetValue(int x, int y, T value)
        {
            if (!IsValid(x, y)) return;
            gridArray[x, y] = value;
        }

        public T GetValue(int x, int y)
        {
            return IsValid(x, y) ? gridArray[x, y] : default;
        }

        bool IsValid(int x, int y) => x >= 0 && y >= 0 && x < levelSetting.width && y < levelSetting.height;

        public Vector2Int GetXY(Vector3 worldPosition) =>
            coordinateConverter.WorldToGrid(worldPosition, cellSize, origin);

        public Vector3 GetWorldPositionCenter(int x, int y) =>
            coordinateConverter.GridToWorldCenter(x, y, cellSize, origin);

        public abstract class CoordinateConverter
        {

            public abstract Vector3 GridToWorldCenter(int x, int y, float cellSize, Vector3 origin);

            public abstract Vector2Int WorldToGrid(Vector3 worldPosition, float cellSize, Vector3 origin);
        }

        private class VerticalConverter : CoordinateConverter
        {
            public override Vector3 GridToWorldCenter(int x, int y, float cellSize, Vector3 origin)
            {
                return new Vector3(x * cellSize + cellSize * 0.5f, y * cellSize + cellSize * 0.5f, 0) + origin;
            }

            public override Vector2Int WorldToGrid(Vector3 worldPosition, float cellSize, Vector3 origin)
            {
                Vector3 gridPosition = (worldPosition - origin) / cellSize;
                var x = Mathf.FloorToInt(gridPosition.x);
                var y = Mathf.FloorToInt(gridPosition.y);
                return new Vector2Int(x, y);
            }
        }
    }
}