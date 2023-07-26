namespace AdaptableDialogAnalyzer.View.BanGDream
{
    public class View_BanGDream_SumItem : View_BanGDream_TriadItem
    {
        public void SetData(int characterAId, int characterBId, int countAToB, int countBToA)
        {
            SetGraphics(characterAId, characterBId);
            txtTotal.text = (countAToB + countBToA).ToString();
            txtAToB.text = countAToB.ToString();
            txtBToA.text = countBToA.ToString();
        }
    }
}