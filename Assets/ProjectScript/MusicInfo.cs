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
        /// 歌词时间
        /// </summary>
        private List<double> m_TimeList = null;
        /// <summary>
        /// 歌词内容
        /// </summary>
        private List<string> m_LyricList = null;       
        #endregion
        #region 属性
        /// <summary>
        /// 时间
        /// </summary>
        public List<double> TimeList
        {
            get
            {
                return m_TimeList;
            }
        }
        /// <summary>
        /// 歌词
        /// </summary>
        public List<string> LyricList
        {
            get
            {
                return m_LyricList;
            }
        }
        
        /// <summary>
        /// 封面
        /// </summary>
        public Sprite Album
        {
            get
            {
                if (m_Sp == null)
                {
                    return MusicPlayerData.DefaultCover;
                }
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
                m_TimeList = new List<double>() { 0 };
                m_LyricList = new List<string>() { "没有找到歌词" };
                callback?.Invoke();
                return;
            }
            m_TimeList = new List<double>();
            m_LyricList = new List<string>();
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
                                string content = mc[0].Groups[2].Value;
                                if(!string.IsNullOrEmpty(content))
                                {
                                    m_TimeList.Add(TimeSpan.Parse("00:" + mc[0].Groups[1].Value).TotalSeconds);
                                    m_LyricList.Add(content);
                                }                              
                            }                           
                        }
                    }
                    if(m_TimeList.Count==0)
                    {
                        m_TimeList.Add(0);
                        m_LyricList.Add("没有歌词");
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
                if (texture2D != null)
                {
                    m_Sp = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
                }
                else
                {
                    m_Sp= MusicPlayerData.DefaultCover;
                }
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
