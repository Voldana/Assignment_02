using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Project.Scripts.UI.HUD
{
    public class Objective : MonoBehaviour
    {
        [SerializeField] private TMP_Text number;
        [SerializeField] private Image icon;

        [Inject] private SignalBus signalBus;
        [Inject] private Color color;

        private int counter = 3;

        private void Start()
        {
            SetColor();
            signalBus.Subscribe<Signals.OnMatch>(OnMatchMade);
        }

        private void SetColor()
        {
            icon.color = color;
        }

        private void OnMatchMade(Signals.OnMatch signal)
        {
            if (!signal.color.Equals(color) || counter <= 0) return;
            counter--;
            if (counter != 0)
                number.text = $"x{counter}";
            else
                signalBus.Fire(new Signals.OnObjectiveComplete { color = color });
        }
        
        public class Factory : PlaceholderFactory<Color, Objective>
        {
        }
    }
}