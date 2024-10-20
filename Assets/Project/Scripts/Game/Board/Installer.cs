using UnityEngine;
using Zenject;

namespace Project.Scripts.Game.Board
{
    public class Installer : MonoInstaller
    {
        [SerializeField] private Gem gem;

        public override void InstallBindings()
        {
            Container.BindFactory<GemType ,Gem, Gem.Factory>().FromComponentInNewPrefab(gem).AsSingle();
        }
    }
}