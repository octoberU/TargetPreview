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
    }
}
