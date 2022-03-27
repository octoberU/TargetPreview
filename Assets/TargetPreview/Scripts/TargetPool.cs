using System.Collections.Generic;
using TargetPreview.Models;
using TargetPreview.ScriptableObjects;
using UnityEngine;

namespace TargetPreview.Display
{
    public class TargetPool : MonoBehaviour
    {
        [SerializeField] AssetContainer assetContainer;
        [SerializeField] VisualConfig visualConfig;
        [SerializeField] Target targetPrefab;

        const int poolSize = 200;
        Stack<Target> targetPool = new Stack<Target>();

        void Awake() => 
            FillPool(poolSize);
        
        void FillPool(int size)
        {
            for (int i = 0; i < size; i++)
                CreateTarget();
        }

        void CreateTarget()
        {
            Target allocatedTarget = Instantiate(targetPrefab, transform);
            allocatedTarget.TargetData = new TargetData(placeHolder: true);
            allocatedTarget.gameObject.SetActive(false);
            targetPool.Push(allocatedTarget);
        }

        /// <summary>
        /// Takes a target from the <see cref="TargetPool"/>.
        /// </summary>
        /// <param name="data"></param>
        /// <returns>A reference to the target</returns>
        public Target Take(TargetData data)
        {
            if(targetPool.Count == 0)
            {
                CreateTarget();
            }

            Target storedTarget = targetPool.Pop();
            storedTarget
                .TargetData = data;
            storedTarget
                .transform.SetParent(null); // Unparented objects perform better,
            storedTarget                    // but it might be unnecessary.
                .gameObject.SetActive(true);
            return storedTarget;
        }

        public void Return(Target target)
        {
            target.gameObject.SetActive(false);
            target.transform.SetParent(transform);
            targetPool.Push(target);
        }
    }

}