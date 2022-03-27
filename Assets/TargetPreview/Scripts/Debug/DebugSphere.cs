using UnityEngine;
using System.Collections;
using TargetPreview.Models;
using System.Collections.Generic;
using TargetPreview.Display;
using System.Linq;
using EasyButtons;
using TargetPreview.Math;

namespace Assets.TargetPreview.Scripts.Debug
{
    public class DebugSphere : MonoBehaviour
    {
        List<Target> sphereTargets = new List<Target>();

        [SerializeField] int pitchCount = 12;
        [SerializeField] Vector3 randomOffset;
        [SerializeField] Vector3 offset;
        

        void Start() => 
            CreateDebugSphere();

        [ContextMenu("Create debug sphere")]
        [Button]
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
            sphereTargets.Clear();

            for (int i = 0; i < (pitchCount - 1); i++)
            {
                TargetPosition targetPos = TargetTransform.CalculateTargetTransform(i,
                    (Random.Range(-randomOffset.x,randomOffset.x) + offset.x,
                     Random.Range(-randomOffset.y,randomOffset.y) + offset.y, 
                     Random.Range(-randomOffset.x,randomOffset.z) + offset.z));
                
                TargetData targetData = new TargetData((TargetBehavior)Random.Range(0, 5), (TargetHandType)Random.Range(1,3), 0, targetPos);
                Target newTarget = targetPool.Take(targetData);
                sphereTargets.Add(newTarget);
            }
        }

    }
}