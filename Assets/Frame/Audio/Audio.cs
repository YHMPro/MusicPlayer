using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using Farme.Tool;
namespace Farme.Audio
{
    [RequireComponent(typeof(AudioSource))]
    /// <summary>
    /// 音效
    /// </summary>
    public class Audio : MonoBehaviour
    {
        protected Audio() { }
        #region 生命周期函数
        private void Awake()
        {
            m_As = GetComponent<AudioSource>();
            m_AudioEvent = new AudioEvent();
            m_As.panStereo = 0;
            m_As.time = 0;
            m_As.clip = null;
            m_As.playOnAwake = false;
            m_As.loop = false;
            m_AbleRecycle = true;
            m_Timer = null;
            m_ListenVolumeExcess = null;
            AudioManager.NotInidleAudioLi.Add(this);
        }       
        #endregion

        #region 字段         
        /// <summary>
        /// 音效源
        /// </summary>
        private AudioSource m_As = null;         
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
        /// 是否能够回收(如果为False,则将永远不能回收到音效管理器当中)
        /// </summary>
        private bool m_AbleRecycle = true;
        /// <summary>
        /// 附属计时器
        /// </summary>
        private Coroutine m_Timer = null;
        /// <summary>
        /// 监听音量过度
        /// </summary>
        private Coroutine m_ListenVolumeExcess = null;
        /// <summary>
        /// 音效事件
        /// </summary>
        private AudioEvent m_AudioEvent = null;
        #endregion

        #region 属性 
        /// <summary>
        /// 音效事件
        /// </summary>
        public AudioEvent Event
        {
            set
            {
                m_AudioEvent = value;
            }
            get
            {
                return m_AudioEvent;
            }
        }
        /// <summary>
        /// 音效组
        /// </summary>
        public AudioMixerGroup Group
        {
            set
            {
                if (m_As.outputAudioMixerGroup != value)
                {
                    Debuger.Log("音效赋予了新的音效组");
                    m_As.outputAudioMixerGroup = value;
                }
            }
            get
            {
                return m_As.outputAudioMixerGroup;
            }
        }
        /// <summary>
        /// 音效剪辑
        /// </summary>
        public AudioClip Clip
        {
            set
            {
                if (m_As.clip != value)
                {
                    m_As.clip = value;
                    Pause();
                }             
            }
            get
            {
                return m_As.clip;
            }
        }
        /// <summary>
        /// 是否播放
        /// </summary>
        public bool IsPlay
        {
            get
            {
                return m_IsPlay;
            }
        }
        /// <summary>
        /// 是否暂停
        /// </summary>
        public bool IsPause
        {
            get
            {
                return m_IsPause;
            }
        }
        /// <summary>
        /// 是否停止
        /// </summary>
        public bool IsStop
        {
            get
            {
                return m_IsStop;
            }
        }
        /// <summary>
        /// 是否循环
        /// </summary>
        public bool Loop
        {
            set
            {               
                m_As.loop = value;
            }
            get
            {
                return m_As.loop;
            }
        }
        /// <summary>
        /// 是否能够回收(如果为False,则将永远不能回收到音效管理器当中)
        /// </summary>
        public bool AbleRecycle
        {
            set
            {
                m_AbleRecycle = value;
            }
            get
            {
                return m_AbleRecycle;
            }
        }         
        /// <summary>
        /// 左声道->右声道的渐变  -1(左)->0(立体)->1(右)
        /// </summary>
        public float StereoPan
        {
            set
            {
                m_As.panStereo = Mathf.Clamp(value, -1, 1);
            }
            get
            {
                return m_As.panStereo;
            }
        }
        /// <summary>
        /// 已播放的时长
        /// </summary>
        public float Time
        {
            set
            {
                m_As.time = value;
                if (m_AbleRecycle)
                {
                    if (!m_As.loop)
                    {
                        RemoveTimer();
                        AppendTimer();
                    }
                }
            }
            get
            {
                return m_As.time;
            }
        }
        /// <summary>
        /// 内置音量(不同于AudioMixerGroup中的音量)  
        /// </summary>
        public float Volume
        {
            set
            {
                m_As.volume = Mathf.Clamp(value, 0, 1);
            }
            get
            {
                return m_As.volume;
            }
        }
        /// <summary>
        /// 音效时长
        /// </summary>
        public float Length
        {
            get
            {
                if (m_As.clip != null)
                {
                    return m_As.clip.length;
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
        /// 添加计时器
        /// </summary>
        private void AppendTimer()
        {
            if(m_Timer == null&& m_As.clip!=null)
            {
                m_Timer = MonoSingletonFactory<ShareMono>.GetSingleton().DelayAction(m_As.clip.length - m_As.time, ()=>
                {                   
                    RemoveTimer();                  
                    if (m_As.loop)
                    {
                        if (m_AudioEvent != null)
                        {
                            m_AudioEvent.FinishEvent?.Invoke();
                        }
                        return;
                    }
                    m_IsPause = false;
                    m_IsPlay = false;
                    m_As.time = 0;   
                    m_IsStop = true;
                    m_As.Stop();
                    if (m_AbleRecycle)
                    {
                        if (AudioManager.NotInidleAudioLi.Contains(this))
                        {
                            AudioManager.NotInidleAudioLi.Remove(this);
                            //判断音效闲置栈中是否存该音效
                            if (!AudioManager.InidleAudios.Contains(this))
                            {
                                AudioManager.InidleAudios.Push(this);
                            }
                            gameObject.SetActive(false);
                        }
                    }
                    if (m_AudioEvent != null)
                    {
                        m_AudioEvent.FinishEvent?.Invoke();
                    }
                });            
            }
        }
        /// <summary>
        /// 移除计时器
        /// </summary>
        private void RemoveTimer()
        {
            if (m_Timer != null)
            {
                MonoSingletonFactory<ShareMono>.GetSingleton().StopCoroutine(m_Timer);//停止协程
                m_Timer = null;
            }
        }
        /// <summary>
        /// 播放
        /// </summary>
        public void Play()
        {            
            m_As.Play();
            AppendTimer();
            m_IsPause = false;
            m_IsPlay = true;
            m_IsStop = false;
            if (m_AudioEvent != null)
            {
                m_AudioEvent.StartEvent?.Invoke();
            }
        }
        /// <summary>
        /// 播放(先播放，然后音量过度到目标值)
        /// </summary>
        /// <param name="startVolume">过度起始时音量</param>
        /// <param name="endVolume">过度结束时音量</param>
        /// <param name="time">过度消耗的时间</param>
        public void Play(float startVolume,float endVolume,float time)
        {
            if (m_ListenVolumeExcess != null)
            {
                MonoSingletonFactory<ShareMono>.GetSingleton().StopCoroutine(m_ListenVolumeExcess);
            }
            m_As.volume = Mathf.Clamp(startVolume,0,1);
            Play();
            VolumeExcess(endVolume, time);
        }
        /// <summary>
        /// 重播
        /// </summary>
        public void RePlay()
        {
            m_As.time = 0;
            RemoveTimer();
            m_As.Play();
             m_IsPause = false;
            m_IsPlay = true;
            m_IsStop = false;       
            AppendTimer();
        }
        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            m_As.Pause();
            RemoveTimer();
            m_IsPause = true;
            m_IsPlay = false;
            m_IsStop = false;
        }
        /// <summary>
        /// 暂停(等待音量过度到目标值，然后暂停)
        /// </summary>
        /// <param name="endVolume">过度结束时音量</param>
        /// <param name="time">过度消耗的时间</param>
        public void Pause(float endVolume, float time)
        {
            if (m_ListenVolumeExcess != null)
            {
                MonoSingletonFactory<ShareMono>.GetSingleton().StopCoroutine(m_ListenVolumeExcess);
            }
            VolumeExcess(endVolume, time,()=> 
            {
                Pause();
            });
        }
        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            m_As.Stop();
            RemoveTimer();
            m_IsPause = false;
            m_IsPlay = false;
            m_IsStop = true;
            m_As.time = 0;
            if (m_AbleRecycle)
            {
                if (AudioManager.NotInidleAudioLi.Contains(this))
                {
                    AudioManager.NotInidleAudioLi.Remove(this);
                    //判断音效闲置栈中是否存该音效
                    if (!AudioManager.InidleAudios.Contains(this))
                    {
                        AudioManager.InidleAudios.Push(this);
                    }
                    gameObject.SetActive(false);
                }
            }
        }
        /// <summary>
        /// 停止(等待音量过度到目标值，然后停止)
        /// </summary>
        /// <param name="endVolume">过度结束时音量</param>
        /// <param name="time">过度消耗的时间</param>
        public void Stop(float endVolume, float time)
        {
            if (m_ListenVolumeExcess != null)
            {
                MonoSingletonFactory<ShareMono>.GetSingleton().StopCoroutine(m_ListenVolumeExcess);
            }
            VolumeExcess(endVolume, time, () =>
            {
                Stop();
                m_As.volume = 1;
            });

        }
        /// <summary>
        /// 音量过度(匀速变化)
        /// </summary>
        /// <param name="volume">目标音量</param>
        /// <param name="time">耗时</param>
        /// <param name="finishCallback">完成回调</param>
        private void VolumeExcess(float volume,float time,UnityAction finishCallback=null)
        {
            m_ListenVolumeExcess = MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IEVolumeExcess(volume,Mathf.Clamp(time,0,m_As.clip.length-m_As.time), finishCallback));        
        }
        private IEnumerator IEVolumeExcess(float volume,float time, UnityAction finishCallback=null)//1秒   50次循环
        {
            int num = (int)(50 * time);
            float interval = Mathf.Abs(m_As.volume - Mathf.Clamp(volume,0,1)) / num;
            WaitForSeconds waitForSeconds = new WaitForSeconds(0.02f);
            while (true)
            {            
                if(num < 0)
                {
                    m_ListenVolumeExcess = null;
                    finishCallback?.Invoke();
                    break;
                }
                m_As.volume = Mathf.MoveTowards(m_As.volume, volume, interval);
                num--;
                yield return waitForSeconds;
            }
        }
        /// <summary>
        /// 获取当前播放音频源的频谱数据块 
        /// </summary>
        /// <param name="samples">要填充音频样本的数组,它的长度一定是2的幂</param>
        /// <param name="channel">取样的通道</param>
        /// <param name="window">采样时使用的fftwwindow类型</param>
        public void GetSpectrumData(float[] samples, int channel, FFTWindow window)
        {
            if (m_As != null)
            {
                m_As.GetSpectrumData(samples, channel, window);
            }
        }
        #endregion
        /// <summary>
        /// 音效事件
        /// </summary>
        public class AudioEvent
        {
            /// <summary>
            /// 开始播放时的回调事件
            /// </summary>
            public UnityAction StartEvent;
            /// <summary>
            /// 播放完成的回调事件
            /// </summary>
            public UnityAction FinishEvent;                                    
        }
    }
}
