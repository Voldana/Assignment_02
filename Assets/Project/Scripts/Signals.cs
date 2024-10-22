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
        
        public struct AddToScore
        {
            public int score;
        }

        public struct OnObjectiveComplete
        {
            public Type type;
        }
    }
}