using UnityEngine;
using System.Collections.Generic;
namespace Farme
{
    public class AudioClipMgr
    {
        #region 字段
        private static Dictionary<string, AudioClip> m_AudioClipDic = null;
        private static Dictionary<string,AudioClip> AudioClipDic
        {
            get
            {
                if(m_AudioClipDic==null)
                {
                    m_AudioClipDic = new Dictionary<string, AudioClip>();
                }
                return m_AudioClipDic;
            }
        }
        #endregion

        #region 方法
        /// <summary>
        /// 获取音效剪辑
        /// </summary>
        /// <param name="audioClipPath">音效剪辑路径</param>
        /// <param name="result">结果</param>
        /// <returns>是否获取成功</returns>
        public static bool GetAudioClip(string audioClipPath,out AudioClip result)
        {            
            string audioClipName = audioClipPath.AssignCharExtract('/');
            if (AudioClipDic.TryGetValue(audioClipName, out result))
            {
                return true;
            }
            else
            {
                if (ResourcesLoad.Load(audioClipPath, out result))
                {
                    AudioClipDic.Add(audioClipName, result);
                    return true;
                }             
            }
            return false;      
        }
        /// <summary>
        /// 获取音效剪辑
        /// </summary>
        /// <param name="audioClipPath">音效剪辑路径</param>
        /// <returns></returns>
        public static AudioClip GetAudioClip(string audioClipPath)
        {
            string audioClipName = audioClipPath.AssignCharExtract('/');
            if (AudioClipDic.TryGetValue(audioClipName, out AudioClip result))
            {
                return result;
            }
            else
            {
                if (ResourcesLoad.Load(audioClipPath, out result))
                {
                    AudioClipDic.Add(audioClipName, result);
                    return result;
                }
            }
            return null;
        }
        #endregion
    }
}
