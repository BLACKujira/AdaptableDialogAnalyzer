using AdaptableDialogAnalyzer.DataStructures;

namespace AdaptableDialogAnalyzer.Unity
{
    /// <summary>
    /// ��ʾ�Ի����ṩ�༭���ܣ��̳д�����ʹ�ò�ͬ��ɸѡ��ʽ�Ͱ�ť����
    /// </summary>
    public abstract class MentionCountDialogueEditor : CountDialogueEditor
    {
        protected MentionedCountMatrix MentionedCountMatrix => (MentionedCountMatrix)CountMatrix;
    }
}