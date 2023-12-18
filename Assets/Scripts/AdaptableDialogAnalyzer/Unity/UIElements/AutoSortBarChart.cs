using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity.UIElements
{
    public abstract class AutoSortBarChart : MonoBehaviour
    {
        [Header("Components")]
        public RectTransform barContainerTransform;
        [Header("Settings")]
        public Direction2 direction = Direction2.Horizontal;
        public int maxBarCount = 10;
        public float distance = 30;
        public float dataFramePerSec = 3;
        public float fadeOutMoveDistance = 30;
        [Header("Prefab")]
        public AutoSortBarChart_Bar barPrefab;

        /// <summary>
        /// 上一帧位置，目标位置，这一帧的时间，返回这一帧的位置
        /// </summary>
        public Func<Vector2, Vector2, float, Vector2> moveFunc = DefaultMoveFunc;

        /// <summary>
        /// 当前在图表中显示的条
        /// </summary>
        protected List<BarManager> activeBars = new List<BarManager>();

        float currentDataFrame = 0;
        public float CurrentDataFrame => currentDataFrame;

        /// <summary>
        /// 条管理类
        /// </summary>
        protected class BarManager
        {
            public AutoSortBarChart_Bar bar;
            public Func<Vector2, Vector2, float, Vector2> moveFunc;

            int id;
            public int Id => id;

            bool isRecycling = false;
            public bool IsRecycling => isRecycling;

            Vector2 targetPosition;
            public Vector2 TargetPosition
            {
                get
                {
                    return targetPosition;
                }
                set
                {
                    if (isRecycling)
                    {
                        Debug.Log("淡出时不可移动");
                        return;
                    }
                    targetPosition = value;
                }
            }

            public BarManager(AutoSortBarChart autoSortBarChart, AutoSortBarChart_Bar bar, int id)
            {
                this.bar = bar;
                this.id = id;
                moveFunc = autoSortBarChart.moveFunc;
                targetPosition = bar.barTransform.anchoredPosition;
            }

            /// <summary>
            /// 计算这一帧的位置并移动
            /// </summary>
            public void Move(float deltaTime)
            {
                bar.barTransform.anchoredPosition = moveFunc(bar.barTransform.anchoredPosition, targetPosition, deltaTime);
            }

            /// <summary>
            /// 淡出并移除此条。
            /// 注意：此条不会立即移除，而是在淡出结束后移除。
            /// 在淡出使依旧需要每帧提供新的数据。
            /// 淡出时不可移动，需要在淡出前设置消失时的位置。
            /// </summary>
            /// <param name="activeBars"></param>
            public void Recycle(List<BarManager> activeBars)
            {
                isRecycling = true;
                bar.FadeOut().OnComplete(() => activeBars.Remove(this));
            }
        }

        /// <summary>
        /// 获取第dataFrame个数据帧的数据
        /// </summary>
        protected abstract List<IAutoSortBarChartData> GetDataFrame(int dataFrame);

        /// <summary>
        /// 获取数据帧总数，当达到最后一个数据帧时，会停止播放。
        /// </summary>
        protected abstract int GetTotalDataFrames();

        /// <summary>
        /// 获得第rank个条的目标位置
        /// </summary>
        protected virtual Vector2 GetTargetPosition(int rank)
        {
            Vector2 targetPosition = new Vector2();
            if (direction == Direction2.Horizontal)
            {
                targetPosition.x = rank * distance;
            }
            else
            {
                targetPosition.y = rank * distance;
            }
            return targetPosition;
        }

        /// <summary>
        /// 对条进行的额外初始化，默认为空
        /// </summary>
        protected virtual void InitializeBar(AutoSortBarChart_Bar bar, IAutoSortBarChartData data)
        {

        }

        /// <summary>
        /// 向图表中添加一个条
        /// </summary>
        protected virtual AutoSortBarChart_Bar AddBar(IAutoSortBarChartData data)
        {
            int rank = activeBars.Count;
            AutoSortBarChart_Bar bar = Instantiate(barPrefab, barContainerTransform);
            InitializeBar(bar, data);
            bar.barTransform.anchoredPosition = GetTargetPosition(rank);
            bar.canvasGroup.alpha = 0;
            bar.FadeIn();

            BarManager barManager = new BarManager(this, bar, data.Id);
            activeBars.Add(barManager);
            return bar;
        }

        /// <summary>
        /// 对数据帧进行排序，默认按照Value降序排序
        /// </summary>
        protected virtual List<IAutoSortBarChartData> GetSortedDataFrame(List<IAutoSortBarChartData> dataFrame)
        {
            return dataFrame
                .OrderByDescending(df => df.Value)
                .ToList();
        }

        /// <summary>
        /// 获取插值后的数据帧
        /// </summary>
        List<IAutoSortBarChartData> GetLerpedDataFrame(float currentDataFrame)
        {
            List<IAutoSortBarChartData> lerpedDataFrame = new List<IAutoSortBarChartData>();
            List<IAutoSortBarChartData> dataFrame1 = GetDataFrame((int)currentDataFrame);
            List<IAutoSortBarChartData> dataFrame2 = GetDataFrame((int)currentDataFrame + 1);

            float t = currentDataFrame - (int)currentDataFrame;
            foreach (var data1 in dataFrame1)
            {
                IAutoSortBarChartData data2 = dataFrame2.Where(df => df.Id == data1.Id).FirstOrDefault();
                if (data2 == null)
                {
                    Debug.Log($"插值错误：在第{(int)currentDataFrame + 1}个数据帧中，找不到{data1.Id}对应的数据");
                    lerpedDataFrame.Add(data1);
                }
                else
                {
                    lerpedDataFrame.Add(data1.Lerp(data2, t));
                }
            }

            return lerpedDataFrame;
        }

        protected virtual void Play()
        {
            StartCoroutine(CoPlay());
        }

        protected virtual IEnumerator CoPlay()
        {
            while (true)
            {
                PlayFrame(currentDataFrame);
                currentDataFrame += Time.deltaTime * dataFramePerSec;
                yield return 1;
            }
        }

        protected virtual void PlayFrame(float currentDataFrame)
        {
            List<IAutoSortBarChartData> lerpedDataFrame;
            int totalDataFrames = GetTotalDataFrames();
            if (currentDataFrame >= totalDataFrames - 1)
            {
                lerpedDataFrame = GetDataFrame(totalDataFrames - 1);
            }
            else
            {
                lerpedDataFrame = GetLerpedDataFrame(currentDataFrame);
            }

            lerpedDataFrame = GetSortedDataFrame(lerpedDataFrame);
            float valueMax = lerpedDataFrame.Max(df => df.Value);

            // 淡出
            foreach (var barManager in activeBars)
            {
                if (barManager.IsRecycling)
                {
                    continue;
                }
                if (!lerpedDataFrame.Any(df => df.Id == barManager.Id) || lerpedDataFrame.FindIndex(df => df.Id == barManager.Id) >= maxBarCount)
                {
                    barManager.TargetPosition += direction == Direction2.Horizontal ? new Vector2(fadeOutMoveDistance, 0) : new Vector2(0, fadeOutMoveDistance); // 淡出时移动
                    barManager.Recycle(activeBars); // 淡出并移除
                }
            }

            // 淡入
            for (int i = 0; i < lerpedDataFrame.Count && i < maxBarCount; i++)
            {
                IAutoSortBarChartData data = lerpedDataFrame[i];
                if (!activeBars.Any(ab => !ab.IsRecycling && ab.Id == data.Id))
                {
                    AddBar(data);
                }
            }

            // 更新值
            foreach (var barManager in activeBars)
            {
                IAutoSortBarChartData data = lerpedDataFrame.Where(df => df.Id == barManager.Id).FirstOrDefault();
                if (data != null)
                {
                    barManager.bar.SetData(data, valueMax);
                    if (!barManager.IsRecycling)
                    {
                        barManager.TargetPosition = GetTargetPosition(lerpedDataFrame.IndexOf(data)); // 如果没有回收则更新位置
                    }
                }
            }

            // 移动
            foreach (var barManager in activeBars)
            {
                barManager.Move(Time.deltaTime);
            }
        }

        public static Vector2 DefaultMoveFunc(Vector2 lastPosition, Vector2 targetPosition, float deltaTime)
        {
            const float LERPT = 2f;
            const float MOVE_DELTA_ADD = 150F;

            Vector2 vector2 = Vector2.Lerp(lastPosition, targetPosition, LERPT * Mathf.Min(deltaTime, 1));
            vector2 = Vector2.MoveTowards(vector2, targetPosition, MOVE_DELTA_ADD * deltaTime);

            return vector2;
        }
    }
}