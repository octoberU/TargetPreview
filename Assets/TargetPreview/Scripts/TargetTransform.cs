using UnityEngine;
using TargetPreview.Models;

namespace TargetPreview.Math
{
    public static class TargetTransform
    {
        public static TargetPosition CalculateTargetTransform(int pitch, (float x, float y, float z) offset)
        {
            return new TargetPosition(new Quaternion(), new Vector3(pitch, 0,0));
        }
        public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
        {
            System.Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

            var mid = Vector3.Lerp(start, end, t);

            return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
        }
    }

}
