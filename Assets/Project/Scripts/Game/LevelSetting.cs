using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Game
{
    [CreateAssetMenu(fileName = "LevelSetting", menuName = "ScriptableObjects/LevelSetting")]
    [Serializable]
    public class LevelSetting : ScriptableObject
    {
        public List<Gem> gems;
        public int height;
        public int width;
        public int moves;
        public int time;
    }

    [Serializable]
    public class Gem
    {
        public Sprite sprite;
        public Type type;
    }

    public enum Type
    {
        Yellow,
        Purple,
        Green,
        Blue,
        Red
    }
}