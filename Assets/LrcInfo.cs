using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Farme.Net;
using UnityEngine.Events;
namespace MusicPlayer
{
    public class LrcInfo
    {
        /// <summary>
        /// 歌曲
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 艺术家
        /// </summary>
        public string Artist { get; set; }
        /// <summary>
        /// 专辑
        /// </summary>
        public string Album { get; set; }
        /// <summary>
        /// 歌词作者
        /// </summary>
        public string LrcBy { get; set; }
        /// <summary>
        /// 偏移量
        /// </summary>
        public string Offset { get; set; }

        /// <summary>
        /// 歌词
        /// </summary>
        public Dictionary<double, string> LrcWord = new Dictionary<double, string>();

        /// <summary>
        /// 获得歌词信息
        /// </summary>
        /// <param name="lrcURL">歌词绝对路径地址</param>   
        /// <param name="resultCallback">结果回调：歌词信息</param>   
        public static void GetLrc(string lrcURL,UnityAction<LrcInfo> resultCallback)
        {      
            DownLoad.DownLoadTextAsset(lrcURL, (lrcContent) =>
            {
                LrcInfo lrc = new LrcInfo();//实例歌词信息
                StringReader sr = new StringReader(lrcContent);//创建字符串读取工具实例
                string[] lrcContentLines = SplitString(lrcContent);//转换成一行对应一组数据
                string lrcLine;
                while ((lrcLine=sr.ReadLine())!=null)
                {
                    if (lrcLine.StartsWith("[ti:"))
                    {
                        lrc.Title = SplitInfo(lrcLine);
                    }
                    else if (lrcLine.StartsWith("[ar:"))
                    {
                        lrc.Artist = SplitInfo(lrcLine);
                    }
                    else if (lrcLine.StartsWith("[al:"))
                    {
                        lrc.Album = SplitInfo(lrcLine);
                    }
                    else if (lrcLine.StartsWith("[by:"))
                    {
                        lrc.LrcBy = SplitInfo(lrcLine);
                    }
                    else if (lrcLine.StartsWith("[offset:"))
                    {
                        lrc.Offset = SplitInfo(lrcLine);
                    }
                    else
                    {                       
                        Regex regex = new Regex(@"\[([0-9.:]*)\]+(.*)", RegexOptions.Compiled);
                        MatchCollection mc = regex.Matches(lrcLine);
                        double time = TimeSpan.Parse("00:" + mc[0].Groups[1].Value).TotalSeconds;
                        string word = mc[0].Groups[2].Value;
                        lrc.LrcWord.Add(time, word);
                    }
                }              
                sr.Close();
                resultCallback?.Invoke(lrc);
             });                                     
        }

        /// <summary>
        /// 处理信息(私有方法)
        /// </summary>
        /// <param name="line"></param>
        /// <returns>返回基础信息</returns>
        private static string SplitInfo(string line)
        {
            return line.Substring(line.IndexOf(":") + 1).TrimEnd(']');
        }
        /// <summary>
        /// 分割字符串（换行符）
        /// </summary>
        /// <param name="content">字符串</param>
        /// <returns></returns>
        private static string[] SplitString(string content)
        {
            return content.Split(Environment.NewLine.ToCharArray());//new string[] { LINE }, StringSplitOptions.None);
        }
    }
}