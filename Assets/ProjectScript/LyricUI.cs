using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MusicPlayer
{
    public class LyricUI : MonoBehaviour
    {
        private Text m_Lyric = null;
        /// <summary>
        /// 动态索引(代表歌词的实时索引,有可能与歌词进度不同,用于查阅歌词)
        /// </summary>
        private int m_DynamicIndex = -1;
        /// <summary>
        /// 进度索引(与歌曲进度同步)
        /// </summary>
        private int m_ProgressIndex = -1;

        public RectTransform rectTransform
        {
            get { return transform as RectTransform; }
        }
        private void Awake()
        {
            m_Lyric = GetComponent<Text>();
        }
       
        public void RefreshLyric(int index)
        {
            m_ProgressIndex = index;
            //当前歌词为所唱歌词则变成蓝色
            if (m_Lyric!=null)
            {
                m_Lyric.color = (index == m_DynamicIndex) ? Color.yellow : Color.white;
            }
            
        }
        /// <summary>
        /// 设置歌词
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="index">索引</param>
        public void SetLyric(string content,int index)
        {
            m_Lyric.text = content;
            m_DynamicIndex = index;
            if (m_Lyric != null)
            {
                m_Lyric.color = (index == m_ProgressIndex) ? Color.yellow : Color.white;
            }        
        }
    }
}
