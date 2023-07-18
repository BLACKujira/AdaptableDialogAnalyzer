using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Games.BanGDream;
using AdaptableDialogAnalyzer.Live2D2;
using AdaptableDialogAnalyzer.UIElements;
using AdaptableDialogAnalyzer.Unity;
using DG.Tweening;
using live2d;
using live2d.framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_CharacterMentions : MonoBehaviour
    {
        [Header("Components")]
        public List<View_BanGDream_CharacterMentions_Item> items;
        public List<View_BanGDream_CharacterMentions_Line> lines;
        public RawImage imgLive2D;
        public SimpleLive2DModel live2DModel;
        public Transform tfUIEffect;
        public RectTransform rtMain;
        public CanvasGroup cgMain;
        [Header("Settings")]
        public bool mainCharacterOnly = true;
        public bool passSelf = false;
        public int speakerId = 1;
        [Header("Adapter")]
        public MentionedCountManagerLoader mentionedCountManagerLoader;
        [Header("Prefab")]
        public View_BanGDream_ItemEffect itemEffectPrefab;
        [Header("Time")]
        public float lineMoveInterval = 0.2f;
        public float itemFadeInterval = 0.033f;
        public float live2dFadeInDelay = 3;
        public float live2dFadeInDuration = 0.3f;
        public float live2dPlayVoiceDelay = 2;
        [Header("Assets")]
        public TextAsset live2DMotionAsset;
        public TextAsset live2DExpressionAsset;
        [Header("FadeOut")]
        public float fadeOutDuration;

        MentionedCountManager mentionedCountManager;
        Live2DMotion live2DMotion;
        L2DExpressionMotion live2DExpression;

        public void Initialize()
        {
            mentionedCountManager = mentionedCountManagerLoader.MentionedCountManager;

            List<CharacterMentionStats> characterMentionStatsList = mentionedCountManager.GetMentionStatsList(speakerId, false, passSelf);

            if (mainCharacterOnly)
            {
                characterMentionStatsList = characterMentionStatsList
                    .Where(cms => BanGDreamHelper.IsMainCharacter(cms.MentionedPersonId))
                    .ToList();
            }

            characterMentionStatsList = characterMentionStatsList
                .OrderBy(cms => -cms.Total)
                .ToList();

            int mentionTotal = characterMentionStatsList.Sum(cms => cms.Total);
            int serifCount = mentionedCountManager.CountSerif(speakerId);

            foreach (var item in items)
            {
                View_BanGDream_ItemEffect itemEffect = Instantiate(itemEffectPrefab, tfUIEffect);
                item.Initialize(itemEffect);
            }

            for (int i = 0; i < items.Count; i++)
            {
                View_BanGDream_CharacterMentions_Item item = items[i];
                if (i >= characterMentionStatsList.Count)
                {
                    item.gameObject.SetActive(false);
                }
                else
                {
                    CharacterMentionStats characterMentionStats = characterMentionStatsList[i];
                    item.SetData(characterMentionStats.MentionedPersonId, characterMentionStats.Total, mentionTotal);
                }
            }

            imgLive2D.color = new Color(1, 1, 1, 0);
            if (live2DMotionAsset) live2DMotion = Live2DMotion.loadMotion(live2DMotionAsset.bytes);
            if (live2DExpressionAsset) live2DExpression = L2DExpressionMotion.loadJson(live2DExpressionAsset.bytes);
        }

        public void FadeIn()
        {
            StartCoroutine(CoMoveLines());
            StartCoroutine(CoFadeInItem());
            StartCoroutine(CoPlayLive2D());
        }

        IEnumerator CoMoveLines()
        {
            foreach (var line in lines)
            {
                line.Move();
                yield return new WaitForSeconds(lineMoveInterval);
            }
        }

        IEnumerator CoFadeInItem()
        {
            View_BanGDream_CharacterMentions_Item[] randomItems = MathHelper.GetRandomArray(items.Count)
                .Select(i => items[i])
                .Where(i => i.enabled)
                .ToArray();

            foreach (var item in randomItems)
            {
                item.FadeIn();
                yield return new WaitForSeconds(itemFadeInterval);
            }
        }

        IEnumerator CoPlayLive2D()
        {
            yield return new WaitForSeconds(live2dFadeInDelay);
            if (live2DMotion != null) live2DModel.PlayMotion(live2DMotion);
            if (live2DExpression != null) live2DModel.PlayExpression(live2DExpression);
            imgLive2D.DOFade(1, live2dFadeInDuration);

            yield return new WaitForSeconds(live2dPlayVoiceDelay);
            live2DModel.AudioSource.Play();
        }

        public void FadeOut()
        {
            cgMain.DOFade(0, fadeOutDuration).SetEase(Ease.InCubic);
        }
    }
}