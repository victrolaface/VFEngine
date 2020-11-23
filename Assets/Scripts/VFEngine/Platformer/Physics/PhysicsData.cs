﻿using UnityEngine;
using VFEngine.Platformer.Event.Boxcast.SafetyBoxcast;
using VFEngine.Platformer.Event.Raycast;
using VFEngine.Platformer.Event.Raycast.StickyRaycast.LeftStickyRaycast;
using VFEngine.Platformer.Event.Raycast.StickyRaycast.RightStickyRaycast;
using VFEngine.Platformer.Event.Raycast.UpRaycast;
using VFEngine.Platformer.Physics.Collider.RaycastHitCollider;
using VFEngine.Platformer.Physics.Collider.RaycastHitCollider.DownRaycastHitCollider;
using VFEngine.Platformer.Physics.Collider.RaycastHitCollider.LeftRaycastHitCollider;
using VFEngine.Platformer.Physics.Collider.RaycastHitCollider.RightRaycastHitCollider;
using VFEngine.Platformer.Physics.Collider.RaycastHitCollider.StickyRaycastHitCollider;
using VFEngine.Tools;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable RedundantAssignment
namespace VFEngine.Platformer.Physics
{
    using static ScriptableObjectExtensions;

    public class PhysicsData
    {
        #region fields

        #region dependencies

        #endregion

        private const string PhPath = "Physics/";
        private static readonly string ModelAssetPath = $"{PhPath}PhysicsModel.asset";

        #endregion

        #region properties

        #region dependencies

        public PlatformerRuntimeData PlatformerRuntimeData { get; set; }
        public RaycastRuntimeData RaycastRuntimeData { get; set; }
        public UpRaycastRuntimeData UpRaycastRuntimeData { get; set; }
        public LeftStickyRaycastRuntimeData LeftStickyRaycastRuntimeData { get; set; }
        public RightStickyRaycastRuntimeData RightStickyRaycastRuntimeData { get; set; }
        public SafetyBoxcastRuntimeData SafetyBoxcastRuntimeData { get; set; }
        public RaycastHitColliderRuntimeData RaycastHitColliderRuntimeData { get; set; }
        public LeftRaycastHitColliderRuntimeData LeftRaycastHitColliderRuntimeData { get; set; }
        public RightRaycastHitColliderRuntimeData RightRaycastHitColliderRuntimeData { get; set; }
        public DownRaycastHitColliderRuntimeData DownRaycastHitColliderRuntimeData { get; set; }
        public StickyRaycastHitColliderRuntimeData StickyRaycastHitColliderRuntimeData { get; set; }
        public GameObject Character { get; set; }
        public Transform Transform => Character.transform;
        public bool HasTransform => Transform;
        public PhysicsSettings Settings { get; set; }
        public bool HasSettings => Settings;
        public float Physics2DPushForceSetting => Settings.physics2DPushForce;
        public bool Physics2DInteractionControlSetting => Settings.physics2DInteractionControl;
        public Vector2 MaximumVelocitySetting => Settings.maximumVelocity;
        public AnimationCurve SlopeAngleSpeedFactorSetting => Settings.slopeAngleSpeedFactor;
        public bool SafetyBoxcastControlSetting => Settings.safetyBoxcastControl;
        public float MaximumSlopeAngleSetting => Settings.maximumSlopeAngle;
        public bool StickToSlopesControlSetting => Settings.stickToSlopeControl;
        public bool SafeSetTransformControlSetting => Settings.safeSetTransformControl;
        public bool DisplayWarningsControlSetting => Settings.displayWarningsControl;
        public bool AutomaticGravityControlSetting => Settings.automaticGravityControl;
        public float AscentMultiplierSetting => Settings.ascentMultiplier;
        public float FallMultiplierSetting => Settings.fallMultiplier;
        public float GravitySetting => Settings.gravity;
        public float MovingPlatformCurrentGravity { get; set; }
        public Vector2 MovingPlatformCurrentSpeed { get; set; }
        public float BoundsWidth { get; set; }
        public float RayOffset { get; set; }
        public float CurrentDownHitSmallestDistance { get; set; }
        public float BoundsHeight { get; set; }
        public RaycastHit2D SafetyBoxcastHit { get; set; }
        public float LeftStickyRaycastOriginY { get; set; }
        public float RightStickyRaycastOriginY { get; set; }
        public RaycastHit2D LeftStickyRaycastHit { get; set; }
        public RaycastHit2D RightStickyRaycastHit { get; set; }
        public float UpRaycastSmallestDistance { get; set; }
        public float DistanceBetweenRightHitAndRaycastOrigin { get; set; }
        public float DistanceBetweenLeftHitAndRaycastOrigin { get; set; }
        public float BelowSlopeAngle { get; set; }
        public RaycastHitColliderContactList ContactList { get; set; }

        #endregion

        public PhysicsController Controller { get; set; }
        public PhysicsRuntimeData RuntimeData { get; set; }
        public AnimationCurve SlopeAngleSpeedFactor { get; set; }
        public float CurrentVerticalSpeedFactor { get; set; }
        public bool Physics2DInteractionControl { get; set; }
        public bool SafetyBoxcastControl { get; set; }
        public bool StickToSlopesControl { get; set; }
        public bool SafeSetTransformControl { get; set; }
        public bool IsJumping { get; set; }
        public bool IsFalling { get; set; }
        public bool GravityActive { get; set; }
        public int HorizontalMovementDirection { get; set; }
        public float Physics2DPushForce { get; set; }
        public float MaximumSlopeAngle { get; set; }
        public float FallSlowFactor { get; set; }
        public float SmallValue { get; set; }
        public float Gravity { get; set; }
        public float MovementDirectionThreshold { get; set; }
        public Vector2 MaximumVelocity { get; set; }
        public Vector2 Speed { get; set; }
        public Vector2 NewPosition { get; set; }

        public float NewPositionY
        {
            set => value = NewPosition.y;
        }

        public float NewPositionX
        {
            set => value = NewPosition.x;
        }

        public Vector2 ExternalForce { get; set; }

        public float ExternalForceY
        {
            set => value = ExternalForce.y;
        }

        public bool DisplayWarningsControl { get; set; }
        public bool AutomaticGravityControl { get; set; }
        public float AscentMultiplier { get; set; }
        public float FallMultiplier { get; set; }
        public Vector2 WorldSpeed { get; set; } = Vector2.zero;
        public Vector2 ForcesApplied { get; set; } = Vector2.zero;
        public Rigidbody2D CurrentHitRigidBody { get; set; }
        public bool CurrentHitRigidBodyCanBePushed { get; set; }
        public Vector2 CurrentPushDirection { get; set; } = Vector2.zero;
        public float CurrentGravity { get; set; }
        public int StoredHorizontalMovementDirection { get; set; }

        public float SpeedX
        {
            get => Speed.x;
            set => value = Speed.x;
        }

        public float SpeedY
        {
            get => Speed.y;
            set => value = Speed.y;
        }

        public static readonly string PhysicsModelPath = $"{PlatformerScriptableObjectsPath}{ModelAssetPath}";

        /*public void SetRuntimeData(PlatformerRuntimeData platformerRuntimeData, RaycastRuntimeData raycastRuntimeData,
            UpRaycastRuntimeData upRaycastRuntimeData, LeftStickyRaycastRuntimeData leftStickyRaycastRuntimeData,
            RightStickyRaycastRuntimeData rightStickyRaycastRuntimeData,
            SafetyBoxcastRuntimeData safetyBoxcastRuntimeData,
            RaycastHitColliderRuntimeData raycastHitColliderRuntimeData,
            LeftRaycastHitColliderRuntimeData leftRaycastHitColliderRuntimeData,
            RightRaycastHitColliderRuntimeData rightRaycastHitColliderRuntimeData,
            DownRaycastHitColliderRuntimeData downRaycastHitColliderRuntimeData,
            StickyRaycastHitColliderRuntimeData stickyRaycastHitColliderRuntimeData)
        {
            PlatformerRuntimeData = platformerRuntimeData;
            RaycastRuntimeData = raycastRuntimeData;
            UpRaycastRuntimeData = upRaycastRuntimeData;
            LeftStickyRaycastRuntimeData = leftStickyRaycastRuntimeData;
            RightStickyRaycastRuntimeData = rightStickyRaycastRuntimeData;
            SafetyBoxcastRuntimeData = safetyBoxcastRuntimeData;
            RaycastHitColliderRuntimeData = raycastHitColliderRuntimeData;
            LeftRaycastHitColliderRuntimeData = leftRaycastHitColliderRuntimeData;
            RightRaycastHitColliderRuntimeData = rightRaycastHitColliderRuntimeData;
            DownRaycastHitColliderRuntimeData = downRaycastHitColliderRuntimeData;
            StickyRaycastHitColliderRuntimeData = stickyRaycastHitColliderRuntimeData;
        }*/

        #endregion
    }
}