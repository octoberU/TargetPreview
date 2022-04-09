﻿using System.Runtime.CompilerServices;
using TargetPreview.Math;
using TargetPreview.ScriptableObjects;
using UnityEngine;

namespace TargetPreview.Targets
{
    public class MeleeTarget : Target
    {
        [Space, Header("Melee")]
        
        /// <summary><see cref="targetFlyInDistance"/></summary>
        public float meleeFlyInDistance = 8f;

        /// <summary> Melee animation length in ms </summary>
        public override float TargetFlyInTime => 1000f;

        /// <summary> The influence of <see cref="GridTarget.targetFlyInDistance"/> on horizontal position in the fly-in animation for melees. </summary>
        public float horizontalInfluence = .4f;

        /// <summary> Positional offset for melees. </summary>
        public float horizontalMeleeSpawnOffset = 3f;

        /// <summary> Speed at which melees rotate. </summary>
        public float meleeSpinSpeed = 500f;

        /// <summary>
        /// The influence of <see cref="GridTarget.targetFlyInDistance"/> on vertical position in the fly-in animation for melees.
        /// </summary>
        public float verticalMeleeInfluence = .4f;
        
        float flyInDistance => meleeFlyInDistance;

        int meleeDirection;

        [SerializeField] MeshRenderer meleeRenderer;
        [SerializeField] Transform meleeSphereTransform;
        [SerializeField] TrailRenderer trailRenderer;
        

        public override void UpdateVisuals(TargetData newData)
        {
            meleeRenderer.material = VisualConfig.Instance.meleeTargetMaterial;
            meleeDirection = targetData.transformData.position.x > 0 ? 1 : -1;
            currentHandColor = VisualConfig.GetColorForHandType(targetData.handType);
        }

        public override void TimeUpdate()
        {
            meleeSphereTransform.localRotation = Quaternion.Euler(
                Vector3.up * (meleeSpinSpeed * TargetManager.Time * meleeDirection) +
                Vector3.right * 90 * VisualConfig.Instance
                    .meleeRotationSpeed);

            trailRenderer.startColor = currentHandColor;
            
            var temporalDistance = TemporalDistance;
            AnimateFlyIn(temporalDistance);
            
            var shouldRender = temporalDistance > 0.01f && temporalDistance < 1;
            meleeRenderer.enabled = shouldRender;
            trailRenderer.enabled = shouldRender;
        }


        /// <summary>
        /// Animates the target along an arc.
        /// </summary>
        /// <param name="distance">A 0-1 lerp used to drive the animation time.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void AnimateFlyIn(float distance)
        {
            meleeSphereTransform.position = TargetTransform.MeleeParabola(Vector3.zero, GetFlyInPosition(),
                flyInDistance * verticalMeleeInfluence, flyInDistance * horizontalInfluence, distance,
                meleeDirection);
        }

        public Vector3 GetFlyInPosition()
            => new Vector3(transform.position.x + -(horizontalMeleeSpawnOffset * meleeDirection),
                transform.position.y, meleeFlyInDistance);


    }
}