using AdaptableDialogAnalyzer.Games.ProjectSekai;
using AdaptableDialogAnalyzer.Unity;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AdaptableDialogAnalyzer.UnityEditor
{

    public class ProjectSekai_EventIconListGenerator : EditorWindow
    {
        public string masterEventPath;
        public string iconFolderPath;

        [MenuItem("Window/AdaptableDialogAnalyzer/ProjectSekai/EventIconListGenerator")]
        public static void ShowWindow()
        {
            ProjectSekai_EventIconListGenerator window = GetWindow<ProjectSekai_EventIconListGenerator>();
            window.titleContent = new GUIContent("EventIconListGenerator");
            window.Show();
        }

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            masterEventPath = EditorGUILayout.TextField("Master Event Path", masterEventPath);
            iconFolderPath = EditorGUILayout.TextField("Icon Folder Path", iconFolderPath);
            if (GUILayout.Button("Create"))
            {
                Create();
            }
            GUILayout.EndVertical();
        }

        void Create()
        {
            MasterEvent[] masterEvents = JsonHelper.getJsonArray<MasterEvent>(File.ReadAllText(masterEventPath));
            int lastEventId = masterEvents.Max(e => e.id);

            Sprite[] icons = new Sprite[lastEventId + 1];
            foreach (MasterEvent masterEvent in masterEvents)
            {
                string spriteAssetPath = $"{iconFolderPath}/{masterEvent.assetbundleName}/logo_rip/logo.png";
                Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spriteAssetPath);
                icons[masterEvent.id] = sprite;
            }

            SpriteList spriteList = CreateInstance<SpriteList>();
            spriteList.sprites = icons.ToList();
            string savePath = $"{iconFolderPath}/EventLogos.asset";
            AssetDatabase.CreateAsset(spriteList, savePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
