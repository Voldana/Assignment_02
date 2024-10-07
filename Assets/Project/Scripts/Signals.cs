using Project.Scripts.Game;
using UnityEngine;

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