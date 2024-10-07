using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
        public void OnLevelClick(string levelNumber)
        {
            SceneManager.LoadSceneAsync(levelNumber);
        }

        public void OnExitClick()
        {
            Application.Quit();
        }
    }
}
