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

        [Header("Telegraph presets")]
        public TelegraphPreset standardTelegraph;
        public TelegraphPreset sustainTelegraph;
        public TelegraphPreset angleTelegraph;

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

        public static Texture2D GetTextureForBehavior(TargetBehavior behavior) =>
            behavior switch
            {
                TargetBehavior.Standard => Instance.standardTexture,
                TargetBehavior.Hold => Instance.sustainTexture,
                TargetBehavior.Horizontal => Instance.angleTexture,
                TargetBehavior.Vertical => Instance.angleTexture,
                TargetBehavior.Chain => Instance.chainTexture,
                TargetBehavior.ChainStart => Instance.chainStartTexture,
                TargetBehavior.Melee => Instance.meleeTexture,
                TargetBehavior.Dodge => Instance.dodgeTexture,
                _ => default,
            };

        public static TelegraphPreset GetTelegraphForBehavior(TargetBehavior behavior) =>
        behavior switch
        {
            TargetBehavior.Standard => Instance.standardTelegraph,
            TargetBehavior.Hold => Instance.sustainTelegraph,
            TargetBehavior.Horizontal => Instance.angleTelegraph,
            TargetBehavior.Vertical => Instance.angleTelegraph,
            TargetBehavior.ChainStart => Instance.sustainTelegraph,
            _ => null,
        };

    }

}