namespace AdaptableDialogAnalyzer.Unity.UIElements
{
    public interface IAutoSortBarChartData
    {
        int Id { get; }
        int ValueMax { get; }
        int Value { get; }
        IAutoSortBarChartData Lerp(IAutoSortBarChartData target, float t);
    }

}