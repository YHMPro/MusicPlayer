using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farme.Tool;
using System;
using MusicPlayer.Manager;
using Farme.Net;

namespace MusicPlayer
{
    [Serializable]
    /// <summary>
    /// 音乐播放器数据
    /// </summary>
    public class MusicPlayerData 
    {
        /// <summary>
        /// 音乐文件后缀
        /// </summary>
        public static string MusicFileSuffix
        {
            get
            {
                return ".mp3";
            }
        }
        /// <summary>
        /// 歌词文件后缀
        /// </summary>
        public static string LyrlcFileSuffix
        {
            get
            {
                return ".lrc";
            }
        }
        /// <summary>
        /// 音乐文件夹路径
        /// </summary>
        private static string m_MusicFilePath = "";
        /// <summary>
        /// 音乐文件夹路径
        /// </summary>
        public static string MusicFilePath
        {
            get
            {
                return m_MusicFilePath;
            }
            set
            {
                if(!Equals(m_MusicFilePath,value))
                {
                    Debuger.Log("音乐文件路径刷新");
                    m_MusicFilePath = value;
                    //依照此路径进行查找
                    m_MusicFileNames=MusicPlayerTool.GetAllChildFileNames(m_MusicFilePath,"*"+MusicFileSuffix);
                    m_MusicFileNum = MusicFileNames.Length;
                    Debuger.Log("文件数量为:" + m_MusicFileNum);
                    MusicInfoManager.ClearData();
                    foreach(var fileName in MusicFileNames)
                    {
                        MusicInfoManager.AddMusicInfo(fileName);
                    }
                }
            }
        }
        /// <summary>
        /// 音乐文件名称数组
        /// </summary>
        private static string[] m_MusicFileNames = null;
        /// <summary>
        /// 音乐文件名称数组(不含文件后缀)
        /// </summary>
        public static string[] MusicFileNames
        {
            get
            {        
                if(m_MusicFileNames==null)
                {
                    m_MusicFileNames = new string[0];
                }
                return m_MusicFileNames;
            }
        }
        private static int m_MusicFileNum = 0;
        /// <summary>
        /// 音乐文件数量
        /// </summary>
        public static int MusicFileNum
        {
            get
            {
                return m_MusicFileNum;
            }
        }
        /// <summary>
        /// 当前播放的音乐索引
        /// </summary>
        private static int m_NowPlayMusicIndex = -1;
        /// <summary>
        /// 当前播放的音乐索引
        /// 0~检测到的文件最大数量
        /// </summary>
        public static int NowPlayMusicIndex
        {
            get
            {
                return m_NowPlayMusicIndex;
            }
            set
            {
                m_NowPlayMusicIndex = Mathf.Clamp(value,0, m_MusicFileNum-1);
            }
        }      
        /// <summary>
        /// 当前播放音乐文件的名称(不含后缀)
        /// </summary>
        public static string NowPlayMusicFileName
        {
            get
            {
                if(m_MusicFileNames!=null)
                {
                    if (m_NowPlayMusicIndex >= 0 && m_NowPlayMusicIndex < m_MusicFileNames.Length)
                    {
                        return m_MusicFileNames[m_NowPlayMusicIndex];
                    }
                }
                return "";
            }
        }
        /// <summary>
        /// 当前播放歌曲的信息
        /// </summary>
        private static MusicInfo m_NowPlayMusicInfo = null;
        /// <summary>
        /// 当前播放歌曲的信息
        /// </summary>
        public static MusicInfo NowPlayMusicInfo
        {
            get
            {
                return m_NowPlayMusicInfo;
            }
            set
            {
                m_NowPlayMusicInfo = value;
            }
        }
        /// <summary>
        /// 当前播放的歌曲是否为第一首
        /// </summary>
        public static bool NowPlayMusicIsStart
        {
            get
            {
                return m_NowPlayMusicIndex > 0;
            }
        }
        /// <summary>
        /// 当前播放的歌曲是否为最后一首
        /// </summary>
        public static bool NowPlayMusicIsEnd
        {
            get
            {
                return m_NowPlayMusicIndex < m_MusicFileNum - 1;
            }
        }
    }
}
