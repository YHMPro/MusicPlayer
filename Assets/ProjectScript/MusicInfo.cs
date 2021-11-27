using System.Collections;
using System.Collections.Generic;
using System.IO;
using Farme.Net;
using Farme.Extend;
using UnityEngine;

namespace MusicPlayer
{
    /// <summary>
    /// 音乐信息
    /// </summary>
    public class MusicInfo 
    {
        #region 字段       
        /// <summary>
        /// 歌曲封面
        /// </summary>
        private Sprite m_Sp = null;
        /// <summary>
        /// 歌曲名称
        /// </summary>
        private string m_MusicName = "";
        /// <summary>
        /// 歌手名称
        /// </summary>
        private string m_SingerName = "";
        /// <summary>
        /// 专辑名称
        /// </summary>
        private string m_AlbumName = "";
        /// <summary>
        /// 歌曲歌词
        /// </summary>
        private Dictionary<float, string> m_LyricDic = null;
        #endregion
        #region 属性
        /// <summary>
        /// 歌曲名称
        /// </summary>
        public string MusicName
        {
            get
            {
                return m_MusicName;
            }
        }
        /// <summary>
        /// 歌手名称
        /// </summary>
        public string SingerName
        {
            get
            {
                return m_SingerName;
            }
        }
        /// <summary>
        /// 专辑名称
        /// </summary>
        public string AlbumName
        {
            get
            {
                return m_AlbumName;
            }
        }
        #endregion

        /// <summary>
        /// 加载基础信息(列表显示时加载)  
        /// </summary>
        public void LoadBaseInfo(string musicFilePath)
        {
            if (!File.Exists(musicFilePath))
            {
                FromFilePathExtractBaseInfo(musicFilePath);
                return; }
            WebDownloadTool.WebDownloadText(musicFilePath, (info) =>
            {
                if (info != null)
                {
                    using (StringReader sr = new StringReader(info))//创建字符串读取工具实例
                    {
                        string lrcLine = null;
                        while ((lrcLine = sr.ReadLine()) != null)
                        {
                            if (lrcLine.StartsWith("[ti:"))
                            {
                                m_MusicName = MusicPlayerTool.SplitInfo(lrcLine);
                            }
                            else if (lrcLine.StartsWith("[ar:"))
                            {
                                m_SingerName = MusicPlayerTool.SplitInfo(lrcLine);
                            }
                            else if (lrcLine.StartsWith("[al:"))
                            {
                                m_AlbumName = MusicPlayerTool.SplitInfo(lrcLine);
                            }else
                            {
                                break;
                            }                          
                        }
                    }
                }
                FromFilePathExtractBaseInfo(musicFilePath);
            });
        }
        /// <summary>
        /// 加载歌词信息(播放歌曲时加载)
        /// </summary>
        public void LoadLyriInfo()
        {
           
        }
        /// <summary>
        /// 从文件路径中提取基础信息
        /// </summary>
        /// <param name="musicFilePath"></param>
        private void FromFilePathExtractBaseInfo(string musicFilePath)
        {
            if (string.IsNullOrEmpty(m_MusicName))
            {
                //从路径中提取歌曲名称与歌手名称  (不含后缀名称)
                string fileName = musicFilePath.AssignCharExtract('\\').Split('.')[0];
                if (!string.IsNullOrEmpty(fileName))
                {
                    string[] names = fileName.Split('-');
                    m_MusicName = names[0];
                    if (names.Length > 1)
                    {
                        m_SingerName = names[1];
                    }
                }
            }
        }
    }
}
