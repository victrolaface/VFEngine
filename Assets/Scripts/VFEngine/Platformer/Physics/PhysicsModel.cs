﻿using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using VFEngine.Tools;
using UniTaskExtensions = VFEngine.Tools.UniTaskExtensions;

namespace VFEngine.Platformer.Physics
{
    using static DebugExtensions;
    using static Quaternion;
    using static UniTaskExtensions;
    using static Time;
    using static Mathf;

    [CreateAssetMenu(fileName = "PhysicsModel", menuName = "VFEngine/Platformer/Physics/Physics Model", order = 0)]
    public class PhysicsModel : ScriptableObject, IModel
    {
        /* fields: dependencies */
        [LabelText("Physics Data")] [SerializeField]
        private PhysicsData p;

        /* fields */

        /* fields: methods */
        private async UniTaskVoid Initialize()
        {
            var pTask1 = Async(InitializeData());
            var pTask2 = Async(GetWarningMessages());
            var pTask = await (pTask1, pTask2);
            await SetYieldOrSwitchToThreadPoolAsync();
        }

        private async UniTaskVoid InitializeData()
        {
            p.TransformRef = p.Transform;
            p.SpeedRef = p.Speed;
            p.GravityActiveRef = p.state.GravityActive;
            p.FallSlowFactorRef = p.FallSlowFactor;
            p.state.Reset();
            if (p.AutomaticGravityControl && !p.HasGravityController) p.Transform.rotation = identity;
            await SetYieldOrSwitchToThreadPoolAsync();
        }

        private async UniTaskVoid GetWarningMessages()
        {
            if (DisplayWarnings)
            {
                const string ph = "Physics";
                var settings = $"{ph} Settings";
                var warningMessage = "";
                var warningMessageCount = 0;
                if (!p.HasSettings) warningMessage += FieldString($"{settings}", $"{settings}");
                if (!p.HasGravityController) warningMessage += FieldParentString("Gravity Controller", $"{settings}");
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

            await SetYieldOrSwitchToThreadPoolAsync();
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
            p.state.Reset();
        }

        private void TranslatePlatformSpeedToTransform()
        {
            p.Transform.Translate(p.MovingPlatformCurrentSpeed * deltaTime);
        }

        private void DisableGravity()
        {
            p.state.SetGravity(false);
        }

        private void ApplyMovingPlatformSpeedToNewPosition()
        {
            p.NewPositionY = p.MovingPlatformCurrentSpeed.y * deltaTime;
        }

        private void StopHorizontalSpeed()
        {
            p.Speed = -p.NewPosition / deltaTime;
            p.SpeedX = -p.SpeedX;
        }

        private void SetForcesApplied()
        {
            p.ForcesApplied = p.Speed;
        }

        private void SetMovementDirectionToStored()
        {
            p.MovementDirection = p.StoredMovementDirection;
        }
        
        private void SetNegativeMovementDirection()
        {
            p.MovementDirection = -1;
        }

        private void SetPositiveMovementDirection()
        {
            p.MovementDirection = 1;
        }

        private void ApplyPlatformSpeedToMovementDirection()
        {
            p.MovementDirection = Sign(p.MovingPlatformCurrentSpeed.x);
        }

        private void SetStoredMovementDirection()
        {
            p.StoredMovementDirection = p.MovementDirection;
        }
        
        /* properties: dependencies */
        private bool DisplayWarnings => p.DisplayWarnings;

        /* properties: methods */
        public void OnInitialize()
        {
            Async(Initialize());
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

        public void OnStopHorizontalSpeed()
        {
            StopHorizontalSpeed();
        }

        public void OnSetForcesApplied()
        {
            SetForcesApplied();
        }

        public void OnSetMovementDirectionToStored()
        {
            SetMovementDirectionToStored();
        }
        public void OnSetNegativeMovementDirection()
        {
            SetNegativeMovementDirection();
        }

        public void OnSetPositiveMovementDirection()
        {
            SetPositiveMovementDirection();
        }

        public void OnApplyPlatformSpeedToMovementDirection()
        {
            ApplyPlatformSpeedToMovementDirection();
        }

        public void OnSetStoredMovementDirection()
        {
            SetStoredMovementDirection();
        }
    }
}