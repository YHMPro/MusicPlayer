using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Farme;
using Farme.UI;
using Farme.Tool;
using UnityEngine.Events;
namespace MusicPlayer.Panel
{
    /// <summary>
    /// 音乐列表面板
    /// </summary>
    public class MusicListPanel : BasePanel
    {
        /// <summary>
        /// 音乐UI列表
        /// </summary>
        private List<RectTransform> m_MusicLi = new List<RectTransform>();
        /// <summary>
        /// 滚动矩形
        /// </summary>
        private ScrollRect m_SR = null;
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
        }


        protected override void Start()
        {
            base.Start();
            if(GetComponent("MusicScrollRect",out m_SR))
            {
                m_SR.onValueChanged.AddListener(SliderEvent);
            }
            if (GetComponent("MusicList", out VerticalLayoutGroup group))
            {
                m_MusicList = group.transform as RectTransform;
            }
            MusicListInit();
        }
        /// <summary>
        /// 初始化音乐列表
        /// </summary>
        public void MusicListInit()
        {
            if(m_MusicList!=null)
            {
                //获取音乐的数量  从音乐数据类中获取
                int musicNum = 0;//MusicData.MusicNum;
                //固定占用  顶部偏移+底部偏移
                float fixWitch = 20;
                //歌曲总的占用=(group.spacing + 100) * musicNum - group.spacing;
                float musicSumWitch = 120f * musicNum - 20;
                //总占用 
                Vector2 sizeDelta = m_MusicList.sizeDelta;
                sizeDelta.y = musicSumWitch + fixWitch;
                m_MusicList.sizeDelta = sizeDelta;
                /*添加音乐UI  
                 *        
                 *
                */
                int productMusicUINum = 0;               
                if(m_MusicList.childCount<11)
                {
                    if (MusicData.MusicFileNum < 11)
                    {
                        productMusicUINum = MusicData.MusicFileNum - m_MusicList.childCount;
                    }
                    else
                    {
                        productMusicUINum = 11 - m_MusicList.childCount;
                    }
                }
                if(productMusicUINum>0)
                {
                    //生成

                }
                else if(productMusicUINum<0)
                {
                    //回收

                }
                                                                
            }           
        }

        /// <summary>
        /// 滑动事件
        /// </summary>
        private void SliderEvent(Vector2 pos)
        {
            if(m_MusicList!=null)
            {
                if(MusicData.MusicFileNum < 11)//不具备切换歌曲的功能
                {
                    return;
                }
                //获取顶部的歌曲UI
                if (m_Top == null)
                {
                    m_Top = m_MusicList.GetChild(0) as RectTransform;
                }
                //获取底部的歌曲UI
                if (m_Bottom == null)
                {
                    m_Bottom = m_MusicList.GetChild(m_MusicList.childCount - 1) as RectTransform;
                }
                /*顶部歌曲UI的世界Y轴超过1150时触发切换事件  顶部移动至底部歌曲UI下面
                 * 相对底部歌曲UI距离为L  
                 * L=歌曲UI宽度+间隔
                */
                if(m_Top.position.y>=1150)
                {
                    pos = m_Top.position;
                    pos.y = m_Bottom.position.y - 120f;
                    //触发歌曲UI更新事件  top与bottom
                    //m_Top.SetParent()
                    m_Bottom = m_Top;
                    m_Top= m_MusicList.GetChild(0) as RectTransform;
                    Debuger.Log("更新歌曲UI的顶部与底部");
                }
                
            }
        }
    }
}
