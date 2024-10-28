using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Project.Scripts.Game.Board
{
    public class Spawner
    {
        private readonly LevelSetting levelSetting;

        [Inject]
        public Spawner(LevelSetting settings)
        {
            levelSetting = settings;
        }

        public GemType UniformDistributionSpawner(Grid<GridObject<Gem>> grid, int x, int y)
        {
            var availableGemTypes = levelSetting.gems;
            var neighborCounts = new Dictionary<GemType, int>();

            foreach (var gemType in availableGemTypes)
                neighborCounts[gemType] = 0; 

            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    var neighborX = x + i;
                    var neighborY = y + j;
                    if (!IsValid(neighborX, neighborY)) continue;
                    var neighborGem = grid.GetValue(neighborX, neighborY)?.GetValue();
                    if (!neighborGem) continue;
                    var gemType = neighborGem.GetGemType();
                    neighborCounts[gemType]++;
                }
            }
            
            var probabilities = new Dictionary<GemType, float>();
            var totalWeight = 0f;

            foreach (var gemType in availableGemTypes)
            {
                float weight = 1 + neighborCounts[gemType];
                probabilities[gemType] = weight;
                totalWeight += weight;
            }

            foreach (var gemType in availableGemTypes)
                probabilities[gemType] /= totalWeight;
            
            var randomValue = Random.Range(0f, 1f);
            var cumulativeProbability = 0f;

            foreach (var gemType in availableGemTypes)
            {
                cumulativeProbability += probabilities[gemType];
                if (randomValue <= cumulativeProbability)
                    return gemType; 
            }
            
            return availableGemTypes[Random.Range(0, availableGemTypes.Count)];
        }
        
        public GemType NormalDistributionSpawner(Grid<GridObject<Gem>> grid, int x, int y, bool isFirstTile)
        {
            var availableGemTypes = levelSetting.gems;
            
            if (y == 0)
                return availableGemTypes[Random.Range(0, availableGemTypes.Count)];
            
            var belowTile = grid.GetValue(x, y - 1)?.GetValue();
            var belowColor = belowTile?.GetGemType() ?? availableGemTypes[Random.Range(0, availableGemTypes.Count)];
            var matchProbability = isFirstTile ? 0.4f : 0.6f;

            if (Random.value < matchProbability)
                return belowColor;
            
            GemType newColor;
            do
            {
                newColor = availableGemTypes[Random.Range(0, availableGemTypes.Count)];
            } while (newColor == belowColor);
            return newColor;
        }

        private bool IsValid(int neighborX, int neighborY)
        {
            return neighborX >= 0 && neighborX < levelSetting.width && neighborY >= 0 &&
                   neighborY < levelSetting.height;
        }
    }
}