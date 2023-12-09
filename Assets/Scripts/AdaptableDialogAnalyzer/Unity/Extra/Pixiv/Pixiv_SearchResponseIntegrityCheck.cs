using AdaptableDialogAnalyzer.Extra.Pixiv.SearchResponse;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace AdaptableDialogAnalyzer.Unity
{
    public class Pixiv_SearchResponseIntegrityCheck : TaskWindow
    {
        [Header("Settings")]
        public string sampleFolder;
        public string saveLogFile;

        List<string> messages = new List<string>();

        private void Start()
        {
            string[] tagFolders = Directory.GetDirectories(sampleFolder);
            foreach (var folder in tagFolders)
            {
                Checker checker = new Checker(folder, this);
                checker.Check();
            }

            Priority = 1;
            Progress = $"完成";

            if(!string.IsNullOrEmpty(saveLogFile))
            {
                string messageText = string.Join("\n", messages);
                File.WriteAllText(saveLogFile, messageText);
                Debug.Log($"日志已保存到{saveLogFile}");
            }
        }

        public class Checker
        {
            Pixiv_SearchResponseIntegrityCheck owner;
            string targetTagFolder;
            string tag;



            public Checker(string targetTagFolder, Pixiv_SearchResponseIntegrityCheck owner)
            {
                this.owner = owner;
                this.targetTagFolder = targetTagFolder;
                tag = Path.GetFileName(targetTagFolder);
            }

            public void Check()
            {
                string illustFolder = Path.Combine(targetTagFolder, "illust");
                string mangaFolder = Path.Combine(targetTagFolder, "manga");
                string novelFolder = Path.Combine(targetTagFolder, "novel");

                Check<IllustrationsRoot>(illustFolder);
                Check<MangaRoot>(mangaFolder);
                Check<NovelsRoot>(novelFolder);
            }

            int GetLastPage(object obj)
            {
                if(obj is IllustrationsRoot illustrationsRoot)
                {
                    return illustrationsRoot.body.illust.lastPage;
                }
                else if(obj is MangaRoot mangaRoot)
                {
                    return mangaRoot.body.manga.lastPage;
                }
                else if(obj is NovelsRoot novelsRoot)
                {
                    return novelsRoot.body.novel.lastPage;
                }
                else
                {
                    throw new System.Exception("未知类型");
                }
            }

            bool IsResponseEmpty(object obj)
            {
                if (obj is IllustrationsRoot illustrationsRoot)
                {
                    return illustrationsRoot.body.illust.data.Count == 0;
                }
                else if (obj is MangaRoot mangaRoot)
                {
                    return mangaRoot.body.manga.data.Count == 0;
                }
                else if (obj is NovelsRoot novelsRoot)
                {
                    return novelsRoot.body.novel.data.Count == 0;
                }
                else
                {
                    throw new System.Exception("未知类型");
                }
            }

            int GetTotal(object obj)
            {
                if (obj is IllustrationsRoot illustrationsRoot)
                {
                    return illustrationsRoot.body.illust.total;
                }
                else if (obj is MangaRoot mangaRoot)
                {
                    return mangaRoot.body.manga.total;
                }
                else if (obj is NovelsRoot novelsRoot)
                {
                    return novelsRoot.body.novel.total;
                }
                else
                {
                    throw new System.Exception("未知类型");
                }
            }

            bool GetError(object obj)
            {
                if (obj is IllustrationsRoot illustrationsRoot)
                {
                    return illustrationsRoot.error;
                }
                else if (obj is MangaRoot mangaRoot)
                {
                    return mangaRoot.error;
                }
                else if (obj is NovelsRoot novelsRoot)
                {
                    return novelsRoot.error;
                }
                else
                {
                    throw new System.Exception("未知类型");
                }
            }

            void Check<T>(string folder)
            {
                string type = Path.GetFileName(folder);

                int lastPage = -1;
                int maxLastPage = -1;

                int lastTotal = -1;
                int minTotal = -1;
                int maxTotal = -1;

                string[] files = Directory.GetFiles(folder);
                Dictionary<int, T> rootDic = new Dictionary<int, T>();
                foreach (var file in files)
                {
                    string fileName = Path.GetFileNameWithoutExtension(file);
                    if (!int.TryParse(fileName, out int index))
                    {
                        if (fileName.StartsWith("d_")) 
                        {
                            owner.Log($"tag[{tag}] type[{type}]: 暂不支持判断倒序");
                        }
                        owner.Log($"tag[{tag}] type[{type}]: 文件名{fileName}不是数字");
                        continue;
                    }

                    string json = File.ReadAllText(file);
                    T root = JsonUtility.FromJson<T>(json);
                    rootDic[index] = root;
                }

                var roots = rootDic
                    .OrderBy(kvp => kvp.Key)
                    .ToArray();


                foreach (var kvp in roots)
                {
                    T root = kvp.Value;
                    string fileName = kvp.Key.ToString();

                    if(IsResponseEmpty(root))
                    {
                        owner.Log($"tag[{tag}] type[{type}]: 文件{fileName}响应为空");
                    }

                    int currentLastPage = GetLastPage(root);

                    // 判断是否有错误
                    bool error = GetError(root);                    
                    if(error)
                    {
                        owner.Log($"tag[{tag}] type[{type}]: 文件{fileName}响应有错误");
                    }

                    // 判断lastpage是否发生了变更
                    if (lastPage != -1 && lastPage != currentLastPage)
                    {
                        owner.Log($"tag[{tag}] type[{type}]: lastPage在文件{fileName}发生了变更：{lastPage}->{currentLastPage}");
                    }

                    // 记录当前页数和最大页数
                    if(currentLastPage > maxLastPage)
                    {
                        maxLastPage = currentLastPage;
                    }
                    lastPage = currentLastPage;

                    int currentTotal = GetTotal(root);

                    // 检查total是否负增长
                    if(lastTotal != -1 &&  currentTotal < lastTotal)
                    {
                        owner.Log($"tag[{tag}] type[{type}]: total在文件{fileName}发生了负增长：{lastTotal}->{currentTotal}");
                    }

                    // 记录当前total和最大、最小total
                    if(minTotal == -1 || minTotal > currentTotal)
                    {
                        minTotal = currentTotal;
                    }
                    if(maxTotal == -1 || maxTotal < currentTotal)
                    {
                        maxTotal = currentTotal;
                    }
                    lastTotal = currentTotal;
                }

                // 检查maxLastPage和最大的文件名是否一致
                int maxFileName = rootDic.Max(kvp => kvp.Key);
                if (maxLastPage != maxFileName)
                {
                    owner.Log($"tag[{tag}] type[{type}]: 最大的lastPage和最大的文件名不一致：{maxLastPage}->{maxFileName}");
                }
            }
        }

        void Log(string message)
        {
            Debug.Log(message);
            messages.Add(message);
        }
    }
}