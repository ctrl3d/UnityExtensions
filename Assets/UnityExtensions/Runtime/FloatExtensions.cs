using UnityEngine;

namespace work.ctrl3d
{
    public static class FloatExtensions
    {
        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return Mathf.Lerp(from2, to2, Mathf.InverseLerp(from1, to1, value));
        }
        
        public static float RemapUnclamped(this float value, float from1, float to1, float from2, float to2)
        {
            var t = (value - from1) / (to1 - from1);
            return from2 + (to2 - from2) * t;
        }
    }
}
