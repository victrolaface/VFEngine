﻿using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using VFEngine.Tools;
using UniTaskExtensions = VFEngine.Tools.UniTaskExtensions;

namespace VFEngine.Platformer.Physics.Collider.RaycastHitCollider.StickyRaycastHitCollider
{
    using static UniTaskExtensions;
    using static ScriptableObjectExtensions;

    [CreateAssetMenu(fileName = "StickyRaycastHitColliderModel", menuName = PlatformerStickyRaycastHitColliderModelPath,
        order = 0)]
    public class StickyRaycastHitColliderModel : ScriptableObject, IModel
    {
        #region fields

        #region dependencies

        [LabelText("Sticky Raycast Hit Collider Data")] [SerializeField]
        private StickyRaycastHitColliderData s;

        #endregion

        #region private methods

        private void Initialize()
        {
            InitializeModel();
        }

        private void InitializeModel()
        {
            InitializeBelowSlopeAngle();
        }

        private void ResetState()
        {
            InitializeBelowSlopeAngle();
        }

        private void InitializeBelowSlopeAngle()
        {
            s.belowSlopeAngle = 0f;
        }

        private void SetBelowSlopeAngleToBelowSlopeAngleLeft()
        {
            s.belowSlopeAngle = s.BelowSlopeAngleLeft;
        }

        private void SetBelowSlopeAngleToBelowSlopeAngleRight()
        {
            s.belowSlopeAngle = s.BelowSlopeAngleRight;
        }

        #endregion

        #endregion

        #region properties

        #region public methods

        public void OnResetState()
        {
            ResetState();
        }

        public void OnInitializeBelowSlopeAngle()
        {
            InitializeBelowSlopeAngle();
        }

        public void OnSetBelowSlopeAngleToBelowSlopeAngleLeft()
        {
            SetBelowSlopeAngleToBelowSlopeAngleLeft();
        }

        public void OnSetBelowSlopeAngleToBelowSlopeAngleRight()
        {
            SetBelowSlopeAngleToBelowSlopeAngleRight();
        }

        public async UniTaskVoid OnInitialize()
        {
            Initialize();
            await SetYieldOrSwitchToThreadPoolAsync();
        }

        #endregion

        #endregion
    }
}