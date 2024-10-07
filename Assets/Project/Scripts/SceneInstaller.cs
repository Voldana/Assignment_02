using Project.Scripts.Game;
using UnityEngine;
using Zenject;

namespace Project.Scripts
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private LevelSetting levelSetting;

        public override void InstallBindings()
        {
            Container.BindInstance(levelSetting).AsSingle();
            
            Container.DeclareSignal<Signals.OnObjectiveComplete>();
            Container.DeclareSignal<Signals.OnMatch>();
            Container.DeclareSignal<Signals.OnMove>();
        }
    }
}