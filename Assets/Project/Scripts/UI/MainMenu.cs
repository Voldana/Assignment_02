using UnityEngine;
using UnityEngine.SceneManagement;

namespace Project.Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
        public void OnLevelClick(string levelNumber)
        {
            SceneManager.LoadScene(levelNumber);
        }

        public void OnExitClick()
        {
            Application.Quit();
        }
    }
}
