﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Project.Scripts.Game.Board
{
    public class Manager : MonoBehaviour
    {
        [Inject] private LevelSetting levelSetting;
        [Inject] private AudioManager audioManager;
        [Inject] private Gem.Factory factory;
        [Inject] private SignalBus signalBus;

        private readonly Vector3 originPosition = new(-.5f, -1.3f, 0);
        private const Ease Ease = DG.Tweening.Ease.InQuad;
        private const float CellSize = 1f;

        [SerializeField] private GameObject explosion;

        private InputReader inputReader;

        private Grid<GridObject<Gem>> grid;

        private Vector2Int selectedGem = Vector2Int.one * -1;

        void Awake()
        {
            inputReader = GetComponent<InputReader>();
            // audioManager = GetComponent<AudioManager>();
        }

        private void Start()
        {
            InitializeGrid();
            inputReader.Fire += OnSelectGem;
            StartCoroutine(CheckMatches());
        }

        private void OnDestroy()
        {
            inputReader.Fire -= OnSelectGem;
        }

        private void OnSelectGem()
        {
            var gridPos = grid.GetXY(Camera.main.ScreenToWorldPoint(inputReader.Selected));

            if (!IsValidPosition(gridPos) || IsEmptyPosition(gridPos)) return;

            if (selectedGem == gridPos)
            {
                DeselectGem();
                // audioManager.PlayDeselect();
            }
            else if (selectedGem == Vector2Int.one * -1)
            {
                SelectGem(gridPos);
                // audioManager.PlayClick();
            }
            else
            {
                StartCoroutine(RunGameLoop(selectedGem, gridPos));
            }
        }

        IEnumerator RunGameLoop(Vector2Int gridPosA, Vector2Int gridPosB)
        {
            yield return StartCoroutine(SwapGems(gridPosA, gridPosB));

            // Matches?
            yield return CheckMatches();
            // List<Vector2Int> matches = FindMatches();
            // // TODO: Calculate score
            // // Make Gems explode
            // yield return StartCoroutine(ExplodeGems(matches));
            // // Make gems fall
            // yield return StartCoroutine(MakeGemsFall());
            // // Fill empty spots
            // yield return StartCoroutine(FillEmptySpots());


            // TODO: Check if game is over

            DeselectGem();
        }

        private IEnumerator CheckMatches()
        {
            var matches = FindMatches();
            if (matches.Count == 0)
                yield break;
            // TODO: Calculate score
            yield return StartCoroutine(ExplodeGems(matches));
            yield return StartCoroutine(MakeGemsFall());
            yield return StartCoroutine(FillEmptySpots());
            StartCoroutine(CheckMatches());
        }

        private IEnumerator FillEmptySpots()
        {
            for (var x = 0; x < levelSetting.width; x++)
            {
                for (var y = 0; y < levelSetting.height; y++)
                {
                    if (grid.GetValue(x, y) != null) continue;
                    CreateGem(x, y);
                    // audioManager.PlayPop();
                    yield return new WaitForSeconds(0.1f);
                    ;
                }
            }
        }

        private IEnumerator MakeGemsFall()
        {
            // TODO: Make this more efficient
            for (var x = 0; x < levelSetting.width; x++)
            {
                for (var y = 0; y < levelSetting.height; y++)
                {
                    if (grid.GetValue(x, y) != null) continue;
                    for (var i = y + 1; i < levelSetting.height; i++)
                    {
                        if (grid.GetValue(x, i) == null) continue;
                        var gem = grid.GetValue(x, i).GetValue();
                        grid.SetValue(x, y, grid.GetValue(x, i));
                        grid.SetValue(x, i, null);
                        gem.transform
                            .DOLocalMove(grid.GetWorldPositionCenter(x, y), 0.5f)
                            .SetEase(Ease);
                        // audioManager.PlayWoosh();
                        yield return new WaitForSeconds(0.1f);
                        break;
                    }
                }
            }
        }

        IEnumerator ExplodeGems(List<Vector2Int> matches)
        {
            foreach (var match in matches)
            {
                var gem = grid.GetValue(match.x, match.y).GetValue();
                grid.SetValue(match.x, match.y, null);
                ExplodeVFX(match);
                gem.transform.DOPunchScale(Vector3.one * 0.1f, 0.1f, 1, 0.5f);
                yield return new WaitForSeconds(0.1f);
                Destroy(gem.gameObject, 0.1f);
            }
        }

        private void ExplodeVFX(Vector2Int match)
        {
            audioManager.PlayPop();
            var fx = Instantiate(explosion, transform);
            fx.transform.position = grid.GetWorldPositionCenter(match.x, match.y);
            Destroy(fx, 5f);
        }

        List<Vector2Int> FindMatches()
        {
            HashSet<Vector2Int> matches = new();

            // Horizontal
            for (var y = 0; y < levelSetting.height; y++)
            {
                for (var x = 0; x < levelSetting.width - 2; x++)
                {
                    var gemA = grid.GetValue(x, y);
                    var gemB = grid.GetValue(x + 1, y);
                    var gemC = grid.GetValue(x + 2, y);

                    if (gemA == null || gemB == null || gemC == null) continue;

                    if (gemA.GetValue().GetGemType() != gemB.GetValue().GetGemType()
                        || gemB.GetValue().GetGemType() != gemC.GetValue().GetGemType()) continue;
                    matches.Add(new Vector2Int(x, y));
                    matches.Add(new Vector2Int(x + 1, y));
                    matches.Add(new Vector2Int(x + 2, y));
                }
            }

            // Vertical
            for (var x = 0; x < levelSetting.width; x++)
            {
                for (var y = 0; y < levelSetting.height - 2; y++)
                {
                    var gemA = grid.GetValue(x, y);
                    var gemB = grid.GetValue(x, y + 1);
                    var gemC = grid.GetValue(x, y + 2);

                    if (gemA == null || gemB == null || gemC == null) continue;

                    if (gemA.GetValue().GetGemType() != gemB.GetValue().GetGemType()
                        || gemB.GetValue().GetGemType() != gemC.GetValue().GetGemType()) continue;
                    matches.Add(new Vector2Int(x, y));
                    matches.Add(new Vector2Int(x, y + 1));
                    matches.Add(new Vector2Int(x, y + 2));
                }
            }

            if (matches.Count == 0)
            {
                // audioManager.PlayNoMatch();
            }
            else
            {
                // audioManager.PlayMatch();
            }

            return new List<Vector2Int>(matches);
        }

        private IEnumerator SwapGems(Vector2Int gridPosA, Vector2Int gridPosB)
        {
            var gridObjectA = grid.GetValue(gridPosA.x, gridPosA.y);
            var gridObjectB = grid.GetValue(gridPosB.x, gridPosB.y);

            gridObjectA.GetValue().transform
                .DOLocalMove(grid.GetWorldPositionCenter(gridPosB.x, gridPosB.y), 0.5f)
                .SetEase(Ease);
            gridObjectB.GetValue().transform
                .DOLocalMove(grid.GetWorldPositionCenter(gridPosA.x, gridPosA.y), 0.5f)
                .SetEase(Ease);

            grid.SetValue(gridPosA.x, gridPosA.y, gridObjectB);
            grid.SetValue(gridPosB.x, gridPosB.y, gridObjectA);
            signalBus.Fire(new Signals.OnMove());
            yield return new WaitForSeconds(0.5f);
        }

        void InitializeGrid()
        {
            grid = Grid<GridObject<Gem>>.VerticalGrid(levelSetting, CellSize, originPosition);

            for (var x = 0; x < levelSetting.width; x++)
            for (var y = 0; y < levelSetting.height; y++)
                CreateGem(x, y);
        }

        private void CreateGem(int x, int y)
        {
            var type = levelSetting.gems[Random.Range(0, levelSetting.gems.Count)];
            var gem = factory.Create(type);
            gem.transform.position = grid.GetWorldPositionCenter(x, y);
            gem.transform.SetParent(transform);
            gem.transform.rotation = quaternion.identity;
            var gridObject = new GridObject<Gem>(grid, x, y);
            gridObject.SetValue(gem);
            grid.SetValue(x, y, gridObject);
        }

        private void DeselectGem() => selectedGem = new Vector2Int(-1, -1);
        private void SelectGem(Vector2Int gridPos) => selectedGem = gridPos;

        private bool IsEmptyPosition(Vector2Int gridPosition) => grid.GetValue(gridPosition.x, gridPosition.y) == null;

        private bool IsValidPosition(Vector2 gridPosition)
        {
            return gridPosition.x >= 0 && gridPosition.x < levelSetting.width && gridPosition.y >= 0 &&
                   gridPosition.y < levelSetting.height;
        }
    }
}