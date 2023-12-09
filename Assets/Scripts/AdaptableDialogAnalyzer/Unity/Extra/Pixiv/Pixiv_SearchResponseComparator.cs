using AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    public class Pixiv_SearchResponseComparator : TaskWindow
    {
        [Header("Components")]
        public Pixiv_SearchResponseLoader searchResponseLoaderA;
        public Pixiv_SearchResponseLoader searchResponseLoaderB;

        MergedResponse mergedResponseA;
        MergedResponse mergedResponseB;

        private void Start()
        {
            mergedResponseA = searchResponseLoaderA.MergedResponse;
            mergedResponseB = searchResponseLoaderB.MergedResponse;

            CheckArtworks();
            Priority = 1;
            Progress = $"完成";
        }

        void CheckArtworks()
        {
            HashSet<Extra.Pixiv.SearchResponse.Artwork.DataItem> artworksAHasBMissing = new HashSet<Extra.Pixiv.SearchResponse.Artwork.DataItem>();
            HashSet<Extra.Pixiv.SearchResponse.Artwork.DataItem> artworksBHasAMissing = new HashSet<Extra.Pixiv.SearchResponse.Artwork.DataItem>();

            HashSet<string> artworksAIds = new HashSet<string>(mergedResponseA.artworks.Select(a=>a.id));
            HashSet<string> artworksBIds = new HashSet<string>(mergedResponseB.artworks.Select(a=>a.id));

            DateTime artworksAHasBMissingStartDate = DateTime.MinValue;
            DateTime artworksAHasBMissingEndDate = DateTime.MinValue;
            DateTime artworksBHasAMissingStartDate = DateTime.MinValue;
            DateTime artworksBHasAMissingEndDate = DateTime.MinValue;

            foreach (var artwork in mergedResponseA.artworks)
            {
                if (!artworksBIds.Contains(artwork.id))
                {
                    artworksAHasBMissing.Add(artwork);
                    if(artworksAHasBMissingStartDate == DateTime.MinValue || artworksAHasBMissingStartDate > artwork.CreateDate)
                    {
                        artworksAHasBMissingStartDate = artwork.CreateDate;
                    }
                    if(artworksAHasBMissingEndDate == DateTime.MinValue || artworksAHasBMissingEndDate < artwork.CreateDate)
                    {
                        artworksAHasBMissingEndDate = artwork.CreateDate;
                    }
                }
            }

            foreach (var artwork in mergedResponseB.artworks)
            {
                if (!artworksAIds.Contains(artwork.id))
                {
                    artworksBHasAMissing.Add(artwork);
                    if (artworksBHasAMissingStartDate == DateTime.MinValue || artworksBHasAMissingStartDate > artwork.CreateDate)
                    {
                        artworksBHasAMissingStartDate = artwork.CreateDate;
                    }
                    if (artworksBHasAMissingEndDate == DateTime.MinValue || artworksBHasAMissingEndDate < artwork.CreateDate)
                    {
                        artworksBHasAMissingEndDate = artwork.CreateDate;
                    }
                }
            }

            Debug.Log($"样本集A艺术作品数量：{mergedResponseA.artworks.Count}");
            Debug.Log($"样本集B艺术作品数量：{mergedResponseB.artworks.Count}");

            Debug.Log($"样本集A拥有而B缺失的艺术作品：{string.Join(" ", artworksAHasBMissing.Select(a=>a.id))}");
            Debug.Log($"样本集B拥有而A缺失的艺术作品：{string.Join(" ", artworksBHasAMissing.Select(a=>a.id))}");

            Debug.Log($"样本集A拥有而B缺失的艺术作品范围：{artworksAHasBMissingStartDate} - {artworksAHasBMissingEndDate}");
            Debug.Log($"样本集B拥有而A缺失的艺术作品范围：{artworksBHasAMissingStartDate} - {artworksBHasAMissingEndDate}");
        }
    }
}
