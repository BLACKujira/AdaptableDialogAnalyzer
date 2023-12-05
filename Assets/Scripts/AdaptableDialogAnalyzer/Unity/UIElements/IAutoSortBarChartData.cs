namespace AdaptableDialogAnalyzer.Unity.UIElements
{
    public interface IAutoSortBarChartData
    {
        int Id { get; }
        float Value { get; }
        IAutoSortBarChartData Lerp(IAutoSortBarChartData target, float t);
    }

}