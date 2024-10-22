using TMPro;
using UnityEngine;
using Zenject;

namespace Project.Scripts.UI
{
    public class WinMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText, finalScore, remainingMoves, remainingTime;

        [Inject] private WinDetails winDetails;

        private void Start()
        {
            // scoreText.text = $"Score: {score}";
        }

        private void CalculateScore()
        {
            var final = winDetails.score + winDetails.remainingTime * 5 + winDetails.remainingMoves * 10;
            scoreText.text += winDetails.score;
        }

        public class Factory : PlaceholderFactory<WinDetails, WinMenu>
        {
        }
    }

    public struct WinDetails
    {
        public int remainingMoves;
        public int remainingTime;
        public int score;
    }
}