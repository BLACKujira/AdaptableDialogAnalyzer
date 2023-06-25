using AdaptableDialogAnalyzer.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AdaptableDialogAnalyzer.UnityEditor
{
    public class NicknameMappingGenerator : EditorWindow
    {
        private CharacterDefinition characterDefinition;
        private NicknameMapping nicknameMapping;

        [MenuItem("Window/AdaptableDialogAnalyzer/NicknameMappingGenerator")]
        public static void ShowWindow()
        {
            NicknameMappingGenerator window = GetWindow<NicknameMappingGenerator>();
            window.titleContent = new GUIContent("NicknameMappingGenerator");
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

                if(character.name.Contains(" "))
                {
                    string[] nicknames = character.name.Split(' ');
                    nicknameList.nicknames.AddRange(nicknames);
                    nicknameList.nicknames.Add(character.name.Replace(" ", ""));
                }
                else
                {
                    nicknameList.nicknames.Add(character.name);
                }

                // 将昵称列表添加到 NicknameMapping 的列表中
                nicknameMapping.nicknameLists.Add(nicknameList);
            }

            //去重
            HashSet<string> duplicateNicknames = new HashSet<string>(
                nicknameMapping.nicknameLists
                .SelectMany(nl => nl.nicknames)
                .GroupBy(nickname => nickname)
                .Where(group => group.Count() > 1)
                .Select(group => group.Key)
                );

            foreach (var nicknameList in nicknameMapping.nicknameLists)
            {
                nicknameList.nicknames.RemoveAll(nickname => duplicateNicknames.Contains(nickname));
            }

            // 将创建的 NicknameMapping 保存为资源文件
            string path = EditorUtility.SaveFilePanelInProject("Save Nickname Mapping", "Common", "asset", "Save Nickname Mapping");
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
