using UnityEngine;
using Zenject;

namespace Project.Scripts.Game.Board
{
    public class Installer : MonoInstaller
    {
        [SerializeField] private Cell cell;

        public override void InstallBindings()
        {
            Container.BindFactory<Gem, Cell, Cell.Factory>().FromComponentInNewPrefab(cell).AsSingle();
        }
    }
}