using UnityEngine;
using TargetPreview.ScriptableObjects;
using TargetPreview.Math;
using System.Runtime.CompilerServices;

namespace TargetPreview.Models
{
    public class Target : MonoBehaviour
    {
        #region References
        /// <summary>
        /// A reference to the physical target that needs a fly-in animation.
        /// </summary>
        Transform physicalTarget;
        MeshRenderer approachRing;
        MeshFilter approachRingFilter;
        TrailRenderer trailRenderer;
        MeshRenderer telegraph;
        MeshFilter meshFilter;
        MeshRenderer meshRenderer;
        public virtual void Awake()
        {
            physicalTarget = transform.Find("PhysicalTarget");
            meshFilter = physicalTarget.GetComponent<MeshFilter>();
            meshRenderer = physicalTarget.GetComponent<MeshRenderer>();
            telegraph = transform.Find("Telegraph").GetComponent<MeshRenderer>();
            trailRenderer = physicalTarget.GetComponent<TrailRenderer>();
            approachRing = transform.Find("Ring").GetComponent<MeshRenderer>();
            approachRingFilter = approachRing.GetComponent<MeshFilter>();
            approachRingStartSize = approachRing.transform.localScale;
            TargetData = new TargetData(placeHolder: true); //DEBUG
        }
        #endregion

        public uint time;
        /// <summary>
        /// Target animation length in ms
        /// </summary>
        public float targetFlyInTime = 500f;
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
        /// <remarks>This is captured during <see cref="Awake"/></remarks>
        public Vector3 approachRingStartSize;

        #region Melee
        [Space, Header("Melee")]
        /// <summary>
        /// <see cref="targetFlyInDistance"/>
        /// </summary>
        public float meleeFlyInDistance = 8f;
        /// <summary>
        /// Melee animation length in ms
        /// </summary>
        public float meleeFlyInTime = 1000f;
        /// <summary>
        /// The influence of <see cref="targetFlyInDistance"/> on horizontal position in the fly-in animation for melees.
        /// </summary>
        public float horizontalInfluence = .4f;
        /// <summary>
        /// The influence of <see cref="targetFlyInDistance"/> on vertical position in the fly-in animation for melees.
        /// </summary>
        public float verticalMeleeInfluence = .4f;
        /// <summary>
        /// Positional offset for melees.
        /// </summary>
        public float horizontalMeleeSpawnOffset = 3f;
        /// <summary>
        /// Speed at which melees rotate.
        /// </summary>
        public float meleeSpinSpeed = 500f;
        #endregion

        Color currentHandColor;

        protected TargetData targetData;
        /// <summary>
        /// Contains all target data which influences the target's appearance.
        /// </summary>
        /// <remarks>Setting this to a new value will update all the visuals.</remarks>
        public TargetData TargetData { get => targetData; set { targetData = value; UpdateVisuals(value); } }

        private int meleeDirection;
        private float flyInTime => targetData.behavior == TargetBehavior.Melee ?
            VisualConfig.Instance.meleeSpeedMultiplier * meleeFlyInTime :
            VisualConfig.Instance.targetSpeedMultiplier * targetFlyInTime;

        private float flyInDistance => targetData.behavior == TargetBehavior.Melee ?
            meleeFlyInDistance : targetFlyInDistance;

        /// <summary>
        /// Update target's appearance based on targetData.
        /// </summary>
        /// <remarks>Inherited types might need to call base then add their own implementation</remarks>
        public virtual void UpdateVisuals(TargetData newData)
        {
            meshFilter.mesh = AssetContainer.GetMeshForBehavior(newData.behavior);
            currentHandColor = VisualConfig.GetColorForHandType(newData.handType);
            meshRenderer.material.mainTexture = AssetContainer.GetTextureForBehavior(newData.behavior);

            if (newData.behavior == TargetBehavior.Melee)
            {
                meshRenderer.material.SetFloat("_Metallic", 0f);
                meshRenderer.material.SetFloat("_Smoothness", 1f);
                meleeDirection = newData.transformData.position.x > 0 ? 1 : -1;
            }

            transform.localPosition = newData.transformData.position;
            transform.localRotation = newData.transformData.rotation;
            
            //Reset transforms
            telegraph.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            physicalTarget.transform.localRotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            
            physicalTarget.transform.localScale =
                newData.behavior == TargetBehavior.Melee ? Vector3.one : Vector3.one * 20f; //Bootleg, melees are normal scale while others are 20x. Fix this later
            approachRing.transform.localRotation = Quaternion.identity;

            UpdateTelegraphVisuals(newData);
            trailRenderer.startColor = currentHandColor;
            approachRing.material.color = VisualConfig.GetTelegraphColorForHandType(newData.handType);
            approachRingFilter.mesh = AssetContainer.GetApproachRingForBehavior(newData.behavior);

            meshRenderer.material = newData.behavior == TargetBehavior.Melee
                ? VisualConfig.Instance.meleeTargetMaterial
                : VisualConfig.Instance.standardTargetMaterial;
               
            
            //Fix orientation for angled targets
            if(newData.behavior == TargetBehavior.Vertical)
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
                telegraph.material.mainTexture = telegraphPreset.cloudTexture;
                telegraph.material.SetTexture("_MaskTex", telegraphPreset.maskTexture);
                telegraph.material.color = VisualConfig.GetTelegraphColorForHandType(newData.handType);
                telegraph.material.SetFloat("_Strength", telegraphPreset.twirlAmount);
                telegraph.material.SetFloat("_Scale", telegraphPreset.cloudSize);
                telegraph.material.SetFloat("_Spherize", telegraphPreset.spherizeAmount);
                telegraph.material.SetFloat("_SpinSpeed", telegraphPreset.spinSpeed);
                telegraph.material.SetFloat("_MaskScale", telegraphPreset.maskSize);
                telegraph.material.SetFloat("_TargetTime", newData.time);
                telegraph.material.SetFloat("_FadeInDuration", flyInTime);
                telegraph.material.SetFloat("_FadeOutDuration", flyInTime / 4f);
            }
            else
            {
                telegraph.gameObject.SetActive(false);
            }
        }

        public void Update() =>
            AnimatePhysicalTarget(TargetManager.Time);

        /// <summary>
        /// Animate target based on time.
        /// </summary>
        /// <param name="time">Time in the current song</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void AnimatePhysicalTarget(float time)
        {
            float timeDifference = TargetData.time - time;
            float distance = (Mathf.Clamp(timeDifference, 0f, flyInTime) / flyInTime);
            
            AnimateFlyIn(distance);

            if (targetData.behavior == TargetBehavior.Melee)
                physicalTarget.localRotation = Quaternion.Euler(Vector3.up * (meleeSpinSpeed * time * meleeDirection) + Vector3.right * 90 * VisualConfig.Instance.meleeRotationSpeed);
            
            //Fade in physical target
            meshRenderer.material.color = Color.Lerp(currentHandColor, Color.black, distance * distance);

            if (distance > 0.99f) 
                approachRing.transform.localScale = Vector3.zero;
            else 
                approachRing.transform.localScale = Vector3.Lerp(Vector3.zero, approachRingStartSize, -(distance * (distance - 2))); //Quadratic ease out. This might need to be linear

            physicalTarget.gameObject.SetActive(time < TargetData.time && timeDifference < flyInTime);
        }

        /// <summary>
        /// Animates the target along an arc.
        /// </summary>
        /// <param name="distance">A 0-1 lerp used to drive the animation time.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AnimateFlyIn(float distance) =>
            physicalTarget.transform.localPosition =
            targetData.behavior == TargetBehavior.Melee ? TargetTransform.MeleeParabola(Vector3.zero, GetFlyInPosition(), flyInDistance * verticalMeleeInfluence, flyInDistance * horizontalInfluence, distance, meleeDirection) :
            TargetTransform.Parabola(Vector3.zero, GetFlyInPosition(), flyInDistance * verticalInfluence, distance);

        Vector3 GetFlyInPosition()
        {
            float direction = targetData.handType == TargetHandType.Left ? -1f : 1f;
            return targetData.behavior == TargetBehavior.Melee ? 
                new Vector3(transform.position.x + -(horizontalMeleeSpawnOffset * meleeDirection), transform.position.y, meleeFlyInDistance) : //If its a melee just move it forward.
                new Vector3(flyInDistance * direction, flyInDistance * verticalInfluence, flyInDistance * verticalInfluence); //Else use vertical influence.
        }            
    }

    [System.Serializable]
    public struct TargetData
    {
        public TargetBehavior behavior;
        public TargetHandType handType;
        public float time;
        public TargetPosition transformData;

        public TargetData(TargetBehavior behavior, TargetHandType color, float time, TargetPosition transformData)
        {
            this.behavior = behavior;
            this.handType = color;
            this.time = time;
            this.transformData = transformData;
        }
        /// <summary>
        /// A constructor for creating placeholder targetData.
        /// </summary>
        /// <param name="placeHolder"></param>
        public TargetData(bool placeHolder)
        {
            this.behavior = TargetBehavior.Standard;
            this.handType = TargetHandType.Left;
            this.time = 500;
            this.transformData = new TargetPosition(new Quaternion(), new Vector3());
        }
    }

    public struct TargetPosition
    {
        public Quaternion rotation;
        public Vector3 position;
        public TargetPosition(Quaternion rotation, Vector3 position)
        {
            this.rotation = rotation;
            this.position = position;
        }
    }
}