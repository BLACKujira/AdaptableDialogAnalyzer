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
            // ����һ���µ� NicknameMapping
            nicknameMapping = CreateInstance<NicknameMapping>();

            // ���� CharacterDefinition �еĽ�ɫ�б�
            foreach (Character character in characterDefinition.characters)
            {
                // ����һ���µ� NicknameList��������ɫ���ǳ���ӵ��б���
                NicknameList nicknameList = new NicknameList();
                nicknameList.nicknames.Add(character.name);

                // ���ǳ��б���ӵ� NicknameMapping ���б���
                nicknameMapping.nicknameLists.Add(nicknameList);
            }

            // �������� NicknameMapping ����Ϊ��Դ�ļ�
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
