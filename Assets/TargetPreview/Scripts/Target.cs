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
        MeshFilter meshFilter;
        MeshRenderer meshRenderer;
        public virtual void Awake()
        {
            physicalTarget = transform.Find("PhysicalTarget");
            meshFilter = physicalTarget.GetComponent<MeshFilter>();
            meshRenderer = physicalTarget.GetComponent<MeshRenderer>();
            TargetData = new TargetData(placeHolder: true); //DEBUG
        }
        #endregion

        public uint time;
        /// <summary>
        /// Target animation length in ms
        /// </summary>
        public float flyInTime = 500f;
        /// <summary>
        /// The distance between a target's base and end position in the animation
        /// </summary>
        public float flyInDistance = 4f;
        /// <summary>
        /// The influence of <see cref="flyInDistance"/> on vertical position in the fly-in animation.
        /// </summary>
        public float verticalInfluence = 0.2f;

        Color currentHandColor;

        protected TargetData targetData;
        /// <summary>
        /// Contains all target data which influences the target's appearance.
        /// </summary>
        /// <remarks>Setting this to a new value will update all the visuals.</remarks>
        public TargetData TargetData { get => targetData; set { targetData = value; UpdateVisuals(); } }

        /// <summary>
        /// Update target's appearance based on targetData.
        /// </summary>
        /// <remarks>Inherited types might need to call base then add their own implementation</remarks>
        public virtual void UpdateVisuals()
        {
            meshFilter.mesh = AssetContainer.GetMeshForBehavior(targetData.behavior);
            currentHandColor = meshRenderer.material.color = VisualConfig.GetColorForHandType(targetData.handType);
            meshRenderer.material.mainTexture = AssetContainer.GetTextureForBehavior(targetData.behavior);
            transform.position = targetData.transformData.position;
            transform.rotation = targetData.transformData.rotation;
        }
        void Update() =>
            AnimatePhysicalTarget(time);

        /// <summary>
        /// Animate target based on time.
        /// </summary>
        /// <param name="time">Time in the current song</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public virtual void AnimatePhysicalTarget(uint time)
        {
            float timeDifference = TargetData.time - (float)time;
            float distance = (Mathf.Clamp(timeDifference, 0f, flyInTime) / flyInTime);
            
            AnimateFlyIn(distance);
            
            meshRenderer.material.color = Color.Lerp(currentHandColor, Color.black, distance * distance);
        }

        /// <summary>
        /// Animates the target along an arc.
        /// </summary>
        /// <param name="distance">A 0-1 lerp used to drive the animation time.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AnimateFlyIn(float distance)
        {
            float direction = targetData.handType == TargetHandType.Left ? -1f : 1f;
            Vector3 endPos = new Vector3(flyInDistance * direction, flyInDistance * verticalInfluence, flyInDistance * verticalInfluence);
            physicalTarget.transform.localPosition = TargetTransform.Parabola(Vector3.zero, endPos, flyInDistance * verticalInfluence, distance);
        }
    }

    public struct TargetData
    {
        public TargetBehavior behavior;
        public TargetHandType handType;
        public uint time;
        public TargetPosition transformData;

        public TargetData(TargetBehavior behavior, TargetHandType color, uint time, TargetPosition transformData)
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
            this.behavior = TargetBehavior.Hold;
            this.handType = TargetHandType.Left;
            this.time = 1000;
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