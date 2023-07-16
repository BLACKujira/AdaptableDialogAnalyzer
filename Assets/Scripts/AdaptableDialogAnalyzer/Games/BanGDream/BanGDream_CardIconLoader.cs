using Google.Protobuf.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Games.BanGDream
{
    public class BanGDream_CardIconLoader : MonoBehaviour
    {
        public string assetBundleFolder;
        public BanGDream_MasterLoader masterLoader;

        Dictionary<string, string> iconPathMap = null;
        Dictionary<int, Texture2D> loadedIcons = new Dictionary<int, Texture2D>();

        Vector2Int iconSize = new Vector2Int(180, 180);

        private void Initialize()
        {
            string iconFolder = @$"{assetBundleFolder}\assets\star\forassetbundle\startapp\thumbnail\character";
            string[] files = Directory.GetDirectories(iconFolder)
                .SelectMany(f => Directory.GetFiles(f))
                .ToArray();

            iconPathMap = new Dictionary<string, string>();
            Regex regexNormalCard = new Regex("res\\d\\d\\d\\d\\d\\d(?=_normal)");
            foreach (var file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                Match match = regexNormalCard.Match(fileName);
                if (!match.Success) continue;
                iconPathMap[match.Value] = file;
            }
        }

        private Texture2D LoadFromFolder(string resourceSetName)
        {
            if (iconPathMap == null) Initialize();
            if (!iconPathMap.ContainsKey(resourceSetName))
            {
                Debug.Log($"未找到{resourceSetName}的图标储存文件夹");
            }

            string logoFile = iconPathMap[resourceSetName];
            if (!File.Exists(logoFile))
            {
                Debug.Log($"未找到图标文件{logoFile}");
            }

            byte[] bytes = File.ReadAllBytes(logoFile);
            Texture2D texture2D = new Texture2D(iconSize.x, iconSize.y);
            texture2D.LoadImage(bytes);
            return texture2D;
        }

        public Texture2D GetIcon(int situationId)
        {
            if (loadedIcons.ContainsKey(situationId)) return loadedIcons[situationId];

            MapField<uint, MasterCharacterSituation> cards = masterLoader.SuiteMasterGetResponse.MasterCharacterSituationMap.Entries;
            if (!cards.ContainsKey((uint)situationId))
            {
                Debug.LogError($"未找到卡片{situationId}的数据");
                return null;
            }

            MasterCharacterSituation masterCharacterSituation = cards[(uint)situationId];
            loadedIcons[situationId] = LoadFromFolder(masterCharacterSituation.ResourceSetName);
            return loadedIcons[situationId];
        }
    }
}