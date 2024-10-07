using Project.Scripts.Game;
using UnityEngine;
using Zenject;

namespace Project.Scripts.UI.HUD
{
    public class Installer : MonoInstaller
    {
        [SerializeField] private Objective objective;

        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);
            Container.BindFactory<Gem, Objective, Objective.Factory>().FromComponentInNewPrefab(objective).AsSingle();
        }
    }
}