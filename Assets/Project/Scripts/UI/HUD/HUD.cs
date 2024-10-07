using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Scripts.UI.HUD
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private Transform objectives;
        
        [Inject] private Objective.Factory factory;
        [Inject] private List<Color> colors;

        private void Start()
        {
            CreateObjectives();
        }

        private void CreateObjectives()
        {
            foreach (var color in colors)
                factory.Create(color).transform.SetParent(objectives);
        }
    }
}
