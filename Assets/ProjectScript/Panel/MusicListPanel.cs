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
        /// 需要刷新的音乐UI索引
        /// </summary>
        private int m_RefreshMusicUIIndex = 0; //常规 如果是更新底部则+9即可
        /// <summary>
        /// 顶部索引
        /// </summary>
        private int m_TopIndex = 0;
        /// <summary>
        /// 底部索引
        /// </summary>
        private int m_BottomIndex = 9;
        /// <summary>
        /// 音乐UI列表
        /// </summary>
        private List<RectTransform> m_MusicLi = new List<RectTransform>();
        /// <summary>
        /// 用于记录音乐列表中的位置信息  在每次关闭时记录一次(本地坐标)
        /// </summary>
        private Vector2[] m_MusicPoss = null;
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
            RegisterComponentsTypes<VerticalLayoutGroup>();
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
            if (GetComponent("MusicList", out VerticalLayoutGroup group))
            {
                m_MusicList = group.transform as RectTransform;
            }
            MusicListInit();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            if (m_MusicPoss != null)
            {
                MonoSingletonFactory<ShareMono>.GetSingleton().DelayAction(0.05f, () =>
                {
                    for (int index = 0; index < m_MusicLi.Count; index++)
                    {
                        m_MusicLi[index].localPosition = m_MusicPoss[index];
                    }
                });
            }
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            m_MusicPoss = new Vector2[m_MusicLi.Count];
            for(int index=0;index< m_MusicPoss.Length;index++)
            {
                m_MusicPoss[index] = m_MusicLi[index].localPosition;
            }
          
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
                float fixWitch = 20f;
                //歌曲总的占用=(group.spacing + 100) * musicNum - group.spacing;
                float musicSumWitch = 120f * musicNum - 20f;
                //总占用 
                Vector2 sizeDelta = m_MusicList.sizeDelta;
                sizeDelta.y = musicSumWitch + fixWitch;
                m_MusicList.sizeDelta = sizeDelta;              
                int productMusicUINum = Mathf.Clamp(musicNum, 0, 10);
                for (int index = 0; index < productMusicUINum; index++)
                {
                    if (GoLoad.Take("Music", out GameObject go, m_MusicList))
                    {
                        MusicUI musicUI = go.GetComponent<MusicUI>();
                        m_MusicLi.Add(musicUI.rectTransform);
                        //设置信息   通过歌词文件进行获取
                        



                    }
                }
            }           
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        public void RefreshList()
        {

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
        /// 音乐列表滑动事件  拖拽滑动条也会触发此事件
        /// </summary>
        private void ScrollRectEvent(Vector2 pos)
        {
            if (m_MusicLi != null)
            {
                if(m_MusicLi.Count < 10)//不具备切换歌曲的功能
                {                
                    return;
                }
                //获取顶部的歌曲UI
                if (m_Top == null)
                {
                    m_Top = m_MusicLi[0];
                }
                //获取底部的歌曲UI
                if (m_Bottom == null)
                {
                    m_Bottom = m_MusicLi[9];
                }
                m_MusicNextPosY = m_MusicList.position.y;
                if (m_MusicNextPosY != 0 && m_MusicLastPosY != 0)
                {
                    if (m_MusicNextPosY != m_MusicLastPosY)
                    {
                        /*
                         *手指从上往下滑动  
                         *当末尾元素的世界坐标<=-50时转移元素
                         *手指从下往上滑动
                         *当头部元素的世界坐标>=1030或1050时转移元素
                        */
                        float dir = m_MusicNextPosY - m_MusicLastPosY;
                        #region 手指从上往下滑动  负
                        if (dir < 0)
                        {
                            if (m_Bottom.position.y <=-50 )
                            {
                                Vector2 v = m_Bottom.localPosition;
                                v.y = m_Top.localPosition.y + 120f;
                                m_MusicLi.Remove(m_Bottom);
                                m_MusicLi.Insert(0, m_Bottom);
                                m_Bottom.localPosition = v;

                                m_Top = m_MusicLi[0];
                                m_Bottom = m_MusicLi[9];
                                m_RefreshMusicUIIndex--;
                                UpdateMusicUIData(dir);
                            }
                        }
                        #endregion

                        #region 手指从下往上滑动  正
                        if (dir > 0)
                        {
                            if (m_Top.position.y >= 1050)
                            {                               
                                Vector2 v = m_Top.localPosition;
                                v.y = m_Bottom.localPosition.y - 120f;
                                m_MusicLi.Remove(m_Top);
                                m_MusicLi.Add(m_Top);
                                m_Top.localPosition = v;


                                m_Top = m_MusicLi[0];
                                m_Bottom = m_MusicLi[9];
                                m_RefreshMusicUIIndex++;
                                UpdateMusicUIData(dir);
                            }
                        }
                        #endregion
                        ////更新顶部与底部引用
                        //if (!Equals(m_Top, m_MusicLi[0]))
                        //{
                        //    m_Top = m_MusicLi[0];
                        //    m_RefreshMusicUIIndex--;
                        //    UpdateMusicUIData(dir);
                        //}
                        //if (!Equals(m_Bottom, m_MusicLi[9]))
                        //{
                        //    m_Bottom = m_MusicLi[9];
                        //    m_RefreshMusicUIIndex++;
                        //    UpdateMusicUIData(dir);
                        //}
                       
                    }
                }
                m_MusicLastPosY = m_MusicNextPosY;             
            }
        }
        /// <summary>
        /// 更新音乐UI数据
        /// </summary>
        /// <param name="dir"></param>
        private void UpdateMusicUIData(float dir)//调用此方法前已排除 dir=0的情况
        {
            #region 获取需要更新的音乐文件名称
            string musicFileName = "";
            m_RefreshMusicUIIndex = Mathf.Clamp(m_RefreshMusicUIIndex, 0, MusicPlayerData.MusicFileNames.Length - 1);
            MusicUI musicUI = null;
            if (dir > 0)
            {
                //更新底部UI的数据   m_RefreshMusicUIIndex+9
                musicFileName = MusicPlayerData.MusicFileNames[m_RefreshMusicUIIndex + 9];
                musicUI = m_Bottom.GetComponent<MusicUI>();
            }
            if (dir < 0)
            {
                //更新顶部UI的数据
                musicFileName = MusicPlayerData.MusicFileNames[m_RefreshMusicUIIndex];
                musicUI = m_Top.GetComponent<MusicUI>();
            }
            #endregion
            //根据音乐名称加载对于的Lrc文件
            MusicData musicData = MusicDataManager.GetMusicData(musicFileName);
            musicData.LoadLrc(musicFileName, () =>
            {
                musicUI.SetInfo(musicData.LrcInfo.Title, musicData.LrcInfo.Artist, musicData.LrcInfo.Album);
            });

        }

    }
}
