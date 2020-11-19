﻿using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;
using VFEngine.Tools;

namespace VFEngine.Platformer.Physics.Movement.PathMovement
{
    using static ScriptableObjectExtensions;
    [CreateAssetMenu(fileName = "PathMovementData", menuName = PlatformerPathMovementDataPath, order = 0)]
    [InlineEditor]
    public class PathMovementData : ScriptableObject
    {
        #region fields

        #region dependencies

        #endregion

        [SerializeField] private Vector2Reference currentSpeed = new Vector2Reference();
        private static readonly string ModelAssetPath = $"{PathMovementPath}PathMovementModel.asset";

        #endregion

        #region properties

        #region dependencies

        #endregion

        public Vector2 CurrentSpeed => currentSpeed.Value;
        private const string PathMovementPath = "Physics/Movement/PathMovement/";
        public static readonly string PathMovementModelPath = $"{PlatformerScriptableObjectsPath}{ModelAssetPath}";

        #endregion
    }
}