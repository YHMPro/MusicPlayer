using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farme.UI;
using UnityEngine.UI;
using MusicPlayer.Manager;
using Farme;
using Farme.Tool;
using DG.Tweening;
namespace MusicPlayer.Panel
{
    public class MusicLyricPanel : BasePanel
    {       
        /// <summary>
        /// 顶部歌词的索引
        /// </summary>
        private int m_TopLyricIndex = 0;
        /// <summary>
        /// 歌词列表
        /// </summary>
        private List<RectTransform> m_LyricList = null;
        private int m_LyricTopUsingIndex = 0;//顶部
        private int m_LyricBottomUsingIndex = 0;//底部
        /// <summary>
        /// 歌词(行)的最大数量
        /// </summary>
        private int m_LyricMax = 11;
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
        /// <summary>
        /// 歌词列表上一帧的位置(世界坐标)
        /// </summary>
        private float m_LyricLastPosY = 0;
        /// <summary>
        /// 歌词列表下一帧的位置(世界坐标)
        /// </summary>
        private float m_LyricNextPosY = 0;



        protected override void Awake()
        {
            base.Awake();
            RegisterComponentsTypes<Scrollbar>();
            RegisterComponentsTypes<Image>();
            RegisterComponentsTypes<ScrollRect>();

            m_LyricRoom = GetComponent<Image>("LyricRoom").rectTransform;
            m_LyricScrollRect = GetComponent<ScrollRect>("LyricScrollView");
            m_LyricBar = GetComponent<Scrollbar>("LyricBar");
        }

        protected override void Start()
        {
            base.Start();
            m_LyricScrollRect.onValueChanged.AddListener(LyricScrollRectEvent);        
            LyricListInit();
        }

        private void Update()
        {
            if (m_IsRoll)
            {
                //触发滚动事件的条件限制
                RollEvent();
            }
        }
        protected override void OnDestroy()
        {
            m_LyricScrollRect.onValueChanged.RemoveListener(LyricScrollRectEvent);
            base.OnDestroy();
        }
        /// <summary>
        /// 歌词列表初始化
        /// </summary>
        public void LyricListInit()
        {
            Debuger.Log("初始化歌词列表");
            //重置歌词列表为默认位置
            Vector3 localPos = m_LyricRoom.localPosition;
            localPos.y = 0;
            m_LyricRoom.localPosition = localPos;
            //设置歌词列表长度
            if (MusicPlayerData.NowPlayMusicInfo != null)
            {
                int lyricNum = MusicPlayerData.NowPlayMusicInfo.LyricList.Count;//歌词数量
                float fixWitch = 225f;//固定占用
                float lyricSumWitch = 50f * lyricNum-25f;//歌词总占用
                Vector2 sizeDelta = m_LyricRoom.sizeDelta;
                sizeDelta.y = lyricSumWitch + fixWitch;
                m_LyricRoom.sizeDelta = sizeDelta;
                if (m_LyricList == null)
                {
                    m_LyricList = new List<RectTransform>();
                    for (int index = 0; index < m_LyricMax; index++)
                    {
                        if (GoLoad.Take("UI/Lyric", out GameObject go, m_LyricRoom))
                        {
                            //获取引用
                            LyricUI lyricUI = go.GetComponent<LyricUI>();
                            //设置位置信息
                            lyricUI.rectTransform.localPosition = new Vector3(300f, -225 + (index * -50), 0);
                            //设置信息   通过歌词文件进行获取
                            lyricUI.SetLyric(MusicPlayerData.NowPlayMusicInfo.LyricList[index], index);
                            //添加引用
                            m_LyricList.Add(lyricUI.rectTransform);
                        }
                    }
                }
                else
                {
                    for (int index = 0; index < m_LyricMax; index++)
                    {
                        //获取引用
                        LyricUI lyricUI = m_LyricList[index].GetComponent<LyricUI>();
                        //设置位置信息
                        lyricUI.rectTransform.localPosition = new Vector3(300f, -225 + (index * -50), 0);
                        //设置信息   通过歌词文件进行获取
                        try
                        {
                            lyricUI.SetLyric(MusicPlayerData.NowPlayMusicInfo.LyricList[index], index);
                        }
                        catch
                        {
                            lyricUI.SetLyric("",-1);
                        }
                    }
                }
            }           
            m_LyricTopUsingIndex = 0;
            m_LyricBottomUsingIndex = m_LyricMax - 1;
            m_LyricLastPosY = m_LyricRoom.position.y;
        }
        /// <summary>
        /// 刷新面板
        /// </summary>
        /// <param name="lyricIndex">歌词索引</param>
        public void RefreshPanel(int lyricIndex)
        {          
            foreach (var lyricTran in m_LyricList)
            {
                lyricTran.GetComponent<LyricUI>().RefreshLyric(lyricIndex);//刷新当前歌词的颜色
            }
            LocationNowLyric(lyricIndex);//定位当前歌词
        }
        bool m_IsRoll = false;
        /// <summary>
        /// 滚动事件
        /// </summary>
        private void RollEvent()
        {
            m_LyricNextPosY = m_LyricRoom.position.y;
            float value = m_LyricNextPosY - m_LyricLastPosY;
            if (value == 0)
            {
                m_IsRoll = false;
                return;
            }
            LyricIndexRefresh(value > 0 ? BottomLyricRefresh() : TopLyricRefresh());
            m_LyricLastPosY = m_LyricNextPosY;
        }
        /// <summary>
        /// 歌词滚动矩形事件
        /// </summary>
        /// <param name="pos"></param>
        private void LyricScrollRectEvent(Vector2 pos)
        {
            if (m_LyricList != null)
            {
                if (m_LyricList.Count < m_LyricMax - 1)//不具备切换歌曲的功能的限制条件
                {
                    return;
                }
                //获取顶部的歌曲UI
                if (m_Top == null)
                {
                    m_Top = m_LyricList[m_LyricTopUsingIndex];
                }
                //获取底部的歌曲UI
                if (m_Bottom == null)
                {
                    m_Bottom = m_LyricList[m_LyricBottomUsingIndex];
                }
                m_IsRoll = true;
            }
        }
        /// <summary>
        /// 更新底部音乐
        /// </summary>
        /// <returns></returns>
        private int BottomLyricRefresh()
        {
            //790    
            if (m_Top.position.y >= 790)
            {
                #region 计算引用的偏移
                m_LyricBottomUsingIndex = m_LyricTopUsingIndex;
                m_LyricTopUsingIndex = (m_LyricTopUsingIndex == (m_LyricMax - 1)) ? 0 : (++m_LyricTopUsingIndex);
                Vector2 v = m_Top.localPosition;
                v.y = m_Bottom.localPosition.y - 50f;
                m_Top.localPosition = v;
                #endregion             
                return 1;
            }
            return 0;
        }
        /// <summary>
        /// 更新顶部音乐
        /// </summary>
        /// <returns></returns>
        private int TopLyricRefresh()
        {
            if (m_LyricRoom.localPosition.y >= 225)
            {
                //250 
                if (m_Bottom.position.y <= 250)
                {
                    #region 计算引用的偏移
                    m_LyricTopUsingIndex = m_LyricBottomUsingIndex;
                    m_LyricBottomUsingIndex = (m_LyricBottomUsingIndex == 0) ? (m_LyricMax - 1) : (--m_LyricBottomUsingIndex);
                    Vector2 v = m_Bottom.localPosition;
                    v.y = m_Top.localPosition.y + 50;
                    m_Bottom.localPosition = v;
                    #endregion
                    return -1;
                }
            }
            return 0;
        }
        /// <summary>
        /// 更新歌词索引
        /// </summary>
        /// <param name="value"></param>
        private void LyricIndexRefresh(int value)
        {
            if (value == 0)
            {
                return;
            }
            #region 更新顶部与底部的引用                    
            m_Top = m_LyricList[m_LyricTopUsingIndex];
            m_Bottom = m_LyricList[m_LyricBottomUsingIndex];
            #endregion         
            m_TopLyricIndex += value;
            MusicInfoRefresh(value > 0 ? false : true);//>0 底部更新  <0顶部更新
        }
        /// <summary>
        /// 歌曲信息更新  更新顶部或底部的歌曲信息
        /// </summary>
        private void MusicInfoRefresh(bool isTop)
        {
            if(MusicPlayerData.NowPlayMusicInfo != null)
            {
                //筛选条件
                if (m_TopLyricIndex < 0 || m_TopLyricIndex > MusicPlayerData.NowPlayMusicInfo.LyricList.Count - m_LyricMax)
                {
                    return;
                }                       
                (isTop ? m_Top.GetComponent<LyricUI>() : m_Bottom.GetComponent<LyricUI>()).SetLyric(MusicPlayerData.NowPlayMusicInfo.LyricList[GetLyricIndex(isTop)],GetLyricIndex(isTop));
            }                    
        }
        /// <summary>
        /// 获取歌词索引
        /// </summary>
        /// <param name="isTop">是否顶部</param>
        /// <returns></returns>
        private int GetLyricIndex(bool isTop)
        {
            return isTop ? m_TopLyricIndex : m_TopLyricIndex + m_LyricMax - 1;
        }
        /// <summary>
        /// 定位当前歌词
        /// </summary>
        /// <param name="index">歌词索引</param>
        private void LocationNowLyric(int index)
        {
            Vector3 localPos = m_LyricRoom.localPosition;
            localPos.y = index*50f;
            m_LyricRoom.DOLocalMove(localPos, 0.5f);
        }


    }
}
