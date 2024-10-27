using Project.Scripts.Game;
using Project.Scripts.Game.Board;
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
            var spawner = new Spawner(levelSetting);
            Container.BindInstance(levelSetting).AsSingle();
            Container.BindInstance(audioManager).AsSingle();
            Container.BindInstance(spawner).AsSingle();
            Container.Bind<ScoreManager>().AsSingle();
            
            Container.DeclareSignal<Signals.OnObjectiveComplete>();
            Container.DeclareSignal<Signals.GameLoopProgress>();
            Container.DeclareSignal<Signals.DeselectAllGems>();
            Container.DeclareSignal<Signals.AddToScore>();
            Container.DeclareSignal<Signals.OnMatch>();
            Container.DeclareSignal<Signals.OnMove>();
        }
    }
}