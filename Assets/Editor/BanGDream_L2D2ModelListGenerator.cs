using AdaptableDialogAnalyzer.Live2D2;
using AdaptableDialogAnalyzer.Unity;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AdaptableDialogAnalyzer.UnityEditor
{
    public class BanGDream_L2D2ModelListGenerator : EditorWindow
    {
        private string modelPath = "Assets/View/BanGDream/live2d";
        private string assetPath = "Assets/View/BanGDream/live2d/ModelInfoList.asset";

        [MenuItem("Window/AdaptableDialogAnalyzer/BanGDream/L2D2ModelListGenerator")]
        public static void ShowWindow()
        {
            BanGDream_L2D2ModelListGenerator window = GetWindow<BanGDream_L2D2ModelListGenerator>();
            window.titleContent = new GUIContent("L2D2ModelListGenerator");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Character Definition", EditorStyles.boldLabel);
            modelPath = EditorGUILayout.TextField("SpritePath", modelPath);
            assetPath = EditorGUILayout.TextField("AssetPath", assetPath);

            if (!string.IsNullOrEmpty(modelPath) && !string.IsNullOrEmpty(assetPath))
            {
                GUILayout.Space(10);
                if (GUILayout.Button("Generate"))
                {
                    Generate();
                }
            }
        }

        private void Generate()
        {
            // 初始化存储 ModelInfo 的列表
            List<IndexedModelInfo> modelInfos = new List<IndexedModelInfo>();

            // 遍历 1 到 40 的角色 ID
            for (int i = 1; i <= 40; i++)
            {
                // 创建新的 ModelInfo 实例
                ModelInfo modelInfo = new ModelInfo();

                // 获取 .moc.bytes 文件
                string mocFileName = GetMocFileName(i);
                if (!string.IsNullOrEmpty(mocFileName))
                {
                    modelInfo.mocFile = AssetDatabase.LoadAssetAtPath<TextAsset>(mocFileName);
                    if (modelInfo.mocFile == null)
                    {
                        Debug.LogError($"Failed to load .moc.bytes file as TextAsset: {mocFileName}");
                    }
                }

                // 获取 .physics.json 文件
                string physicsFileName = GetPhysicsFileName(i);
                if (!string.IsNullOrEmpty(physicsFileName))
                {
                    modelInfo.physicsFile = AssetDatabase.LoadAssetAtPath<TextAsset>(physicsFileName);
                    if (modelInfo.physicsFile == null)
                    {
                        Debug.LogError($"Failed to load .physics.json file as TextAsset: {physicsFileName}");
                    }
                }

                // 获取所有 .png 文件
                string[] textureFileNames = GetTextureFileName(i);
                if (textureFileNames != null && textureFileNames.Length > 0)
                {
                    modelInfo.textureFiles = textureFileNames
                        .Select(fileName => AssetDatabase.LoadAssetAtPath<Texture2D>(fileName))
                        .Where(texture => texture != null)
                        .ToArray();

                    if (modelInfo.textureFiles.Length == 0)
                    {
                        Debug.LogWarning($"No valid textures loaded for character ID: {i}");
                    }
                }
                else
                {
                    Debug.LogWarning($"No texture files found for character ID: {i}");
                }

                // 设置默认属性
                modelInfo.scaleVolume = 8; // 默认缩放比例
                modelInfo.smoothing = true; // 默认启用平滑

                // 将 ModelInfo 添加到列表中
                modelInfos.Add(new IndexedModelInfo(i, modelInfo));
            }

            IndexedModelInfoList indexedModelInfoList = ScriptableObject.CreateInstance<IndexedModelInfoList>();
            indexedModelInfoList.IndexedModelInfos = modelInfos;

            AssetDatabase.CreateAsset(indexedModelInfoList, assetPath);

            // Save the asset
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            // 输出生成完成的消息
            Debug.Log($"Generated {modelInfos.Count} ModelInfo entries.");
        }

        /// <summary>
        /// Gets the file name of the `.moc.bytes` file for a given character ID.
        /// </summary>
        /// <param name="characterId">The ID of the character.</param>
        /// <returns>The file name of the `.moc.bytes` file, or `null` if not found.</returns>
        string GetMocFileName(int characterId)
        {
            string folder = Path.Combine(modelPath, $"{characterId:000}_live_default");

            // Check if the directory exists
            if (!Directory.Exists(folder))
            {
                Debug.LogError($"Directory not found: {folder}");
                return null;
            }

            // Get all files in the folder and filter by `.moc.bytes`
            string[] files = Directory.GetFiles(folder);
            string mocFile = files.FirstOrDefault(f => f.ToLower().EndsWith(".moc.bytes"));

            if (string.IsNullOrEmpty(mocFile))
            {
                Debug.LogWarning($"No .moc.bytes file found for character ID: {characterId} in {folder}");
            }

            return mocFile;
        }

        /// <summary>
        /// Gets the file name of the `.physics.json` file for a given character ID.
        /// </summary>
        /// <param name="characterId">The ID of the character.</param>
        /// <returns>The file name of the `.physics.json` file, or `null` if not found.</returns>
        string GetPhysicsFileName(int characterId)
        {
            string folder = Path.Combine(modelPath, $"{characterId:000}_live_default");

            // Check if the directory exists
            if (!Directory.Exists(folder))
            {
                Debug.LogError($"Directory not found: {folder}");
                return null;
            }

            // Get all files in the folder and filter by `.physics.json`
            string[] files = Directory.GetFiles(folder);
            string physicsFile = files.FirstOrDefault(f => f.ToLower().EndsWith(".physics.json"));

            if (string.IsNullOrEmpty(physicsFile))
            {
                Debug.LogWarning($"No .physics.json file found for character ID: {characterId} in {folder}");
            }

            return physicsFile;
        }

        /// <summary>
        /// Gets the file names of all `.png` texture files for a given character ID.
        /// </summary>
        /// <param name="characterId">The ID of the character.</param>
        /// <returns>An array of file names for `.png` textures, or an empty array if not found.</returns>
        string[] GetTextureFileName(int characterId)
        {
            string folderBase = Path.Combine(modelPath, $"{characterId:000}_live_default");

            // Check if the base directory exists
            if (!Directory.Exists(folderBase))
            {
                Debug.LogError($"Base directory not found: {folderBase}");
                return new string[0];
            }

            // Find the specific subdirectory ending with ".1024"
            string folderTex = Directory.GetDirectories(folderBase).FirstOrDefault(f => f.EndsWith(".1024"));

            if (string.IsNullOrEmpty(folderTex) || !Directory.Exists(folderTex))
            {
                Debug.LogWarning($"No texture folder ending with '.1024' found for character ID: {characterId} in {folderBase}");
                return new string[0];
            }

            // Get all `.png` files in the texture folder
            string[] files = Directory.GetFiles(folderTex);
            string[] pngFiles = files.Where(f => f.ToLower().EndsWith(".png")).ToArray();

            if (pngFiles.Length == 0)
            {
                Debug.LogWarning($"No .png texture files found for character ID: {characterId} in {folderTex}");
            }

            return pngFiles;
        }
    }
}
