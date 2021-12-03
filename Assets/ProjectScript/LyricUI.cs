using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MusicPlayer
{
    public class LyricUI : MonoBehaviour
    {
        private Text m_Lyric = null;
        private int m_Index = -1;



        public RectTransform rectTransform
        {
            get { return transform as RectTransform; }
        }
        private void Awake()
        {
            m_Lyric = GetComponent<Text>();
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void RefreshLyric(int index)
        {
            //当前歌词为所唱歌词则变成蓝色
            if(m_Lyric!=null)
            {
                m_Lyric.color = (index == m_Index) ? Color.yellow : Color.white;
            }
        }
        /// <summary>
        /// 设置歌词
        /// </summary>
        /// <param name="content">内容</param>
        /// <param name="index">索引</param>
        public void SetLyric(string content, int index)
        {
            m_Lyric.text = content;
            m_Index = index;
        }
    }
}
