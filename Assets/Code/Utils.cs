using UnityEngine;

namespace JamSpace
{
    public static class Utils
    {
        public static Vector3 WithY(this Vector3 v, float y)
        {
            v.y = y;
            return v;
        }

        public static Vector3 WithClampedX(this Vector3 v, float min, float max)
        {
            v.x = Mathf.Clamp(v.x, min, max);
            return v;
        }
    }
}