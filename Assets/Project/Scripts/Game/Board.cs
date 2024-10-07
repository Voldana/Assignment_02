using UnityEngine;
using Zenject;

namespace Project.Scripts.Game
{
    public class Board : MonoBehaviour
    {
        [Inject] private LevelSetting levelSetting;
        [Inject] private Cell.Factory factory;
        
        
    }
}
