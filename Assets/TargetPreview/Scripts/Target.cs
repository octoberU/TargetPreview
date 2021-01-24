using UnityEngine;
using TargetPreview.ScriptableObjects;

namespace TargetPreview.Models
{
    public class Target : MonoBehaviour
    {
        MeshFilter meshFilter;
        MeshRenderer meshRenderer;

        protected TargetData targetData;
        /// <summary>
        /// Contains all target data which influences the target's appearance.
        /// </summary>
        /// <remarks>Setting this to a new value will update all the visuals.</remarks>
        public TargetData TargetData { get => targetData; set { targetData = value; UpdateVisuals(); } }

        protected void UpdateVisuals()
        {
            meshFilter.mesh = AssetContainer.GetMeshForBehavior(targetData.behavior);
            meshRenderer.material.color = VisualConfig.GetColorForHandType(TargetData.handType);
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