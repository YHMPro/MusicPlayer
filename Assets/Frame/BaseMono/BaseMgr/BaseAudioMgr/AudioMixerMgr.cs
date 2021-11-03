using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
namespace Farme
{
    /// <summary>
    /// 音效混合器值的类型
    /// </summary>
    public enum EnumAudioMixerValueType
    {

    }
    /// <summary>
    /// 音效混合器管理器
    /// </summary>
    public class AudioMixerMgr
    {
        #region 字段
        private static string m_MainAudioMixerPath = "YHMFarmeLockFile/AudioMixer";
        private static AudioMixer m_MainAudioMixer = null;
        private static Dictionary<string, AudioMixerGroup> m_AudioMixerGroupDic = null;
        private static AudioMixerGroup m_NowOperationAudioMixerGroup = null;
        #endregion

        #region 属性
        /// <summary>
        /// 主音效混合器
        /// </summary>
        public static AudioMixer MainAudioMixer
        {
            get
            {
                if(m_MainAudioMixer==null)
                {
                    if(ResourcesLoad.Load(m_MainAudioMixerPath, out m_MainAudioMixer))
                    {
                        Debug.Log("加载主音效混合器成功!");
                    }
                }
                return m_MainAudioMixer;
            }
        }
        /// <summary>
        /// 音效混合器组容器
        /// </summary>
        private static Dictionary<string, AudioMixerGroup> AudioMixerGroupDic
        {
            get
            {
                if(m_AudioMixerGroupDic==null)
                {
                    m_AudioMixerGroupDic = new Dictionary<string, AudioMixerGroup>();
                    AudioMixerGroup[] groups = MainAudioMixer.FindMatchingGroups("Master");
                    foreach (var group in groups)
                    {
                        if (!m_AudioMixerGroupDic.ContainsKey(group.name))
                        {
                            m_AudioMixerGroupDic.Add(group.name, group);
                        }
                    }
                }
                return m_AudioMixerGroupDic;
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 获取音效混合器组
        /// </summary>
        /// <param name="groupName">组名</param>
        /// <param name="result">结果</param>
        /// <returns>是否获取成功</returns>
        public static bool GetAudioMixerGroup(string groupName,out AudioMixerGroup result)
        {        
            if (m_NowOperationAudioMixerGroup != null && m_NowOperationAudioMixerGroup.name == groupName)
            {
                result = m_NowOperationAudioMixerGroup;
                return true;
            }        
            if (AudioMixerGroupDic.TryGetValue(groupName,out result))
            {
                m_NowOperationAudioMixerGroup = result;
                return true;
            }
       
            return false;
        }
        /// <summary>
        /// 获取音效混合器组
        /// </summary>
        /// <param name="groupName">组名</param>
        /// <returns>音效混合器组</returns>
        public static AudioMixerGroup GetAudioMixerGroup(string groupName)
        {
            if(m_NowOperationAudioMixerGroup!=null&& m_NowOperationAudioMixerGroup.name== groupName)
            {
                return m_NowOperationAudioMixerGroup;
            }
            if (AudioMixerGroupDic.TryGetValue(groupName, out AudioMixerGroup result))
            {
                m_NowOperationAudioMixerGroup = result;
                return m_NowOperationAudioMixerGroup;
            }
            return null;
        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="groupName">组名称</param>
        /// <param name="valueName">值名称</param>
        /// <param name="value">值</param>
        /// <returns>是否设置成功</returns>
        public static bool SetFloat(string groupName,string valueName,float value)
        {
            if (m_NowOperationAudioMixerGroup != null && m_NowOperationAudioMixerGroup.name == groupName)
            {
                m_NowOperationAudioMixerGroup.audioMixer.SetFloat(valueName, value);
                return true;
            }
            if (GetAudioMixerGroup(groupName,out AudioMixerGroup result))
            {
                result.audioMixer.SetFloat(valueName, value);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="groupName">组名</param>
        /// <param name="valueName">值名称</param>
        /// <param name="value">值</param>
        /// <returns>是否获取成功</returns>
        public static bool GetFloat(string groupName, string valueName,out float value)
        {
            if (m_NowOperationAudioMixerGroup != null && m_NowOperationAudioMixerGroup.name == groupName)
            {
                m_NowOperationAudioMixerGroup.audioMixer.GetFloat(valueName,out value);
                return true;
            }
            if (GetAudioMixerGroup(groupName, out AudioMixerGroup result))
            {
                result.audioMixer.GetFloat(valueName,out value);
                return true;
            }
            value = 0;
            return false;
        }
        #endregion
    }
}
