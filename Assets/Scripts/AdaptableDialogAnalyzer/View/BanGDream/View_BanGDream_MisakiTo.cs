using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Unity;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_MisakiTo : MonoBehaviour, IInitializable, IFadeIn
    {
        [Header("Components")]
        public List<View_BanGDream_MisakiTo_Item> items;
        public Text txtTitle;
        public SpriteRenderer srGaussian;
        public SpriteRenderer srTriangle;
        public Transform tfUIEffect;
        [Header("Adapter")]
        public MentionedCountManagerLoader mentionedCountManagerLoader;
        [Header("Time")]
        public float textFadeDuration = 0.5f;
        public float bgFadeDuration = 1.0f;
        public float itemFadeInterval = 0.033f;
        public float triangleAlpha = 0.05f;

        int misakiId = 15;
        int michelleId = 601;

        public void Initialize()
        {
            foreach (var item in items)
            {
                if (item != null) item.Initialize(tfUIEffect);
            }

            MentionedCountManager mentionedCountManager = mentionedCountManagerLoader.MentionedCountManager;

            (int i, int countMisaki, int countMichelle)[] countArray = Enumerable.Range(0, 36)
                .Select(i => (i, mentionedCountManager[i, misakiId]?.Total ?? 0, mentionedCountManager[i, michelleId]?.Total ?? 0))
                .ToArray();

            for (int i = 1; i < 36; i++)
            {
                View_BanGDream_MisakiTo_Item item = items[i];
                (int id, int countMisaki, int countMichelle) = countArray[i];
                item.SetData(id, countMisaki, countMichelle);
            }
        }

        public void FadeIn()
        {
            StartCoroutine(CoFadeIn());
        }

        IEnumerator CoFadeIn()
        {
            txtTitle.DOFade(0, textFadeDuration);
            yield return new WaitForSeconds(textFadeDuration);

            WaitForSeconds waitForSeconds = new WaitForSeconds(itemFadeInterval);

            View_BanGDream_MisakiTo_Item[] randomItems = MathHelper.GetRandomArray(35)
                .Select(i => items[i+1])
                .ToArray();

            foreach (var item in randomItems)
            {
                item.FadeIn();
                yield return waitForSeconds;
            }

            srGaussian.DOFade(1, bgFadeDuration);
            srTriangle.DOFade(triangleAlpha, bgFadeDuration);
        }
    }
}