﻿using UnityEngine;
using VFEngine.Platformer.Layer.Mask;
using VFEngine.Platformer.Physics;

namespace VFEngine.Platformer.Event.Raycast
{
    using static ScriptableObject;
    using static Debug;
    using static Color;
    using static Vector2;

    public class RaycastController : MonoBehaviour
    {
        #region fields

        #region dependencies

        [SerializeField] private new Collider2D collider;
        [SerializeField] private RaycastSettings settings;
        private LayerMaskData _layerMask;
        private PhysicsData _physics;
        private PlatformerData _platformer;
        private RaycastModel Raycast { get; set; }
        
        #endregion

        #region internal

        #endregion

        #region private methods

        #region initialization

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _layerMask = GetComponent<LayerMaskController>().Data;
            _physics = GetComponent<PhysicsController>().Data;
            _platformer = GetComponent<PlatformerController>().Data;
            if (!collider) collider = GetComponent<BoxCollider2D>();
            if (!settings) settings = CreateInstance<RaycastSettings>();
            Raycast = new RaycastModel(ref collider, ref settings, ref _layerMask, ref _physics, ref _platformer);
        }

        #endregion

        #endregion

        #endregion

        #region properties

        public RaycastData Data => Raycast.Data;

        #region public methods

        
        public void OnPlatformerInitializeFrame()
        {
            Raycast.OnInitializeFrame();
        }

        public void OnPlatformerCastRaysDown()
        {
            Raycast.OnCastRaysDown();
        }

        public void OnPlatformerSetDownHitAtOneWayPlatform()
        {
            Raycast.SetDownHitAtOneWayPlatform();
        }

        private RaycastData RaycastData => Raycast.Data;
        private Vector2 Origin => RaycastData.Origin;
        private Vector2 DownRayOrigin => Origin;
        private float SkinWidth => RaycastData.SkinWidth;
        private Vector2 DownRayDirection => down * SkinWidth * 2;
        private static Color DownRayColor => blue;

        public void OnPlatformerDownHit()
        {
            Raycast.OnDownHit();
            DrawRay(DownRayOrigin, DownRayDirection, DownRayColor);
        }

        public void OnPlatformerSlopeBehavior()
        {
            Raycast.OnSlopeBehavior();
        }

        public void OnPlatformerInitializeLengthForSideRay()
        {
            Raycast.InitializeLengthForSideRay();
        }

        private Vector2 SideRayOrigin => Origin;
        private PhysicsData Physics => _physics;
        private int HorizontalMovementDirection => Physics.HorizontalMovementDirection;
        private float Length => RaycastData.Length;
        private float SideRayLength => Length;
        private Vector2 SideRayDirection => right * HorizontalMovementDirection * SideRayLength;
        private static Color SideRayColor => red;

        public void OnPlatformerCastRaysToSides()
        {
            Raycast.OnCastRaysToSides();
            DrawRay(SideRayOrigin, SideRayDirection, SideRayColor);
        }

        public void OnPlatformerOnFirstSideHit()
        {
            Raycast.OnFirstSideHit();
        }

        public void OnPlatformerSetLengthForSideRay()
        {
            Raycast.SetLengthForSideRay();
        }

        public void OnPlatformerHitWall()
        {
            Raycast.OnHitWall();
        }

        public void OnPlatformerOnStopHorizontalSpeedHit()
        {
            Raycast.OnStopHorizontalSpeedHit();
        }

        public void OnPlatformerInitializeLengthForVerticalRay()
        {
            Raycast.InitializeLengthForVerticalRay();
        }

        private Vector2 VerticalRayOrigin => Origin;
        private int VerticalMovementDirection => Physics.VerticalMovementDirection;
        private float VerticalRayLength => Length;
        private Vector2 VerticalRayDirection => up * VerticalMovementDirection * VerticalRayLength;
        private static Color VerticalRayColor => red;

        public void OnPlatformerCastRaysVertically()
        {
            Raycast.OnCastRaysVertically();
            DrawRay(VerticalRayOrigin, VerticalRayDirection, VerticalRayColor);
        }

        public void OnPlatformerSetVerticalHitAtOneWayPlatform()
        {
            Raycast.SetVerticalHitAtOneWayPlatform();
        }
        public void OnPlatformerVerticalHit()
        {
            Raycast.OnVerticalHit();
        }

        public void OnPlatformerClimbSteepSlope()
        {
            Raycast.OnClimbSteepSlope();
        }

        #endregion

        #endregion
    }
}