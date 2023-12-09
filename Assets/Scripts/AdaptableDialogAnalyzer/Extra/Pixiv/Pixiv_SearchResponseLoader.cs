using AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse;
using DG.Tweening;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    public class Pixiv_SearchResponseLoader : MonoBehaviour
    {
        /// <summary>
        /// 文件夹的组织结构
        /// </summary>
        public enum PathType
        {
            Folder_Tag_Type,
            Folder_Type,
            File_MergedResponse
        }

        [Header("Input")]
        public string path;
        public PathType folderStructureType = PathType.Folder_Tag_Type;
        [Header("Settings")]
        [Tooltip("在File_MergedResponse模式不适用")] public bool loadArtwork = true;
        [Tooltip("在File_MergedResponse模式不适用")] public bool loadNovel = true;
        public string removePostBefore;
        public string removePostAfter;
        public bool removeUseAI = false;
        public List<string> removeHasTags;

        MergedResponse mergedResponse = new MergedResponse();
        public MergedResponse MergedResponse
        {
            get
            {
                if (mergedResponse == null)
                {
                    Initialize();
                }
                return mergedResponse;
            }
        }

        #region 读取单个文件
        /// <summary>
        /// 读取插画和漫画搜索结果
        /// </summary>
        ArtworksRoot LoadArtworks(string file)
        {
            string json = File.ReadAllText(file);
            return JsonUtility.FromJson<ArtworksRoot>(json);
        }

        /// <summary>
        /// 读取插画搜索结果
        /// </summary>
        IllustrationsRoot LoadIllustrations(string file)
        {
            string json = File.ReadAllText(file);
            return JsonUtility.FromJson<IllustrationsRoot>(json);
        }

        /// <summary>
        /// 读取漫画搜索结果
        /// </summary>
        MangaRoot LoadManga(string file)
        {
            string json = File.ReadAllText(file);
            return JsonUtility.FromJson<MangaRoot>(json);
        }

        /// <summary>
        /// 读取小说搜索结果
        /// </summary>
        NovelsRoot LoadNovels(string file)
        {
            string json = File.ReadAllText(file);
            return JsonUtility.FromJson<NovelsRoot>(json);
        }
        #endregion

        /// <summary>
        /// 去重
        /// </summary>
        void Distinct()
        {
            mergedResponse.artworks = mergedResponse.artworks
                .GroupBy(a=>a.id)
                .Select(a=>a.First())
                .ToList();

            mergedResponse.novels = mergedResponse.novels
                .GroupBy(a => a.id)
                .Select(a => a.First())
                .ToList();
        }

        /// <summary>
        /// 将列表中的所有文件作为某一类型的结果读取并添加到合并结果中
        /// </summary>
        void LoadAndAdd(string[] files, ResponseType responseType)
        {
            foreach (string file in files)
            {
                List<Extra.Pixiv.SearchResponse.Artwork.DataItem> artworks = new List<Extra.Pixiv.SearchResponse.Artwork.DataItem>();
                List<Extra.Pixiv.SearchResponse.Novel.DataItem> novels = new List<Extra.Pixiv.SearchResponse.Novel.DataItem>();

                switch (responseType)
                {
                    case ResponseType.artworks:
                        artworks.AddRange(LoadArtworks(file).body.illustManga.data);
                        break;
                    case ResponseType.illustrations:
                        artworks.AddRange(LoadIllustrations(file).body.illust.data);
                        break;
                    case ResponseType.manga:
                        artworks.AddRange(LoadManga(file).body.manga.data);
                        break;
                    case ResponseType.novels:
                        novels.AddRange(LoadNovels(file).body.novel.data);
                        break;
                }

                mergedResponse.artworks.AddRange(artworks);
                mergedResponse.novels.AddRange(novels);
            }
        }

        /// <summary>
        /// 读取以 标签/类型 结构组织的文件夹
        /// </summary>
        void LoadFile_TagType()
        {
            string[] tagFolders = Directory.GetDirectories(path);
            foreach (var tagFolder in tagFolders)
            {
                string illustFolder = Path.Combine(tagFolder, "illust");
                string mangaFolder = Path.Combine(tagFolder, "manga");
                string novelFolder = Path.Combine(tagFolder, "novel");

                if(loadArtwork && Directory.Exists(illustFolder))
                {
                    LoadAndAdd(Directory.GetFiles(illustFolder), ResponseType.illustrations);
                }
                if (loadArtwork && Directory.Exists(mangaFolder))
                {
                    LoadAndAdd(Directory.GetFiles(mangaFolder), ResponseType.manga);
                }
                if (loadNovel && Directory.Exists(novelFolder))
                {
                    LoadAndAdd(Directory.GetFiles(novelFolder), ResponseType.novels);
                }
            }
        }

        void LoadFile_Type(string tagFolder)
        {
            string illustFolder = Path.Combine(tagFolder, "illust");
            string mangaFolder = Path.Combine(tagFolder, "manga");
            string novelFolder = Path.Combine(tagFolder, "novel");

            if (loadArtwork && Directory.Exists(illustFolder))
            {
                LoadAndAdd(Directory.GetFiles(illustFolder), ResponseType.illustrations);
            }
            if (loadArtwork && Directory.Exists(mangaFolder))
            {
                LoadAndAdd(Directory.GetFiles(mangaFolder), ResponseType.manga);
            }
            if (loadNovel && Directory.Exists(novelFolder))
            {
                LoadAndAdd(Directory.GetFiles(novelFolder), ResponseType.novels);
            }
        }

        /// <summary>
        /// 读取合并的文件
        /// </summary>
        void LoadFile_MergedResponse()
        {
            using (var fileStream = File.OpenRead(path))
            {
                mergedResponse = Serializer.Deserialize<MergedResponse>(fileStream);
            }
        }

        /// <summary>
        /// 读取文件
        /// </summary>
        void Initialize()
        {
            switch (folderStructureType)
            {
                case PathType.Folder_Tag_Type:
                    LoadFile_TagType();
                    break;
                case PathType.File_MergedResponse:
                    LoadFile_MergedResponse();
                    break;
                case PathType.Folder_Type:
                    LoadFile_Type(path);
                    break;
            }

            Distinct();
            ApplyFilters();
        }

        /// <summary>
        /// 按投稿日期排序
        /// </summary>
        public void Sort()
        {
            mergedResponse.artworks = MergedResponse.artworks
                .OrderBy(a => DateTime.Parse(a.createDate))
                .ToList();

            mergedResponse.novels = MergedResponse.novels
                .OrderBy(a => DateTime.Parse(a.createDate))
                .ToList();
        }

        public void ApplyFilters()
        {
            if (removeUseAI) mergedResponse.RemoveUseAI();
        }
    }
}