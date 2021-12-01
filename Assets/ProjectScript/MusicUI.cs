using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farme;
using UnityEngine.UI;
using Farme.Net;
using MusicPlayer.Manager;
using Farme.UI;
using MusicPlayer.Panel;
namespace MusicPlayer
{
    /// <summary>
    /// 音乐UI
    /// </summary>
    public class MusicUI : BaseMono
    {
        private static MusicUI m_LastMusicUI = null;
        private Text m_MusicIndex;
        private Text m_MusicName;
        private Text m_SingerName;
        private Text m_AlbumName;
        private Button m_Btn;
        public RectTransform rectTransform
        {
            get
            {
               return  transform as RectTransform;
            }
        }
        /// <summary>
        /// 用于匹配音频文件的密钥
        /// </summary>
        //private int m_MusicIndex = -1;
        protected override void Awake()
        {
            base.Awake();
            RegisterComponentsTypes<Text>();
            m_Btn = GetComponent<Button>();
            m_Btn.onClick.AddListener(DoubleClickEvent);
            m_MusicIndex = GetComponent<Text>("Index");
            m_MusicName = GetComponent<Text>("TitleContent");
            m_SingerName = GetComponent<Text>("ArtistContent");
            m_AlbumName = GetComponent<Text>("AlbumContent");
        }

        protected override void Start()
        {
            base.Start();
               
        }
        protected override void OnDestroy()
        {
            m_Btn.onClick.RemoveListener(DoubleClickEvent);
            base.OnDestroy();
        }
        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="musicIndex">音乐索引</param>
        /// <param name="title">歌曲名称</param>
        /// <param name="artist">歌手名称</param>
        /// <param name="album">专辑名称</param>
        public void SetInfo(int musicIndex, string title,string artist,string album)
        {        
            m_MusicIndex.text = (musicIndex+1).ToString();
            m_MusicName.text = title.Trim();//去除字符串前后空字符在赋值
            m_SingerName.text = artist.Trim();//去除字符串前后空字符在赋值
            m_AlbumName.text = album.Trim();//去除字符串前后空字符在赋值       
            RefreshState();//更新状态信息
        }
        /// <summary>
        /// 单击
        /// </summary>
        private bool m_SingleClick = false;        
        /// <summary>
        /// 双击事件
        /// </summary>
        private void DoubleClickEvent()
        {
            if (!m_SingleClick)
            {
                m_SingleClick = true;
                //单击               
                MonoSingletonFactory<ShareMono>.GetSingleton().DelayAction(0.35f, () =>
                 {
                     if (m_SingleClick)
                     {
                         m_SingleClick = false;//重置为未点击
                     }
                 });                
            }
            else
            {                
                //双击
                if (GetComponent("TitleContent", out Text contentText))
                {
                    Debug.Log("播放>" + contentText.text+ "<音乐");
                }
                #region 更新数据
                MusicPlayerData.NowPlayMusicIndex = int.Parse(m_MusicIndex.text)-1;
                #endregion    
                MusicController.MusicPlay();            
                m_LastMusicUI = this;                  
                m_SingleClick = false;//直接重置为未点击
            }      
        }  
        /// <summary>
        /// 更新UI的状态  当设置的信息为已播放的音乐的信息，则显示为不可交互状态
        /// </summary>
        public void RefreshState()
        {
            m_Btn.interactable = ((int.Parse(m_MusicIndex.text) - 1) == MusicPlayerData.NowPlayMusicIndex) ? false : true;
        }
    }
}
