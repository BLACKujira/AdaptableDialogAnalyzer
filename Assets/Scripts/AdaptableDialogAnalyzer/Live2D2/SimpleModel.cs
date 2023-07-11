using live2d.framework;
using live2d;
using System;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Live2D2
{
    [ExecuteInEditMode]
    public class SimpleModel : MonoBehaviour
    {
        public TextAsset mocFile;
        public TextAsset physicsFile;
        public Texture2D[] textureFiles;
        [Header("Settings_LipSync")]
        public float scaleVolume = 8;
        public bool smoothing = true;

        private Live2DModelUnity live2DModel;
        private EyeBlinkMotion eyeBlink = new EyeBlinkMotion();
        private L2DPhysics physics;
        private Matrix4x4 live2DCanvasPos;

        private float lastVolume = 0;

        private MotionQueueManager motionMgr = new MotionQueueManager();

        AudioSource audioSource;
        private AudioSource AudioSource
        {
            get
            {
                if (audioSource == null) audioSource = GetComponent<AudioSource>();
                return audioSource;
            }
        }

        void Start()
        {
            live2d.Live2D.init();

            load();
        }

        void load()
        {
            live2DModel = Live2DModelUnity.loadModel(mocFile.bytes);

            for (int i = 0; i < textureFiles.Length; i++)
            {
                live2DModel.setTexture(i, textureFiles[i]);
            }

            float modelWidth = live2DModel.getCanvasWidth();
            live2DCanvasPos = Matrix4x4.Ortho(0, modelWidth, modelWidth, 0, -50.0f, 50.0f);

            if (physicsFile != null) physics = L2DPhysics.load(physicsFile.bytes);
        }

        void Update()
        {
            if (live2DModel == null) load();
            live2DModel.setMatrix(transform.localToWorldMatrix * live2DCanvasPos);
            if (!Application.isPlaying)
            {
                live2DModel.update();
                return;
            }

            double timeSec = UtSystem.getUserTimeMSec() / 1000.0;
            double t = timeSec * 2 * Math.PI;
            live2DModel.setParamFloat("PARAM_BREATH", (float)(0.5f + 0.5f * Math.Sin(t / 3.0)));

            eyeBlink.setParam(live2DModel);

            if (physics != null) physics.updateParam(live2DModel);

            live2DModel.update();
            UpdateLipSync();
            motionMgr.updateParam(live2DModel);
        }

        void UpdateLipSync()
        {
            if (live2DModel == null) return;
            live2DModel.setMatrix(transform.localToWorldMatrix * live2DCanvasPos);

            if (!Application.isPlaying)
            {
                live2DModel.update();
                live2DModel.draw();
                return;
            }
            float volume = 0;

            if (smoothing)
            {
                float currentVolume = AudioHelper.GetCurrentVolume(AudioSource);

                if (Mathf.Abs(lastVolume - currentVolume) < 0.2f)
                {
                    volume = lastVolume * 0.9f + currentVolume * 0.1f;
                }
                else if (lastVolume - currentVolume > 0.2f)
                {
                    volume = lastVolume * 0.7f + currentVolume * 0.3f;
                }
                else
                {
                    volume = lastVolume * 0.2f + currentVolume * 0.8f;
                }
                lastVolume = volume;
            }
            else
            {
                volume = AudioHelper.GetCurrentVolume(GetComponent<AudioSource>());
            }

            live2DModel.setParamFloat("PARAM_MOUTH_OPEN_Y", volume * scaleVolume);

            live2DModel.update();
        }

        void OnRenderObject()
        {
            if (live2DModel == null) load();
            if (live2DModel.getRenderMode() == live2d.Live2D.L2D_RENDER_DRAW_MESH_NOW) live2DModel.draw();
        }

        public void PlayMotion(Live2DMotion motion)
        {
            motionMgr.startMotion(motion);
        }
    }
}