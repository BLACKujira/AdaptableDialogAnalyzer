using AdaptableDialogAnalyzer.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AdaptableDialogAnalyzer.UnityEditor
{
    public class NicknameMappingGenerator : EditorWindow
    {
        private CharacterDefinition characterDefinition;
        private NicknameMapping nicknameMapping;

        [MenuItem("Window/Nickname Mapping Editor")]
        public static void ShowWindow()
        {
            NicknameMappingGenerator window = GetWindow<NicknameMappingGenerator>();
            window.titleContent = new GUIContent("Nickname Mapping Editor");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("Character Definition", EditorStyles.boldLabel);
            characterDefinition = EditorGUILayout.ObjectField("Character Definition", characterDefinition, typeof(CharacterDefinition), false) as CharacterDefinition;

            if (characterDefinition != null)
            {
                GUILayout.Space(10);

                if (GUILayout.Button("Create Nickname Mapping"))
                {
                    CreateNicknameMapping();
                }
            }
        }

        private void CreateNicknameMapping()
        {
            // 创建一个新的 NicknameMapping
            nicknameMapping = CreateInstance<NicknameMapping>();

            // 遍历 CharacterDefinition 中的角色列表
            foreach (Character character in characterDefinition.characters)
            {
                // 创建一个新的 NicknameList，并将角色的昵称添加到列表中
                NicknameList nicknameList = new NicknameList();
                nicknameList.nicknames.Add(character.name);

                // 将昵称列表添加到 NicknameMapping 的列表中
                nicknameMapping.nicknameLists.Add(nicknameList);
            }

            // 将创建的 NicknameMapping 保存为资源文件
            string path = EditorUtility.SaveFilePanelInProject("Save Nickname Mapping", "New Nickname Mapping", "asset", "Save Nickname Mapping");
            if (!string.IsNullOrEmpty(path))
            {
                AssetDatabase.CreateAsset(nicknameMapping, path);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            Debug.Log("Nickname Mapping created!");
        }
    }
}
