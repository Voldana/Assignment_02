using Project.Scripts.Game;

namespace Project.Scripts
{
    public class Signals
    {
        public struct OnMatch
        {
            public Type type;
        }

        public struct OnMove
        {
        }

        public struct OnObjectiveComplete
        {
            public Type type;
        }
    }
}