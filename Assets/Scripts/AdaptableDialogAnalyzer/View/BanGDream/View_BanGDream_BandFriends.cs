using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Games.BanGDream;
using AdaptableDialogAnalyzer.Unity;
using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static AdaptableDialogAnalyzer.Games.BanGDream.GameDefine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_BandFriends : MonoBehaviour
    {
        [Header("Components")]
        public List<View_BanGDream_BandFriends_Item> items;
        public Transform tfUIEffect;
        public CanvasGroup cgMain;
        [Header("Settings")]
        public BandIdName band;
        public float fadeDuration = 0.8f;
        [Header("Adapter")]
        public MentionedCountManagerLoader mentionedCountManagerLoader;

        public void Initialize()
        {
            // 初始化转场粒子效果
            foreach (var item in items)
            {
                item.Initialize(tfUIEffect);
            }

            MentionedCountManager mentionedCountManager = mentionedCountManagerLoader.MentionedCountManager;

            // 筛选其他乐队角色
            Character[] otherCharacters = GlobalConfig.CharacterDefinition.Characters
                .Where(c => BanGDreamHelper.GetCharacterBand(c.id) != BandIdName.None)
                .Where(c => BanGDreamHelper.GetCharacterBand(c.id) != band)
                .ToArray();

            // 筛选当前乐队角色
            Character[] bandCharacters = GlobalConfig.CharacterDefinition.Characters
                .Where(c => BanGDreamHelper.GetCharacterBand(c.id) == band)
                .ToArray();

            // 函数，用于获取乐队角色和其他乐队角色之间的提及次数
            Func<Character, Vector2Int[]> getCountV2I = (Character oc) => bandCharacters
                .Select(bc => mentionedCountManager[bc.id, oc.id])
                .Select(m => new Vector2Int(m.SpeakerId, m.Total))
                .OrderBy(v => v.x)
                .ToArray();

            // 根据提及次数对其他乐队角色进行排序，并将结果存储在 countArray 数组中
            (Character oc, Vector2Int[] count)[] countArray = otherCharacters
                .Select(oc => (oc, getCountV2I(oc)))
                .OrderByDescending(t => t.Item2.Sum(v => v.y))
                .ToArray();

            // 遍历 items 列表，并设置 BandFriends 子项的数据
            for (int i = 0; i < items.Count; i++)
            {
                View_BanGDream_BandFriends_Item item = items[i];
                (Character oc, Vector2Int[] count) = countArray[i];
                item.SetData(oc.id, count);
            }

            cgMain.alpha = 0;
        }

        public void FadeIn()
        {
            cgMain.DOFade(1, fadeDuration);
            foreach (var item in items)
            {
                item.FadeIn();
            }
        }
    }
}