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
        public DrumPack drumpack;

        [Header("Mesh data")]
        public Mesh standardMesh;
        public Mesh sustainMesh;
        public Mesh angleMesh;
        public Mesh chainStartMesh;
        public Mesh chainMesh;
        public Mesh meleeMesh;
        public Mesh dodgeMesh;

        [Header("Texture data")]
        public Texture2D standardTexture;
        public Texture2D sustainTexture;
        public Texture2D angleTexture;
        public Texture2D chainStartTexture;
        public Texture2D chainTexture;
        public Texture2D meleeTexture;
        public Texture2D dodgeTexture;

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