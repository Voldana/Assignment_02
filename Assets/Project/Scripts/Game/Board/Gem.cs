using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.Game.Board
{
    public class Gem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer image;
        [SerializeField] private GameObject selected;
        [Inject] private Vector2Int coordinate;
        [Inject] private SignalBus signalBus;
        [Inject] private GemType type;

        private void Start()
        {
            image.sprite = type.sprite;
            signalBus.Subscribe<Signals.DeselectAllGems>(() =>
            {
                SetSelected(false);
            });
        }
        

        public void SetSelected(bool status)
        {
            if(!selected) return;
            selected.SetActive(status);
        }

        public GemType GetGemType() => type;

        private void OnDestroy()
        {
            signalBus.TryUnsubscribe<Signals.DeselectAllGems>(() =>
            {
                SetSelected(false);
            });
        }

        public class Factory : PlaceholderFactory<GemType, Vector2Int, Gem>
        {
        }
    }
}