using System;

namespace SampleCode.Inventory.Runtime
{
    public static class RandomID
    {
        private static Random s_random;

        public static uint Empty => 0;

        private static Random Random => RandomID.s_random ?? (RandomID.s_random = new Random());

        public static uint Generate() => (uint) (RandomID.Random.Next(1073741824) << 2) | (uint) RandomID.Random.Next(4);

        public static bool IsIDEmpty(uint id) => (int) id == (int) RandomID.Empty;
    }
}