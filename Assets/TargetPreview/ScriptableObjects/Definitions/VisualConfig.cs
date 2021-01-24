using TargetPreview.Models;
using UnityEngine;

namespace TargetPreview.ScriptableObjects
{
    public class VisualConfig : SingletonScriptableObject<VisualConfig>
    {
        [Header("Colors")]
        public Color leftHandColor;
        public Color rightHandColor;

        public static Color GetColorForHandType(TargetHandType handType) =>
            handType switch
            {
                TargetHandType.Left => Instance.leftHandColor,
                TargetHandType.Right => Instance.leftHandColor,
                _ => Color.white,
            };
    }
}
