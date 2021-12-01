using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Farme;
using Farme.UI;
using Farme.Tool;
using UnityEngine.Events;
using MusicPlayer.Manager;
namespace MusicPlayer.Panel
{
    /// <summary>
    /// 音乐列表面板
    /// </summary>
    public class MusicListPanel : BasePanel
    {
        /// <summary>
        /// 递增距离  基础值 up  1050   bottom  -50
        /// </summary>
        private float Dis
        {
            get
            {
                return (m_MusicMax - 10f) * 60f;
            }
        }
        /// <summary>
        /// 音乐列表的最大数量  翻滚的条件限制
        /// </summary>
        private int m_MusicMax = 10;
        /// <summary>
        /// 音乐列表是否处于滚动状态
        /// </summary>
        private bool m_IsRoll = false;      
        /// <summary>
        /// 顶部音乐的索引
        /// </summary>
        private int m_MusicTopIndex = 0;
        #region 顶部与底部对于矩形转换数组(m_MusicLi)的引用索引
        private int m_MusicTopUsingIndex = 0;//顶部
        private int m_MusicBottomUsingIndex = 0;//底部
        #endregion
        /// <summary>
        /// 音乐UI列表
        /// </summary>
        private List<RectTransform> m_MusicLi = new List<RectTransform>();              
        /// <summary>
        /// 滚动矩形
        /// </summary>
        private ScrollRect m_SR = null;
        /// <summary>
        /// 滑动条
        /// </summary>
        private Scrollbar m_Scrollbar = null;
        /// <summary>
        /// 音乐列表
        /// </summary>
        private RectTransform m_MusicList = null;
        /// <summary>
        /// 顶部
        /// </summary>
        private RectTransform m_Top;
        /// <summary>
        /// 底部
        /// </summary>
        private RectTransform m_Bottom;
        protected override void Awake()
        {
            base.Awake();
            RegisterComponentsTypes<Image>();
            RegisterComponentsTypes<ScrollRect>();
            RegisterComponentsTypes<Scrollbar>();
        }


        protected override void Start()
        {
            base.Start();
            if(GetComponent("MusicScrollRect",out m_SR))
            {
                m_SR.onValueChanged.AddListener(ScrollRectEvent);
            }
            if (GetComponent("MusicScrollbar", out m_Scrollbar))
            {
                Debuger.Log("问题标记");
                /*
                 * 由于滑动条滑动太快会导致界面出现空缺，暂时滑动条不能交互设置为不可交互
                 */
                m_Scrollbar.interactable = false;
                //m_Scrollbar.onValueChanged.AddListener(ScrollbarEvent);

            }
            if (GetComponent("MusicList", out Image group))
            {
                m_MusicList = group.transform as RectTransform;
            }
            MusicListInit();
        }

        private void Update()
        {
            if(m_IsRoll)
            {
                //触发滚动事件的条件限制
                RollEvent();
            }
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();                
        }
        /// <summary>
        /// 音乐列表上一帧的位置(世界坐标)
        /// </summary>
        private float m_MusicLastPosY = 0;
        /// <summary>
        /// 音乐列表下一帧的位置(世界坐标)
        /// </summary>
        private float m_MusicNextPosY = 0;
        /// <summary>
        /// 初始化音乐列表
        /// </summary>
        private void MusicListInit()
        {
            if(m_MusicList!=null)
            {
                //获取音乐的数量  从音乐数据类中获取
                int musicNum = MusicPlayerData.MusicFileNum;
                //固定占用  顶部偏移+底部偏移
                float fixWitch = 40f;
                //歌曲总的占用=(group.spacing + 100) * musicNum - group.spacing;
                float musicSumWitch = (120f * musicNum) - 20f;
                //总占用 
                Vector2 sizeDelta = m_MusicList.sizeDelta;
                sizeDelta.y = musicSumWitch + fixWitch;
                m_MusicList.sizeDelta = sizeDelta;              
                int productMusicUINum = Mathf.Clamp(musicNum, 0, m_MusicMax);
                for (int index = 0; index < productMusicUINum; index++)
                {
                    if (GoLoad.Take("Music", out GameObject go, m_MusicList))
                    {
                        MusicUI musicUI = go.GetComponent<MusicUI>();
                        //设置位置信息
                        musicUI.rectTransform.localPosition = new Vector3(951.55f, -70 + (index * -120), 0);
                        //设置信息   通过歌词文件进行获取
                        MusicInfo musicInfo = MusicInfoManager.GetMusicInfo(MusicPlayerData.MusicFileNames[index]);
                        if (musicInfo != null)
                        {
                            musicUI.SetInfo(index,musicInfo.MusicName, musicInfo.SingerName, musicInfo.AlbumName);
                        }
                        m_MusicLi.Add(musicUI.rectTransform);
                    }                  
                }
                m_MusicTopUsingIndex = 0;
                m_MusicBottomUsingIndex = m_MusicMax - 1;
                //记录列表此时的世界Y坐标
                m_MusicLastPosY = m_MusicList.position.y;
            }
        }
        /// <summary>
        /// 刷新列表
        /// </summary>
        public void RefreshPanel()
        {
            #region 更新音乐UI状态
            foreach (var tran in m_MusicLi)
            {
                tran.GetComponent<MusicUI>().RefreshState();
            }
            #endregion
        }
        /// <summary>
        /// 音乐列表滑动事件 
        /// </summary>
        /// <param name="value"></param>
        private void ScrollbarEvent(float value)
        {
            //待解决   问题:滑动条拖拽太空会导致界面出现空白的问题
        }
        /// <summary>
        /// 滚动事件
        /// </summary>
        private void RollEvent()
        {
            m_MusicNextPosY = m_MusicList.position.y;
            float value = m_MusicNextPosY - m_MusicLastPosY;
            if(value==0)
            {               
                m_IsRoll = false;
                return;
            }
            MusicIndexRefresh(value > 0 ? BottomMusicRefresh() : TopMusicRefresh());
            m_MusicLastPosY = m_MusicNextPosY;
        }
        #region 更新事件
        /// <summary>
        /// 更新底部音乐
        /// </summary>
        /// <returns></returns>
        private int BottomMusicRefresh()
        {
            //1630  <- 20首音乐  每首+60的距离
            if (m_Top.position.y >= 1030+ Dis)
            {
                #region 计算引用的偏移
                m_MusicBottomUsingIndex = m_MusicTopUsingIndex;
                m_MusicTopUsingIndex = (m_MusicTopUsingIndex == (m_MusicMax - 1)) ? 0 : (++m_MusicTopUsingIndex);               
                Vector2 v = m_Top.localPosition;
                v.y = m_Bottom.localPosition.y - 120f;
                m_Top.localPosition = v;
                #endregion
                #region 之前的解决方案
                //Vector2 v = m_Top.localPosition;
                //v.y = m_Bottom.localPosition.y - 120f;
                //m_MusicLi.Remove(m_Top);
                //m_MusicLi.Add(m_Top);
                //m_Top.localPosition = v;
                #endregion
                return 1;
            }
            return 0;
        }
        /// <summary>
        /// 更新顶部音乐
        /// </summary>
        /// <returns></returns>
        private int TopMusicRefresh()
        {
            //-650  <- 20首音乐  每首-60的距离
            if (m_Bottom.position.y <= -50 - Dis)
            {
                #region 计算引用的偏移
                m_MusicTopUsingIndex = m_MusicBottomUsingIndex;
                m_MusicBottomUsingIndex = (m_MusicBottomUsingIndex == 0) ? (m_MusicMax - 1) : (--m_MusicBottomUsingIndex);                                   
                Vector2 v = m_Bottom.localPosition;
                v.y = m_Top.localPosition.y + 120f;
                m_Bottom.localPosition = v;
                #endregion
                #region 之前的解决方案
                //Vector2 v = m_Bottom.localPosition;
                //v.y = m_Top.localPosition.y + 120f;
                //m_MusicLi.Remove(m_Bottom);
                //m_MusicLi.Insert(0, m_Bottom);
                //m_Bottom.localPosition = v;
                #endregion
                return -1;
            }
            return 0;
        }
        /// <summary>
        /// 更新音乐索引
        /// </summary>
        /// <param name="value"></param>
        private void MusicIndexRefresh(int value)
        {
            if(value==0)
            {
                return;
            }
            #region 更新顶部与底部的引用                    
            m_Top = m_MusicLi[m_MusicTopUsingIndex];
            m_Bottom = m_MusicLi[m_MusicBottomUsingIndex];
            #endregion
            #region 之前的解决方案
            //m_Top = m_MusicLi[0];
            //m_Bottom = m_MusicLi[m_MusicLi.Count - 1];
            #endregion
            m_MusicTopIndex += value;
            MusicInfoRefresh(value > 0 ? false : true);//>0 底部更新  <0顶部更新
        }
        /// <summary>
        /// 歌曲信息更新  更新顶部或底部的歌曲信息
        /// </summary>
        private void MusicInfoRefresh(bool isTop)
        {
            //筛选条件
            if (m_MusicTopIndex < 0 || m_MusicTopIndex > MusicPlayerData.MusicFileNames.Length - m_MusicMax)
            {
                return;
            }
            #region 获取音乐文件名称       
            //string musicFileName = MusicPlayerData.MusicFileNames[GetMusicIndex(isTop)];//提取音乐文件名称
            //Debuger.LogWarning("更新歌曲:" + musicFileName + "歌曲索引:" + (isTop ? m_MusicIndex : m_MusicIndex + m_MusicMax - 1));
            #endregion              
            MusicUI musicUI = isTop ? m_Top.GetComponent<MusicUI>() : m_Bottom.GetComponent<MusicUI>();
            MusicInfo musicInfo = MusicInfoManager.GetMusicInfo(MusicPlayerData.MusicFileNames[GetMusicIndex(isTop)]);
            if (musicInfo != null)
            {
                musicUI.SetInfo(GetMusicIndex(isTop), musicInfo.MusicName, musicInfo.SingerName, musicInfo.AlbumName);
            }     
        }
        #endregion
        /// <summary>
        /// 获取音乐索引
        /// </summary>
        /// <param name="isTop">是否顶部</param>
        /// <returns></returns>
        private int GetMusicIndex(bool isTop)
        {
            return isTop ? m_MusicTopIndex : m_MusicTopIndex + m_MusicMax - 1;
        }
        /// <summary>
        /// 音乐列表滑动事件  
        /// </summary>
        private void ScrollRectEvent(Vector2 pos)
        {
            if (m_MusicLi != null)
            {
                if (m_MusicLi.Count < m_MusicMax-1)//不具备切换歌曲的功能的限制条件
                {
                    return;
                }
                //获取顶部的歌曲UI
                if (m_Top == null)
                {
                    m_Top = m_MusicLi[m_MusicTopUsingIndex];
                }
                //获取底部的歌曲UI
                if (m_Bottom == null)
                {
                    m_Bottom = m_MusicLi[m_MusicBottomUsingIndex];
                }
                m_IsRoll = true;
            }
        }
        /// <summary>
        /// 排序(从小到大排序)冒泡
        /// </summary>
        /// <param name="array">数组</param>
        /// <returns></returns>
        private void Sort(int[] array)
        {
            int value = 0;
            for (int index_1 = 0; index_1 < array.Length; index_1++)
            {
                for (int index_2 = index_1 + 1; index_2 < array.Length; index_2++)
                {
                    if (array[index_1] > array[index_2])
                    {
                        //交换两者的元素值
                        value = array[index_1];
                        array[index_1] = array[index_2];
                        array[index_2] = value;
                    }
                }
            }
        }
    }
}
