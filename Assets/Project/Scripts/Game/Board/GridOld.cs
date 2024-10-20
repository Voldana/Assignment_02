using UnityEngine;
using Zenject;

namespace Project.Scripts.Game.Board
{
    public class GridOld : MonoBehaviour
    {
        [SerializeField] private Transform icons;
        [Inject] private LevelSetting levelSetting;
        [Inject] private Cell.Factory factory;

        private Cell[,] cells;

        private void Start()
        {
            cells = new Cell[levelSetting.height,levelSetting.width];
            CreateBoard();
        }

        private void CreateBoard()
        {
            for (var i = 0; i < levelSetting.height; i++)
            {
                for (var j = 0; j < levelSetting.width; j++)
                {
                    // cells[i,j] = factory.Create(icons,AssignRandomGem());
                    cells[i,j].transform.SetParent(transform);
                    cells[i,j].SetCoordinates(i,j);
                }
            }

            return;

            Gem AssignRandomGem()
            {
                var number = Random.Range(0, 5);
                // return levelSetting.gems[number];
                return null;
            }
        }
    }
}