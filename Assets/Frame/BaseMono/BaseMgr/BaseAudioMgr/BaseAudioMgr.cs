using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
namespace Farme
{
    /// <summary>
    /// Audio管理基类
    /// </summary>
    public abstract class BaseAudioMgr : BaseMono
    {

        #region 生命周期函数
        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();
            if(SetVolume("Master","MasterVolume",1.0f))
            {
                Debug.Log("初始化主音量成功");
            }
        }
        public void Update()
        {
            if(m_InidleAudioControlLi.Count> m_cacheAudioMax)
            {
                IAudioControl audioControl = m_InidleAudioControlLi[m_InidleAudioControlLi.Count - 1];
                if((audioControl as BaseMono)!=null)
                {
                    m_InidleAudioControlLi.Remove(audioControl);
                    Destroy((audioControl as BaseAudio).gameObject); 
                }
            }
        }
        #endregion
        #region 字段
        /// <summary>
        /// 闲置的音效播放器控制接口列表
        /// </summary>
        protected List<IAudioControl> m_InidleAudioControlLi = new List<IAudioControl>();
        /// <summary>
        /// 非闲置的音效播放器控制接口列表
        /// </summary>
        protected List<IAudioControl> m_NotInidleAudioControlLi = new List<IAudioControl>();
        [SerializeField]
        /// <summary>
        /// 缓存的音效播放器最大数量
        /// </summary>
        protected int  m_cacheAudioMax = 5;
        #endregion

        #region 属性
        /// <summary>
        /// 缓存的音效播放器最大数量
        /// </summary>
        public int CacheAudioMax
        {
            set
            {
                m_cacheAudioMax = Mathf.Clamp(value, 0,1000);
            }
            get
            {
                return m_cacheAudioMax;
            }
        }
        /// <summary>
        /// 非闲置的音效播放器控制接口列表
        /// </summary>
        public List<IAudioControl> NotInidleAudioControlLi
        {
            get
            {
                return m_NotInidleAudioControlLi;
            }
        }
        /// <summary>
        /// 闲置的音效播放器控制接口列表
        /// </summary>
        public List<IAudioControl> InidleAudioControlLi
        {
            get
            {
                return m_InidleAudioControlLi;
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 闲置与非闲置的置换
        /// </summary>
        /// <param name="baseAudio">音效控制接口</param>
        /// <returns>是否转换成功</returns>
        public bool InidleWithNotInidleTransform(IAudioControl audioControl)
        {
            if (m_InidleAudioControlLi.Contains(audioControl))
            {
                m_InidleAudioControlLi.Remove(audioControl);
                m_NotInidleAudioControlLi.Add(audioControl);
                return m_NotInidleAudioControlLi.Contains(audioControl);
            }
            if (m_NotInidleAudioControlLi.Contains(audioControl))
            {
                m_NotInidleAudioControlLi.Remove(audioControl);
                m_InidleAudioControlLi.Add(audioControl);
                return m_InidleAudioControlLi.Contains(audioControl);
            }
            return false;
        }
        /// <summary>
        /// 获取音效实例
        /// </summary>
        /// <returns>音效基类实例</returns>
        protected abstract BaseAudio GetAudioInstance();
        /// <summary>
        /// 申请音效控制器接口
        /// </summary>
        /// <returns></returns>
        public IAudioControl ApplyForAudioControl()
        {
            IAudioControl audioControl;
            if (m_InidleAudioControlLi.Count >= 1)
            {
                audioControl = m_InidleAudioControlLi[0];
                InidleWithNotInidleTransform(audioControl);
            }
            else
            {
                audioControl= GetAudioInstance().Control;               
            }
            return audioControl;
        }        
        /// <summary>
        /// 暂停音效
        /// </summary>
        /// <param name="audioMixerGroup">音效混合组</param>
        public void PauseAudio(AudioMixerGroup audioMixerGroup)
        {
            for (int index = m_NotInidleAudioControlLi.Count - 1; index >= 0; index--)
            {
                IAudioControl audioControl = m_NotInidleAudioControlLi[index];
                if (audioControl != null)
                {
                    audioControl.Pause();
                }
            }
        }
        /// <summary>
        /// 停止音效
        /// </summary>
        /// <param name="audioMixerGroup">音效混合组</param>
        public void StopAudio(AudioMixerGroup audioMixerGroup)
        {          
            for(int index= m_NotInidleAudioControlLi.Count-1;index>=0;index--)
            {
                IAudioControl audioControl = m_NotInidleAudioControlLi[index];
                if(audioControl!=null)
                {
                    audioControl.Stop();
                }
            }          
        }
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioMixerGroup">音效混合组</param>
        public void PlayAudio(AudioMixerGroup audioMixerGroup)
        {
            for (int index = m_NotInidleAudioControlLi.Count - 1; index >= 0; index--)
            {
                IAudioControl audioControl = m_NotInidleAudioControlLi[index];
                if (audioControl != null)
                {
                    audioControl.Play();
                }
            }
        }
        /// <summary>
        /// 重新播放音效
        /// </summary>
        /// <param name="audioMixerGroup">音效混合组</param>
        public void RePlayAudio(AudioMixerGroup audioMixerGroup)
        {
            for (int index = m_NotInidleAudioControlLi.Count - 1; index >= 0; index--)
            {
                IAudioControl audioControl = m_NotInidleAudioControlLi[index];
                if (audioControl != null)
                {
                    audioControl.RePlay();
                }
            }
        }
        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="groupName">组名称</param>
        /// <param name="valueName">值名称</param>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        public static bool SetVolume(string groupName, string valueName,float value)
        {
            value = Mathf.Clamp(value, 0, 1);
            value = value*80.0f -80.0f;
            return SetFloat(groupName, valueName, value);         
        }
        /// <summary>
        /// 获取音量
        /// </summary>
        /// <param name="groupName">组名称</param>
        /// <param name="valueName">值名称</param>
        /// <param name="value">值</param>
        /// <returns>是否获取成功</returns>
        public static bool GetVolume(string groupName, string valueName,out float value)
        {
           if( GetFloat(groupName, valueName,out value))
            {
                value = (value + 80.0f) / 80.0f;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="groupName">组名称</param>
        /// <param name="valueName">值名称</param>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        private static bool SetFloat(string groupName, string valueName,float value)
        {           
            return AudioMixerMgr.SetFloat(groupName, valueName,value);
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="groupName">组名称</param>
        /// <param name="valueName">值名称</param>
        /// <param name="value">值</param>
        /// <returns>是否获取成功</returns>
        private static bool GetFloat(string groupName, string valueName, out float value)
        {
            return AudioMixerMgr.GetFloat(groupName, valueName, out value);              
        }
        #endregion
    }
}
