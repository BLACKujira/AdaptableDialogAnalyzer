using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Live2D2;
using AdaptableDialogAnalyzer.Unity;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_OMCDisplay : MonoBehaviour
    {
        [Header("Components")]
        public EquidistantLayoutScroll equidistantLayoutScroll;
        public SimpleMentionCountResultLoader mentionCountResultLoader;
        public SimpleL2D2MutiModelManager mutiModelManager;
        public ColorTransition transitionIn;
        public ColorTransition transitionOut;
        [Header("Settings")]
        public float fadeInScrollValue = 800;
        public bool sortByPercent = false;
        public List<int> debugCharacterIDs = new List<int>();
        [Header("Settings2")]
        public float initItemPosDelta = 1920f; // 初始化时Item的水平位移
        public float initMoveItemDuration = 1f; // 初始化移动动画的持续时间
        public float initMoveItemInterval = 0.3f; // 初始化移动动画的间隔时间
        public float initFadeDelay = 1f; // 初始化淡入效果的延迟时间
        public float initFadeInterval = 550f / 100f; // 初始化淡入效果的间隔时间
        public float delayBeforeEnableScroll= 5f; // 延迟启用滚动的时间
        [Header("Settings3")]
        public float delayBeforeLastItemFadeIn = (800f - 550f) / 100f;
        public float transitionInDelay = 5f;
        public float playDelay = 1f;
        public float transitionOutDelay = 5f;

        private void Start()
        {
            // 加载提及计数结果并进行处理
            SimpleMentionCountResult countResult = mentionCountResultLoader.SimpleMentionCountResult;
            HackCountResult(countResult);

            // 对结果进行排序，并生成布局中的Item
            List<SimpleMentionCountResultItemWithRank> sortedCountResult;
            if(sortByPercent)
            {
                sortedCountResult = countResult.GetResultWithRank().OrderByDescending(x => x.percentRank).ToList();
            }
            else
            {
                sortedCountResult = countResult.GetResultWithRank().OrderByDescending(x => x.rank).ToList();
            }

            equidistantLayoutScroll.equidistantLayoutGenerator.Generate(sortedCountResult.Count, (gobj, id) =>
            {
                View_BanGDream_OMCItem oMCItem = gobj.GetComponent<View_BanGDream_OMCItem>();
                oMCItem.Initlize(sortedCountResult[id], mutiModelManager);
                gobj.SetActive(false); // 初始设置为不可见
            });

            // 注册滚动事件：当Item进入视图范围时激活并加载模型
            equidistantLayoutScroll.onItemEnter += (gobj) =>
            {
                gobj.SetActive(true);
                gobj.GetComponent<View_BanGDream_OMCItem>().ActiveModel();
            };

            // 注册滚动事件：当Item滚动到指定范围时触发淡入效果
            ScrollEvent scrollEvent = new ScrollEvent(fadeInScrollValue);
            scrollEvent.callEvent += (gobj) =>
            {
                gobj.GetComponent<View_BanGDream_OMCItem>().FadeIn();
            };
            equidistantLayoutScroll.scrollItemEvents.Add(scrollEvent);

            // 注册滚动事件：当Item退出视图范围时停用模型并隐藏
            equidistantLayoutScroll.onItemExit += (gobj) =>
            {
                gobj.GetComponent<View_BanGDream_OMCItem>().InactiveModel();
                gobj.SetActive(false);
            };

            // 注册播放结束事件
            equidistantLayoutScroll.onScrollEnd += OnScrollEnd;

            // 初始化时逆序排列Item，暂时解决显示顺序问题
            ReverseItems();

            // 激活初始进入范围内的Item
            equidistantLayoutScroll.ForEachAlreadyEnterRect((gobj) =>
            {
                gobj.SetActive(true);
                gobj.GetComponent<View_BanGDream_OMCItem>().ActiveModel();
            });

            equidistantLayoutScroll.enabled = false; // 初始化结束后隐藏滚动布局
            StartCoroutine(CoPlay()); // 启动播放协程
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

            // 如果调试角色不为空，则仅保留调试角色
            if (debugCharacterIDs.Count > 0)
            {
                foreach (var item in countResult.items)
                {
                    if (!debugCharacterIDs.Contains(item.characterID))
                    {
                        removeItems.Add(item);
                    }
                }
            }

            foreach (var item in removeItems)
            {
                countResult.items.Remove(item); // 移除无关数据项
            }
        }

        // 播放初始化动画的协程
        IEnumerator CoPlay()
        {
            // 获取初始显示的Item
            List<View_BanGDream_OMCItem> initItems = equidistantLayoutScroll.equidistantLayoutGenerator.Items
                .Take(4)
                .Select(gobj => gobj.GetComponent<View_BanGDream_OMCItem>())
                .ToList();

            // 将Item初始化到指定位置
            foreach (var item in initItems)
            {
                item.RectTransform.anchoredPosition = new Vector2(item.RectTransform.anchoredPosition.x + initItemPosDelta, item.RectTransform.anchoredPosition.y);
            }

            yield return new WaitForSeconds(transitionInDelay);

            // 播放转场动画
            transitionIn.StartTransition();
            yield return new WaitForSeconds(playDelay); // 等待延迟时间

            // 播放Item的移动动画
            foreach (var item in initItems)
            {
                item.RectTransform.DOAnchorPosX(item.RectTransform.anchoredPosition.x - initItemPosDelta, initMoveItemDuration).SetEase(Ease.OutBack);
                yield return new WaitForSeconds(initMoveItemInterval);
            }

            yield return new WaitForSeconds(initFadeDelay); // 等待淡入延迟时间

            initItems.RemoveAt(3);
            // 播放Item的淡入效果
            foreach (var item in initItems)
            {
                item.FadeIn();
                yield return new WaitForSeconds(initFadeInterval);
            }

            yield return new WaitForSeconds(delayBeforeEnableScroll);
            // 启用卷轴
            equidistantLayoutScroll.enabled = true;
            equidistantLayoutScroll.enableScroll = true;
        }

        // 将Item的层级逆序排列
        void ReverseItems()
        {
            foreach (var item in equidistantLayoutScroll.equidistantLayoutGenerator.Items)
            {
                item.transform.SetAsFirstSibling();
            }
        }

        void OnScrollEnd()
        {
            StartCoroutine(CoOnScrollEnd());
        }

        // 卷轴结束后播放
        IEnumerator CoOnScrollEnd()
        {
            yield return new WaitForSeconds(delayBeforeLastItemFadeIn);
            View_BanGDream_OMCItem lastItem = equidistantLayoutScroll.equidistantLayoutGenerator.Items.Last().GetComponent<View_BanGDream_OMCItem>();
            lastItem.FadeIn();

            yield return new WaitForSeconds(transitionOutDelay);
            transitionOut.StartTransition();
        }
    }
}