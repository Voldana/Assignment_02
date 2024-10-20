using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.Game
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private Image icon;

        [Inject] private Transform icons;
        // [Inject] private Gem gem;
        
        private int x, y;

        private void Start()
        {
            SetGem();
        }

        private void SetGem()
        {
            // icon.sprite = gem.sprite;
        }

        public void OnClick()
        {
            
        }

        public void SetCoordinates(int row, int column)
        {
            y = column;
            x = row;
        }

        public class Factory : PlaceholderFactory<Transform,GemType, Cell>
        {
        }
    }
}