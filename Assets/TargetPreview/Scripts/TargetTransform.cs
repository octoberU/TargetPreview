using UnityEngine;
using TargetPreview.Models;

namespace TargetPreview.Math
{
    public static class TargetTransform
    {
        const int
            numRows = 7,
            numCols = 12;

        const float
            degreesBetweenX = 17f,
            degreesBetweenY = 11f,
            distance = 15f,
            sphereOffsetY = -1.5f,
            sphereOffsetZ = 6f;
        
        
        public static TargetPosition CalculateTargetTransform(int pitch, (float x, float y, float z) offset)
        {
            float column, row, zOffset;
            
            //Convert pitches to column and row.
            column = (pitch - ((pitch / numCols) * numCols));
            row = pitch/numCols;

            //Center the columns so that 0 is in the middle.
            column -= (float)(numCols -1 ) / 2f;
            row -= (float)(numRows - 1) / 2f;
            
            row += 1.5f;

            //Add the offset
            column += offset.x;
            row += offset.y;
            zOffset = distance + offset.z;

            //Convert to degrees
            Quaternion rotation = Quaternion.identity * Quaternion.Euler(row * degreesBetweenY * -1, column * degreesBetweenX, 0);
            Vector3 position = rotation * Vector3.forward * zOffset;
            
            position.y += sphereOffsetY;
            position.z += sphereOffsetZ;

            return new TargetPosition(rotation, position);
        }
        public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
        {
            System.Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

            var mid = Vector3.Lerp(start, end, t);

            return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
        }
    }

}
