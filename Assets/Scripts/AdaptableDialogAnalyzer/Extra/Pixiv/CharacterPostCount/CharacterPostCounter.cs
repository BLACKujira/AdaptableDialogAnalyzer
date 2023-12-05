using AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse;
using System;

namespace AdaptableDialogAnalyzer.Extra.Pixiv.CharacterPostCount
{
    public class CharacterPostCounter
    {
        /// <summary>
        /// ���ش���Ʒ�Ľ�ɫID����
        /// </summary>
        public Func<SearchResponse.Artwork.DataItem, int[]> getArtworkCharacters;
        /// <summary>
        /// ���ش�С˵�Ľ�ɫID����
        /// </summary>
        public Func<SearchResponse.Novel.DataItem, int[]> getNovelCharacters;
    
        /// <summary>
        /// ͳ�Ʋ�����ͳ�ƽ��
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