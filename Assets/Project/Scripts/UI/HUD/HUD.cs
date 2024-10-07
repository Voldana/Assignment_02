using Project.Scripts.Game;
using TMPro;
using UnityEngine;
using Zenject;

namespace Project.Scripts.UI.HUD
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private Transform objectives;
        [SerializeField] private TMP_Text clock;
        [SerializeField] private TMP_Text moves;

        [Inject] private Objective.Factory factory;
        [Inject] private LevelSetting levelSetting;

        private Timer timer;

        private void Start()
        {
            SetRemainingMoves();
            CreateObjectives();
            SetTimer();
        }

        private void SetRemainingMoves()
        {
            moves.text = levelSetting.moves.ToString();
        }

        private void CreateObjectives()
        {
            foreach (var color in levelSetting.colors)
                factory.Create(color).transform.SetParent(objectives);
        }

        private void SetTimer()
        {
            UpdateClock(levelSetting.time * 60);
            timer = new Timer(levelSetting.time * 60, UpdateClock, OnTimerEnd);
            timer.Start();
        }

        private void UpdateClock(int newTime)
        {
            clock.text = $"{newTime / 60}:{newTime % 60}";
        }

        private void OnTimerEnd()
        {
        }
    }
}