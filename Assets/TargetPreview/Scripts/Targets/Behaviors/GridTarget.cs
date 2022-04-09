using System.Runtime.CompilerServices;
using TargetPreview.Math;
using TargetPreview.ScriptableObjects;
using UnityEngine;

namespace TargetPreview.Targets
{
    public class GridTarget : Target
    {
        [SerializeField] Transform physicalTarget;
        [SerializeField] MeshRenderer approachRing;
        [SerializeField] MeshFilter approachRingFilter;
        [SerializeField] TrailRenderer trailRenderer;
        [SerializeField] MeshRenderer telegraph;
        [SerializeField] MeshFilter meshFilter;
        [SerializeField] MeshRenderer meshRenderer;

        /// <summary>
        /// Target animation length in ms
        /// </summary>
        public override float TargetFlyInTime => 500f;

        /// <summary>
        /// The distance between a target's base and end position in the animation
        /// </summary>
        public float targetFlyInDistance = 4f;

        /// <summary>
        /// The influence of <see cref="targetFlyInDistance"/> on vertical position in the fly-in animation.
        /// </summary>
        public float verticalInfluence = 0.2f;

        /// <summary>
        /// Start size of the approach ring.
        /// </summary>
        /// <remarks>This is captured during <see cref="Target.Awake"/></remarks>
        public Vector3 approachRingStartSize;

        MaterialPropertyBlock physicalTargetPropertyBlock;
        

        float flyInDistance => targetFlyInDistance;

        public void Awake() =>
            approachRingStartSize = approachRing.transform.localScale;

        /// <summary>
        /// Update target's appearance based on targetData.
        /// </summary>
        /// <remarks>Inherited types might need to call base then add their own implementation</remarks>
        public override void UpdateVisuals(TargetData newData)
        {
            meshFilter.mesh = AssetContainer.GetMeshForBehavior(newData.behavior);
            currentHandColor = VisualConfig.GetColorForHandType(newData.handType);
            var propertyBlock = GetPropertyBlock();

            physicalTargetPropertyBlock =
                AssetContainer.Instance.GetPropertyBlockPhysicalTarget(newData.behavior, currentHandColor);
            meshRenderer.SetPropertyBlock(physicalTargetPropertyBlock);


            transform.localPosition = newData.transformData.position;
            transform.localRotation = newData.transformData.rotation;

            //Reset transforms
            telegraph.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            physicalTarget.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));


            physicalTarget.transform.localScale =
                newData.behavior == TargetBehavior.Melee
                    ? Vector3.one
                    : Vector3.one * 20f; //Bootleg, melees are normal scale while others are 20x. Fix this later
            approachRing.transform.localRotation = Quaternion.identity;

            UpdateTelegraphVisuals(newData);
            trailRenderer.startColor = currentHandColor;

            //approachRing.SetPropertyBlock(propertyBlock);
            approachRing.SetPropertyBlock(physicalTargetPropertyBlock);

            approachRingFilter.mesh = AssetContainer.GetApproachRingForBehavior(newData.behavior);

            meshRenderer.material = VisualConfig.Instance.standardTargetMaterial;


            //Fix orientation for angled targets
            if (newData.behavior == TargetBehavior.Vertical)
            {
                approachRing.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, -90));
                telegraph.transform.Rotate(-180, -90, 180, Space.Self);
            }
            else if (newData.behavior == TargetBehavior.Horizontal)
            {
                physicalTarget.Rotate(-180, -90, 180, Space.Self);
                telegraph.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void UpdateTelegraphVisuals(TargetData newData)
        {
            TelegraphPreset telegraphPreset = AssetContainer.GetTelegraphForBehavior(newData.behavior);
            if (telegraphPreset != null)
            {
                telegraph.gameObject.SetActive(true);
                var propertyBlock = telegraphPreset.GetMaterialPropertyBlock();
                propertyBlock.SetColor("_Color", currentHandColor);
                propertyBlock.SetFloat("_TargetTime", newData.time);
                propertyBlock.SetFloat("_FadeInDuration", ModifiedFlyInTime);
                propertyBlock.SetFloat("_FadeOutDuration", ModifiedFlyInTime / 4f);
                telegraph.SetPropertyBlock(propertyBlock);
            }
            else
            {
                telegraph.gameObject.SetActive(false);
            }
        }

        public override void TimeUpdate()
        {
            float distance = TemporalDistance;
            float timeDifference = TargetData.time - TargetManager.Time;

            AnimateFlyIn(distance);
            
            //Fade in physical target
            physicalTargetPropertyBlock.SetColor("_Color",
                Color.Lerp(currentHandColor, Color.black, distance * distance));
            meshRenderer.SetPropertyBlock(physicalTargetPropertyBlock);

            if (distance > 0.99f)
                approachRing.transform.localScale = Vector3.zero;
            else
                approachRing.transform.localScale =
                    Vector3.Lerp(Vector3.zero, approachRingStartSize,
                        -(distance * (distance - 2))); //Quadratic ease out. This might need to be linear

            physicalTarget.gameObject.SetActive(TargetManager.Time < TargetData.time && timeDifference < ModifiedFlyInTime);
        }
        

        /// <summary>
        /// Animates the target along an arc.
        /// </summary>
        /// <param name="distance">A 0-1 lerp used to drive the animation time.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void AnimateFlyIn(float distance) =>
            physicalTarget.transform.localPosition = TargetTransform.Parabola(Vector3.zero, GetFlyInPosition(), flyInDistance * verticalInfluence,
                        distance);

        Vector3 GetFlyInPosition()
        {
            float direction = targetData.handType == TargetHandType.Left ? -1f : 1f;
            return new Vector3(flyInDistance * direction, flyInDistance * verticalInfluence,
                    flyInDistance * verticalInfluence); //Else use vertical influence.
        }

        MaterialPropertyBlock GetPropertyBlock()
        {
            var propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetColor("_Color", currentHandColor);
            return new MaterialPropertyBlock();
        }
    }
}