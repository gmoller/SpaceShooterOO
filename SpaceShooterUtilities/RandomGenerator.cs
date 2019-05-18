using System;

namespace SpaceShooterUtilities
{
    public sealed class RandomGenerator
    {
        private static readonly Lazy<RandomGenerator> Lazy = new Lazy<RandomGenerator>(() => new RandomGenerator());

        private Random _random;

        public static RandomGenerator Instance => Lazy.Value;

        private RandomGenerator()
        {
            _random = new Random();
        }

        public int GetRandomInt(int minNumber, int maxNumber)
        {
            return _random.Next(minNumber, maxNumber + 1);
        }

        public float GetRandomFloat(float minNumber, float maxNumber)
        {
            return (float)_random.NextDouble() * (maxNumber - minNumber) + minNumber;
        }

        public void SetNewRandom(int seed)
        {
            _random = new Random(seed);
        }
    }
}