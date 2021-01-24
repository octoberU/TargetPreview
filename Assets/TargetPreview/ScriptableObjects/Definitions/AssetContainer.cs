using TargetPreview.Models;
using UnityEngine;

namespace TargetPreview.ScriptableObjects
{
    /// <summary>
    /// A flyweight pattern for storing all the memory demanding assets.
    /// </summary>
    public class AssetContainer : SingletonScriptableObject<AssetContainer>
    {
        [Header("Audio")]
        public AudioClip kick;
        public AudioClip snare;
        public AudioClip percussion;
        public AudioClip chainStart;
        public AudioClip chain;
        public AudioClip melee;

        [Header("Mesh data")]
        public Mesh standardMesh;
        public Mesh sustainMesh;
        public Mesh angleMesh;
        public Mesh chainStartMesh;
        public Mesh chainMesh;
        public Mesh meleeMesh;
        public Mesh dodgeMesh;

        public static Mesh GetMeshForBehavior(TargetBehavior behavior) =>
            behavior switch
            {
                TargetBehavior.Standard => Instance.standardMesh,
                TargetBehavior.Hold => Instance.sustainMesh,
                TargetBehavior.Horizontal => Instance.angleMesh,
                TargetBehavior.Vertical => Instance.angleMesh,
                TargetBehavior.Chain => Instance.chainMesh,
                TargetBehavior.ChainStart => Instance.chainStartMesh,
                TargetBehavior.Melee => Instance.meleeMesh,
                TargetBehavior.Dodge => Instance.dodgeMesh,
                _ => default,
            };
    }

}