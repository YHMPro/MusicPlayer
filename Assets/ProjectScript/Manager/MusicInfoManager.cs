using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MusicPlayer.Manager
{
    /// <summary>
    /// 歌曲信息管理
    /// </summary>
    public class MusicInfoManager 
    {
        /// <summary>
        /// 歌曲信息容器  key:歌曲名称+歌手名称+专辑名称   value:歌曲信息
        /// </summary>
        private static Dictionary<string, MusicInfo> m_MusicInfoDic = new Dictionary<string, MusicInfo>();
        /// <summary>
        /// 缓存歌曲信息(最多100首)
        /// </summary>
        /// <param name="musicFileName">音乐文件名称(含后缀名)</param>
        public static void AddMusicInfo(string musicFileName)
        {
            if(m_MusicInfoDic.ContainsKey(musicFileName))
            { return; }
            MusicInfo musicInfo = new MusicInfo();
            musicInfo.LoadBaseInfo(MusicPlayerData.MusicFilePath+ @"\" + musicFileName);
            m_MusicInfoDic.Add(musicFileName, musicInfo);     
        }
        /// <summary>
        /// 获取音乐信息
        /// </summary>
        /// <param name="musicFileName">音乐文件名称(含后缀名)</param>
        /// <returns></returns>
        public static MusicInfo GetMusicInfo(string musicFileName)
        {
            if (m_MusicInfoDic.TryGetValue(musicFileName, out MusicInfo musicInfo))
            {
                return musicInfo;
            }
            return null;
        }
        /// <summary>
        /// 清除数据
        /// </summary>
        public static void ClearData()
        {
            m_MusicInfoDic.Clear();
        }
    }
}
