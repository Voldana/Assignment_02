using Project.Scripts.UI.HUD;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Game
{
    public class Cell : MonoBehaviour
    {
        
        public class Factory : PlaceholderFactory<Cell>
        {
        }
    }
}