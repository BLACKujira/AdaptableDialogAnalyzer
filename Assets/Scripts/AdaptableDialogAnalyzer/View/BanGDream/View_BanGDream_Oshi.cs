using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Games.BanGDream;
using AdaptableDialogAnalyzer.Unity;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_Oshi : MonoBehaviour, IInitializable, IFadeIn
    {
        [Header("Components")]
        public List<View_BanGDream_Oshi_Item> items;
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
        public float itemFadeInterval = 0.067f;
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

            List<CharacterMentionStats> stats = new List<CharacterMentionStats>();
            foreach (Character speaker in characters)
            {
                CharacterMentionStats mostMentionStat = characters
                    .Where(c => c.id != speaker.id)
                    .Select(mp => mentionedCountManager[speaker.id, mp.id])
                    .OrderBy(s => -s.Total)
                    .First();
                stats.Add(mostMentionStat);
            }

            Func<CharacterMentionStats, int> getAllMentionCount = (s) =>
            {
                return characters
                    .Select(c => mentionedCountManager[s.SpeakerId, c.id])
                    .Sum(s => s.Total);
            };

            (CharacterMentionStats stat, int allMentionCount, int mentionSelfCount)[] count = stats
                .Select(s => (s, getAllMentionCount(s), mentionedCountManager[s.SpeakerId, s.SpeakerId].Total))
                .OrderByDescending(t => (float)t.s.Total / (t.Item2 - t.Total))
                .ToArray();

            for (int i = 0; i < stats.Count; i++)
            {
                View_BanGDream_Oshi_Item item = items[i];
                (CharacterMentionStats stat, int allMentionCount, int mentionSelfCount) = count[i];
                item.SetData(stat.SpeakerId, stat.MentionedPersonId, stat.Total, allMentionCount, mentionSelfCount);
            }
        }
    }
}