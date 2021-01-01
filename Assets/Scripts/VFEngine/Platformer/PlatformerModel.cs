﻿using UnityEngine;
using VFEngine.Platformer.Event.Raycast;
using VFEngine.Platformer.Layer.Mask;
using VFEngine.Platformer.Physics;

namespace VFEngine.Platformer
{
    using static Mathf;
    using static Vector2;

    public class PlatformerModel
    {
        #region fields

        #region internal

        private PlatformerData Platformer { get; }
        private RaycastController Raycast { get; }
        private PhysicsController Physics { get; }
        private LayerMaskController LayerMask { get; }
        private RaycastData RaycastData => Raycast.Data;
        private RaycastCollision Collision => RaycastData.Collision;
        private PhysicsData PhysicsData => Physics.Data;
        private int VerticalRays => RaycastData.VerticalRays;

        #endregion

        #region private methods

        private void RunInternal()
        {
            InitializeFrame();
            SetGroundCollision();
            SetForces();
            SetSlopeBehavior();
            SetHorizontalCollision();
            SetVerticalCollision();
            OnSlopeChange();
        }

        private void InitializeFrame()
        {
            Raycast.OnPlatformerInitializeFrame();
            LayerMask.OnPlatformerInitializeFrame();
            Physics.OnPlatformerInitializeFrame();
            Platformer.OnInitializeFrame();
        }

        private RaycastHit2D Hit => RaycastData.Hit;
        private float IgnorePlatformsTime => Platformer.IgnorePlatformsTime;
        private bool SetDownHitAtOneWayPlatform => !Hit && IgnorePlatformsTime <= 0;
        private bool DownHitMissed => Hit.distance <= 0;
        private bool CastNextRayDown => !Hit;

        private void SetGroundCollision()
        {
            for (var i = 0; i < VerticalRays; i++)
            {
                Platformer.SetIndex(i);
                Raycast.OnPlatformerCastRaysDown();
                if (SetDownHitAtOneWayPlatform)
                {
                    Raycast.OnPlatformerSetDownHitAtOneWayPlatform();
                    if (DownHitMissed) continue;
                }

                if (CastNextRayDown) continue;
                Raycast.OnPlatformerDownHit();
                break;
            }
        }

        private bool IgnoreFriction => PhysicsData.IgnoreFriction;
        private bool OnSlope => RaycastData.Collision.OnSlope;
        private float GroundAngle => RaycastData.Collision.GroundAngle;
        private float MaximumSlopeAngle => PhysicsData.MaximumSlopeAngle;
        private bool FailedMaximumSlopeAngle => GroundAngle > MaximumSlopeAngle;
        private bool ExceededMaximumSlopeAngle => OnSlope && FailedMaximumSlopeAngle;
        private float MinimumWallAngle => PhysicsData.MinimumWallAngle;
        private bool OnAngle => GroundAngle < MinimumWallAngle;
        private Vector2 Speed => PhysicsData.Speed;
        private float HorizontalSpeed => Speed.x;
        private bool NoHorizontalSpeed => HorizontalSpeed == 0;
        private bool ApplyForcesToExternal => ExceededMaximumSlopeAngle && (OnAngle || NoHorizontalSpeed);
        private bool CannotSetForces => IgnoreFriction;

        private void SetForces()
        {
            if (CannotSetForces) return;
            Physics.OnPlatformerSetExternalForce();
            Physics.OnPlatformerApplyGravity();
            if (ApplyForcesToExternal) Physics.OnPlatformerApplyForcesToExternal();
        }

        private int HorizontalMovementDirection => PhysicsData.HorizontalMovementDirection;
        private Vector2 Movement => PhysicsData.Movement;
        private float HorizontalMovement => Movement.x;
        private bool HasHorizontalMovement => HorizontalMovement != 0;
        private float VerticalMovement => Movement.y;
        private bool NoVerticalMovement => VerticalMovement == 0;
        private bool NegativeVerticalMovement => VerticalMovement < 0;
        private bool NegativeOrNoVerticalMovement => NoVerticalMovement || NegativeVerticalMovement;
        private int GroundDirection => Collision.GroundDirection;
        private bool MovementIsGroundDirection => GroundDirection == HorizontalMovementDirection;
        private bool DescendSlope => NegativeOrNoVerticalMovement && OnSlope && MovementIsGroundDirection;
        private bool ClimbSlope => OnAngle;
        private bool CannotSetSlopeBehavior => !HasHorizontalMovement;

        private void SetSlopeBehavior()
        {
            if (CannotSetSlopeBehavior) return;
            if (DescendSlope) OnDescendSlope();
            else if (ClimbSlope) OnClimbSlope();
        }

        private void OnDescendSlope()
        {
            Physics.OnPlatformerDescendSlope();
            Raycast.OnPlatformerSlopeBehavior();
        }

        private void OnClimbSlope()
        {
            Physics.OnPlatformerClimbSlope();
            Raycast.OnPlatformerSlopeBehavior();
        }

        private bool CastToSides => HasHorizontalMovement;
        private int HorizontalRays => RaycastData.HorizontalRays;
        private int Index => Platformer.Index;
        private float SideAngle => Angle(Hit.normal, up);
        private bool OnSideAngle => SideAngle < MinimumWallAngle;
        private bool FirstSideRay => Index == 0;
        private bool FirstSideHitOnAngle => FirstSideRay && !OnSlope && OnSideAngle;
        private bool SideHitMissed => !Hit;
        private bool CastNextRayToSides => FirstSideRay && OnSlope || !FailedMaximumSlopeAngle || OnSideAngle;
        private bool SetVerticalMovement => OnSlope && OnAngle;
        private bool MetMinimumWallAngle => GroundAngle >= MinimumWallAngle;
        private bool GroundNotHorizontalMovementDirection => GroundDirection != HorizontalMovementDirection;
        private float VerticalSpeed => Speed.y;
        private bool NegativeVerticalSpeed => VerticalSpeed < 0;

        private bool StopHorizontalSpeedAndSetHit => OnSlope && MetMinimumWallAngle &&
                                                     GroundNotHorizontalMovementDirection && NegativeVerticalSpeed;

        private void SetHorizontalCollision()
        {
            if (CastToSides)
            {
                Raycast.OnPlatformerInitializeLengthForSideRay();
                for (var i = 0; i < HorizontalRays; i++)
                {
                    Platformer.SetIndex(i);
                    Raycast.OnPlatformerCastRaysToSides();
                    if (SideHitMissed) continue;
                    if (FirstSideHitOnAngle)
                    {
                        Raycast.OnPlatformerOnFirstSideHit();
                        Physics.OnPlatformerOnFirstSideHit();
                        Raycast.OnPlatformerSetLengthForSideRay();
                    }

                    if (CastNextRayToSides) continue;
                    Physics.OnPlatformerOnSideHit();
                    Raycast.OnPlatformerSetLengthForSideRay();
                    if (SetVerticalMovement)
                    {
                        if (NegativeVerticalMovement) Physics.OnPlatformerStopVerticalMovement();
                        else Physics.OnPlatformerAdjustVerticalMovementToSlope();
                    }

                    Raycast.OnPlatformerHitWall();
                    Physics.OnPlatformerHitWall();
                }
            }

            if (!StopHorizontalSpeedAndSetHit) return;
            Raycast.OnPlatformerOnStopHorizontalSpeedHit();
            Physics.OnPlatformerStopHorizontalSpeed();
        }

        private bool PositiveVerticalMovement => VerticalMovement > 0;
        private bool NoHorizontalMovement => HorizontalMovement == 0;
        private bool InAir => !OnSlope || NoHorizontalMovement;
        private bool CastVertically => PositiveVerticalMovement || NegativeVerticalMovement && InAir;
        private bool VerticalHitMissed => !Hit;
        private bool SetVerticalHitAtOneWayPlatform => NegativeVerticalMovement && VerticalHitMissed;
        private bool ApplySlopeBehaviorToPhysics => OnSlope && PositiveVerticalMovement;

        private void SetVerticalCollision()
        {
            if (!CastVertically) return;
            Raycast.OnPlatformerInitializeLengthForVerticalRay();
            for (var i = 0; i < VerticalRays; i++)
            {
                Platformer.SetIndex(i);
                Raycast.OnPlatformerCastRaysVertically();
                if (SetVerticalHitAtOneWayPlatform) Raycast.OnPlatformerSetVerticalHitAtOneWayPlatform();
                if (VerticalHitMissed) continue;
                Physics.OnPlatformerVerticalHit();
                Raycast.OnPlatformerVerticalHit();
                if (ApplySlopeBehaviorToPhysics) Physics.OnPlatformerApplyGroundAngle();
            }
        }

        private bool OnGround => Collision.OnGround;
        private bool NoVerticalSpeed => VerticalSpeed == 0;
        private bool NegativeOrNoVerticalSpeed => NoVerticalSpeed || NegativeVerticalSpeed;
        private bool SlopeChange => OnGround && HasHorizontalMovement && NegativeOrNoVerticalSpeed;
        private bool PositiveSlopeChange => PositiveVerticalMovement;

        private void OnSlopeChange()
        {
            if (!SlopeChange) return;
            if (PositiveSlopeChange) OnPositiveSlopeChange();
        }

        private void OnPositiveSlopeChange()
        {
            OnSteepSlope();
            OnMildSlope();
        }

        private bool SteepSlopeHit => Hit;
        private float SteepSlopeAngle => SideAngle;
        private float Tolerance => Platformer.Tolerance;
        private bool SteepSlopeNotGroundAngle => Abs(SteepSlopeAngle - GroundAngle) > Tolerance;
        private bool ApplySteepSlopeBehavior => SteepSlopeHit && SteepSlopeNotGroundAngle;

        private void OnSteepSlope()
        {
            Raycast.OnPlatformerClimbSteepSlope();
            if (!ApplySteepSlopeBehavior) return;
            //Physics.OnPlatformerApplySteepSlopeBehavior();
            //Raycast.OnPlatformerApplySteepSlopeBehavior();
            Platformer.OnApplySteepSlopeBehavior();
        }

        private bool ClimbedSteepSlope => Platformer.ClimbedSteepSlope;
        private bool ClimbMildSlope => !ClimbedSteepSlope;

        private void OnMildSlope()
        {
            if (!ClimbMildSlope) return;
            //Raycast.OnPlatformerClimbMildSlope();
            //if (!ApplyMildSlopeBehavior) return;
        }

        #endregion

        #endregion

        #region properties

        public PlatformerData Data => Platformer;

        #region public methods

        public PlatformerModel(PlatformerSettings settings, ref RaycastController raycast,
            ref LayerMaskController layerMask, ref PhysicsController physics)
        {
            Platformer = new PlatformerData(settings);
            Raycast = raycast;
            LayerMask = layerMask;
            Physics = physics;
        }

        public void Run()
        {
            RunInternal();
        }

        #endregion

        #endregion
    }
}