using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Project.Scripts.Game;
using TMPro;
using UnityEngine;
using Zenject;

namespace Project.Scripts.UI.HUD
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private Transform objectives;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text clock;
        [SerializeField] private TMP_Text moves;

        [Inject] private LoseMenu.Factory loseFactory;
        [Inject] private WinMenu.Factory winFactory;
        [Inject] private Objective.Factory factory;
        [Inject] private LevelSetting levelSetting;
        [Inject] private ScoreManager scoreManager;
        [Inject] private AudioManager audioManager;
        [Inject] private SignalBus signalBus;

        private Dictionary<Type, bool> objs;
        private int remainingMoves, score;
        private bool inProgress, timeEnded, movesEnded;
        private Timer timer;

        private void Start()
        {
            Time.timeScale = 1;
            DOTween.timeScale = 1;
            objs = new Dictionary<Type, bool>
            {
                { Type.Green, false }, { Type.Purple, false }, { Type.Blue, false }, { Type.Red, false },
                { Type.Yellow, false }
            };
            remainingMoves = levelSetting.moves;
            SubscribeSignals();
            SetRemainingMoves();
            CreateObjectives();
            SetTimer();
        }

        private void SubscribeSignals()
        {
            signalBus.Subscribe<Signals.OnObjectiveComplete>(ObjectiveComplete);
            signalBus.Subscribe<Signals.GameLoopProgress>(GameIsInProgress);
            signalBus.Subscribe<Signals.OnMove>(OnGemSwap);
            signalBus.Subscribe<Signals.AddToScore>(AddToScore);
        }

        private void GameIsInProgress(Signals.GameLoopProgress signal)
        {
            inProgress = signal.state;
            if(!inProgress && (timeEnded || movesEnded))
                ShowLoseScreen();
        }

        private void AddToScore(Signals.AddToScore signal)
        {
            score += signal.score;
            scoreText.text = score.ToString();
        }

        private void OnGemSwap()
        {
            remainingMoves--;
            if (remainingMoves < 0) remainingMoves = 0;
            if (remainingMoves <= 0)
                movesEnded = true;
            SetRemainingMoves();
        }

        private void ObjectiveComplete(Signals.OnObjectiveComplete signal)
        {
            objs[signal.type] = true;
            CheckAllObjectives();
        }

        private void CheckAllObjectives()
        {
            if (objs.Any(objective => !objective.Value))
                return;
            ShowWinScreen();
        }

        private void ShowWinScreen()
        {
            timer.Stop();
            DOVirtual.DelayedCall(1.5f, () =>
            {
                Time.timeScale = 0;
                DOTween.KillAll();
                audioManager.PlayWin();
                winFactory.Create(new WinDetails
                {
                    score = score,
                    remainingMoves = remainingMoves,
                    remainingTime = timer.GetRemainingTime()
                }).transform.SetParent(transform.parent, false);
            });
        }

        private void ShowLoseScreen()
        {
            timer.Stop();
            Time.timeScale = 0;
            DOTween.KillAll();
            DOVirtual.DelayedCall(1.5f, () =>
            {
                loseFactory.Create(new LossDetails()
                {
                    score = score,
                    reason = "",
                    level = levelSetting.level
                }).transform.SetParent(transform.parent, false);
            });
        }

        private void SetRemainingMoves()
        {
            moves.text = remainingMoves.ToString();
        }

        private void CreateObjectives()
        {
            foreach (var color in levelSetting.gems)
                factory.Create(color).transform.SetParent(objectives, false);
        }

        private void SetTimer()
        {
            UpdateClock(levelSetting.time);
            timer = new Timer(levelSetting.time, UpdateClock, OnTimerEnd);
            timer.Start();
        }

        private void UpdateClock(int newTime)
        {
            clock.text = $"{newTime / 60}:{newTime % 60:D2}";
        }

        private void OnTimerEnd()
        {
            timeEnded = true;
            timer.Stop();
            if(!inProgress)
                ShowLoseScreen();
        }
    }
}