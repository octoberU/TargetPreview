using UnityEngine;
using TargetPreview.Models;

namespace TargetPreview.Math
{
    public static class TargetTransform
    {
        public static TargetPosition CalculateTargetTransform(int pitch, (float, float, float) offset)
        {
            return new TargetPosition(new Quaternion(), new Vector3());
        }
    }
}
