using UnityEngine;
using Zenject;

namespace Project.Scripts.UI.HUD
{
    public class PauseButton : MonoBehaviour
    {
        [Inject] private PauseMenu.Factory factory;

        public void OnClick()
        {
            factory.Create().transform.SetParent(transform.parent.parent,false);
        }
    }
}
