using TargetPreview.Targets;
using UnityEngine;

namespace TargetPreview.ScriptableObjects
{
    public class VisualConfig : SingletonScriptableObject<VisualConfig>
    {
        [Header("Colors")]
        public Color leftHandColor;
        public Color rightHandColor;
        public Color noHandColor = Color.grey;
        [Header("Rendering")]
        public float targetBloomAmount = 2.416924f;
        public Material meleeTargetMaterial;
        public Material standardTargetMaterial;
        public Material telegraphMaterial;
        [Header("Settings")]
        public float targetSpeedMultiplier = 1f;
        public float meleeSpeedMultiplier = 1f;
        public float meleeRotationSpeed = 0.1f;
        

        public static Color GetColorForHandType(TargetHandType handType) =>
            handType switch
            {
                TargetHandType.Left => Instance.leftHandColor,
                TargetHandType.Right => Instance.rightHandColor,
                _ => Instance.noHandColor,
            };

        public static Color GetTelegraphColorForHandType(TargetHandType handType)
        {
            float factor = Mathf.Pow(2, Instance.targetBloomAmount);
            Color handColor = GetColorForHandType(handType);
            return new Color(handColor.r * factor, handColor.g * factor, handColor.b * factor);
        }
    }
}
