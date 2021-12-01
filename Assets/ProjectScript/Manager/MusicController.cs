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
                MusicInfo info = MusicInfoManager.GetMusicInfo(MusicPlayerData.MusicFileNames[MusicPlayerData.NowPlayMusicIndex]);
                if (info != null)
                {
                    info.LoadAlbum(MusicPlayerData.MusicFileNames[MusicPlayerData.NowPlayMusicIndex],() =>
                    {
                        RefreshControllerPanel();
                    });
                    info.LoadLyriInfo(MusicPlayerData.MusicFileNames[MusicPlayerData.NowPlayMusicIndex],()=> 
                    {
                        #region 测试
                        MusicInfo musicInfo = MusicInfoManager.GetMusicInfo(MusicPlayerData.MusicFileNames[MusicPlayerData.NowPlayMusicIndex]);
                        if (musicInfo != null)
                        {
                            foreach(var str in musicInfo.LyricDic.Values)
                            {
                                Debuger.Log(str);
                            }                         
                        }
                        #endregion
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
                }
                else
                {
                    m_MusicAudioTo.Pause();
                }
            }
            else
            {
                if (m_MusicAudioFrom.IsPause)
                {
                    m_MusicAudioFrom.Play();
                }
                else
                {
                    m_MusicAudioFrom.Pause();
                }                    
            }
        }
        /// <summary>
        /// 继续播放
        /// </summary>
        public static void MusicContinue()
        {
            if (IsFrom_To)
            {
                m_MusicAudioTo.Play();
            }
            else
            {
                m_MusicAudioFrom.Play();
            }
        }
        /// <summary>
        /// 暂停
        /// </summary>
        public static void MusicPause()
        {
            if(IsFrom_To)
            {
                m_MusicAudioTo.Pause();
            }
            else
            {
                m_MusicAudioFrom.Pause();
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
                    MusicListPanel panel = window.GetPanel<MusicListPanel>("MusicListPanel");
                    if (panel != null)
                    {
                        panel.RefreshPanel();//刷新音乐列表面板
                    }
                }
            }

        }
    }
}
