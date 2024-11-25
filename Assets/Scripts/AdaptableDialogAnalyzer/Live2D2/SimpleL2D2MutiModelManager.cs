using System.Collections.Generic;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Live2D2
{
    public partial class SimpleL2D2MutiModelManager : MonoBehaviour
    {
        [Header("Components")]
        public Transform spawnTransform;
        [Header("Settings")]
        public float distance = -5;
        [Header("Prefab")]
        public RenderTexture live2dRenderTexturePrefab;
        public SimpleLive2DModel simpleLive2DModelPrefab;
        public Camera live2DCameraPrefab;

        public List<ModelInstanceInfo> modelInstanceInfos = new List<ModelInstanceInfo>();

        public ModelInstanceInfo AddModel(ModelInfo modelInfo)
        {
            // 复制渲染纹理
            RenderTexture copyRenderTexture = Instantiate(live2dRenderTexturePrefab);

            // 生成模型
            SimpleLive2DModel simpleLive2DModel = Instantiate(simpleLive2DModelPrefab, spawnTransform);
            simpleLive2DModel.SetData(modelInfo);

            // 生成相机
            Camera live2DCamera = Instantiate(live2DCameraPrefab, spawnTransform);
            live2DCamera.targetTexture = copyRenderTexture;

            // 打包ModelInstanceInfo
            ModelInstanceInfo modelInstanceInfo = new ModelInstanceInfo();
            modelInstanceInfo.renderTexture = copyRenderTexture;
            modelInstanceInfo.simpleLive2DModel = simpleLive2DModel;
            modelInstanceInfo.live2DCamera = live2DCamera;
            modelInstanceInfos.Add(modelInstanceInfo);

            // 获取index
            int index = modelInstanceInfos.IndexOf(modelInstanceInfo);
            modelInstanceInfo.index = index;

            // 设置相机和模型距离
            float xPosition = distance * index;
            live2DCamera.transform.localPosition = new Vector3(xPosition, 0, live2DCamera.transform.localPosition.z);
            simpleLive2DModel.transform.localPosition = new Vector3(xPosition, 0, simpleLive2DModel.transform.localPosition.z);

            return modelInstanceInfo;
        }
    }
}