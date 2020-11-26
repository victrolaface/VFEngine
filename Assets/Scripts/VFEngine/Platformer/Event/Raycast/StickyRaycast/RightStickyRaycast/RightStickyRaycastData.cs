﻿using UnityEngine;
using VFEngine.Tools;

namespace VFEngine.Platformer.Event.Raycast.StickyRaycast.RightStickyRaycast
{
    using static ScriptableObjectExtensions;
    using static StickyRaycastData;

    public class RightStickyRaycastData : ScriptableObject
    {
        #region fields

        #region dependencies

        #endregion

        private static readonly string RightStickyRaycastPath = $"{StickyRaycastPath}RightStickyRaycast/";
        private static readonly string ModelAssetPath = $"{RightStickyRaycastPath}RightRaycastModel.asset";

        #endregion

        #region properties

        #region dependencies

        #endregion

        public RaycastHit2D RightStickyRaycastHit { get; set; }
        public float RightStickyRaycastLength { get; set; }
        public Vector2 RightStickyRaycastOrigin { get; } = Vector2.zero;

        public float RightStickyRaycastOriginX
        {
            set => value = RightStickyRaycastOrigin.x;
        }

        public float RightStickyRaycastOriginY
        {
            set => value = RightStickyRaycastOrigin.y;
        }

        public static readonly string RightStickyRaycastModelPath =
            $"{PlatformerScriptableObjectsPath}{ModelAssetPath}";

        #endregion
    }
}