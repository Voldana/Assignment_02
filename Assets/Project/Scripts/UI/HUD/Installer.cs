using Project.Scripts.Game;
using UnityEngine;
using Zenject;

namespace Project.Scripts.UI.HUD
{
    public class Installer : MonoInstaller
    {
        [SerializeField] private Objective objective;
        [SerializeField] private LoseMenu loseMenu;
        [SerializeField] private WinMenu winMenu;
        

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            
            Container.BindFactory<GemType, Objective, Objective.Factory>().FromComponentInNewPrefab(objective).AsSingle();
            Container.BindFactory<string, LoseMenu, LoseMenu.Factory>().FromComponentInNewPrefab(loseMenu).AsSingle();
            Container.BindFactory<int, WinMenu, WinMenu.Factory>().FromComponentInNewPrefab(winMenu).AsSingle();
        }
    }
}