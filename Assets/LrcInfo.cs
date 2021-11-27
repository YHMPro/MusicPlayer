//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Text;
//using System.Text.RegularExpressions;
//using UnityEngine;
//using Farme.Net;
//using UnityEngine.Events;
//namespace MusicPlayer
//{
//    public class LrcInfo
//    {
//        private string m_Title = "未知";
//        private string m_Artist = "未知";
//        private string m_Album = "未知";
//        private string m_LrcBy = "未知";
//        private Dictionary<double, string> m_LrcWord = new Dictionary<double, string>();
//        /// <summary>
//        /// 歌曲
//        /// </summary>
//        public string Title { get { return m_Title; } set { m_Title = value; } }
//        /// <summary>
//        /// 艺术家
//        /// </summary>
//        public string Artist { get { return m_Artist; } set { m_Artist = value; } }
//        /// <summary>
//        /// 专辑
//        /// </summary>
//        public string Album { get { return m_Album; } set { m_Album = value; } }
//        /// <summary>
//        /// 歌词作者
//        /// </summary>
//        public string LrcBy { get { return m_LrcBy; } set { m_LrcBy = value; } }
//        /// <summary>
//        /// 偏移量
//        /// </summary>
//        public string Offset { get; set; }
//        /// <summary>
//        /// 歌词
//        /// </summary>
//        public Dictionary<double, string> LrcWord
//        {
//            get
//            {             
//                return m_LrcWord;
//            }
//        }



//        /// <summary>
//        /// 获得歌词信息
//        /// </summary>
//        /// <param name="lrcURL">歌词绝对路径地址</param>   
//        /// <param name="resultCallback">结果回调：歌词信息</param>   
//        public static void GetLrc(string lrcURL,UnityAction<LrcInfo> resultCallback)
//        {      
//            WebDownloadTool.WebDownloadText(lrcURL, (lrcContent) =>
//            {
//                if (lrcContent != null)
//                {
//                    LrcInfo lrc = new LrcInfo();//实例歌词信息
//                    StringReader sr = new StringReader(lrcContent);//创建字符串读取工具实例
//                    string[] lrcContentLines = SplitString(lrcContent);//转换成一行对应一组数据
//                    string lrcLine;
//                    while ((lrcLine = sr.ReadLine()) != null)
//                    {
//                        if (lrcLine.StartsWith("[ti:"))
//                        {
//                            lrc.Title = SplitInfo(lrcLine);
//                        }
//                        else if (lrcLine.StartsWith("[ar:"))
//                        {
//                            lrc.Artist = SplitInfo(lrcLine);
//                        }
//                        else if (lrcLine.StartsWith("[al:"))
//                        {
//                            lrc.Album = SplitInfo(lrcLine);
//                        }
//                        else if (lrcLine.StartsWith("[by:"))
//                        {
//                            lrc.LrcBy = SplitInfo(lrcLine);
//                        }
//                        else if (lrcLine.StartsWith("[offset:"))
//                        {
//                            lrc.Offset = SplitInfo(lrcLine);
//                        }
//                        else
//                        {
//                            if (lrcLine[0] == '[' && Regex.IsMatch(lrcLine, @"\S{10,}"))//匹配满10位以上
//                            {
//                                Regex regex = new Regex(@"\[([0-9.:]*)\]+(.*)", RegexOptions.Compiled);
//                                MatchCollection mc = regex.Matches(lrcLine);
//                                double time = TimeSpan.Parse("00:" + mc[0].Groups[1].Value).TotalSeconds;
//                                string word = mc[0].Groups[2].Value;
//                                lrc.LrcWord.Add(time, word);
//                            }
//                        }
//                    }
//                    sr.Close();
//                    resultCallback?.Invoke(lrc);
//                }
//             });                                     
//        }
      
        
//    }
//}