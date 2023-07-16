using Google.Protobuf.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    public class BanGDream_EventLogoLoader : MonoBehaviour
    {
        public string assetBundleFolder;
        public BanGDream_SuiteMasterLoader suiteMasterLoader;

        Dictionary<string, string> logoPathMap = null;
        Dictionary<int, Texture2D> loadedLogos = new Dictionary<int, Texture2D>();

        Vector2Int logoSize = new Vector2Int(450, 200);

        private void Initialize()
        {
            string eventFolder = @$"{assetBundleFolder}\assets\star\forassetbundle\asneeded\event";
            string[] subFolders = Directory.GetDirectories(eventFolder)
                .SelectMany(f => Directory.GetDirectories(f))
                .ToArray();

            logoPathMap = new Dictionary<string, string>();
            foreach (string subFolder in subFolders)
            {
                string eventName = Path.GetFileName(subFolder);
                logoPathMap[eventName] = subFolder;
            }
        }

        private Texture2D LoadFromFolder(string eventName)
        {
            if (logoPathMap == null) Initialize();
            if (!logoPathMap.ContainsKey(eventName))
            {
                Debug.Log($"未找到活动{eventName}的图标储存文件夹");
            }

            string logoFile = $"{logoPathMap[eventName]}/images/logo.png";
            if (!File.Exists(logoFile))
            {
                Debug.Log($"未找到图标文件{logoFile}");
            }

            byte[] bytes = File.ReadAllBytes(logoFile);
            Texture2D texture2D = new Texture2D(logoSize.x, logoSize.y);
            texture2D.LoadImage(bytes);
            return texture2D;
        }

        public Texture2D GetLogo(int eventId)
        {
            if(loadedLogos.ContainsKey(eventId)) return loadedLogos[eventId];

            MapField<uint, MasterEvent> events = suiteMasterLoader.SuiteMasterGetResponse.MasterEventMapForExchanges.Entries;
            if(!events.ContainsKey((uint)eventId))
            {
                Debug.LogError($"未找到活动{eventId}的数据");
                return null;
            }

            MasterEvent masterEvent = events[(uint)eventId];
            loadedLogos[eventId] = LoadFromFolder(masterEvent.AssetBundleName);
            return loadedLogos[eventId];
        }
    }
}