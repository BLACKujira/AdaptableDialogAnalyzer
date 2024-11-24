using AdaptableDialogAnalyzer.Unity;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AdaptableDialogAnalyzer.UnityEditor
{
    /// <summary>
    /// 读取项目目录下的所有sprite资源生成SpriteList
    /// </summary>
    public class SpriteListGenerator : EditorWindow
    {
        private string spritePath;
        private string assetPath;

        [MenuItem("Window/AdaptableDialogAnalyzer/SpriteListGenerator")]
        public static void ShowWindow()
        {
            SpriteListGenerator window = GetWindow<SpriteListGenerator>();
            window.titleContent = new GUIContent("SpriteListGenerator");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Character Definition", EditorStyles.boldLabel);
            spritePath = EditorGUILayout.TextField("SpritePath", spritePath);
            assetPath = EditorGUILayout.TextField("AssetPath", assetPath);

            if (!string.IsNullOrEmpty(spritePath) && !string.IsNullOrEmpty(assetPath))
            {
                GUILayout.Space(10);
                if (GUILayout.Button("Generate"))
                {
                    Generate();
                }
            }

            void Generate()
            {
                // Ensure the spritePath is valid and exists
                if (string.IsNullOrEmpty(spritePath))
                {
                    EditorUtility.DisplayDialog("Error", "Please enter a valid sprite path.", "OK");
                    return;
                }

                if (!Directory.Exists(spritePath))
                {
                    EditorUtility.DisplayDialog("Error", "Directory does not exist.", "OK");
                    return;
                }

                // Get all sprite files in the directory
                string[] spriteFiles = Directory.GetFiles(spritePath, "*.png", SearchOption.TopDirectoryOnly);
                List<Sprite> sprites = new List<Sprite>();

                foreach (var file in spriteFiles)
                {
                    string relativePath = file;
                    Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(relativePath);
                    if (sprite != null)
                    {
                        sprites.Add(sprite);
                    }
                }

                if (sprites.Count == 0)
                {
                    EditorUtility.DisplayDialog("Error", "No sprites found in the specified directory.", "OK");
                    return;
                }

                // Create or update the SpriteList asset
                SpriteList spriteList = AssetDatabase.LoadAssetAtPath<SpriteList>(assetPath);
                if (spriteList == null)
                {
                    spriteList = ScriptableObject.CreateInstance<SpriteList>();
                    AssetDatabase.CreateAsset(spriteList, assetPath);
                }

                spriteList.sprites = sprites;

                // Save the asset
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                EditorUtility.DisplayDialog("Success", "Sprite list generated successfully!", "OK");
            }
        }
    }
}
