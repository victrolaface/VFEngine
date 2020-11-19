﻿using Sirenix.OdinInspector;
using UnityEngine;
using VFEngine.Tools;

// ReSharper disable RedundantDefaultMemberInitializer
// ReSharper disable UnusedVariable
namespace VFEngine.Platformer.Physics
{
    using static DebugExtensions;
    using static Quaternion;
    using static Time;
    using static Mathf;
    using static Vector2;
    using static RigidbodyType2D;
    using static ScriptableObjectExtensions;

    [CreateAssetMenu(fileName = "PhysicsModel", menuName = PlatformerPhysicsModelPath, order = 0)]
    [InlineEditor]
    public class PhysicsModel : ScriptableObject, IModel
    {
        #region fields

        #region dependencies

        [LabelText("Physics Data")] [SerializeField]
        private PhysicsData p = null;

        #endregion

        #region private methods

        private void Initialize()
        {
            InitializeData();
            InitializeModel();
            if (p.DisplayWarningsControl) GetWarningMessages();
        }

        private void InitializeData()
        {
            p.RuntimeData = p.Character.GetComponentNoAllocation<PlatformerController>().RuntimeData;
            p.Physics2DPushForce = p.Physics2DPushForceSetting;
            p.Physics2DInteractionControl = p.Physics2DInteractionControlSetting;
            p.MaximumVelocity = p.MaximumVelocitySetting;
            p.SafetyBoxcastControl = p.SafetyBoxcastControlSetting;
            p.MaximumSlopeAngle = p.MaximumSlopeAngleSetting;
            p.StickToSlopesControl = p.StickToSlopesControlSetting;
            p.SafeSetTransformControl = p.SafeSetTransformControlSetting;
            p.SlopeAngleSpeedFactor = p.SlopeAngleSpeedFactorSetting;
            p.DisplayWarningsControl = p.DisplayWarningsControlSetting;
            p.AutomaticGravityControl = p.AutomaticGravityControlSetting;
            p.AscentMultiplier = p.AscentMultiplierSetting;
            p.FallMultiplier = p.FallMultiplierSetting;
            p.IsJumping = false;
            p.Gravity = p.GravitySetting;
            p.FallSlowFactor = 0f;
            p.SmallValue = 0.0001f;
            p.MovementDirectionThreshold = p.SmallValue;
            p.RuntimeData.SetPhysics(p.StickToSlopesControl, p.SafetyBoxcastControl, p.Physics2DInteractionControl,
                p.IsJumping, p.IsFalling, p.GravityActive, p.SafeSetTransformControl, p.HorizontalMovementDirection,
                p.FallSlowFactor, p.Physics2DPushForce, p.MaximumSlopeAngle, p.SmallValue, p.Gravity,
                p.MovementDirectionThreshold, p.CurrentVerticalSpeedFactor, p.Speed, p.MaximumVelocity, p.NewPosition,
                p.ExternalForce, p.Transform);
        }

        private void InitializeModel()
        {
            p.Transform = p.RuntimeData.platformer.Transform;
            p.MovingPlatformCurrentGravity = p.RuntimeData.downRaycastHitCollider.MovingPlatformCurrentGravity;
            p.MovingPlatformCurrentSpeed = p.RuntimeData.downRaycastHitCollider.MovingPlatformCurrentSpeed;
            p.BoundsWidth = p.RuntimeData.raycast.BoundsWidth;
            p.RayOffset = p.RuntimeData.raycast.RayOffset;
            p.CurrentDownHitSmallestDistance = p.RuntimeData.downRaycastHitCollider.CurrentDownHitSmallestDistance;
            p.BoundsHeight = p.RuntimeData.raycast.BoundsHeight;
            p.SafetyBoxcastHit = p.RuntimeData.safetyBoxcast.SafetyBoxcastHit;
            p.LeftStickyRaycastOriginY = p.RuntimeData.leftStickyRaycast.LeftStickyRaycastOriginY;
            p.RightStickyRaycastOriginY = p.RuntimeData.rightStickyRaycast.RightStickyRaycastOriginY;
            p.LeftStickyRaycastHit = p.RuntimeData.leftStickyRaycast.LeftStickyRaycastHit;
            p.RightStickyRaycastHit = p.RuntimeData.rightStickyRaycast.RightStickyRaycastHit;
            p.UpRaycastSmallestDistance = p.RuntimeData.upRaycast.UpRaycastSmallestDistance;
            p.DistanceBetweenLeftHitAndRaycastOrigin =
                p.RuntimeData.leftRaycastHitCollider.DistanceBetweenLeftHitAndRaycastOrigin;
            p.DistanceBetweenRightHitAndRaycastOrigin =
                p.RuntimeData.rightRaycastHitCollider.DistanceBetweenRightHitAndRaycastOrigin;
            p.BelowSlopeAngle = p.RuntimeData.stickyRaycastHitCollider.BelowSlopeAngle;
            p.ContactList = p.RuntimeData.raycastHitCollider.ContactList;
            p.CurrentHitRigidBodyCanBePushed = p.CurrentHitRigidBody != null && !p.CurrentHitRigidBody.isKinematic &&
                                               p.CurrentHitRigidBody.bodyType != Static;
            if (p.AutomaticGravityControl /*&& !p.HasGravityController*/) p.Transform.rotation = identity;
            ResetState();
        }

        private void GetWarningMessages()
        {
            const string ph = "Physics";
            var settings = $"{ph} Settings";
            var warningMessage = "";
            var warningMessageCount = 0;
            if (!p.HasSettings) warningMessage += FieldString($"{settings}", $"{settings}");
            //if (!p.HasGravityController) warningMessage += FieldParentString("Gravity Controller", $"{settings}");
            if (!p.HasTransform) warningMessage += FieldParentString("Transform", $"{settings}");
            DebugLogWarning(warningMessageCount, warningMessage);

            string FieldString(string field, string scriptableObject)
            {
                AddWarningMessageCount();
                return FieldMessage(field, scriptableObject);
            }

            string FieldParentString(string field, string scriptableObject)
            {
                AddWarningMessageCount();
                return FieldParentMessage(field, scriptableObject);
            }

            void AddWarningMessageCount()
            {
                warningMessageCount++;
            }
        }

        private void SetCurrentGravity()
        {
            p.CurrentGravity = p.Gravity;
        }

        private void ApplyAscentMultiplierToCurrentGravity()
        {
            p.CurrentGravity /= p.AscentMultiplier;
        }

        private void ApplyFallMultiplierToCurrentGravity()
        {
            p.CurrentGravity *= p.FallMultiplier;
        }

        private void ApplyGravityToVerticalSpeed()
        {
            var gravity = p.CurrentGravity + p.MovingPlatformCurrentGravity;
            p.SpeedY += gravity * deltaTime;
        }

        private void ApplyFallSlowFactorToVerticalSpeed()
        {
            p.SpeedY *= p.FallSlowFactor;
        }

        private void SetNewPosition()
        {
            p.NewPosition = p.Speed * deltaTime;
        }

        private void ResetState()
        {
            p.GravityActive = true;
        }

        private void TranslatePlatformSpeedToTransform()
        {
            p.Transform.Translate(p.MovingPlatformCurrentSpeed * deltaTime);
        }

        private void DisableGravity()
        {
            p.GravityActive = false;
        }

        private void ApplyMovingPlatformSpeedToNewPosition()
        {
            p.NewPositionY = p.MovingPlatformCurrentSpeed.y * deltaTime;
        }

        private void StopHorizontalSpeedOnPlatformTest()
        {
            p.Speed = -p.NewPosition / deltaTime;
            p.SpeedX = -p.SpeedX;
        }

        private void SetForcesApplied()
        {
            p.ForcesApplied = p.Speed;
        }

        private void SetHorizontalMovementDirectionToStored()
        {
            p.HorizontalMovementDirection = p.StoredHorizontalMovementDirection;
        }

        private void SetNegativeHorizontalMovementDirection()
        {
            p.HorizontalMovementDirection = -1;
        }

        private void SetPositiveHorizontalMovementDirection()
        {
            p.HorizontalMovementDirection = 1;
        }

        private void ApplyPlatformSpeedToHorizontalMovementDirection()
        {
            p.HorizontalMovementDirection = (int) Sign(p.MovingPlatformCurrentSpeed.x);
        }

        private void SetStoredHorizontalMovementDirection()
        {
            p.StoredHorizontalMovementDirection = p.HorizontalMovementDirection;
        }

        private void SetNewPositiveHorizontalPosition()
        {
            p.NewPositionX = SetNewHorizontalPosition(true, p.DistanceBetweenRightHitAndRaycastOrigin, p.BoundsWidth,
                p.RayOffset);
        }

        private void SetNewNegativeHorizontalPosition()
        {
            p.NewPositionX = SetNewHorizontalPosition(false, p.DistanceBetweenLeftHitAndRaycastOrigin, p.BoundsWidth,
                p.RayOffset);
        }

        private static float SetNewHorizontalPosition(bool positiveDirection, float distance, float width, float offset)
        {
            var positiveX = -distance + width / 2 + offset * 2;
            var negativeX = distance - width / 2 - offset * 2;
            return positiveDirection ? positiveX : negativeX;
        }

        private void StopHorizontalSpeed()
        {
            p.SpeedX = 0;
        }

        private void StopNewHorizontalPosition()
        {
            p.NewPositionX = 0;
        }

        private void SetIsFalling()
        {
            p.IsFalling = true;
        }

        private void SetIsNotFalling()
        {
            p.IsFalling = false;
        }

        private void ApplySpeedToHorizontalNewPosition()
        {
            p.NewPositionY = p.SpeedY * deltaTime;
        }

        private void ApplyHalfBoundsHeightAndRayOffsetToNegativeVerticalNewPosition()
        {
            p.NewPositionY = -p.CurrentDownHitSmallestDistance + p.BoundsHeight / 2 + p.RayOffset;
        }

        private void ApplySpeedToVerticalNewPosition()
        {
            p.NewPositionY = p.NewPosition.y + p.Speed.y * deltaTime;
        }

        private void StopNewVerticalPosition()
        {
            p.NewPositionY = 0;
        }

        private void SetGravityActive()
        {
            p.GravityActive = true;
        }

        private void ApplySafetyBoxcastAndRightStickyRaycastToNewVerticalPosition()
        {
            p.NewPositionY = SetNewVerticalPositionWithSafetyAndStickyRaycasts(p.SafetyBoxcastHit.point.y,
                p.RightStickyRaycastOriginY, p.BoundsHeight);
        }

        private void ApplyLeftStickyRaycastToNewVerticalPosition()
        {
            p.NewPositionY = SetNewVerticalPositionWithSafetyAndStickyRaycasts(p.LeftStickyRaycastHit.point.y,
                p.LeftStickyRaycastOriginY, p.BoundsHeight);
        }

        private void ApplyRightStickyRaycastToNewVerticalPosition()
        {
            p.NewPositionY = SetNewVerticalPositionWithSafetyAndStickyRaycasts(p.RightStickyRaycastHit.point.y,
                p.RightStickyRaycastOriginY, p.BoundsHeight);
        }

        private static float SetNewVerticalPositionWithSafetyAndStickyRaycasts(float pointY, float originY,
            float boundsHeight)
        {
            return -Abs(pointY - originY) + boundsHeight / 2;
        }

        private void StopVerticalSpeed()
        {
            p.Speed = new Vector2(p.SpeedX, 0f);
        }

        private void SetNewVerticalPositionWithUpRaycastSmallestDistanceAndBoundsHeight()
        {
            p.NewPositionY = p.UpRaycastSmallestDistance - p.BoundsHeight / 2;
        }

        private void StopVerticalForce()
        {
            p.SpeedY = 0f;
            p.ExternalForceY = 0f;
        }

        private void StopNewPosition()
        {
            p.NewPosition = zero;
        }

        private void SetNewSpeed()
        {
            p.Speed = p.NewPosition / deltaTime;
        }

        private void ApplySlopeAngleSpeedFactorToHorizontalSpeed()
        {
            p.SpeedX *= p.SlopeAngleSpeedFactor.Evaluate(Abs(p.BelowSlopeAngle) * Sign(p.Speed.y));
        }

        private void ClampSpeedToMaxVelocity()
        {
            p.SpeedX = Clamp(p.Speed.x, -p.MaximumVelocity.x, p.MaximumVelocity.x);
            p.SpeedY = Clamp(p.Speed.y, -p.MaximumVelocity.y, p.MaximumVelocity.y);
        }

        private void ContactListHit()
        {
            if (!p.Physics2DInteractionControl) return;
            foreach (var hit in p.ContactList.List)
            {
                p.CurrentHitRigidBody = hit.collider.attachedRigidbody;
                if (!p.CurrentHitRigidBodyCanBePushed) return;
                p.CurrentPushDirection = new Vector2(p.ExternalForce.x, 0);
                p.CurrentHitRigidBody.velocity = p.CurrentPushDirection.normalized * p.Physics2DPushForce;
            }
        }

        private void StopExternalForce()
        {
            p.ExternalForce = zero;
        }

        private void SetWorldSpeedToSpeed()
        {
            p.WorldSpeed = p.Speed;
        }

        private void SlowFall()
        {
            p.FallSlowFactor = p.CurrentVerticalSpeedFactor;
        }

        #endregion

        #endregion

        #region properties

        #region public methods

        public void OnInitialize()
        {
            Initialize();
        }

        public void OnSetCurrentCurrentGravity()
        {
            SetCurrentGravity();
        }

        public void OnApplyAscentMultiplierToCurrentGravity()
        {
            ApplyAscentMultiplierToCurrentGravity();
        }

        public void OnApplyFallMultiplierToCurrentGravity()
        {
            ApplyFallMultiplierToCurrentGravity();
        }

        public void OnApplyGravityToVerticalSpeed()
        {
            ApplyGravityToVerticalSpeed();
        }

        public void OnApplyFallSlowFactorToVerticalSpeed()
        {
            ApplyFallSlowFactorToVerticalSpeed();
        }

        public void OnSetNewPosition()
        {
            SetNewPosition();
        }

        public void OnResetState()
        {
            ResetState();
        }

        public void OnTranslatePlatformSpeedToTransform()
        {
            TranslatePlatformSpeedToTransform();
        }

        public void OnDisableGravity()
        {
            DisableGravity();
        }

        public void OnApplyMovingPlatformSpeedToNewPosition()
        {
            ApplyMovingPlatformSpeedToNewPosition();
        }

        public void OnStopHorizontalSpeedOnPlatformTest()
        {
            StopHorizontalSpeedOnPlatformTest();
        }

        public void OnSetForcesApplied()
        {
            SetForcesApplied();
        }

        public void OnSetHorizontalMovementDirectionToStored()
        {
            SetHorizontalMovementDirectionToStored();
        }

        public void OnSetNegativeHorizontalMovementDirection()
        {
            SetNegativeHorizontalMovementDirection();
        }

        public void OnSetPositiveHorizontalMovementDirection()
        {
            SetPositiveHorizontalMovementDirection();
        }

        public void OnApplyPlatformSpeedToHorizontalMovementDirection()
        {
            ApplyPlatformSpeedToHorizontalMovementDirection();
        }

        public void OnSetStoredHorizontalMovementDirection()
        {
            SetStoredHorizontalMovementDirection();
        }

        public void OnSetNewPositiveHorizontalPosition()
        {
            SetNewPositiveHorizontalPosition();
        }

        public void OnSetNewNegativeHorizontalPosition()
        {
            SetNewNegativeHorizontalPosition();
        }

        public void OnStopHorizontalSpeed()
        {
            StopHorizontalSpeed();
        }

        public void OnStopNewHorizontalPosition()
        {
            StopNewHorizontalPosition();
        }

        public void OnSetIsFalling()
        {
            SetIsFalling();
        }

        public void OnSetIsNotFalling()
        {
            SetIsNotFalling();
        }

        public void OnApplySpeedToHorizontalNewPosition()
        {
            ApplySpeedToHorizontalNewPosition();
        }

        public void OnApplyHalfBoundsHeightAndRayOffsetToNegativeVerticalNewPosition()
        {
            ApplyHalfBoundsHeightAndRayOffsetToNegativeVerticalNewPosition();
        }

        public void OnApplySpeedToVerticalNewPosition()
        {
            ApplySpeedToVerticalNewPosition();
        }

        public void OnStopNewVerticalPosition()
        {
            StopNewVerticalPosition();
        }

        public void OnSetGravityActive()
        {
            SetGravityActive();
        }

        public void OnApplySafetyBoxcastAndRightStickyRaycastToNewVerticalPosition()
        {
            ApplySafetyBoxcastAndRightStickyRaycastToNewVerticalPosition();
        }

        public void OnApplyLeftStickyRaycastToNewVerticalPosition()
        {
            ApplyLeftStickyRaycastToNewVerticalPosition();
        }

        public void OnApplyRightStickyRaycastToNewVerticalPosition()
        {
            ApplyRightStickyRaycastToNewVerticalPosition();
        }

        public void OnStopVerticalSpeed()
        {
            StopVerticalSpeed();
        }

        public void OnSetNewVerticalPositionWithUpRaycastSmallestDistanceAndBoundsHeight()
        {
            SetNewVerticalPositionWithUpRaycastSmallestDistanceAndBoundsHeight();
        }

        public void OnStopVerticalForce()
        {
            StopVerticalForce();
        }

        public void OnStopNewPosition()
        {
            StopNewPosition();
        }

        public void OnSetNewSpeed()
        {
            SetNewSpeed();
        }

        public void OnApplySlopeAngleSpeedFactorToHorizontalSpeed()
        {
            ApplySlopeAngleSpeedFactorToHorizontalSpeed();
        }

        public void OnClampSpeedToMaxVelocity()
        {
            ClampSpeedToMaxVelocity();
        }

        public void OnContactListHit()
        {
            ContactListHit();
        }

        public void OnStopExternalForce()
        {
            StopExternalForce();
        }

        public void OnSetWorldSpeedToSpeed()
        {
            SetWorldSpeedToSpeed();
        }

        public void OnSlowFall()
        {
            SlowFall();
        }

        #endregion

        #endregion
    }
}