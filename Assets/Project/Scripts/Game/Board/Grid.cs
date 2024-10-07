using UnityEngine;
using Zenject;

namespace Project.Scripts.Game.Board
{
    public class Grid : MonoBehaviour
    {
        [Inject] private LevelSetting levelSetting;
        [Inject] private Cell.Factory factory;

        private void Start()
        {
            CreateBoard();
        }

        private void CreateBoard()
        {
            for (var i = 0; i < levelSetting.height * levelSetting.width; i++)
                factory.Create().transform.SetParent(transform);
        }
    }
}
