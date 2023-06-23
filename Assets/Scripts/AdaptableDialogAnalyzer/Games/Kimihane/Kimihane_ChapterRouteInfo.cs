using System.Collections.Generic;

namespace AdaptableDialogAnalyzer.Games.Kimihane
{
    public class Kimihane_ChapterRouteInfo
    {
        public enum Season { none, kimihane, kimikara }

        public Season season;
        public int route; //0=序章,1=共通线,12,13,23

        public static Dictionary<int, Kimihane_ChapterRouteInfo> directory = new Dictionary<int, Kimihane_ChapterRouteInfo>()
        {
            { 121, new Kimihane_ChapterRouteInfo(Season.kimihane, 0) },
            { 122, new Kimihane_ChapterRouteInfo(Season.kimihane, 0) },
            { 123, new Kimihane_ChapterRouteInfo(Season.kimihane, 0) },
            { 124, new Kimihane_ChapterRouteInfo(Season.kimihane, 1) },
            { 125, new Kimihane_ChapterRouteInfo(Season.kimihane, 1) }
        };

        public Kimihane_ChapterRouteInfo(Season season, int route)
        {
            this.season = season;
            this.route = route;
        }
    }
}