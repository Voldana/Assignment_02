using System.Collections;
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
        [Inject] private Spawner spawner;

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
        }

        private void Start()
        {
            InitializeGrid();
            inputReader.Fire += OnSelectGem;
            DOVirtual.DelayedCall(1.5f, () => { StartCoroutine(CheckMatches(true)); });
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

        private IEnumerator RunGameLoop(Vector2Int gridPosA, Vector2Int gridPosB)
        {
            yield return StartCoroutine(SwapGems(gridPosA, gridPosB));
            yield return CheckMatches(false);
            DeselectGem();
        }

        private int multiplier = 1;

        private IEnumerator CheckMatches(bool init)
        {
            var matches = FindMatches();
            if (init && matches.Count > 0)
            {
                foreach (var match in matches)
                    ReplaceGems(match);
                yield break;
            }

            if (matches.Count == 0)
            {
                multiplier = 1;
                yield break;
            }

            CalculateScore(matches.Count);
            SendMatchSignals(matches);
            yield return StartCoroutine(ExplodeGems(matches));
            yield return StartCoroutine(MakeGemsFall());
            yield return StartCoroutine(FillEmptySpots());
            multiplier++;
            StartCoroutine(CheckMatches(init));
        }

        private void CalculateScore(int matches)
        {
            signalBus.Fire(new Signals.AddToScore { score = matches * multiplier * 10 });
        }

        private void ReplaceGems(GemMatch match)
        {
            foreach (var gem in match.coordinates)
            {
                var cell = grid.GetValue(gem.x, gem.y).GetValue();
                var neighborTypes = new HashSet<GemType>();

                if (gem.x > 0)
                    neighborTypes.Add(grid.GetValue(gem.x - 1, gem.y).GetValue().GetGemType());

                if (gem.x < 7)
                    neighborTypes.Add(grid.GetValue(gem.x + 1, gem.y).GetValue().GetGemType());

                if (gem.y > 0)
                    neighborTypes.Add(grid.GetValue(gem.x, gem.y - 1).GetValue().GetGemType());

                if (gem.y < 7)
                    neighborTypes.Add(grid.GetValue(gem.x, gem.y + 1).GetValue().GetGemType());

                grid.SetValue(gem.x, gem.y, null);
                Destroy(cell.gameObject);
                GemType newGemType;
                do
                {
                    newGemType = levelSetting.gems[Random.Range(0, levelSetting.gems.Count)];
                    ;
                } while (neighborTypes.Contains(newGemType));

                CreateGem(gem.x, gem.y, newGemType);
            }
        }

        private void SendMatchSignals(List<GemMatch> matches)
        {
            foreach (var match in matches)
                signalBus.Fire(new Signals.OnMatch { type = match.gemType.type });
        }

        private bool isFirst = true;
        private bool isTileAboveEmpty = false;
        private GemType type;

        private IEnumerator FillEmptySpots()
        {
            for (var y = 0; y < levelSetting.height; y++)
            {
                isFirst = true;
                for (var x = 0; x < levelSetting.width; x++)
                {
                    if (grid.GetValue(x, y) != null)
                    {
                        isFirst = true;
                        continue;
                    }

                    IsTileAboveEmpty(x, y);
                    type = levelSetting.level.Equals(2)
                        ? spawner.UniformDistributionSpawner(grid, x, y)
                        : spawner.NormalDistributionSpawner(grid, x, y, isFirst && isTileAboveEmpty);

                    CreateGem(x, y, type);
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }

        private void IsTileAboveEmpty(int x, int y)
        {
            isTileAboveEmpty =  grid.GetValue(x, y) == null;
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
                            .DOLocalMove(grid.GetWorldPositionCenter(x, y), 0.25f)
                            .SetEase(Ease);
                        // audioManager.PlayWoosh();
                        yield return new WaitForSeconds(0.05f);
                        break;
                    }
                }
            }
        }

        private IEnumerator ExplodeGems(List<GemMatch> matches)
        {
            foreach (var match in matches)
            {
                foreach (var coordinate in match.coordinates)
                {
                    if (grid.GetValue(coordinate.x, coordinate.y) == null) continue;
                    var gem = grid.GetValue(coordinate.x, coordinate.y).GetValue();
                    grid.SetValue(coordinate.x, coordinate.y, null);
                    ExplodeVFX(coordinate);
                    gem.transform.DOPunchScale(Vector3.one * 0.1f, 0.1f, 1, 0.5f);
                    yield return new WaitForSeconds(0.1f);
                    Destroy(gem.gameObject, 0.1f);
                }
            }
        }

        private void ExplodeVFX(Vector2Int match)
        {
            audioManager.PlayPop();
            var fx = Instantiate(explosion, transform);
            fx.transform.position = grid.GetWorldPositionCenter(match.x, match.y);
            Destroy(fx, 5f);
        }

        private List<GemMatch> FindMatches()
        {
            List<GemMatch> matchesWithGemType = new();

            for (var y = 0; y < levelSetting.height; y++)
            {
                for (var x = 0; x < levelSetting.width - 2; x++)
                {
                    var gemA = grid.GetValue(x, y);
                    if (gemA == null) continue;

                    var gemType = gemA.GetValue().GetGemType();
                    List<Vector2Int> currentMatch = new() { new Vector2Int(x, y) };

                    // Check if there are more matching gems horizontally
                    for (var i = x + 1; i < levelSetting.width; i++)
                    {
                        var nextGem = grid.GetValue(i, y);
                        if (nextGem == null || nextGem.GetValue().GetGemType() != gemType) break;

                        currentMatch.Add(new Vector2Int(i, y));
                    }

                    // If the match has 3 or more gems, add it as a valid match
                    if (currentMatch.Count < 3) continue;
                    matchesWithGemType.Add(new GemMatch(currentMatch, gemType));
                    x += currentMatch.Count - 1; // Skip over the matched gems in the horizontal direction
                }
            }


            for (var x = 0; x < levelSetting.width; x++)
            {
                for (var y = 0; y < levelSetting.height - 2; y++)
                {
                    var gemA = grid.GetValue(x, y);
                    if (gemA == null) continue;

                    var gemType = gemA.GetValue().GetGemType();
                    List<Vector2Int> currentMatch = new() { new Vector2Int(x, y) };

                    // Check if there are more matching gems vertically
                    for (var i = y + 1; i < levelSetting.height; i++)
                    {
                        var nextGem = grid.GetValue(x, i);
                        if (nextGem == null || nextGem.GetValue().GetGemType() != gemType) break;

                        currentMatch.Add(new Vector2Int(x, i));
                    }

                    // If the match has 3 or more gems, add it as a valid match
                    if (currentMatch.Count < 3) continue;
                    matchesWithGemType.Add(new GemMatch(currentMatch, gemType));
                    y += currentMatch.Count - 1; // Skip over the matched gems in the vertical direction
                }
            }

            return matchesWithGemType;
        }

        private IEnumerator SwapGems(Vector2Int gridPosA, Vector2Int gridPosB)
        {
            var gridObjectA = grid.GetValue(gridPosA.x, gridPosA.y);
            var gridObjectB = grid.GetValue(gridPosB.x, gridPosB.y);
            if (!IsAdjacent(gridPosA, gridPosB))
            {
                ShakeGems(gridObjectA.GetValue(), gridObjectB.GetValue());
                yield break;
            }

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

        private static void ShakeGems(Gem gemA, Gem gemB)
        {
            gemA.transform.DOShakeRotation(0.5f, Vector3.one * 5);
            gemB.transform.DOShakeRotation(0.5f, Vector3.one * 5);
        }

        private static bool IsAdjacent(Vector2Int posA, Vector2Int posB)
        {
            var difference = (posA - posB).magnitude;
            return !(difference > 1);
        }

        private void InitializeGrid()
        {
            grid = Grid<GridObject<Gem>>.VerticalGrid(levelSetting, CellSize, originPosition);

            for (var x = 0; x < levelSetting.width; x++)
            for (var y = 0; y < levelSetting.height; y++)
            {
                var type = levelSetting.gems[Random.Range(0, levelSetting.gems.Count)];
                CreateGem(x, y, type);
            }
        }

        private void CreateGem(int x, int y, GemType type)
        {
            var gem = factory.Create(type, new Vector2Int(x, y));
            gem.transform.position = grid.GetWorldPositionCenter(x, y);
            gem.transform.SetParent(transform);
            gem.transform.rotation = quaternion.identity;
            var gridObject = new GridObject<Gem>(grid, x, y);
            gridObject.SetValue(gem);
            grid.SetValue(x, y, gridObject);
        }

        private void DeselectGem()
        {
            grid.GetValue(selectedGem.x, selectedGem.y).GetValue().SetSelected(false);
            selectedGem = new Vector2Int(-1, -1);
        }

        private void SelectGem(Vector2Int gridPos)
        {
            Debug.Log(gridPos);
            signalBus.Fire(new Signals.DeselectAllGems());
            grid.GetValue(gridPos.x, gridPos.y).GetValue().SetSelected(true);
            selectedGem = gridPos;
        }

        private bool IsEmptyPosition(Vector2Int gridPosition) => grid.GetValue(gridPosition.x, gridPosition.y) == null;

        private bool IsValidPosition(Vector2 gridPosition)
        {
            return gridPosition.x >= 0 && gridPosition.x < levelSetting.width && gridPosition.y >= 0 &&
                   gridPosition.y < levelSetting.height;
        }
    }

    internal struct GemMatch
    {
        public readonly List<Vector2Int> coordinates;
        public readonly GemType gemType;

        public GemMatch(List<Vector2Int> coordinates, GemType gemType)
        {
            this.coordinates = coordinates;
            this.gemType = gemType;
        }
    }
}