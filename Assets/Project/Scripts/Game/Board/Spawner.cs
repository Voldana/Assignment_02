using System;
using System.Collections.Generic;
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
                    // Skip the center position (x, y itself)
                    if (i == 0 && j == 0) continue;

                    var neighborX = x + i;
                    var neighborY = y + j;

                    // Make sure the neighbor is within bounds
                    if (neighborX < 0 || neighborX >= levelSetting.width || neighborY < 0 ||
                        neighborY >= levelSetting.height) continue;
                    var neighborGem = grid.GetValue(neighborX, neighborY)?.GetValue();
                    if (neighborGem == null) continue;
                    var gemType = neighborGem.GetGemType();
                    neighborCounts[gemType]++; // Count the gem type in the neighborhood
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
            var randomValue = UnityEngine.Random.Range(0f,1f); // Generates a random float between 0 and 1
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
            return availableGemTypes[UnityEngine.Random.Range(0, availableGemTypes.Count)];
        }
    }
}