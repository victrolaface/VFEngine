﻿using UnityEngine;
using VFEngine.Platformer.Event.Raycast;
using VFEngine.Platformer.Event.Raycast.LeftRaycast;
using VFEngine.Tools;

// ReSharper disable ConvertToAutoPropertyWithPrivateSetter
namespace VFEngine.Platformer.Physics.Collider.RaycastHitCollider.LeftRaycastHitCollider
{
    using static MathsExtensions;
    using static Mathf;
    using static Vector2;
    using static RaycastDirection;

    public class LeftRaycastHitColliderController : MonoBehaviour, IController
    {
        #region fields

        #region dependencies

        private PhysicsController physicsController;
        private RaycastController raycastController;
        private LeftRaycastController leftRaycastController;
        private RaycastHitColliderController raycastHitColliderController;
        private LeftRaycastHitColliderData l;
        private PhysicsData physics;
        private RaycastData raycast;
        private LeftRaycastData leftRaycast;
        private RaycastHitColliderData raycastHitCollider;

        #endregion

        private bool IncorrectHitsStorage => l.HitsStorage.Length != raycast.NumberOfHorizontalRaysPerSide;
        private bool CastingLeft => raycast.CurrentRaycastDirection == Left;
        private bool MovingLeft => physics.HorizontalMovementDirection == -1;
        private bool MovementIsRayDirection => CastingLeft && MovingLeft;

        #region private methods

        #region initialization

        private void Awake()
        {
            SetControllers();
            InitializeData();
        }

        private void SetControllers()
        {
            physicsController = GetComponent<PhysicsController>();
            raycastController = GetComponent<RaycastController>();
            leftRaycastController = GetComponent<LeftRaycastController>();
            raycastHitColliderController = GetComponent<RaycastHitColliderController>();
        }

        private void InitializeData()
        {
            l = new LeftRaycastHitColliderData {CurrentWallCollider = null};
        }

        private void Start()
        {
            SetDependencies();
            Initialize();
        }

        private void SetDependencies()
        {
            physics = physicsController.Data;
            raycast = raycastController.Data;
            leftRaycast = leftRaycastController.Data;
            raycastHitCollider = raycastHitColliderController.Data;
        }

        private void Initialize()
        {
            InitializeHitsStorage();
            InitializeCurrentHitsStorage();
            InitializeCurrentHit();
            InitializeHitConnected();
            InitializeCurrentLeftHitCollider();
            InitializeHitIgnoredCollider();
            InitializeCurrentHitAngle();
            InitializeHitWall();
            ResetState();
        }

        private void InitializeHitsStorage()
        {
            l.HitsStorage = new RaycastHit2D[raycast.NumberOfHorizontalRaysPerSide];
        }

        private void InitializeCurrentHitsStorage()
        {
            l.HitsStorage[l.CurrentHitsStorageIndex] = leftRaycast.CurrentRaycastHit;
        }

        private void InitializeCurrentHit()
        {
            l.CurrentHit = l.HitsStorage[l.CurrentHitsStorageIndex];
        }

        private void InitializeHitConnected()
        {
            l.HitConnected = l.CurrentHit.distance > 0;
        }

        private void InitializeCurrentLeftHitCollider()
        {
            l.CurrentHitCollider = l.CurrentHit.collider;
        }

        private void InitializeHitIgnoredCollider()
        {
            l.HitIgnoredCollider = l.CurrentHitCollider == raycastHitCollider.IgnoredCollider;
        }

        private void InitializeCurrentHitAngle()
        {
            l.CurrentHitAngle = Abs(Angle(l.CurrentHit.normal, physics.Transform.up));
        }

        private void InitializeHitWall()
        {
            l.HitWall = l.CurrentHitAngle > physics.MaximumSlopeAngle;
        }

        private void ResetState()
        {
            SetIsNotColliding();
            InitializeDistanceToCollider();
            SetFailedSlopeAngle();
            InitializeLateralSlopeAngle();
        }

        private void SetIsNotColliding()
        {
            l.IsColliding = false;
        }

        private void InitializeDistanceToCollider()
        {
            l.DistanceToCollider = -1;
        }

        private void SetFailedSlopeAngle()
        {
            l.PassedSlopeAngle = false;
        }

        private void InitializeLateralSlopeAngle()
        {
            l.LateralSlopeAngle = 0;
        }

        #endregion

        #region platformer

        private void PlatformerInitializeFrame()
        {
            SetCurrentWallColliderNull();
            ResetState();
        }

        private void PlatformerCastRays()
        {
            if (!CastingLeft) return;
            if (IncorrectHitsStorage) InitializeHitsStorage();
            InitializeCurrentHitsStorageIndex();
        }

        private void PlatformerCastCurrentRay()
        {
            if (!CastingLeft || !l.HitConnected || l.HitIgnoredCollider) return;
            if (MovementIsRayDirection) SetLateralSlopeAngle();
            if (!l.HitWall) return;
            SetIsColliding();
            SetDistanceToCollider();
            if (!MovementIsRayDirection || !CastingLeft) return;
            SetCurrentWallCollider();
            SetFailedSlopeAngle();
            SetDistanceBetweenHitAndRaycastOrigins();
        }

        private void PlatformerAddToCurrentHitsStorageIndex()
        {
            if (!CastingLeft) return;
            AddToCurrentHitsStorageIndex();
        }

        #endregion

        private void SetCurrentWallColliderNull()
        {
            l.CurrentWallCollider = null;
        }

        private void InitializeCurrentHitsStorageIndex()
        {
            l.CurrentHitsStorageIndex = 0;
        }

        private void SetLateralSlopeAngle()
        {
            l.LateralSlopeAngle = l.CurrentHitAngle;
        }

        private void SetIsColliding()
        {
            l.IsColliding = true;
        }

        private void SetDistanceToCollider()
        {
            l.DistanceToCollider = l.CurrentHit.distance;
        }

        private void SetCurrentWallCollider()
        {
            l.CurrentWallCollider = l.CurrentHitCollider.gameObject;
        }

        private void SetDistanceBetweenHitAndRaycastOrigins()
        {
            l.DistanceBetweenHitAndRaycastOrigins = DistanceBetweenPointAndLine(l.CurrentHit.point,
                leftRaycast.RaycastFromBottomOrigin, leftRaycast.RaycastToTopOrigin);
        }

        private void AddToCurrentHitsStorageIndex()
        {
            l.CurrentHitsStorageIndex++;
        }

        #endregion

        #endregion

        #region properties

        public LeftRaycastHitColliderData Data => l;

        #region public methods

        #region platformer

        public void OnPlatformerInitializeFrame()
        {
            PlatformerInitializeFrame();
        }

        public void OnPlatformerCastRays()
        {
            PlatformerCastRays();
        }

        public void OnPlatformerCastCurrentRay()
        {
            PlatformerCastCurrentRay();
        }

        public void OnPlatformerAddToCurrentHitsStorageIndex()
        {
            PlatformerAddToCurrentHitsStorageIndex();
        }

        #endregion

        #endregion

        #endregion
    }
}