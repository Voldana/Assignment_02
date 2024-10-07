using Zenject;

namespace Project.Scripts
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(Container);

            Container.DeclareSignal<Signals.OnObjectiveComplete>();
            Container.DeclareSignal<Signals.OnMatch>();
            Container.DeclareSignal<Signals.OnMove>();
        }
    }
}
