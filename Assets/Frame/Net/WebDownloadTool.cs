using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using System.Text;
using Farme.Tool;
namespace Farme.Net
{
    /// <summary>
    /// 网络下载工具
    /// </summary>
    public class WebDownloadTool 
    {
        public static Texture2D DefaultTexture2D = null;
        private const string GB2312 = "gb2312";//用处含有未知
        private static float m_DownloadTextureRequestOutTime = 10;
        private static float m_DownloadAssetBundleOutTime = 10;
        private static float m_DownloadTextRequestOutTime = 5;


        #region 音效下载
        /// <summary>
        /// 音效下载(仅限MP3)
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="resultCallback">结果回调</param>
        public static void WebDownLoadAudioClipMP3(string url,UnityAction<AudioClip> resultCallback)
        {
            MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IEWebDownLoadAudioClipMP3(url,resultCallback));
        }
        private static IEnumerator IEWebDownLoadAudioClipMP3(string url, UnityAction<AudioClip> resultCallback)
        {
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN);
            UnityWebRequestAsyncOperation uwrao = www.SendWebRequest();//发送请求 
            yield return uwrao;//等待异步请求完成
            if (www.isHttpError || www.isNetworkError)
            {
                Debuger.Log(www.error);
                resultCallback?.Invoke(null);
                yield break;//直接结束协程的后续操作
            }
            MP3Tool.FromMp3Data(www.downloadHandler.data, resultCallback);
        }
        /// <summary>
        /// 音效下载(MP3音效下载不支持)
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="audioType">音效类型</param>
        /// <param name="resultCallback">结果回调</param>
        public static void WebDownLoadAudioClip(string url, AudioType audioType, UnityAction<AudioClip> resultCallback)
        {
            MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IEWebDownLoadAudioClip(url, audioType,resultCallback));
        }
        private static IEnumerator IEWebDownLoadAudioClip(string url,AudioType audioType, UnityAction<AudioClip> resultCallback)
        {
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, audioType);
            UnityWebRequestAsyncOperation uwrao = www.SendWebRequest();//发送请求 
            yield return uwrao;//等待异步请求完成
            if (www.isHttpError || www.isNetworkError)
            {
                Debuger.Log(www.error);
                resultCallback?.Invoke(null);
                yield break;//直接结束协程的后续操作
            }
            resultCallback?.Invoke((www.downloadHandler as DownloadHandlerAudioClip).audioClip);
        }
        #endregion

        #region 文本下载
        /// <summary>
        /// 文本下载
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="resultCallback">结果回调</param>
        public static void WebDownloadText(string url, UnityAction<string> resultCallback)
        {
            MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IEWebDownloadText(url, resultCallback));
        }
        private static IEnumerator IEWebDownloadText(string url, UnityAction<string> resultCallback)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);//创建网络请求
            UnityWebRequestAsyncOperation uwrao = www.SendWebRequest();//发送请求
            yield return uwrao;//等待异步请求完成
            if (www.isHttpError || www.isNetworkError)
            {
                Debuger.Log(www.error);
                resultCallback?.Invoke(null);
                yield break;//直接结束协程的后续操作
            }
            /*Encoding.GetEncoding(GB2312)为特殊处理  暂不知为何为GB2312  本解释只针对文本编码格式ANSI类型转
             * Unity的文本编码UTF-8
             */
            resultCallback?.Invoke(Encoding.GetEncoding(GB2312).GetString(www.downloadHandler.data));
        }
        #endregion

        #region AB包下载
        /// <summary>
        /// AB包下载
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="resultCallback">结果回调</param>
        public static void WebDownloadAssetBundle(string url, UnityAction<AssetBundle> resultCallback)
        {
            MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IEWebDownloadAssetBundle(url, resultCallback));
        }
        private static IEnumerator IEWebDownloadAssetBundle(string url, UnityAction<AssetBundle> resultCallback)
        {
            UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url);//创建网络请求
            UnityWebRequestAsyncOperation uwrao = www.SendWebRequest();//发送请求
            yield return uwrao;//等待异步请求完成
            if (www.isHttpError || www.isNetworkError)
            {
                Debuger.Log(www.error);
                resultCallback?.Invoke(null);
                yield break;//直接结束协程的后续操作
            }
            resultCallback?.Invoke(DownloadHandlerAssetBundle.GetContent(www));           
        }
        #endregion

        #region 纹理下载
        /// <summary>
        /// 纹理下载
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="resultCallback">结果回调</param>
        public static void WebDownloadTexture(string url, UnityAction<Texture2D> resultCallback)
        {
            MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IEWebDownloadTexture(url, resultCallback));
        }       
        private static IEnumerator IEWebDownloadTexture(string url, UnityAction<Texture2D> resultCallback)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);//创建网络请求
            UnityWebRequestAsyncOperation uwrao = www.SendWebRequest();//发送请求
            yield return uwrao;//等待异步请求完成
            if(www.isHttpError||www.isNetworkError)
            {
                Debuger.Log(www.error);
                resultCallback?.Invoke(DefaultTexture2D);
                yield break;//直接结束协程的后续操作
            }        
            resultCallback?.Invoke(DownloadHandlerTexture.GetContent(www));        
        }
        #endregion
    }
}
