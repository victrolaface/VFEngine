﻿using Cysharp.Threading.Tasks;
using UnityEngine;
using VFEngine.Platformer.Event.Boxcast.SafetyBoxcast;
using VFEngine.Platformer.Event.Raycast.StickyRaycast;
using VFEngine.Tools;
using UniTaskExtensions = VFEngine.Tools.UniTaskExtensions;

namespace VFEngine.Platformer.Event.Raycast
{
    using static Debug;
    using static RaycastData;
    using static ScriptableObjectExtensions;
    using static RaycastDirection;
    using static UniTaskExtensions;

    [RequireComponent(typeof(StickyRaycastController))]
    [RequireComponent(typeof(SafetyBoxcastController))]
    public class RaycastController : MonoBehaviour
    {
        /* fields: dependencies */
        [SerializeField] private RaycastModel upRaycastModel;
        [SerializeField] private RaycastModel rightRaycastModel;
        [SerializeField] private RaycastModel downRaycastModel;
        [SerializeField] private RaycastModel leftRaycastModel;

        /* fields */
        private RaycastModel[] models;

        /* fields: methods */
        private async void Awake()
        {
            GetModels();
            var rTask1 = Async(upRaycastModel.Initialize(Up));
            var rTask2 = Async(rightRaycastModel.Initialize(Right));
            var rTask3 = Async(downRaycastModel.Initialize(Down));
            var rTask4 = Async(leftRaycastModel.Initialize(Left));
            var rTask = await (rTask1, rTask2, rTask3, rTask4);
        }

        private void GetModels()
        {
            models = new[] {upRaycastModel, rightRaycastModel, downRaycastModel, leftRaycastModel};
            var names = new[] {"upRaycastModel", "rightRaycastModel", "downRaycastModel", "leftRaycastModel"};
            for (var i = 0; i < models.Length; i++)
            {
                if (models[i]) continue;
                models[i] = LoadData(ModelPath) as RaycastModel;
                switch (i)
                {
                    case 0:
                        upRaycastModel = models[i];
                        break;
                    case 1:
                        rightRaycastModel = models[i];
                        break;
                    case 2:
                        downRaycastModel = models[i];
                        break;
                    case 3:
                        leftRaycastModel = models[i];
                        break;
                }

                Assert(models[i] != null, names[i] + " != null");
            }
        }
        
        /* properties: methods */
        public async UniTaskVoid SetRaysParameters()
        {
            var rTask1 = Async(upRaycastModel.OnSetRaysParameters());
            var rTask2 = Async(rightRaycastModel.OnSetRaysParameters());
            var rTask3 = Async(downRaycastModel.OnSetRaysParameters());
            var rTask4 = Async(leftRaycastModel.OnSetRaysParameters());
            var rTask = await (rTask1, rTask2, rTask3, rTask4);
            await SetYieldOrSwitchToThreadPoolAsync();
        }
    }
}