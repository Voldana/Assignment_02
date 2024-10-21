using TMPro;
using UnityEngine;
using Zenject;

namespace Project.Scripts.UI
{
    public class WinMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;

        [Inject] private int score;

        private void Start()
        {
            scoreText.text = $"Score: {score}";
        }

        public class Factory : PlaceholderFactory<int, WinMenu>
        {
        }
    }
}