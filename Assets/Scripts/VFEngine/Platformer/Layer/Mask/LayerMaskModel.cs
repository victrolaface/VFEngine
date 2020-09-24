﻿using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using VFEngine.Tools;
using UniTaskExtensions = VFEngine.Tools.UniTaskExtensions;

namespace VFEngine.Platformer.Layer.Mask
{
    using static UniTaskExtensions;
    using static ScriptableObjectExtensions;

    [CreateAssetMenu(fileName = "LayerMaskModel", menuName = "VFEngine/Platformer/Layer/Mask/Layer Mask Model",
        order = 0)]
    public class LayerMaskModel : ScriptableObject, IModel
    {
        /* fields: dependencies */
        [LabelText("Layer Mask Data")] [SerializeField]
        private LayerMaskData lm;
        private const string AssetPath = "Layer/Mask/DefaultLayerMaskModel.asset";

        /* fields: methods */
        private async UniTaskVoid InitializeModelAsyncInternal()
        {
            lm.SavedPlatformMask = lm.PlatformMask;
            lm.PlatformMask |= lm.OneWayPlatformMask;
            lm.PlatformMask |= lm.MovingPlatformMask;
            lm.PlatformMask |= lm.MovingOneWayPlatformMask;
            lm.PlatformMask |= lm.MidHeightOneWayPlatformMask;
            await SetYieldOrSwitchToThreadPoolAsync();
        }

        /* properties */
        public static string ModelPath => $"{DefaultPath}{PlatformerPath}{AssetPath}";

        /* properties: methods */
        public void InitializeData()
        {
            lm.Initialize();
        }

        public UniTask<UniTaskVoid> InitializeModelAsync()
        {
            try
            {
                return new UniTask<UniTaskVoid>(InitializeModelAsyncInternal());
            }
            finally
            {
                InitializeModelAsyncInternal().Forget();
            }
        }
    }
}