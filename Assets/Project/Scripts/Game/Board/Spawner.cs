
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
            // Step 1: Define all gem colors and initialize counts
            var availableGemTypes = levelSetting.gems;
            var neighborCounts = new Dictionary<GemType, int>();

            foreach (var gemType in availableGemTypes)
                neighborCounts[gemType] = 0; // Initialize neighbor counts for each gem type


            // Step 2: Check the 8 neighbors around (x, y)
            for (var i = -1; i <= 1; i++)
            {
                for (var j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    var neighborX = x + i;
                    var neighborY = y + j;
                    if (!IsValid(neighborX, neighborY)) continue;
                    var neighborGem = grid.GetValue(neighborX, neighborY)?.GetValue();
                    if (neighborGem == null) continue;
                    var gemType = neighborGem.GetGemType();
                    neighborCounts[gemType]++;
                }
            }

            // Step 3: Calculate the adjusted probabilities based on neighbor counts
            var probabilities = new Dictionary<GemType, float>();
            var totalWeight = 0f;

            foreach (var gemType in availableGemTypes)
            {
                // The new weight: base weight (1) + the number of neighboring tiles with the same color
                float weight = 1 + neighborCounts[gemType];
                probabilities[gemType] = weight;
                totalWeight += weight;
            }

            // Step 4: Normalize the probabilities
            foreach (var gemType in availableGemTypes)
            {
                probabilities[gemType] /= totalWeight; // Normalize to ensure total probability sums to 1
            }

            // Step 5: Randomly select a gem based on the calculated probabilities
            var randomValue = Random.Range(0f, 1f); // Generates a random float between 0 and 1
            var cumulativeProbability = 0f;

            foreach (var gemType in availableGemTypes)
            {
                cumulativeProbability += probabilities[gemType];
                if (randomValue <= cumulativeProbability)
                {
                    return gemType; // This gemType is selected
                }
            }

            // Fallback (should never hit this if the probabilities are correctly calculated)
            return availableGemTypes[Random.Range(0, availableGemTypes.Count)];
        }
        
        public GemType NormalDistributionSpawner(Grid<GridObject<Gem>> grid, int x, int y, bool isFirstTile)
        {
            // Step 1: Define all gem colors and set default probability for each
            var availableGemTypes = levelSetting.gems;
    
            // Step 2: If tile is at the bottom row, choose color randomly with equal probability
            if (y == 0)
                return availableGemTypes[Random.Range(0, availableGemTypes.Count)];
            

            // Step 3: Get the color of the tile directly below if it exists
            var belowTile = grid.GetValue(x, y - 1)?.GetValue();
            var belowColor = belowTile?.GetGemType() ?? availableGemTypes[Random.Range(0, availableGemTypes.Count)];

            // Step 4: Set probability based on position (first tile or subsequent tiles)
            var matchProbability = isFirstTile ? 0.4f : 0.6f;

            // Step 5: Generate the tile color with probability matching the tile below
            if (Random.value < matchProbability)
            {
                return belowColor; // Match the color of the tile below
            }

            // Randomly select a different color if not matching
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