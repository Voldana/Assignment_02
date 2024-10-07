using System;
using System.Collections.Generic;
using Project.Scripts.Game;
using UnityEngine;
using Zenject;

namespace Project.Scripts.UI.HUD
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private Transform objectives;
        
        [Inject] private Objective.Factory factory;
        [Inject] private LevelSetting levelSetting;

        private void Start()
        {
            CreateObjectives();
        }

        private void CreateObjectives()
        {
            foreach (var color in levelSetting.colors)
                factory.Create(color).transform.SetParent(objectives);
        }

        private void SetTimer()
        {
            
        }
    }
}
