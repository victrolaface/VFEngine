﻿using UnityEngine;
using VFEngine.Platformer.Event.Raycast.StickyRaycast.LeftStickyRaycast;
using VFEngine.Tools;

// ReSharper disable RedundantDefaultMemberInitializer
// ReSharper disable RedundantAssignment
namespace VFEngine.Platformer.Physics.Collider.RaycastHitCollider.StickyRaycastHitCollider.LeftStickyRaycastHitCollider
{
    using static StickyRaycastHitColliderData;
    using static ScriptableObjectExtensions;

    public class LeftStickyRaycastHitColliderData
    {
        #region fields

        #region dependencies

        #endregion

        private static readonly string LeftStickyRaycastHitColliderPath =
            $"{StickyRaycastHitColliderPath}LeftStickyRaycastHitCollider/";

        private static readonly string ModelAssetPath =
            $"{LeftStickyRaycastHitColliderPath}LeftStickyRaycastHitColliderModel.asset";

        #endregion

        #region properties

        #region dependencies

        public PhysicsRuntimeData PhysicsRuntimeData { get; set; }
        public LeftStickyRaycastRuntimeData LeftStickyRaycastRuntimeData { get; set; }
        public GameObject Character { get; set; }
        public Transform Transform { get; set; }
        public RaycastHit2D LeftStickyRaycastHit { get; set; }

        #endregion

        public LeftStickyRaycastHitColliderRuntimeData RuntimeData { get; set; }
        public float BelowSlopeAngleLeft { get; set; }
        public Vector3 CrossBelowSlopeAngleLeft { get; set; }

        public static readonly string LeftStickyRaycastHitColliderModelPath =
            $"{PlatformerScriptableObjectsPath}{ModelAssetPath}";

        #endregion
    }
}