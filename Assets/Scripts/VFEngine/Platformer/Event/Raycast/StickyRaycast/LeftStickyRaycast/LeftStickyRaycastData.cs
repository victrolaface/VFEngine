﻿using UnityEngine;
using VFEngine.Platformer.Layer.Mask;
using VFEngine.Platformer.Physics;
using VFEngine.Tools;

namespace VFEngine.Platformer.Event.Raycast.StickyRaycast.LeftStickyRaycast
{
    using static ScriptableObjectExtensions;
    using static StickyRaycastData;

    public class LeftStickyRaycastData
    {
        #region fields

        #region dependencies

        #endregion

        private static readonly string LeftStickyRaycastPath = $"{StickyRaycastPath}LeftStickyRaycast/";
        private static readonly string ModelAssetPath = $"{LeftStickyRaycastPath}LeftRaycastModel.asset";

        #endregion

        #region properties

        #region dependencies

        public RaycastRuntimeData RaycastRuntimeData { get; set; }
        public StickyRaycastRuntimeData StickyRaycastRuntimeData { get; set; }
        public PhysicsRuntimeData PhysicsRuntimeData { get; set; }
        public LayerMaskRuntimeData LayerMaskRuntimeData { get; set; }
        public GameObject Character { get; set; }
        public Transform Transform { get; set; }
        public bool DrawRaycastGizmosControl { get; set; }
        public float StickyRaycastLength { get; set; }
        public float BoundsWidth { get; set; }
        public float MaximumSlopeAngle { get; set; }
        public float BoundsHeight { get; set; }
        public float RayOffset { get; set; }
        public Vector2 BoundsBottomLeftCorner { get; set; }
        public Vector2 NewPosition { get; set; }
        public Vector2 BoundsCenter { get; set; }
        public LayerMask RaysBelowLayerMaskPlatforms { get; set; }

        #endregion

        public LeftStickyRaycastRuntimeData RuntimeData { get; set; }
        public RaycastHit2D LeftStickyRaycastHit { get; set; }
        public float LeftStickyRaycastLength { get; set; }
        public Vector2 LeftStickyRaycastOrigin { get; } = Vector2.zero;

        public float LeftStickyRaycastOriginX
        {
            set => value = LeftStickyRaycastOrigin.x;
        }

        public float LeftStickyRaycastOriginY
        {
            set => value = LeftStickyRaycastOrigin.y;
        }

        public static readonly string LeftStickyRaycastModelPath = $"{PlatformerScriptableObjectsPath}{ModelAssetPath}";

        #endregion
    }
}