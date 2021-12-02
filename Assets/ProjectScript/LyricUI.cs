using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MusicPlayer
{
    public class LyricUI : MonoBehaviour
    {
        private Text m_Lyric = null;
        private float m_Time = -1;
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

        public void RefreshLyric()
        {
            //当前歌词为所唱歌词则变成蓝色
        }

        public void SetLyric(string content,float time)
        {
            m_Lyric.text = content;
            m_Time = time;
        }
    }
}
