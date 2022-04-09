using System;
using UnityEngine;
using TargetPreview.ScriptableObjects;
using TargetPreview.Math;
using System.Runtime.CompilerServices;

namespace TargetPreview.Targets
{
    public abstract class Target : MonoBehaviour
    {
        protected Color currentHandColor;
        protected TargetData targetData;
        
        /// <summary>
        /// Contains all target data which influences the target's appearance.
        /// </summary>
        /// <remarks>Setting this to a new value will update all the visuals.</remarks>
        public TargetData TargetData
        {
            get => targetData;
            set
            {
                targetData = value;
                UpdateVisuals(value);
            }
        }
        void OnEnable()
            => TargetManager.AppendTarget(this);
        
        void OnDisable()
            => TargetManager.RemoveTarget(this);

        public abstract void TimeUpdate();

        public abstract float TargetFlyInTime { get; }
        
        public float ModifiedFlyInTime => VisualConfig.Instance.targetSpeedMultiplier * TargetFlyInTime;

        public float TemporalDistance
        {
            get
            {
                float timeDifference = TargetData.time - TargetManager.Time;
                return (Mathf.Clamp(timeDifference, 0f, ModifiedFlyInTime) / ModifiedFlyInTime); 
            }
        }

        public abstract void UpdateVisuals(TargetData newData);
    }
}