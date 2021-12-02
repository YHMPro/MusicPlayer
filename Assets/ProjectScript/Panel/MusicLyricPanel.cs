using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farme.UI;
using UnityEngine.UI;

namespace MusicPlayer.Panel
{
    public class MusicLyricPanel : BasePanel
    {
        /// <summary>
        /// 歌词列表
        /// </summary>
        private List<Transform> m_LyricList = null;
        private int m_LyricTopUsingIndex = 0;//顶部
        private int m_LyricBottomUsingIndex = 0;//底部
        /// <summary>
        /// 歌词(行)的最大数量
        /// </summary>
        private int m_LyricMax = 10;
        /// <summary>
        /// 滑动条
        /// </summary>
        private Scrollbar m_LyricBar = null;
        /// <summary>
        /// 歌词滚动界面
        /// </summary>
        private ScrollRect m_LyricScrollRect = null;
        /// <summary>
        /// 歌词列表
        /// </summary>
        private RectTransform m_LyricRoom = null;
        /// <summary>
        /// 顶部歌词
        /// </summary>
        private RectTransform m_Top;
        /// <summary>
        /// 底部歌词
        /// </summary>
        private RectTransform m_Bottom;




        protected override void Awake()
        {
            base.Awake();
            RegisterComponentsTypes<Scrollbar>();
            RegisterComponentsTypes<Image>();


            m_LyricRoom = GetComponent<Image>("LyricRoom").rectTransform;
            m_LyricScrollRect = GetComponent<ScrollRect>("LyricScrollView");
            m_LyricBar = GetComponent<Scrollbar>("LyricBar");
        }

        protected override void Start()
        {
            base.Start();
            m_LyricScrollRect.onValueChanged.AddListener(LyricScrollRectEvent);
        }

        protected override void OnDestroy()
        {
            m_LyricScrollRect.onValueChanged.RemoveListener(LyricScrollRectEvent);
            base.OnDestroy();
        }
        /// <summary>
        /// 歌词列表初始化
        /// </summary>
        private void LyricListInit()
        {
            for(int i =0;i<m_LyricMax;)
        }

        public void RefreshPanel()
        {
            foreach(var lyricTran in m_LyricList)
            {
                lyricTran.GetComponent<LyricUI>().RefreshLyric();
            }
        }

        private void LyricScrollRectEvent(Vector2 pos)
        {

        }
    }
}
