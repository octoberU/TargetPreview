using UnityEngine;
using TargetPreview.ScriptableObjects;

namespace TargetPreview.Models
{
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(Animator))]
    public class Target : MonoBehaviour
    {
        #region References
        MeshFilter meshFilter;
        MeshRenderer meshRenderer;
        Animator animator;
        public virtual void Awake()
        {
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
            animator = GetComponent<Animator>();
        }
        #endregion

        protected TargetData targetData;
        /// <summary>
        /// Contains all target data which influences the target's appearance.
        /// </summary>
        /// <remarks>Setting this to a new value will update all the visuals.</remarks>
        public TargetData TargetData { get => targetData; set { targetData = value; UpdateVisuals(); } }

        protected void UpdateVisuals()
        {
            meshFilter.mesh = AssetContainer.GetMeshForBehavior(targetData.behavior);
            meshRenderer.material.color = VisualConfig.GetColorForHandType(targetData.handType);
            meshRenderer.material.mainTexture = AssetContainer.GetTextureForBehavior(targetData.behavior);
            transform.position = targetData.transformData.position;
            transform.rotation = targetData.transformData.rotation;
        }
        public virtual void Update() { }
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
            this.time = 0;
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