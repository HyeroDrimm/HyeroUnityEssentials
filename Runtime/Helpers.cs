using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HyeroUnityEssentials
{
    public static class Helpers
    {
        public static T Random<T>(this T[] array) => array.Length != 0 ? array[UnityEngine.Random.Range(0, array.Length)] : default;
        public static T Random<T>(this List<T> list) => list.Count != 0 ? list[UnityEngine.Random.Range(0, list.Count)] : default;

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1); // you can use other function that returns a random number between 0 and n+1 as integer
                (list[k], list[n]) = (list[n], list[k]);
            }
        }

        public static void Shuffle<T>(this T[] array)
        {
            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = UnityEngine.Random.Range(0, n + 1); // you can use other function that returns a random number between 0 and n+1 as integer
                (array[k], array[n]) = (array[n], array[k]);
            }
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumerable)
        {
            var list = enumerable.ToList();
            list.Shuffle();
            return list;
        }

        public class WeightedRandomList
        {
            private float weightSum;
            private float[] weights;

            public float WeightSum => weightSum;
            public float[] Weights => weights;

            public WeightedRandomList(float[] weights)
            {
                this.weights = weights;
                this.weightSum = weights.Sum();
            }

            public int GetRandomIndex()
            {
                var roll = UnityEngine.Random.Range(0, weightSum);
                var output = 0;
                while (roll >= weights[output])
                {
                    roll -= weights[output];
                    output++;
                }

                return output;
            }
        }

        // WeightedRandomList with item list
        public class WeightedRandomList<T>
        {
            private T[] items;
            private WeightedRandomList baseList;

            public float WeightSum => baseList.WeightSum;
            public float[] Weights => baseList.Weights;
            public T[] Items => items;

            public WeightedRandomList(float[] weights, T[] items)
            {
                baseList = new WeightedRandomList(weights);
                this.items = items;
            }

            public int GetRandomIndex()
            {
                return baseList.GetRandomIndex();
            }

            public T GetRandomItem()
            {
                return Items[baseList.GetRandomIndex()];
            }
        }

        public static Vector2Int[] directions = new Vector2Int[]
        {
            Vector2Int.left,
            Vector2Int.down,
            Vector2Int.right,
            Vector2Int.up,
        };
        public static Vector2Int[] extendedDirections = new Vector2Int[]
        {
            Vector2Int.left,
            new Vector2Int(-1,-1),
            Vector2Int.down,
            new Vector2Int(1,-1),
            Vector2Int.right,
            new Vector2Int(1,1),
            Vector2Int.up,
            new Vector2Int(-1,1),
        };
    }
}