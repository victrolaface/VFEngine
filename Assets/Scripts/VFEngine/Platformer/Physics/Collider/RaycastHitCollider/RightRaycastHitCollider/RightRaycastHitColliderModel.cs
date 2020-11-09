﻿using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using VFEngine.Tools;
using UniTaskExtensions = VFEngine.Tools.UniTaskExtensions;

namespace VFEngine.Platformer.Physics.Collider.RaycastHitCollider.RightRaycastHitCollider
{
    using static RaycastHitColliderModel;
    using static UniTaskExtensions;
    using static MathsExtensions;
    using static ScriptableObjectExtensions;

    [CreateAssetMenu(fileName = "RightRaycastHitColliderModel", menuName = PlatformerRightRaycastHitColliderModelPath,
        order = 0)]
    public class RightRaycastHitColliderModel : ScriptableObject, IModel
    {
        #region fields

        #region dependencies

        [LabelText("Right Raycast Hit Collider Data")] [SerializeField]
        private RightRaycastHitColliderData r;

        #endregion

        #region private methods

        private void Initialize()
        {
            InitializeModel();
        }

        private void InitializeModel()
        {
            r.RightHitsStorageLength = r.RightHitsStorage.Length;
            InitializeRightHitsStorage();
            InitializeCurrentRightHitsStorageIndex();
            ResetState();
        }

        private void InitializeRightHitsStorage()
        {
            r.RightHitsStorage = new RaycastHit2D[r.NumberOfHorizontalRaysPerSide];
        }

        private void InitializeCurrentRightHitsStorageIndex()
        {
            r.CurrentRightHitsStorageIndex = 0;
        }

        private void SetRightRaycastHitConnected()
        {
            r.RightHitConnected = true;
        }

        private void SetRightDistanceToRightCollider()
        {
            r.distanceToRightCollider = r.CurrentRightHitAngle;
        }

        private void SetRightRaycastHitMissed()
        {
            r.RightHitConnected = false;
        }

        private void SetCurrentRightHitsStorage()
        {
            r.RightHitsStorage[r.CurrentRightHitsStorageIndex] = r.CurrentRightRaycast;
        }

        private void SetCurrentRightHitAngle()
        {
            r.CurrentRightHitAngle =
                OnSetRaycastHitAngle(r.RightHitsStorage[r.CurrentRightHitsStorageIndex].normal, r.Transform);
        }

        private void SetIsCollidingRight()
        {
            r.IsCollidingRight = true;
        }

        private void SetRightCurrentWallCollider()
        {
            r.currentRightWallCollider = r.CurrentRightHitCollider.gameObject;
        }

        private void SetCurrentWallColliderNull()
        {
            r.currentRightWallCollider = null;
        }

        private void AddToCurrentRightHitsStorageIndex()
        {
            r.CurrentRightHitsStorageIndex++;
        }

        private void SetCurrentRightHitDistance()
        {
            r.CurrentRightHitDistance = r.RightHitsStorage[r.CurrentRightHitsStorageIndex].distance;
        }

        private void SetCurrentRightHitCollider()
        {
            r.CurrentRightHitCollider = r.RightHitsStorage[r.CurrentRightHitsStorageIndex].collider;
        }

        private void SetCurrentRightLateralSlopeAngle()
        {
            r.rightLateralSlopeAngle = r.CurrentRightHitAngle;
        }

        private void SetRightFailedSlopeAngle()
        {
            r.passedRightSlopeAngle = false;
        }

        private void SetCurrentDistanceBetweenRightHitAndRaycastOrigin()
        {
            r.DistanceBetweenRightHitAndRaycastOrigin = DistanceBetweenPointAndLine(
                r.RightHitsStorage[r.CurrentRightHitsStorageIndex].point, r.RightRaycastFromBottomOrigin,
                r.RightRaycastToTopOrigin);
        }

        private void ResetState()
        {
            SetRightRaycastHitMissed();
            SetRightFailedSlopeAngle();
            SetCurrentWallColliderNull();
            r.IsCollidingRight = false;
            r.CurrentRightHitCollider = null;
            r.CurrentRightHitAngle = 0f;
            r.rightLateralSlopeAngle = 0f;
            r.distanceToRightCollider = -1f;
        }

        #endregion

        #endregion

        #region properties

        #region public methods

        public void OnInitializeRightHitsStorage()
        {
            InitializeRightHitsStorage();
        }

        public void OnInitializeCurrentRightHitsStorageIndex()
        {
            InitializeCurrentRightHitsStorageIndex();
        }

        public void OnSetRightRaycastHitConnected()
        {
            SetRightRaycastHitConnected();
        }

        public void OnSetRightRaycastHitMissed()
        {
            SetRightRaycastHitMissed();
        }

        public void OnSetCurrentRightHitsStorage()
        {
            SetCurrentRightHitsStorage();
        }

        public void OnSetCurrentRightHitAngle()
        {
            SetCurrentRightHitAngle();
        }

        public void OnSetIsCollidingRight()
        {
            SetIsCollidingRight();
        }

        public void OnSetRightDistanceToRightCollider()
        {
            SetRightDistanceToRightCollider();
        }

        public void OnSetRightCurrentWallCollider()
        {
            SetRightCurrentWallCollider();
        }

        public void OnAddToCurrentRightHitsStorageIndex()
        {
            AddToCurrentRightHitsStorageIndex();
        }

        public void OnSetCurrentRightHitDistance()
        {
            SetCurrentRightHitDistance();
        }

        public void OnSetCurrentRightHitCollider()
        {
            SetCurrentRightHitCollider();
        }

        public void OnSetCurrentRightLateralSlopeAngle()
        {
            SetCurrentRightLateralSlopeAngle();
        }

        public void OnSetRightFailedSlopeAngle()
        {
            SetRightFailedSlopeAngle();
        }

        public void OnSetCurrentDistanceBetweenRightHitAndRaycastOrigin()
        {
            SetCurrentDistanceBetweenRightHitAndRaycastOrigin();
        }

        public void OnSetCurrentWallColliderNull()
        {
            SetCurrentWallColliderNull();
        }

        public void OnResetState()
        {
            ResetState();
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