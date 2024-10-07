using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Game
{
    [CreateAssetMenu(fileName = "LevelSetting", menuName = "ScriptableObjects/LevelSetting")]
    [Serializable]
    public class LevelSetting : ScriptableObject
    {
        public List<Color> colors;
        public int height;
        public int width;
        public int moves;
        public int time;
    }
}