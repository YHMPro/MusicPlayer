using UnityEngine;
using UnityEngine.Audio;
namespace Farme
{
    [RequireComponent(typeof(AudioSource))]
    /// <summary>
    /// Audio基类
    /// </summary>
    public abstract class BaseAudio : BaseMono, IAudioControl
    {
        protected BaseAudio() { }

        #region 生命周期函数

        protected override void Awake()
        {
            base.Awake();
            m_As = GetComponent<AudioSource>();
            m_As.panStereo = 0;
            m_As.time = 0;
            m_As.clip = null;
            m_As.playOnAwake = false;
            m_As.loop = false;
            m_Ac = null;
            m_Loop = false;
            m_IsPause = false;
            m_IsStop = true;
            m_IsPlay = false;
            m_Volume = 1;
            m_PanStereo = 0;
            m_IsAutoRecycle = true;
            m_Recoverer = null;
            GetRelyOnAudioMgr().NotInidleAudioControlLi.Add(Control);
        }
        #endregion

        #region 字段         
        [SerializeField]
        /// <summary>
        /// 音效源
        /// </summary>
        protected AudioSource m_As = null;
        [SerializeField]
        /// <summary>
        /// 音效
        /// </summary>
        private AudioClip m_Ac = null;
        [SerializeField]
        /// <summary>
        /// 绑定的音效混合器组
        /// </summary>
        private AudioMixerGroup m_BindAudioMixerGroup = null;
        /// <summary>
        /// 是否播放
        /// </summary>
        private bool m_IsPlay = false;
        /// <summary>
        /// 是否停止
        /// </summary>
        private bool m_IsStop = true;
        /// <summary>
        /// 是否暂停
        /// </summary>
        private bool m_IsPause = true;
        /// <summary>
        /// 是否循环
        /// </summary>
        private bool m_Loop = false;
        /// <summary>
        /// 是否自动回收
        /// </summary>
        private bool m_IsAutoRecycle = true;
        /// <summary>
        /// 音量
        /// </summary>
        private float m_Volume = 1;
        /// <summary>
        /// 左声道->右声道的渐变  -1(左)->0(立体)->1(右)
        /// </summary>
        private float m_PanStereo = 0;
        /// <summary>
        /// 已播放的时长
        /// </summary>
        private float m_PlayedTime = 0;
        /// <summary>
        /// 附属回收器(在非循环状态下则会在播放结束时自动回收)
        /// </summary>
        private Coroutine m_Recoverer;
        #endregion

        #region 属性
        /// <summary>
        /// 是否自动回收
        /// </summary>
        public bool IsAutoRecycle
        {
            set
            {
                if (m_IsAutoRecycle != value)
                {
                    if (value)
                    {
                        AppendRecoverer();
                    }
                    else
                    {
                        RemoveRecoverer();
                    }
                    m_IsAutoRecycle = value;
                }
            }
            get
            {
                return m_IsAutoRecycle;
            }
        }
        /// <summary>
        /// 是否绑定音效混合器
        /// </summary>
        public bool IsBindAudioMixer
        {
            get
            {
                return m_BindAudioMixerGroup == null ? false : true;
            }
        }
        /// <summary>
        /// 音效控制接口
        /// </summary>
        public IAudioControl Control
        {
            get
            {
                return this;
            }
        }
        /// <summary>
        /// 左声道->右声道的渐变  -1(左)->0(立体)->1(右)
        /// </summary>
        public float StereoPan
        {
            set
            {
                m_PanStereo = Mathf.Clamp(value, -1, 1);
                if (m_As != null)
                {
                    m_As.panStereo = m_PanStereo;
                }

            }
            get
            {
                return m_PanStereo;
            }
        }
        /// <summary>
        /// 音量  
        /// </summary>
        public float Volume
        {
            set
            {
                m_Volume = Mathf.Clamp(value, 0, 1);
                if (m_As != null)
                {
                    m_As.volume = m_Volume;
                }
            }
            get
            {
                return m_Volume;
            }
        }
        /// <summary>
        /// 音效时长
        /// </summary>
        public float Length
        {
            get
            {
                if (m_Ac != null)
                {
                    return m_Ac.length;
                }
                return 0;
            }
        }
        /// <summary>
        /// 音效ID
        /// </summary>
        public int ID
        {
            get
            {
                if (m_As != null)
                {
                    return m_As.GetInstanceID();
                }
                return -1;
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 附加回收器
        /// </summary>
        private void AppendRecoverer()
        {
            if (m_Recoverer == null && m_As != null && m_Ac != null)
            {
                m_Recoverer = MonoSingletonFactory<ShareMono>.GetSingleton().DelayUAction(m_Ac.length - m_As.time, () =>//延迟回收时长为调用时音效剪辑总时长-已播放时长
                {
                    m_As.Stop();//停止播放
                    m_IsPause = false;//非暂停
                    m_IsPlay = false;//非播放
                    m_IsStop = true;//停止
                    m_As.time = 0;//重置播放时长                                                   
                    MonoSingletonFactory<ShareMono>.GetSingleton().StopCoroutine(m_Recoverer);//停止协程
                    m_Recoverer = null;//重置为NULL
                    GetRelyOnAudioMgr().InidleWithNotInidleTransform(this);//置换     
                });
            }
        }

        /// <summary>
        /// 移除回收器
        /// </summary>
        private void RemoveRecoverer()
        {
            if (m_Recoverer != null)
            {
                MonoSingletonFactory<ShareMono>.GetSingleton().StopCoroutine(m_Recoverer);//停止协程
            }
        }

        /// <summary>
        /// 获取自身依赖的音效管理器
        /// </summary>
        /// <returns></returns>
        protected abstract BaseAudioMgr GetRelyOnAudioMgr();

        /// <summary>
        /// 音效播放
        /// </summary>
        /// <returns>是否播放成功</returns>
        protected virtual bool AudioPlay()
        {
            if (m_As != null && !m_IsPlay)
            {
                m_As.Play();
                if (IsAutoRecycle)
                {
                    if (!m_Loop)//非循环
                    {
                        AppendRecoverer();//添加回收器
                    }
                }
                m_IsPlay = true;//播放
                m_IsPause = false;//非暂停
                m_IsStop = false;//非停止
                return true;
            }
            return false;
        }

        /// <summary>
        /// 音效暂停
        /// </summary>
        /// <returns>是否暂停成功</returns>
        protected virtual bool AudioPause()
        {
            if (m_As != null && !m_IsPause)
            {
                m_As.Pause();
                if (IsAutoRecycle)
                {
                    if (!m_Loop)//非循环
                    {
                        RemoveRecoverer();//移除回收器
                    }
                }
                m_IsPlay = false;//非播放
                m_IsStop = false;//非停止
                m_IsPause = true;//暂停
                return true;
            }
            return false;
        }

        /// <summary>
        /// 音效停止
        /// </summary>
        /// <returns>是否停止成功</returns>
        protected virtual bool AudioStop()
        {
            if (m_As != null && !m_IsStop)
            {
                m_As.Stop();//停止播放
                if (IsAutoRecycle)
                {
                    if (!m_Loop)//非循环
                    {
                        RemoveRecoverer();//移除回收器
                    }
                }
                m_As.time = 0;//重置播放时长
                m_IsPause = false;//非暂停
                m_IsPlay = false;//非播放           
                m_IsStop = true;//停止
                GetRelyOnAudioMgr().InidleWithNotInidleTransform(this);//置换
                return true;
            }
            return false;
        }

        /// <summary>
        /// 音效重播
        /// </summary>
        /// <returns>是否重播成功</returns>
        protected virtual bool AudioReplay()
        {
            if (m_As != null && !m_IsStop)
            {
                m_As.time = 0;//起始播放时长归零
                m_As.Play();//播放
                if (IsAutoRecycle)
                {
                    if (!m_As.loop)//非循环
                    {
                        RemoveRecoverer();//移除回收器
                        AppendRecoverer();//添加回收器
                    }
                }
                m_IsPlay = true;//播放
                m_IsPause = false;//非暂停
                m_IsStop = false;//非停止
                return true;
            }
            return false;
        }

        public float GetPlayedTime()
        {
            return m_PlayedTime;
        }

        public bool GetAudioMixerGroup(out AudioMixerGroup group)
        {
            if (IsBindAudioMixer)
            {
                group = m_BindAudioMixerGroup;
                return true;
            }
            group = null;
            return false;
        }

        public AudioClip GetAudioClip()
        {
            return m_Ac;
        }

        public bool IsPause()
        {
            return m_IsPause;
        }

        public bool IsStop()
        {
            return m_IsStop;
        }

        public bool IsPlay()
        {
            return m_IsPlay;
        }

        public bool IsLoop()
        {
            return m_Loop;
        }

        public bool Play()
        {
            return AudioPlay();
        }

        public bool Pause()
        {
            return AudioPause();
        }

        public bool RePlay()
        {
            return AudioReplay();
        }

        public bool Stop()
        {
            return AudioStop();
        }

        public bool SetLoop(bool loop)
        {
            if (m_As != null)
            {
                if (m_Loop != loop)
                {
                    if (IsAutoRecycle)
                    {
                        if (loop)
                        {
                            RemoveRecoverer();//移除回收器                       
                        }
                        else
                        {
                            if (IsAutoRecycle)
                            {

                                AppendRecoverer();//添加回收器
                            }
                        }
                    }
                    m_As.loop = loop;
                    m_Loop = loop;
                }
                return true;
            }
            return false;
        }

        public bool SetAudioMixerGroup(AudioMixerGroup group)
        {
            if (m_As != null)
            {
                if (m_BindAudioMixerGroup != group)
                {
                    m_As.outputAudioMixerGroup = group;
                    m_BindAudioMixerGroup = group;
                }
                return true;
            }
            return false;
        }

        public bool SetPlayedTime(float playedTime)
        {
            if (m_As != null)
            {
                m_As.time = playedTime;
                m_PlayedTime = playedTime;
                if (IsAutoRecycle)
                {
                    if (!m_Loop)
                    {
                        //重新设置回收器
                        RemoveRecoverer();//移除
                        AppendRecoverer();//添加
                    }
                }
                return true;
            }
            return false;
        }

        public bool SetAudioClip(AudioClip clip, bool inheritTime = false)
        {
            if (m_As != null)
            {
                if (AudioPause())
                {
                    if (m_Ac != clip)
                    {
                        m_Ac = clip;
                        m_As.clip = clip;
                        if (clip != null)
                        {
                            if (inheritTime)
                            {
                                m_As.time = Mathf.Clamp(m_PlayedTime, 0, m_Ac.length);
                            }
                        }
                    }
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}
