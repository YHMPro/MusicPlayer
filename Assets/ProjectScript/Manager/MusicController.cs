using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farme.Audio;
namespace MusicPlayer.Manager
{
    /// <summary>
    /// 音乐控制
    /// </summary>
    public class MusicController
    {
        #region 用于形成过度音效播放的效果
        private static bool IsFrom_To = true;
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
                    m_MusicAudioFrom.Group = group;
                }
            }
            if (m_ButtonAudio == null)
            {
                m_ButtonAudio = AudioManager.ApplyForAudio();
                m_ButtonAudio.AbleRecycle = false;//不可回收
                m_ButtonAudio.Loop = false;//非循环
                if (AudioMixerManager.GetAudioMixerGroup("Button", out var group))
                {
                    m_MusicAudioFrom.Group = group;
                }
            }
        }

        public static void MusicPlay( AudioClip clip)
        {
            m_MusicAudioFrom.Clip = clip;
            if (m_MusicAudioFrom.Clip != null && m_MusicAudioTo.Clip != null)
            {
                if (IsFrom_To)
                {
                    AudioManager.ExcessPlay(m_MusicAudioFrom, m_MusicAudioTo, 0.5f);
                }
                else
                {
                    AudioManager.ExcessPlay(m_MusicAudioTo, m_MusicAudioFrom, 0.5f);
                }

            }
            else
            {
                //m_MusicAudioFrom.Play(0.5f)
            }
        }

        public static void MusicPause()
        {

        }

        public static void MusicRePlay()
        {

        }


    }
}
