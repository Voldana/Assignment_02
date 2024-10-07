using System;
using Project.Scripts.UI.HUD;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.Game
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private Image icon;

        [Inject] private Gem gem;

        private void Start()
        {
            SetGem();
        }

        private void SetGem()
        {
            icon.sprite = gem.sprite;
        }

        public class Factory : PlaceholderFactory<Gem, Cell>
        {
        }
    }
}