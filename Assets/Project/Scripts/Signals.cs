using UnityEngine;

namespace Project.Scripts
{
    public class Signals
    {
        public struct OnMatch
        {
            public Color color;
        }

        public struct OnMove
        {
        }

        public struct OnObjectiveComplete
        {
            public Color color;
        }
    }
}