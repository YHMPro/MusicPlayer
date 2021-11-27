using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farme.Tool;
using System;
using MusicPlayer.Manager;
namespace MusicPlayer
{
    [Serializable]
    /// <summary>
    /// 音乐播放器数据
    /// </summary>
    public class MusicPlayerData 
    {
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
                    m_MusicFileNames=MusicPlayerTool.GetAllChildFileNames(m_MusicFilePath, "*.mp3");
                    m_MusicFileNum = MusicFileNames.Length;
                    Debuger.Log("文件数量为:" + m_MusicFileNum);
                    MusicInfoManager.ClearData();
                    foreach(var fileName in MusicFileNames)
                    {
                        MusicInfoManager.AddMusicInfo(fileName + ".lrc");
                    }
                }
            }
        }
        /// <summary>
        /// 音乐文件名称数组
        /// </summary>
        private static string[] m_MusicFileNames = null;
        /// <summary>
        /// 音乐文件名称数组
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



        

       
    }
}
