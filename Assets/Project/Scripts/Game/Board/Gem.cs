using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.Game.Board
{
    public class Gem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer image;
        [Inject] private GemType type;

        private void Start()
        {
            image.sprite = type.sprite;
        }

        public GemType GetGemType() => type;
        
        public class Factory : PlaceholderFactory<GemType, Gem>
        {
        }
    }
}