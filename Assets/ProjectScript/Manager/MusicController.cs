using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farme.Audio;
using Farme.Net;
using Farme.Extend;
using Farme;
using Farme.UI;
using MusicPlayer.Panel;
using Farme.Tool;

namespace MusicPlayer.Manager
{
    /// <summary>
    /// 音乐控制
    /// </summary>
    public class MusicController
    {
        #region 用于形成过度音效播放的效果
        private static bool IsFrom_To = false;
        private static Audio m_MusicAudioFrom = null;
        private static Audio m_MusicAudioTo = null;
        #endregion

        #region 按键音效(仅在音乐没有正在播放的情况下才有效)
        private static Audio m_ButtonAudio = null;
        #endregion

        public static void Init()
        {
            if (m_MusicAudioFrom == null)
            {
                m_MusicAudioFrom = AudioManager.ApplyForAudio();
                m_MusicAudioFrom.AbleRecycle = false;//不可回收
                m_MusicAudioFrom.Loop = false;//非循环
                if(AudioMixerManager.GetAudioMixerGroup("BackGround",out var group))
                {
                    m_MusicAudioFrom.Group = group;
                }
                
            }
            if (m_MusicAudioTo == null)
            {
                m_MusicAudioTo = AudioManager.ApplyForAudio();
                m_MusicAudioTo.AbleRecycle = false;//不可回收
                m_MusicAudioTo.Loop = false;//非循环
                if (AudioMixerManager.GetAudioMixerGroup("BackGround", out var group))
                {
                    m_MusicAudioTo.Group = group;
                }
            }
            if (m_ButtonAudio == null)
            {
                m_ButtonAudio = AudioManager.ApplyForAudio();
                m_ButtonAudio.AbleRecycle = false;//不可回收
                m_ButtonAudio.Loop = false;//非循环
                if (AudioMixerManager.GetAudioMixerGroup("Button", out var group))
                {
                    m_ButtonAudio.Group = group;
                }
            }
        }
        /// <summary>
        /// 加载并播放(含渐变)
        /// </summary>
        public static void MusicPlay()
        {
            WebDownloadTool.WebDownLoadAudioClipMP3(MusicPlayerData.MusicFilePath + @"\" + MusicPlayerData.MusicFileNames[MusicPlayerData.NowPlayMusicIndex] + MusicPlayerData.MusicFileSuffix, (clip) =>
            {
                if(clip==null)
                {
                    return;
                }                            
                if (m_MusicAudioFrom.Clip != null)
                {
                    IsFrom_To = !IsFrom_To;
                    if (IsFrom_To)
                    {
                        m_MusicAudioTo.Clip = clip;
                        AudioManager.ExcessPlay(m_MusicAudioFrom, m_MusicAudioTo, 1f, 0.5f);
                    }
                    else
                    {
                        m_MusicAudioFrom.Clip = clip;
                        AudioManager.ExcessPlay(m_MusicAudioTo, m_MusicAudioFrom, 1f, 0.5f);
                    }                 
                }
                else
                {
                    m_MusicAudioFrom.Clip = clip;
                    m_MusicAudioFrom.Play();
                }
                RefreshListPanel();
                //加载封面与歌词
                MusicPlayerData.NowPlayMusicInfo = MusicInfoManager.GetMusicInfo(MusicPlayerData.MusicFileNames[MusicPlayerData.NowPlayMusicIndex]);
                if (MusicPlayerData.NowPlayMusicInfo != null)
                {
                    MusicPlayerData.NowPlayMusicInfo.LoadAlbum(MusicPlayerData.MusicFileNames[MusicPlayerData.NowPlayMusicIndex],() =>
                    {
                        RefreshControllerPanel();
                    });
                    MusicPlayerData.NowPlayMusicInfo.LoadLyriInfo(MusicPlayerData.MusicFileNames[MusicPlayerData.NowPlayMusicIndex],()=> 
                    {                     
                        if (MonoSingletonFactory<WindowRoot>.SingletonExist)
                        {
                            WindowRoot root = MonoSingletonFactory<WindowRoot>.GetSingleton();
                            StandardWindow window = root.GetWindow("Controller");
                            if (window != null)
                            {
                                MusicLyricPanel panel = window.GetPanel<MusicLyricPanel>("LyricPanel");
                                if (panel != null)
                                {
                                    panel.LyricListInit();//歌词列表初始化
                                    panel.RefreshPanel(0);//刷新音乐列表面板
                                    MonoSingletonFactory<ShareMono>.GetSingleton().RemoveUpdateAction(EnumUpdateAction.Standard, LyricLocation);//歌词实时定位
                                    m_LyricIndex = 0;//重置
                                    m_AimTime = MusicPlayerData.NowPlayMusicInfo.TimeList[m_LyricIndex];//记录目标时间
                                    MonoSingletonFactory<ShareMono>.GetSingleton().ApplyUpdateAction(EnumUpdateAction.Standard, LyricLocation);//歌词实时定位
                                }
                            }
                        }
                       
                    });
                }
            });                            
        }

        /// <summary>
        /// 播放或暂停
        /// </summary>
        public static void MusicPlayOrPause()
        {
            if (IsFrom_To)
            {
                if (m_MusicAudioTo.IsPause)
                {
                    m_MusicAudioTo.Play();
                    MonoSingletonFactory<ShareMono>.GetSingleton().ApplyUpdateAction(EnumUpdateAction.Standard, LyricLocation);
                }
                else
                {
                    m_MusicAudioTo.Pause();
                    MonoSingletonFactory<ShareMono>.GetSingleton().RemoveUpdateAction(EnumUpdateAction.Standard, LyricLocation);
                }
            }
            else
            {
                if (m_MusicAudioFrom.IsPause)
                {
                    m_MusicAudioFrom.Play();
                    MonoSingletonFactory<ShareMono>.GetSingleton().ApplyUpdateAction(EnumUpdateAction.Standard, LyricLocation);
                }
                else
                {
                    m_MusicAudioFrom.Pause();
                    MonoSingletonFactory<ShareMono>.GetSingleton().RemoveUpdateAction(EnumUpdateAction.Standard, LyricLocation);
                }                    
            }           
        }      
        /// <summary>
        /// 重播
        /// </summary>
        public static void MusicRePlay()
        {
            if (IsFrom_To)
            {
                m_MusicAudioTo.RePlay();
            }
            else
            {
                m_MusicAudioFrom.RePlay();
            }
            MonoSingletonFactory<ShareMono>.GetSingleton().RemoveUpdateAction(EnumUpdateAction.Standard, LyricLocation);
            MonoSingletonFactory<ShareMono>.GetSingleton().ApplyUpdateAction(EnumUpdateAction.Standard, LyricLocation);
            m_AimTime = MusicPlayerData.NowPlayMusicInfo.TimeList[m_LyricIndex];
            m_LyricIndex = 0;//重置
        }
        /// <summary>
        /// 刷新控制面板
        /// </summary>
        public static void RefreshControllerPanel()
        {
            if (MonoSingletonFactory<WindowRoot>.SingletonExist)
            {
                WindowRoot root = MonoSingletonFactory<WindowRoot>.GetSingleton();
                StandardWindow window = root.GetWindow("Controller");
                if (window != null)
                {
                    MusicPlayControllerPanel panel = window.GetPanel<MusicPlayControllerPanel>("ControllerPanel");
                    if (panel != null)
                    {
                        panel.RefreshPanel();//刷新控制面板
                    }
                }
            }
        }
        /// <summary>
        /// 刷新列表面板
        /// </summary>
        public static void RefreshListPanel()
        {
            if (MonoSingletonFactory<WindowRoot>.SingletonExist)
            {
                WindowRoot root = MonoSingletonFactory<WindowRoot>.GetSingleton();
                StandardWindow window = root.GetWindow("Controller");
                if (window != null)
                {
                    MusicListPanel panel = window.GetPanel<MusicListPanel>("ListPanel");
                    if (panel != null)
                    {
                        panel.RefreshPanel();//刷新音乐列表面板
                    }
                }
            }

        }

        #region 歌词实时同步     
        /// <summary>
        /// 目标时间
        /// </summary>
        private static double m_AimTime = 0;
        /// <summary>
        /// 歌词索引
        /// </summary>
        private static int m_LyricIndex = 0;
        /// <summary>
        /// 歌词定位
        /// </summary>
        public static void LyricLocation()
        {
            if((IsFrom_To ? m_MusicAudioTo.Time : m_MusicAudioFrom.Time) >= m_AimTime)
            {
                Debuger.Log("歌词更新同步");
                if (MonoSingletonFactory<WindowRoot>.SingletonExist)
                {
                    WindowRoot root = MonoSingletonFactory<WindowRoot>.GetSingleton();
                    StandardWindow window = root.GetWindow("Controller");
                    if (window != null)
                    {
                        MusicLyricPanel panel = window.GetPanel<MusicLyricPanel>("LyricPanel");
                        if (panel != null)
                        {
                            panel.RefreshPanel(m_LyricIndex);//刷新音乐歌词面板
                        }
                    }
                }
                if (m_LyricIndex== MusicPlayerData.NowPlayMusicInfo.LyricList.Count-1)
                {
                    Debuger.Log("歌词已到最后一句,移除实时同步函数监听");
                    MonoSingletonFactory<ShareMono>.GetSingleton().RemoveUpdateAction(EnumUpdateAction.Standard, LyricLocation);
                    return;
                }                
                ++m_LyricIndex;
                m_AimTime = MusicPlayerData.NowPlayMusicInfo.TimeList[m_LyricIndex];//刷新目标时间
            }                
        }
        #endregion
        ///// <summary>
        ///// 刷新歌词面板
        ///// </summary>
        //public static void RefreshLyricPanel()
        //{
        //    if (MonoSingletonFactory<WindowRoot>.SingletonExist)
        //    {
        //        WindowRoot root = MonoSingletonFactory<WindowRoot>.GetSingleton();
        //        StandardWindow window = root.GetWindow("Controller");
        //        if (window != null)
        //        {
        //            MusicLyricPanel panel = window.GetPanel<MusicLyricPanel>("LyricPanel");
        //            if (panel != null)
        //            {
        //                panel.RefreshPanel();//刷新音乐列表面板
        //            }
        //        }
        //    }
        //}
    }
}
