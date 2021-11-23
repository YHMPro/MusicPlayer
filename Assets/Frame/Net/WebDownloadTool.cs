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
        private const string GB2312 = "gb2312";//用处含有未知
        private static float m_DownloadTextureRequestOutTime = 10;
        private static float m_DownloadAssetBundleOutTime = 10;
        private static float m_DownloadTextRequestOutTime = 5;

        public static void WebDownLoadMp3(string url, UnityAction<AudioClip> resultCallback, UnityAction<float> downLoadProgress = null, UnityAction requestTimeOutCallback = null)
        {
            MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IEWebDownLoadMp3(url, resultCallback, downLoadProgress, requestTimeOutCallback));
        }

        private static IEnumerator IEWebDownLoadMp3(string url, UnityAction<AudioClip> resultCallback, UnityAction<float> downLoadProgress = null, UnityAction requestTimeOutCallback = null)
        {
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN);
            www.SendWebRequest();//发送请求
            Coroutine cor = MonoSingletonFactory<ShareMono>.GetSingleton().DelayAction(m_DownloadAssetBundleOutTime, requestTimeOutCallback);
            while (true)
            {
                downLoadProgress?.Invoke(www.downloadProgress);//回调下载进度
                if (www.isDone && www.downloadHandler.isDone)
                {
                    break;
                }
                else
                {
                    if (cor != null && www.downloadProgress != 0)
                    {
                        MonoSingletonFactory<ShareMono>.GetSingleton().StopCoroutine(cor);//撤销下载超时回调
                        cor = null;
                    }
                }
                yield return www.downloadProgress;
            }
            NAudioPlayer.FromMp3Data(www.downloadHandler.data, (clip) =>
            {
                resultCallback?.Invoke(clip);//回调下载结果
            });
        }

        #region 文本下载
        public static void WebDownloadText(string url, UnityAction<string> resultCallback, UnityAction<float> downLoadProgress = null, UnityAction requestTimeOutCallback = null)
        {
            MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IEWebDownloadText(url, resultCallback, downLoadProgress, requestTimeOutCallback));
        }
        private static IEnumerator IEWebDownloadText(string url, UnityAction<string> resultCallback, UnityAction<float> downLoadProgress = null, UnityAction requestTimeOutCallback = null)
        {
            UnityWebRequest www = UnityWebRequest.Get(url);//创建网络请求
            www.SendWebRequest();//发送请求
            Coroutine cor = MonoSingletonFactory<ShareMono>.GetSingleton().DelayAction(m_DownloadAssetBundleOutTime, requestTimeOutCallback);
            while (true)
            {
                downLoadProgress?.Invoke(www.downloadProgress);//回调下载进度
                if (www.isDone && www.downloadHandler.isDone)
                {
                    break;
                }
                else
                {
                    if (cor != null && www.downloadProgress != 0)
                    {
                        MonoSingletonFactory<ShareMono>.GetSingleton().StopCoroutine(cor);//撤销下载超时回调
                        cor = null;
                    }
                }
                yield return www.downloadProgress;
            }
            /*Encoding.GetEncoding(GB2312)为特殊处理  暂不知为何为GB2312  本解释只针对文本编码格式ANSI类型转
             * Unity的文本编码UTF-8
             */
            resultCallback?.Invoke(Encoding.GetEncoding(GB2312).GetString(www.downloadHandler.data));
        }
        #endregion

        #region AB包下载
        public static void WebDownloadAssetBundle(string url, UnityAction<AssetBundle> resultCallback, UnityAction<float> downLoadProgress = null, UnityAction requestTimeOutCallback = null)
        {
            MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IEWebDownloadAssetBundle(url, resultCallback, downLoadProgress, requestTimeOutCallback));
        }
        private static IEnumerator IEWebDownloadAssetBundle(string url, UnityAction<AssetBundle> resultCallback, UnityAction<float> downLoadProgress = null, UnityAction requestTimeOutCallback = null)
        {
            UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url);//创建网络请求
            www.SendWebRequest();//发送请求
            while(true)
            {
                if (www.downloadHandler.isDone)
                {
                    Debuger.Log(www.isDone);
                    break;
                }
                yield return www;
            }
            //Coroutine cor = MonoSingletonFactory<ShareMono>.GetSingleton().DelayUAction(m_DownloadAssetBundleOutTime, requestTimeOutCallback);
            //while (true)
            //{                            
            //    downLoadProgress?.Invoke(www.downloadProgress);//回调下载进度
            //    if (www.isDone && www.downloadHandler.isDone)
            //    {                   
            //        break;
            //    }
            //    else
            //    {
            //        if (cor != null&&www.downloadProgress!=0)
            //        {
            //            MonoSingletonFactory<ShareMono>.GetSingleton().StopCoroutine(cor);//撤销下载超时回调
            //            cor = null;
            //        }
            //    }
            
            //}
            //resultCallback?.Invoke(DownloadHandlerAssetBundle.GetContent(www));
        }
        #endregion

        #region 纹理下载
        public static void WebDownloadTexture(string url, UnityAction<Texture2D> resultCallback, UnityAction<float> downLoadProgress = null, UnityAction requestTimeOutCallback=null)
        {
            MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IEWebDownloadTexture(url, resultCallback, downLoadProgress,()=> 
            {

            }));
        }       
        private static IEnumerator IEWebDownloadTexture(string url, UnityAction<Texture2D> resultCallback,UnityAction<float> downLoadProgress = null, UnityAction requestTimeOutCallback=null)
        {
            UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);//创建网络请求
            www.SendWebRequest();//发送请求
            Coroutine cor = MonoSingletonFactory<ShareMono>.GetSingleton().DelayAction(m_DownloadAssetBundleOutTime, requestTimeOutCallback);
            while (true)
            {
                downLoadProgress?.Invoke(www.downloadProgress);//回调下载进度
                if (www.isDone && www.downloadHandler.isDone)
                {
                    break;
                }
                else
                {
                    if (cor != null && www.downloadProgress != 0)
                    {
                        MonoSingletonFactory<ShareMono>.GetSingleton().StopCoroutine(cor);//撤销下载超时回调
                        cor = null;
                    }
                }
                yield return www.downloadProgress;
            }
            resultCallback?.Invoke(DownloadHandlerTexture.GetContent(www));
        }
        #endregion
    }
}
