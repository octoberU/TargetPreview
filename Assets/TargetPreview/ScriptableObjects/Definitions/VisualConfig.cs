﻿using TargetPreview.Models;
using UnityEngine;

namespace TargetPreview.ScriptableObjects
{
    public class VisualConfig : SingletonScriptableObject<VisualConfig>
    {
        [Header("Colors")]
        public Color leftHandColor;
        public Color rightHandColor;
        
        [Header("Rendering")]
        public float targetBloomAmount = 2.416924f;

        public static Color GetColorForHandType(TargetHandType handType) =>
            handType switch
            {
                TargetHandType.Left => Instance.leftHandColor,
                TargetHandType.Right => Instance.rightHandColor,
                _ => Color.white,
            };

        public static Color GetTelegraphColorForHandType(TargetHandType handType)
        {
            float factor = Mathf.Pow(2, Instance.targetBloomAmount);
            Color handColor = GetColorForHandType(handType);
            return new Color(handColor.r * factor, handColor.g * factor, handColor.b * factor);
        }
    }
}
