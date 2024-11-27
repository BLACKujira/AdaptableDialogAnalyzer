using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Unity;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_OMCCount : MonoBehaviour, IInitializable, IFadeIn
    {
        [Header("Components")]
        public List<View_BanGDream_OMCPercent_Item> items;
        public CanvasGroup cgTitle;
        public SpriteRenderer srGaussian;
        public SpriteRenderer srTriangle;
        public Transform tfUIEffect;
        [Header("Adapter")]
        public SimpleMentionCountResultLoader mentionCountResultLoader;
        [Header("Settings")]
        public int startRank = 1;
        [Header("Time")]
        public float textFadeDuration = 0.5f;
        public float bgFadeDuration = 1.0f;
        public float itemFadeInterval = 0.033f;
        public float triangleAlpha = 0.05f;

        private void Start()
        {
            Initialize();
            FadeIn();
        }

        public void FadeIn()
        {
            StartCoroutine(CoFadeIn());
        }

        IEnumerator CoFadeIn()
        {
            //cgTitle.DOFade(0, textFadeDuration);
            //yield return new WaitForSeconds(textFadeDuration);

            WaitForSeconds waitForSeconds = new WaitForSeconds(itemFadeInterval);
            int itemCount = items.Count;

            // 依次淡入每个 item
            for (int i = 0; i < itemCount; i++)
            {
                items[i].FadeIn();

                // 等待一段时间，控制 itemFadeInterval 为间隔
                yield return waitForSeconds;
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

            // 加载提及计数结果并进行处理
            SimpleMentionCountResult countResult = mentionCountResultLoader.SimpleMentionCountResult;
            HackCountResult(countResult);

            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;
            Character[] characters = characterDefinition.Characters;

            List<SimpleMentionCountResultItemWithRank> countResultItem = countResult.GetResultWithRank()
                .OrderBy(r => r.rank)
                .Skip(startRank - 1)
                .Take(items.Count)
                .ToList();

            for (int i = 0; i < items.Count; i++)
            {
                View_BanGDream_OMCPercent_Item currItem = items[i];
                SimpleMentionCountResultItemWithRank currResult = i > countResultItem.Count - 1 ? null : countResultItem[i];

                if (currItem != null)
                {
                    currItem.SetData_Count(currResult);
                }
                else
                {
                    currItem.gameObject.SetActive(false);
                }
            }
        }

        // 处理计数结果，合并或移除特定数据
        void HackCountResult(SimpleMentionCountResult countResult)
        {
            // 合并角色ID 601 的数据到角色ID 15
            countResult.items[15].count += countResult[601].count;
            countResult.items[15].serifCount += countResult[601].serifCount;

            // 移除指定角色的数据
            HashSet<SimpleMentionCountResultItem> removeItems = new HashSet<SimpleMentionCountResultItem>
            {
                countResult[0],
                countResult[601],
                countResult[214],
                countResult[201]
            };

            foreach (var item in removeItems)
            {
                countResult.items.Remove(item); // 移除无关数据项
            }
        }
    }
}