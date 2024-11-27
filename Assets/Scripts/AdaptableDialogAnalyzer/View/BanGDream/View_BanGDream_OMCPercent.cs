using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Unity;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_OMCPercent : MonoBehaviour, IInitializable, IFadeIn
    {
        [Header("Components")]
        public List<View_BanGDream_OMCPercent_Item> items;
        public View_BanGDream_OMCPercent_Title title;
        public SpriteRenderer srGaussian;
        public SpriteRenderer srTriangle;
        public Transform tfUIEffect;
        [Header("Adapter")]
        public SimpleMentionCountResultLoader mentionCountResultLoader;
        [Header("Time")]
        public float playDelay = 2f;
        public float itemFadeDelay = 5f; // 首个 item 淡入前的延迟
        public float bgFadeDuration = 1.0f; // 背景淡入的持续时间
        public float itemFadeInterval = 0.033f; // 每个 item 淡入之间的间隔时间
        public float triangleAlpha = 0.05f; // 三角形背景透明度
        public float holdDuration = 15f; // 保持每组数据的显示时间
        public float refadeDelay = 2f; // 每个 item 的重新淡入延迟时间

        List<SimpleMentionCountResultItemWithRank> countResultItem;

        /// <summary>
        /// Unity生命周期方法：启动时初始化组件并开始淡入流程。
        /// </summary>
        private void Start()
        {
            Initialize(); // 初始化组件
            title.Initialize(); // 初始化标题
            FadeIn(); // 开始淡入动画
        }

        /// <summary>
        /// 开始淡入动画。
        /// </summary>
        public void FadeIn()
        {
            StartCoroutine(CoFadeIn());
        }

        /// <summary>
        /// 处理淡入效果的协程，按顺序淡入标题、每个 item 和背景。
        /// </summary>
        IEnumerator CoFadeIn()
        {
            yield return new WaitForSeconds(playDelay);

            title.FadeIn(); // 标题淡入
            yield return new WaitForSeconds(itemFadeDelay);

            // 按顺序淡入每个 item
            for (int i = 0; i < items.Count; i++)
            {
                items[i].FadeIn();
                yield return new WaitForSeconds(itemFadeInterval); // 控制间隔
            }

            // 背景元素淡入
            srGaussian.DOFade(1, bgFadeDuration);
            srTriangle.DOFade(triangleAlpha, bgFadeDuration);

            // 如果有额外数据，进行分页处理
            List<SimpleMentionCountResultItemWithRank> countResultItemNext = countResultItem.Skip(items.Count).ToList();
            if (countResultItemNext.Count > 0)
            {
                // 分批淡入剩余数据
                for (int loop = 0; loop < (countResultItemNext.Count - 1) / items.Count + 1; loop++)
                {
                    yield return new WaitForSeconds(holdDuration);
                    for (int i = 0; i < items.Count; i++)
                    {
                        int resultIndex = loop * items.Count + i;
                        View_BanGDream_OMCPercent_Item currItem = items[i];
                        SimpleMentionCountResultItemWithRank currResult = resultIndex > countResultItemNext.Count - 1 ? null : countResultItemNext[resultIndex];

                        if (currItem != null)
                        {
                            StartCoroutine(CoFadeItemSecond(currItem, currResult));
                        }
                        else
                        {
                            currItem.gameObject.SetActive(false);
                        }

                        yield return new WaitForSeconds(itemFadeInterval);
                    }
                }
            }
        }

        /// <summary>
        /// 淡出旧数据并重新淡入新数据的协程。
        /// </summary>
        IEnumerator CoFadeItemSecond(View_BanGDream_OMCPercent_Item item, SimpleMentionCountResultItemWithRank result)
        {
            item.FadeOut(); // 旧数据淡出
            yield return new WaitForSeconds(refadeDelay);
            item.SetData(result); // 设置新数据
            item.FadeIn(); // 新数据淡入
        }

        /// <summary>
        /// 初始化组件，加载数据并将初始数据分配到 UI 元素。
        /// </summary>
        public void Initialize()
        {
            // 初始化每个 item
            foreach (var item in items)
            {
                item.Initialize(tfUIEffect);
            }

            // 加载提及计数结果
            SimpleMentionCountResult countResult = mentionCountResultLoader.SimpleMentionCountResult;
            HackCountResult(countResult); // 修改和清理计数结果

            // 获取所有角色信息
            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;
            Character[] characters = characterDefinition.Characters;

            // 排序并准备数据
            countResultItem = countResult.GetResultWithRank()
                .OrderBy(r => r.percentRank)
                .ToList();

            // 分配数据到 UI 元素
            for (int i = 0; i < items.Count; i++)
            {
                View_BanGDream_OMCPercent_Item currItem = items[i];
                SimpleMentionCountResultItemWithRank currResult = i > countResultItem.Count - 1 ? null : countResultItem[i];

                if (currItem != null)
                {
                    currItem.SetData(currResult);
                }
                else
                {
                    currItem.gameObject.SetActive(false);
                }
            }
        }

        /// <summary>
        /// 对提及计数结果进行预处理，包括合并特定数据和移除无关数据。
        /// </summary>
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