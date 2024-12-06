using AdaptableDialogAnalyzer.DataStructures;
using AdaptableDialogAnalyzer.Unity;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_OMCMostChapter : MonoBehaviour, IInitializable, IFadeIn
    {
        [Header("Components")]
        public List<View_BanGDream_OMCMostChapter_Item> items;
        public View_BanGDream_OMCPercent_Title title;
        public SpriteRenderer srGaussian;
        public SpriteRenderer srTriangle;
        public Transform tfUIEffect;
        [Header("Adapter")]
        public ObjectMentionedCountMutiManagerLoader countManagerLoader;
        public int characterId = 0;
        [Header("Time")]
        public float playDelay = 2f;
        public float itemFadeDelay = 5f; // 首个 item 淡入前的延迟
        public float bgFadeDuration = 1.0f; // 背景淡入的持续时间
        public float itemFadeInterval = 0.033f; // 每个 item 淡入之间的间隔时间
        public float triangleAlpha = 0.05f; // 三角形背景透明度
        public float holdDuration = 15f; // 保持每组数据的显示时间
        public float refadeDelay = 2f; // 每个 item 的重新淡入延迟时间

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
            ObjectMentionedCountMutiManager mentionedCountManager = countManagerLoader.MentionedCountManager;

            // 获取所有角色信息
            CharacterDefinition characterDefinition = GlobalConfig.CharacterDefinition;
            Character[] characters = characterDefinition.Characters;

            // 排序并准备数据
            Dictionary<ObjectMentionedCountMutiMatrix, int> countPerChapters = new Dictionary<ObjectMentionedCountMutiMatrix, int>();
            foreach (var countMatrix in mentionedCountManager.mentionedCountMatrices)
            {
                if(countMatrix[characterId] == null) continue;
                countPerChapters.Add(countMatrix, countMatrix[characterId].Count);
            }
            var orderedCountPerChapters = countPerChapters.OrderByDescending(kvp => kvp.Value).ToList();

            // 分配数据到 UI 元素
            for (int i = 0; i < items.Count; i++)
            {
                View_BanGDream_OMCMostChapter_Item currItem = items[i];
                ObjectMentionedCountMutiMatrix countMatrix = orderedCountPerChapters[i].Key;

                if (currItem != null)
                {
                    currItem.SetData(countMatrix, characterId, i + 1);
                }
                else
                {
                    currItem.gameObject.SetActive(false);
                }
            }
        }
    }
}