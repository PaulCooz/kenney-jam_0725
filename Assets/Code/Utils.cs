using System.Collections.Generic;
using UnityEngine;

namespace JamSpace
{
    public static class Utils
    {
        public static Vector3 WithX(this Vector3 v, float x)
        {
            v.x = x;
            return v;
        }

        public static Vector3 WithY(this Vector3 v, float y)
        {
            v.y = y;
            return v;
        }

        public static Vector3 WithZ(this Vector3 v, float z)
        {
            v.z = z;
            return v;
        }

        public static T GetRand<T>(this IReadOnlyList<T> list) => list[Random.Range(0, list.Count)];
    }
}