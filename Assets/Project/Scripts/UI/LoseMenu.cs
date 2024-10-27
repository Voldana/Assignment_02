using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Project.Scripts.UI
{
    public class LoseMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text reasonText, scoreText;
        
        [Inject] private LossDetails lossDetails;
        
        private void Start()
        {
            DOTween.KillAll();
            reasonText.text = lossDetails.reason;
            scoreText.text += $" {lossDetails.score}";
        }

        public void OnMainMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadSceneAsync("Main Menu");
        }

        public void OnRetry()
        {
            Time.timeScale = 1;
            SceneManager.LoadSceneAsync(lossDetails.level);
        }
        

        public class Factory : PlaceholderFactory<LossDetails, LoseMenu>
        {
        }
    }
        public struct LossDetails
        {
            public string reason;
            public int score;
            public int level;
        }
}