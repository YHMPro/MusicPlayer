using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace MusicPlayer.Manager
{
    /// <summary>
    /// 音乐数据管理器
    /// </summary>
    public class MusicDataManager 
    {
        /// <summary>
        /// 音乐数据容器  用于缓存
        /// </summary>
        private static Dictionary<string, MusicData> m_MusicDataDic = new Dictionary<string, MusicData>();

       



        public static MusicData GetMusicData(string musicFileName)
        {
            if(m_MusicDataDic.TryGetValue(musicFileName,out MusicData data))
            {
                return data;
            }
            MusicData musicData = new MusicData();
            m_MusicDataDic.Add(musicFileName, musicData);
            return musicData;
        }

        public static void RemoveMusicData(string musicFileName)
        {
            //重置MusicData数据
        }

    }
}
