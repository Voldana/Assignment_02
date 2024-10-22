using Project.Scripts.Game;
using UnityEngine;
using Zenject;

namespace Project.Scripts
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private LevelSetting levelSetting;
        [SerializeField] private AudioManager audioManager;

        public override void InstallBindings()
        {
            Container.BindInstance(levelSetting).AsSingle();
            Container.BindInstance(audioManager).AsSingle();
            Container.Bind<ScoreManager>().AsSingle();
            
            Container.DeclareSignal<Signals.OnObjectiveComplete>();
            Container.DeclareSignal<Signals.AddToScore>();
            Container.DeclareSignal<Signals.OnMatch>();
            Container.DeclareSignal<Signals.OnMove>();
        }
    }
}