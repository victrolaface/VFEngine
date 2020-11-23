﻿using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using VFEngine.Platformer.Physics;
using VFEngine.Platformer.Physics.Collider.RaycastHitCollider;
using VFEngine.Tools;
using UniTaskExtensions = VFEngine.Tools.UniTaskExtensions;

// ReSharper disable RedundantDefaultMemberInitializer
// ReSharper disable UnusedVariable
namespace VFEngine.Platformer.Layer.Mask
{
    using static LayerMask;
    using static DebugExtensions;
    using static ScriptableObjectExtensions;
    using static UniTaskExtensions;

    [CreateAssetMenu(fileName = "LayerMaskModel", menuName = PlatformerLayerMaskModelPath, order = 0)]
    [InlineEditor]
    public class LayerMaskModel : ScriptableObject, IModel
    {
        #region fields

        #region dependencies

        [SerializeField] private GameObject character;
        private LayerMaskData l;

        #endregion

        #region private methods

        private void InitializeData()
        {
            l = new LayerMaskData {Character = character, Settings = CreateInstance<LayerMaskSettings>()};
            l.Controller = l.Character.GetComponentNoAllocation<LayerMaskController>();
            l.DisplayWarningsControl = l.DisplayWarningsControlSetting;
            l.PlatformMask = l.PlatformMaskSetting;
            l.MovingPlatformMask = l.MovingPlatformMaskSetting;
            l.OneWayPlatformMask = l.OneWayPlatformMaskSetting;
            l.MovingOneWayPlatformMask = l.MovingOneWayPlatformMaskSetting;
            l.MidHeightOneWayPlatformMask = l.MidHeightOneWayPlatformMaskSetting;
            l.StairsMask = l.StairsMaskSetting;
            l.SavedPlatformMask = l.PlatformMask;
            l.PlatformMask |= l.OneWayPlatformMask;
            l.PlatformMask |= l.MovingPlatformMask;
            l.PlatformMask |= l.MovingOneWayPlatformMask;
            l.PlatformMask |= l.MidHeightOneWayPlatformMask;
            l.RuntimeData = LayerMaskRuntimeData.CreateInstance(l.Controller, l.SavedBelowLayer, l.PlatformMask, l.MovingPlatformMask, l.OneWayPlatformMask,
                l.MovingOneWayPlatformMask, l.MidHeightOneWayPlatformMask, l.StairsMask,
                l.RaysBelowLayerMaskPlatformsWithoutOneWay, l.RaysBelowLayerMaskPlatformsWithoutMidHeight,
                l.RaysBelowLayerMaskPlatforms);
            if (l.DisplayWarningsControl) GetWarningMessages();
        }

        private void InitializeModel()
        {
            l.PhysicsRuntimeData = l.Character.GetComponentNoAllocation<PhysicsController>().PhysicsRuntimeData;
            l.DownRaycastHitColliderRuntimeData = l.Character.GetComponentNoAllocation<RaycastHitColliderController>().DownRaycastHitColliderRuntimeData;
            l.Transform = l.PhysicsRuntimeData.Transform;
            l.StandingOnLastFrameLayer = l.DownRaycastHitColliderRuntimeData.StandingOnLastFrame.layer;
        }

        private void GetWarningMessages()
        {
            const string lM = "Layer Mask";
            const string player = "Player";
            const string platform = "Platform";
            const string movingPlatform = "MovingPlatform";
            const string oneWayPlatform = "OneWayPlatform";
            const string movingOneWayPlatform = "MovingOneWayPlatform";
            const string midHeightOneWayPlatform = "MidHeightOneWayPlatform";
            const string stairs = "Stairs";
            var platformLayers = GetMask($"{player}", $"{platform}");
            var movingPlatformLayer = GetMask($"{movingPlatform}");
            var oneWayPlatformLayer = GetMask($"{oneWayPlatform}");
            var movingOneWayPlatformLayer = GetMask($"{movingOneWayPlatform}");
            var midHeightOneWayPlatformLayer = GetMask($"{midHeightOneWayPlatform}");
            var stairsLayer = GetMask($"{stairs}");
            LayerMask[] layers =
            {
                platformLayers, movingPlatformLayer, oneWayPlatformLayer, movingOneWayPlatformLayer,
                midHeightOneWayPlatformLayer, stairsLayer
            };
            LayerMask[] layerMasks =
            {
                l.PlatformMask, l.MovingPlatformMask, l.OneWayPlatformMask, l.MovingOneWayPlatformMask,
                l.MidHeightOneWayPlatformMask, l.StairsMask
            };
            var settings = $"{lM} Settings";
            var warningMessage = "";
            var warningMessageCount = 0;
            if (!l.HasSettings) warningMessage += FieldString($"{settings}", $"{settings}");
            for (var i = 0; i < layers.Length; i++)
            {
                if (layers[i].value == layerMasks[i].value) continue;
                var mask = LayerToName(layerMasks[i]);
                var layer = LayerToName(layers[i]);
                warningMessage += FieldPropertyString($"{mask}", $"{layer}", $"{settings}");
            }

            string FieldString(string field, string scriptableObject)
            {
                AddWarningMessageCount();
                return FieldMessage(field, scriptableObject);
            }

            string FieldPropertyString(string field, string property, string scriptableObject)
            {
                AddWarningMessageCount();
                return FieldPropertyMessage(field, property, scriptableObject);
            }

            void AddWarningMessageCount()
            {
                warningMessageCount++;
            }

            DebugLogWarning(warningMessageCount, warningMessage);
        }

        private void SetRaysBelowLayerMaskPlatforms()
        {
            l.RaysBelowLayerMaskPlatforms = l.PlatformMask;
        }

        private void SetRaysBelowLayerMaskPlatformsWithoutOneWay()
        {
            l.RaysBelowLayerMaskPlatformsWithoutOneWay = l.PlatformMask & ~l.MidHeightOneWayPlatformMask &
                                                         ~l.OneWayPlatformMask & ~l.MovingOneWayPlatformMask;
        }

        private void SetRaysBelowLayerMaskPlatformsWithoutMidHeight()
        {
            l.RaysBelowLayerMaskPlatformsWithoutMidHeight =
                l.RaysBelowLayerMaskPlatforms & ~l.MidHeightOneWayPlatformMask;
        }

        private void SetRaysBelowLayerMaskPlatformsToPlatformsWithoutHeight()
        {
            l.RaysBelowLayerMaskPlatforms = l.RaysBelowLayerMaskPlatformsWithoutMidHeight;
        }

        private void SetRaysBelowLayerMaskPlatformsToOneWayOrStairs()
        {
            l.RaysBelowLayerMaskPlatforms = (l.RaysBelowLayerMaskPlatforms & ~l.OneWayPlatformMask) | l.StairsMask;
        }

        private void SetRaysBelowLayerMaskPlatformsToOneWay()
        {
            l.RaysBelowLayerMaskPlatforms &= ~l.OneWayPlatformMask;
        }

        private void SetSavedBelowLayerToStandingOnLastFrameLayer()
        {
            l.SavedBelowLayer = l.StandingOnLastFrameLayer;
        }

        #endregion

        #endregion

        #region properties

        public LayerMaskRuntimeData RuntimeData => l.RuntimeData;

        #region public methods
        
        public async UniTaskVoid OnInitializeData()
        {
            InitializeData();
            await SetYieldOrSwitchToThreadPoolAsync();
        }

        public async UniTaskVoid OnInitializeModel()
        {
            InitializeModel();
            await SetYieldOrSwitchToThreadPoolAsync();
        }

        public void OnSetRaysBelowLayerMaskPlatforms()
        {
            SetRaysBelowLayerMaskPlatforms();
        }

        public void OnSetRaysBelowLayerMaskPlatformsWithoutOneWay()
        {
            SetRaysBelowLayerMaskPlatformsWithoutOneWay();
        }

        public void OnSetRaysBelowLayerMaskPlatformsWithoutMidHeight()
        {
            SetRaysBelowLayerMaskPlatformsWithoutMidHeight();
        }

        public void OnSetRaysBelowLayerMaskPlatformsToPlatformsWithoutHeight()
        {
            SetRaysBelowLayerMaskPlatformsToPlatformsWithoutHeight();
        }

        public void OnSetRaysBelowLayerMaskPlatformsToOneWayOrStairs()
        {
            SetRaysBelowLayerMaskPlatformsToOneWayOrStairs();
        }

        public void OnSetRaysBelowLayerMaskPlatformsToOneWay()
        {
            SetRaysBelowLayerMaskPlatformsToOneWay();
        }

        public void OnSetSavedBelowLayerToStandingOnLastFrameLayer()
        {
            SetSavedBelowLayerToStandingOnLastFrameLayer();
        }

        #endregion

        #endregion
    }
}