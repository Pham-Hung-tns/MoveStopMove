using System.Collections.Generic;
using UnityEngine;

public class CacheComponents<T> where T : Component
{
    private static readonly Dictionary<int, T> cacheDict = new Dictionary<int, T>();

    public static T Get(GameObject o)
    {
        int key = o.GetHashCode();
        if (!cacheDict.ContainsKey(key))
        {
            cacheDict.Add(key, o.GetComponent<T>());
        }
        return cacheDict[key];
    }

    public static void Clear()
    {
        cacheDict.Clear();
    }
}