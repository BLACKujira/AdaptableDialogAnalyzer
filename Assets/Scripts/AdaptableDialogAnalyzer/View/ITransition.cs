using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptableDialogAnalyzer.View
{

    public interface ITransition
    {
        void StartTransition();

        /// <summary>
        /// 在过度的正中间，画面被完全覆盖时执行
        /// 如执行切换页面
        /// </summary>
        event Action OnTransitionMiddle;

        /// <summary>
        /// 在转场播放结束后执行
        /// </summary>
        event Action OnTransitionEnd;
    }
}
