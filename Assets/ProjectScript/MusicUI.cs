using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farme;
using UnityEngine.UI;
namespace MusicPlayer
{
    /// <summary>
    /// 音乐UI
    /// </summary>
    public class MusicUI : BaseMono
    {
        public RectTransform rectTransform
        {
            get
            {
               return  transform as RectTransform;
            }
        }
        /// <summary>
        /// 用于获取这首歌的路径的钥匙
        /// </summary>
        private string m_MusicKey = "";
        protected override void Awake()
        {
            base.Awake();
            RegisterComponentsTypes<Text>();
        }

        protected override void Start()
        {
            base.Start();
            GetComponent<Button>().onClick.AddListener(DoubleClickEvent);
        }

        protected override void OnDestroy()
        {
            GetComponent<Button>().onClick.RemoveListener(DoubleClickEvent);
            base.OnDestroy();
        }
        /// <summary>
        /// 设置信息
        /// </summary>
        /// <param name="title">歌曲名称</param>
        /// <param name="artist">歌手名称</param>
        /// <param name="album">专辑名称</param>
        public void SetInfo(string title,string artist,string album)
        {
            string[] strNameArray = new string[] { "TitleContent", "ArtistContent", "AlbumContent" };
            foreach(var strName in  strNameArray)
            {
                if (GetComponent(strName, out Text contentText))
                {
                    string content = "";
                    switch (strName)
                    {
                        case "TitleContent":
                            {
                                content = title;
                                break;
                            }
                        case "ArtistContent":
                            {
                                content = artist;
                                break;
                            }
                        case "AlbumContent":
                            {
                                content = album;
                                break;
                            }
                    }
                    contentText.text = content;
                }
            }
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
                m_SingleClick = false;//直接重置为未点击
            }      
        }  




    }
}
