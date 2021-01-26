using UnityEngine;
using System.Collections;
using NUnit.Framework;
using TargetPreview.Models;
using System.Collections.Generic;
using TargetPreview.Display;
using System.Linq;
using TargetPreview.Math;

namespace Assets.TargetPreview.Scripts.Debug
{
    public class DebugSphere : MonoBehaviour
    {
        List<Target> sphereTargets = new List<Target>();

        int pitchCount = 83;

        [ContextMenu("Create debug sphere")]
        void CreateDebugSphere()
        {
            TargetPool targetPool = FindObjectOfType<TargetPool>();
            
            if (sphereTargets.Any())
            {
                foreach (Target target in sphereTargets)
                {
                    targetPool.Return(target);
                }
            }
            
            for (int i = 0; i < (pitchCount - 1); i++)
            {
                TargetPosition targetPos = TargetTransform.CalculateTargetTransform(i + 1, (0f, 0f, 0f));
                TargetData targetData = new TargetData(TargetBehavior.Standard, TargetHandType.Left, 0, targetPos);
                Target newTarget = targetPool.Take(targetData);
                sphereTargets.Add(newTarget);
            }
        }

    }
}