using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Project.Scripts.UI
{
    public class PauseMenu: MonoBehaviour
    {
        private void Start()
        {
            ChangeTimeScale(true);
        }

        private static void ChangeTimeScale(bool freeze)
        {
            Time.timeScale = freeze ? 0 : 1;
            DOTween.timeScale = freeze ? 0 : 1;
        }

        public void OnResume()
        {
            ChangeTimeScale(false);
            Destroy(gameObject);
        }

        public void OnMainMenu()
        {
            ChangeTimeScale(true);
            SceneManager.LoadScene("Main Menu");
        }
        
        public class Factory : PlaceholderFactory<PauseMenu>
        {
        }
    }
}