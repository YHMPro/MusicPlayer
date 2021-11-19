using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Farme.Tool;
namespace Farme.Audio
{
    /// <summary>
    /// 音效管理器
    /// </summary>
    public class AudioManager 
    {
        #region 字段
        /// <summary>
        /// 闲置音效栈
        /// </summary>
        private static Stack<Audio> m_InidleAudios = null;       
        /// <summary>
        /// 非闲置的音效列表
        /// </summary>
        private static List<Audio> m_NotInidleAudioLi = null;
        /// <summary>
        /// 缓存的音效播放器最大数量
        /// </summary>
        private static int m_cacheAudioMax = 5;      
        #endregion

        #region 属性
        /// <summary>
        /// 非闲置的音效列表
        /// </summary>
        public static List<Audio> NotInidleAudioLi
        {
            get
            {
                if(m_NotInidleAudioLi==null)
                {
                    m_NotInidleAudioLi = new List<Audio>();
                }
                return m_NotInidleAudioLi;
            }
        }
        /// <summary>
        /// 闲置音效栈
        /// </summary>
        public static Stack<Audio> InidleAudios
        {
            get
            {
                if (m_InidleAudios == null)
                {
                    m_InidleAudios = new Stack<Audio>();
                }
                return m_InidleAudios;
            }
        }      
        /// <summary>
        /// 缓存的最大音效数量
        /// </summary>
        public static int CacheAudioMax
        {
            set
            {
                m_cacheAudioMax = value;
            }
            get
            {
                return m_cacheAudioMax;
            }
        }
        /// <summary>
        /// 主音量
        /// </summary>
        public static float MainVolume
        {
            set
            {
                AudioMixerManager.MainAudioMixer.SetFloat("MasterVolume", Mathf.Clamp(value, 0, 1));
            }
            get
            {
                AudioMixerManager.MainAudioMixer.GetFloat("MasterVolume", out float volume);
                return volume;
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 清除缓存(清除掉[现有数量-m_cacheAudioMax])
        /// </summary>
        public static void ClearCache()
        {
            ClearNULL_USING();//清除一次空引用
            while(InidleAudios.Count > m_cacheAudioMax)
            {             
                //销毁掉多余的音效
                Object.Destroy(InidleAudios.Pop().gameObject);              
            }
        }
        /// <summary>
        /// 清除空引用
        /// </summary>
        public static void ClearNULL_USING()
        {
            for (int index = NotInidleAudioLi.Count - 1; index >= 0; index--)
            {
                if (NotInidleAudioLi[index] == null)
                {
                    NotInidleAudioLi.RemoveAt(index);
                }
            }
            //保证音效闲置栈中的第一个Head元素不为NULL即可
            while(InidleAudios.Count>0&& InidleAudios.Peek()==null)
            {
                InidleAudios.Pop();//弹出该NULL  
            }            
        }
        /// <summary>
        /// 申请音效
        /// </summary>
        /// <returns></returns>
        public static Audio ApplyForAudio()
        {
            ClearNULL_USING();//清除一次空引用
            Audio audio;
            if(InidleAudios.Count>0)
            {
                audio = InidleAudios.Pop();
                if (!NotInidleAudioLi.Contains(audio))
                {
                    NotInidleAudioLi.Add(audio);
                }
                audio.gameObject.SetActive(true);
            }
            else
            {
                audio = MonoFactory<Audio>.GetInstance(new GameObject("Audio"));
            }                   
            return audio;
        }
        /// <summary>
        /// 全局暂停
        /// </summary>
        public static void GlobalPause()
        {
            for (int index = NotInidleAudioLi.Count - 1; index >= 0; index--)
            {
                Audio audio = NotInidleAudioLi[index];
                if (audio != null)
                {
                    audio.Pause();
                }
            }
        }
        /// <summary>
        /// 全局停止
        /// </summary>
        public static void GlobalStop()
        {
            for (int index = NotInidleAudioLi.Count - 1; index >= 0; index--)
            {
                Audio audio = NotInidleAudioLi[index];
                if (audio != null)
                {
                    audio.Stop();
                }
            }
        }
        /// <summary>
        /// 全局播放
        /// </summary>
        public static void GlobalPlay()
        {
            for (int index = NotInidleAudioLi.Count - 1; index >= 0; index--)
            {
                Audio audio = NotInidleAudioLi[index];
                if (audio != null)
                {
                    audio.Play();
                }
            }
        }
        /// <summary>
        /// 暂停音效
        /// </summary>
        /// <param name="audioMixerGroup">音效混合组</param>
        public static void Pause(AudioMixerGroup audioMixerGroup)
        {
            for (int index = NotInidleAudioLi.Count - 1; index >= 0; index--)
            {
                Audio audio = NotInidleAudioLi[index];
                if (audio != null && audio.Group == audioMixerGroup)
                {
                    audio.Pause();
                }
            }
        }
        /// <summary>
        /// 停止音效
        /// </summary>
        /// <param name="audioMixerGroup">音效混合组</param>
        public static void Stop(AudioMixerGroup audioMixerGroup)
        {
            for (int index = NotInidleAudioLi.Count - 1; index >= 0; index--)
            {
                Audio audio = NotInidleAudioLi[index];
                if (audio != null&& audio.Group== audioMixerGroup)
                {
                    audio.Stop();
                }
            }
        }
        /// <summary>
        /// 播放音效
        /// </summary>
        /// <param name="audioMixerGroup">音效混合组</param>
        public void Play(AudioMixerGroup audioMixerGroup)
        {
            for (int index = NotInidleAudioLi.Count - 1; index >= 0; index--)
            {
                Audio audio = NotInidleAudioLi[index];
                if (audio != null && audio.Group == audioMixerGroup)
                {
                    audio.Play();
                }
            }
        }
        /// <summary>
        /// 重新播放音效
        /// </summary>
        /// <param name="audioMixerGroup">音效混合组</param>
        public static void RePlay(AudioMixerGroup audioMixerGroup)
        {
            for (int index = NotInidleAudioLi.Count - 1; index >= 0; index--)
            {
                Audio audio = NotInidleAudioLi[index];
                if (audio != null&& audio.Group == audioMixerGroup)
                {
                    audio.RePlay();
                }
            }
        }
        /// <summary>
        /// 过度播放  一个音频过度到另一个音频的播放形式
        /// </summary>
        /// <param name="form">即将停止播放的音效</param>
        /// <param name="to">即将开始播放的音效</param>
        /// <param name="endVolume">过度结束时的音量</param>
        /// <param name="time">过度所消耗的时间</param>
        public static void ExcessPlay(Audio form,Audio to,float endVolume=1,float time=0)
        {
            if(form == to)
            {
                Debuger.LogWarning("相同的音效不能进行过度播放。");
            }
            form.Stop(0, time);
            to.Play(0, endVolume, time);
        }      
        /// <summary>
        /// 设置音量
        /// </summary>
        /// <param name="groupName">组名称</param>
        /// <param name="valueName">值名称</param>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        public static bool SetVolume(string groupName, string valueName, float value)
        {
            value = Mathf.Clamp(value, 0, 1);
            value = value * 80.0f - 80.0f;
            return SetFloat(groupName, valueName, value);
        }
        /// <summary>
        /// 获取音量
        /// </summary>
        /// <param name="groupName">组名称</param>
        /// <param name="valueName">值名称</param>
        /// <param name="value">值</param>
        /// <returns>是否获取成功</returns>
        public static bool GetVolume(string groupName, string valueName, out float value)
        {
            if (GetFloat(groupName, valueName, out value))
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
        private static bool SetFloat(string groupName, string valueName, float value)
        {
            return AudioMixerManager.SetFloat(groupName, valueName, value);
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
            return AudioMixerManager.GetFloat(groupName, valueName, out value);
        }
        #endregion
    }
}
