using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Games.BanGDream;
using AdaptableDialogAnalyzer.Unity;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_Kataomoi : MonoBehaviour, IInitializable, IFadeIn
    {
        [Header("Components")]
        public List<View_BanGDream_KataomoiItem> items;
        public CanvasGroup cgTitle;
        public SpriteRenderer srGaussian;
        public SpriteRenderer srTriangle;
        public Transform tfUIEffect;
        [Header("Adapter")]
        public MentionedCountManagerLoader mentionedCountManagerLoader;
        [Header("Settings")]
        public bool mainCharacterOnly = true;
        [Header("Time")]
        public float textFadeDuration = 0.5f;
        public float bgFadeDuration = 1.0f;
        public float itemFadeInterval = 0.033f;
        public float triangleAlpha = 0.05f;

        public void FadeIn()
        {
            StartCoroutine(CoFadeIn());
        }

        IEnumerator CoFadeIn()
        {
            cgTitle.DOFade(0, textFadeDuration);
            yield return new WaitForSeconds(textFadeDuration);

            WaitForSeconds waitForSeconds = new WaitForSeconds(itemFadeInterval);
            int itemCount = items.Count;

            // 从列表的两端向中间依次淡入每个 item
            for (int i = 0; i < itemCount / 2; i++)
            {
                items[i].FadeIn();
                items[itemCount - 1 - i].FadeIn();

                // 等待一段时间，控制 itemFadeInterval 为间隔
                yield return waitForSeconds;
            }

            // 如果列表长度为奇数，最后一个 item 位于中间位置，单独淡入
            if (itemCount % 2 != 0)
            {
                items[itemCount / 2].FadeIn();
            }

            srGaussian.DOFade(1, bgFadeDuration);
            srTriangle.DOFade(triangleAlpha, bgFadeDuration);
        }

        public void Initialize()
        {
            foreach (var item in items)
            {
                item.Initialize(tfUIEffect);
            }

            MentionedCountManager mentionedCountManager = mentionedCountManagerLoader.MentionedCountManager;
            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;

            Character[] characters = characterDefinition.Characters;
            if (mainCharacterOnly)
            {
                characters = characters
                    .Where(c => BanGDreamHelper.IsMainCharacter(c.id))
                    .ToArray();
            }

            List<CharacterMentionStatsPair> characterMentionStatsPairs = mentionedCountManager.GetCharacterMentionStatsPairs(false);
            if (mainCharacterOnly)
            {
                characterMentionStatsPairs = characterMentionStatsPairs
                    .Where(p => BanGDreamHelper.IsMainCharacter(p.CharacterAId) && BanGDreamHelper.IsMainCharacter(p.CharacterBId))
                    .ToList();
            }


            Dictionary<int, int> allMentionCountMap = new Dictionary<int, int>();
            int GetAllMentionCount(CharacterMentionStats s)
            {
                if (!allMentionCountMap.ContainsKey(s.SpeakerId))
                {
                    int allMentionCount = characters
                        .Where(c => c.id != s.SpeakerId)
                        .Select(c => mentionedCountManager[s.SpeakerId, c.id])
                        .Sum(s => s.Total);
                    allMentionCountMap[s.SpeakerId] = allMentionCount;
                }
                return allMentionCountMap[s.SpeakerId];
            }

            List<(CharacterMentionStatsPair statsPair, float percentAToB, float percentBToA)> count = characterMentionStatsPairs
                .Select(p => (p, (float)p.StatsAToB.Total / GetAllMentionCount(p.StatsAToB), (float)p.StatsBToA.Total / GetAllMentionCount(p.StatsBToA)))
                .OrderByDescending(t => Mathf.Abs(t.Item2 - t.Item3))
                .Take(items.Count)
                .ToList();

            for (int i = 0; i < count.Count; i++)
            {
                var (statsPair, percentAToB, percentBToA) = count[i];
                if (percentAToB < percentBToA)
                {
                    statsPair.Swap();
                    count[i] = (statsPair, percentBToA, percentAToB);
                }
            }

            for (int i = 0; i < items.Count; i++)
            {
                View_BanGDream_KataomoiItem item = items[i];
                (CharacterMentionStatsPair statsPair, float percentAToB, float percentBToA) = count[i];
                item.SetData(statsPair.CharacterAId,
                             statsPair.CharacterBId,
                             percentAToB,
                             percentBToA);
            }
        }
    }
}