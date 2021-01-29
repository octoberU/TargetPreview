using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TargetPreview.ScriptableObjects
{
    [CreateAssetMenu(menuName = "TargetPreview/Telegraph Preset")]
    public class TelegraphPreset : ScriptableObject
    {
        [Header("Cloud")]
        public Texture2D cloudTexture;
        public float cloudSize = 1.15f;
        public float twirlAmount = -6.9f;
        public float spinSpeed = -30f;
        public float spherizeAmount = 2.2f;

        [Header("Mask")]
        public Texture2D maskTexture;
        public float maskSize = 1.19f;
    }

}