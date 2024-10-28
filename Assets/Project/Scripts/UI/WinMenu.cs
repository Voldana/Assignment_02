using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Project.Scripts.UI
{
    public class WinMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText, finalScore, remainingMoves, remainingTime;
        [SerializeField] private List<Transform> stars;

        [Inject] private WinDetails winDetails;

        private void Start()
        {
            DOTween.timeScale = 1;
            Time.timeScale = 1;
            CalculateScore();
        }

        private void CalculateScore()
        {
            var final = winDetails.score + winDetails.remainingTime * 5 + winDetails.remainingMoves * 10;
            remainingMoves.text += winDetails.remainingMoves;
            remainingTime.text += winDetails.remainingTime;
            scoreText.text += winDetails.score;
            finalScore.text += final;
            switch (final)
            {
                case > 1000:
                    ShowStars(2);
                    break;
                case > 700:
                    ShowStars(1);
                    break;
                default:
                    ShowStars(0);
                    break;
            }
        }

        private void ShowStars(int number)
        {
            if(number < 0 ) return;
            stars[number].DOScale(Vector3.one, 1).OnComplete(() => ShowStars(number - 1));
        }

        public void OnContinue()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("Main Menu");
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