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
                factory.Create(AssignRandomGem()).transform.SetParent(transform);
            
            Gem AssignRandomGem()
            {
                var number = Random.Range(0, 5);
                return levelSetting.gems[number];
            }
        }
    }
}
