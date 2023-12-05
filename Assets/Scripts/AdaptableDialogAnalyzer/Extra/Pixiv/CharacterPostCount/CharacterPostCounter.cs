using AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse;
using System;

namespace AdaptableDialogAnalyzer.Extra.Pixiv.CharacterPostCount
{
    public class CharacterPostCounter
    {
        /// <summary>
        /// 返回此作品的角色ID数组
        /// </summary>
        public Func<SearchResponse.Artwork.DataItem, int[]> getArtworkCharacters;
        /// <summary>
        /// 返回此小说的角色ID数组
        /// </summary>
        public Func<SearchResponse.Novel.DataItem, int[]> getNovelCharacters;
    
        /// <summary>
        /// 统计并返回统计结果
        /// </summary>
        public CharacterPostCountManager Count(MergedResponse mergedResponse)
        {
            CharacterPostCountManager characterPostCountManager = new CharacterPostCountManager(CountManagerType.Delta);

            foreach (var dataItem in mergedResponse.artworks)
            {
                int[] characterIds = getArtworkCharacters(dataItem);
                DateTime dateTime = DateTime.Parse(dataItem.createDate);
                foreach (var characterId in characterIds)
                {
                    characterPostCountManager.Add(dateTime.Date, characterId);
                }
            }

            foreach (var dataItem in mergedResponse.novels)
            {
                int[] characterIds = getNovelCharacters(dataItem);
                DateTime dateTime = DateTime.Parse(dataItem.createDate);
                foreach (var characterId in characterIds)
                {
                    characterPostCountManager.Add(dateTime.Date, characterId);
                }
            }

            return characterPostCountManager;
        }
    }
}