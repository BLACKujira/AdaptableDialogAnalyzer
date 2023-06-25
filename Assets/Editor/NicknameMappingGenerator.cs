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
            // ����һ���µ� NicknameMapping
            nicknameMapping = CreateInstance<NicknameMapping>();

            // ���� CharacterDefinition �еĽ�ɫ�б�
            foreach (Character character in characterDefinition.characters)
            {
                // ����һ���µ� NicknameList��������ɫ���ǳ���ӵ��б���
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

                // ���ǳ��б���ӵ� NicknameMapping ���б���
                nicknameMapping.nicknameLists.Add(nicknameList);
            }

            //ȥ��
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

            // �������� NicknameMapping ����Ϊ��Դ�ļ�
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
