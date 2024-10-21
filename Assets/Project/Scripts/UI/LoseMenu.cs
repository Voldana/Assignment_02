using TMPro;
using UnityEngine;
using Zenject;

namespace Project.Scripts.UI
{
    public class LoseMenu : MonoBehaviour
    {
        [SerializeField] private TMP_Text reasonText;
        
        [Inject] private string reason;

        private void Start()
        {
            reasonText.text = reason;
        }

        public class Factory : PlaceholderFactory<string, LoseMenu>
        {
        }
    }
}