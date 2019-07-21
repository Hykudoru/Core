﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;

namespace Gemukodo
{
    public static class Utility
    {
        static readonly System.Random random = new System.Random();

        public static bool IsEven(int value)
        {
            return value % 2 == 0;
        }

        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }

        public static void Swap<T>(ref T param1, ref T param2)
        {
            T temp = param1;
            param1 = param2;
            param2 = temp;
        }

        public static bool[] RandomBools(int length)
        {
            bool[] bools = new bool[length];
            for (int i = 0; i < bools.Length; i++)
            {
                if (IsOdd(random.Next()))
                    bools[i] = true;
                else
                    bools[i] = true;
            }

            return bools;
        }

        public static int[] RandomInts(int length)
        {
            int[] randNums = new int[length];
            for (int i = 0; i < length; i++)
            {
                randNums[i] = random.Next();
            }

            return randNums;
        }

        public static T Random<T>(T[] array)
        {
            return array[random.Next() % array.Length];
        }

        public static T Random<T>(IList<T> list)
        {
            return list[random.Next() % list.Count];
        } 

        public static T[] Randomize<T>(T[] items)
        {
            Array.Sort(RandomInts(items.Length), items);

            return items;
        }

        public static IList<T> Randomize<T>(IList<T> list)
        {
            T[] copy = new T[list.Count];
            list.CopyTo(copy, 0);
            Array.Sort(RandomInts(list.Count), copy);

            return copy;
        }

        public static Transform[] Randomize(Transform[] transforms)
        {
            int count = transforms.Length;

            // Preserve original index order of transform positions
            Vector3[] positions = new Vector3[count];
            for (int i = 0; i < count; i++)
            {
                positions[i] = transforms[i].localPosition;
            }

            // Randomize original
            Array.Sort(Utility.RandomInts(count), transforms);

            // Set positions to initial index order
            for (int i = 0; i < count; i++)
            {
                transforms[i].localPosition = positions[i];
            }

            return transforms;
        }

        public static T[] Shuffle<T>(T[] items)
        {
            return Utility.Randomize(items);
        }

        public static IList<T> Shuffle<T>(IList<T> items)
        {
            return Utility.Randomize(items);
        }
    }

    public static partial class ExtensionMethods
    {
        public static bool IsEven(this int value)
        {
            return value % 2 == 0;
        }

        public static bool IsOdd(this int value)
        {
            return value % 2 != 0;
        }

        public static T Clone<T>(this T source) where T : class, new()
        {
            T clone = null;

            if (source != null)
            {
                clone = JsonUtility.FromJson<T>(JsonUtility.ToJson(source));
            }
            else
            {
                clone = default(T);
            }

            return clone;
        }

        public static T Random<T>(this T[] array)
        {
            return Utility.Random(array);
        }

        public static T Random<T>(this IList<T> list)
        {
            return Utility.Random(list);
        }

        public static T[] Randomize<T>(this T[] items)
        {
            return Utility.Randomize(items);
        }

        public static IList<T> Randomize<T>(this IList<T> list)
        {
            return Utility.Randomize(list);
        }

        public static T[] Shuffle<T>(this T[] items)
        {
            return Utility.Randomize(items);
        }

        public static IList<T> Shuffle<T>(this IList<T> items)
        {
            return Utility.Randomize(items);
        }

        public static GameObject Component<T>(this GameObject gameObject, Action<T> action) where T : Component
        {
            if (gameObject != null)
            {
                T component = gameObject.GetComponent<T>();
                if (component != null)
                {
                    action(component);
                }
            }

            return gameObject;
        }
    }
    /*
    [Serializable]
    public class SerializableKeyValuePair<TKey, TValue> : KeyValuePair<TKey, TValue>
    {
        [SerializeField] TKey key;
        [SerializeField] TValue value;

        public TKey Key { get { return key; } set { key = value; } }
        public TValue Value { get { return value; } set { this.value = value; } }
    }

    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        [SerializeField] TKey[] keys;
        [SerializeField] TValue[] values;

        [SerializeField] Dictionary<TKey, TValue> dictionary;

        public SerializableDictionary(int capacity = 1)
        {
            dictionary = new Dictionary<TKey, TValue>(capacity);
        }
    }

    public class ShuffledArray<T>
    {
        private T[] items;
        //...
        public ShuffledArray(int length)
        {
            items = new T[length];
        }

        public T this[int i]
        {
            get { return items[i]; }
            set { items[i] = value; }
        }
    }
    */
}