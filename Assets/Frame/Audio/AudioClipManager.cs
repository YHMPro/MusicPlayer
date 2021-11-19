using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;
using Farme.Extend;
namespace Farme.Audio
{
    /// <summary>
    /// 音效剪辑管理器
    /// </summary>
    public class AudioClipManager
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
        ///获取音效剪辑
        /// 基于Resources加载
        /// </summary>
        /// <param name="audioClipPath">音效剪辑路径</param>
        /// <param name="callback">回调</param>
        public static void GetAudioClip(string audioClipPath,UnityAction<AudioClip> callback)
        {
            string audioClipName = audioClipPath.AssignCharExtract('/');
            if (AudioClipDic.TryGetValue(audioClipName, out AudioClip result))
            {
                callback?.Invoke(result);
            }
            else
            {
                ResourcesLoad.LoadAsync<AudioClip>(audioClipPath, (clip) =>
                {
                    AudioClipDic.Add(audioClipName, clip);
                    callback?.Invoke(result);
                });                                                              
            }
        }
        /// <summary>
        /// 获取音效剪辑
        /// 基于Resources加载
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
        /// 基于Resources加载
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
        /// <summary>
        /// 基于AssetsBundle加载
        /// 异步加载
        /// </summary>
        /// <param name="abName">包名</param>
        /// <param name="resName">资源名</param>
        /// <param name="callback">回调</param>
        public static void GetAudioClip(string abName,string resName,UnityAction<AudioClip> callback)
        {
            if (AudioClipDic.TryGetValue(resName, out AudioClip result))
            {
                callback?.Invoke(result);
            }
            else
            {
                AssetBundleLoad.LoadAssetAsync<AudioClip>(abName, resName,(clip) =>
                 {
                     AudioClipDic.Add(resName, clip);
                     callback?.Invoke(result);
                 });                                             
            }
        }
        /// <summary>
        /// 获取音频剪辑
        /// 基于AssetsBundle加载
        /// </summary>
        /// <param name="abName">包名</param>
        /// <param name="resName">资源名</param>
        /// <param name="result">结果</param>
        /// <returns></returns>
        public static bool GetAudioClip(string abName, string resName,out AudioClip result)
        {
            if (AudioClipDic.TryGetValue(resName, out result))
            {
                return true;
            }
            else
            {
                if (AssetBundleLoad.LoadAsset(abName, resName, out result))
                {
                    AudioClipDic.Add(resName, result);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 获取音频剪辑
        /// 基于AssetsBundle加载
        /// </summary>
        /// <param name="abName">包名</param>
        /// <param name="resName">资源名</param>
        /// <returns></returns>
        public static AudioClip GetAudioClip(string abName,string resName)
        {
            if (AudioClipDic.TryGetValue(resName, out AudioClip result))
            {
                return result;
            }
            else
            {
                if(AssetBundleLoad.LoadAsset(abName,resName,out result))
                {
                    AudioClipDic.Add(resName, result);
                    return result;
                }
            }
            return null;
        }
        #endregion
    }
}
