using System.Collections;
using System.Collections.Generic;
using System.IO;
using Farme.Net;
using Farme.Extend;
using UnityEngine;
using UnityEngine.Events;
using System.Text.RegularExpressions;
using System;

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
        private Dictionary<double, string> m_LyricDic = null;
        #endregion
        #region 属性
        /// <summary>
        /// 歌曲歌词
        /// </summary>
        public Dictionary<double,string> LyricDic
        {
            get
            {
                return m_LyricDic;
            }
        }
        /// <summary>
        /// 封面
        /// </summary>
        public Sprite Album
        {
            get
            {
                return m_Sp;
            }
        }
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
        /// 加载基础信息
        /// </summary>
        /// <param name="musicFileName">音乐文件名(不含后缀名)</param>
        public void LoadBaseInfo(string musicFileName)
        {
            string musicFilePath = MusicPlayerData.MusicFilePath + @"\" + musicFileName + MusicPlayerData.LyrlcFileSuffix;
            if (!File.Exists(musicFilePath))
            {
                FromFilePathExtractBaseInfo(musicFileName);
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
                FromFilePathExtractBaseInfo(musicFileName);
            });
        }
        /// <summary>
        /// 加载歌词信息(播放歌曲时加载)
        /// <param name="musicFileName">音乐文件名(不含后缀名)</param>
        /// </summary>
        public void LoadLyriInfo(string musicFileName,UnityAction callback)
        {
            string musicFilePath = MusicPlayerData.MusicFilePath + @"\" + musicFileName + MusicPlayerData.LyrlcFileSuffix;
            if (!File.Exists(musicFilePath))
            {
                m_LyricDic = new Dictionary<double, string>();
                m_LyricDic.Add(0, "没有找到歌词");
                callback?.Invoke();
                return;
            }
            m_LyricDic = new Dictionary<double, string>();
            WebDownloadTool.WebDownloadText(musicFilePath, (info) =>
            {
                if (info != null)
                {
                    using (StringReader sr = new StringReader(info))//创建字符串读取工具实例
                    {
                        string lrcLine = null;
                        while ((lrcLine = sr.ReadLine()) != null)
                        {
                            if(MusicPlayerTool.MatchWord(lrcLine))
                            {
                                Regex regex = new Regex(@"\[([0-9.:]*)\]+(.*)", RegexOptions.Compiled);
                                MatchCollection mc = regex.Matches(lrcLine);
                                double time = TimeSpan.Parse("00:" + mc[0].Groups[1].Value).TotalSeconds;
                                string word = mc[0].Groups[2].Value;
                                m_LyricDic.Add(time, word);
                            }                           
                        }
                    }
                    if(m_LyricDic.Count==0)
                    {
                        m_LyricDic.Add(0, "没有歌词");
                    }
                    callback?.Invoke();
                }
            });
        }
        /// <summary>
        /// 加载封面
        /// </summary>
        /// <param name="musicFileName">音乐文件名称(不含后缀名，之后可能会将.mp3文件中的封面保存至本地在进行读取)</param>
        /// <param name="callback">加载完成后的回调</param>
        public void LoadAlbum(string musicFileName,UnityAction callback)
        {
            string musicFilePath = MusicPlayerData.MusicFilePath + @"\" + musicFileName + MusicPlayerData.MusicFileSuffix;
            if (!File.Exists(musicFilePath))
            {
                callback?.Invoke();
                return;
            }
            if(m_Sp!=null)
            {
                callback?.Invoke();
                return;
            }
            MusicPlayerTool.GetAlbumCover(musicFilePath, (texture2D) =>
            {
                m_Sp = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
                callback?.Invoke();
            });
        }
        /// <summary>
        /// 从文件路径中提取基础信息
        /// </summary>
        /// <param name="musicFileName">音乐文件名称(不含后缀名)</param>
        private void FromFilePathExtractBaseInfo(string musicFileName)
        {
            if (string.IsNullOrEmpty(m_MusicName))
            {
                string[] names = musicFileName.Split('-');
                m_MusicName = names[0];
                if (names.Length > 1)
                {
                    m_SingerName = names[1];
                }
            }
        }
    }
}
