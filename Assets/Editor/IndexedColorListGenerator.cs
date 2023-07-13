using AdaptableDialogAnalyzer.Unity;
using UnityEditor;
using UnityEngine;

namespace AdaptableDialogAnalyzer.UnityEditor
{
    public class IndexedColorListGenerator : EditorWindow
    {
        private CharacterDefinition characterDefinition;

        [MenuItem("Window/AdaptableDialogAnalyzer/IndexedColorListGenerator")]
        public static void ShowWindow()
        {
            IndexedColorListGenerator window = GetWindow<IndexedColorListGenerator>();
            window.titleContent = new GUIContent("IndexedColorListGenerator");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Character Definition", EditorStyles.boldLabel);
            characterDefinition = EditorGUILayout.ObjectField("Character Definition", characterDefinition, typeof(CharacterDefinition), false) as CharacterDefinition;

            if (characterDefinition != null)
            {
                GUILayout.Space(10);

                if (GUILayout.Button("Create Indexed Color List"))
                {
                    CreateIndexedColorList();
                }
            }
        }

        private void CreateIndexedColorList()
        {
            // 创建一个新的 IndexedColorList
            IndexedColorList indexedColorList = CreateInstance<IndexedColorList>();

            // 遍历 CharacterDefinition 中的角色列表
            foreach (Character character in characterDefinition.Characters)
            {
                IndexedColor indexedColor = new IndexedColor();
                indexedColor.id = character.id;
                indexedColor.color = character.color;

                // 将昵称列表添加到 NicknameMapping 的列表中
                indexedColorList.indexedColors.Add(indexedColor);
            }

            // 将创建的 NicknameMapping 保存为资源文件
            string path = EditorUtility.SaveFilePanelInProject("Save Nickname Mapping", "IndexedColorList", "asset", "Save Nickname Mapping");
            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.CreateAsset(indexedColorList, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            Debug.Log("Nickname Mapping created!");
        }
    }
}
