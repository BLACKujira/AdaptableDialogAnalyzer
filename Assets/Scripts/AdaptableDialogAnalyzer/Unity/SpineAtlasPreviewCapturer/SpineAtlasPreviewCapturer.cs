#if USE_SPINE
using AdaptableDialogAnalyzer.Spine;
using Spine;
using Spine.Unity;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.U2D;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 对使用同一骨骼，不同纹理图集的spine模型生成预览
    /// </summary>
    public class SpineAtlasPreviewCapturer : TaskWindow
    {
        [Header("Component")]
        public Canvas targetCanvas;
        public RectTransform captureArea;
        public SkeletonAnimation targetSkeletonAnimation;
        public RenderTexture targetRenderTexture;
        [Header("Settings")]
        public SpineAtlasList spineAtlasList;
        public AnimationReferenceAsset animationReference;
        public string saveFolder;

        private void Start()
        {
            StartCoroutine(CoCapture());
        }

        IEnumerator CoCapture()
        {
            for (int i = 0; i < spineAtlasList.spineAtlasAssets.Count; i++)
            {
                SpineAtlasAsset spineAtlas = spineAtlasList.spineAtlasAssets[i];

                Priority = (float)i / spineAtlasList.spineAtlasAssets.Count;
                Progress = spineAtlas.name;

                targetSkeletonAnimation.SkeletonDataAsset.atlasAssets[0] = spineAtlas;
                foreach (var atlasAsset in targetSkeletonAnimation.skeletonDataAsset.atlasAssets)
                {
                    if (atlasAsset != null)
                        atlasAsset.Clear();
                    targetSkeletonAnimation.skeletonDataAsset.Clear();
                }
                targetSkeletonAnimation.Initialize(true);

                TrackEntry trackEntry = targetSkeletonAnimation.AnimationState.SetAnimation(0, animationReference.name, true);
                trackEntry.TrackTime = 0;
                yield return new WaitForSeconds(animationReference.Animation.Duration);

                yield return new WaitForEndOfFrame();
                Texture2D texture2D = Capturer.Capture(captureArea, targetRenderTexture);
                File.WriteAllBytes($"{saveFolder}/{i}_{spineAtlas.name}.png", texture2D.EncodeToPNG());
            }

            Priority = 1;
            Progress = "完成";
        }
    }
}
#endif