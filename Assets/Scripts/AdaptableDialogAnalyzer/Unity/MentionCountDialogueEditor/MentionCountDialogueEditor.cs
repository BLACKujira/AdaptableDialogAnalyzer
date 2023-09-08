using AdaptableDialogAnalyzer.DataStructures;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// 显示对话并提供编辑功能，继承此类已使用不同的筛选方式和按钮功能
    /// </summary>
    public abstract class MentionCountDialogueEditor : CountDialogueEditor
    {
        protected MentionedCountMatrix MentionedCountMatrix => (MentionedCountMatrix)CountMatrix;
    }
}