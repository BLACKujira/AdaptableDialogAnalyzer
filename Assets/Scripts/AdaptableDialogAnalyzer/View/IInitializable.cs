using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptableDialogAnalyzer.View
{
    /// <summary>
    /// 用于解耦图表与图表显示控制器，传递一个初始化函数
    /// </summary>
    public interface IInitializable
    {
        void Initialize();
    }
}
