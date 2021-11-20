using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
namespace Farme.Net
{
    public class DownLoad
    {
        /// <summary>
        /// 未知
        /// </summary>
        private const string GB2312 = "gb2312";
        /// <summary>
        /// 下载Audio资源
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="resultCallback">结果回调</param>
        /// <param name="progressCallback">进度回调</param>
        public static void DownLoadAudioClipAsset(string url,UnityAction<AudioClip> resultCallback, UnityAction<float> progressCallback = null)
        {
            MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IEDownLoadAudioClipAsset(url, resultCallback, progressCallback));
        }
        /// <summary>
        /// 下载Audio资源  结合NAudio处理音频文件  最终返回AudioClip类型
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="resultCallback">结果回调</param>
        /// <param name="progressCallback">进度回调</param>
        private static IEnumerator IEDownLoadAudioClipAsset(string url, UnityAction<AudioClip> resultCallback, UnityAction<float> progressCallback = null)
        {          
            UnityWebRequest uwr = CreateWebRequest(url);//创建Web请求
            DownloadHandlerAudioClip handle = new DownloadHandlerAudioClip(url, AudioType.UNKNOWN);//建立下载程序            
            uwr.downloadHandler = handle;//赋值
            UnityWebRequestAsyncOperation ao=uwr.SendWebRequest();//发送请求
            while (true)
            {           
                progressCallback?.Invoke(uwr.downloadProgress);//1.远程下载进度
                if (uwr.isDone && uwr.downloadHandler.isDone)//1.与远程建立通信是否成功  2.给予远程的任务是否完成
                { break; }
                yield return uwr.downloadProgress;
            }         
            progressCallback?.Invoke(1);//回调1          
            NAudioPlayer.FromMp3Data(uwr.downloadHandler.data, (clip) =>
             {
                 resultCallback?.Invoke(clip);//回调下载结果
             });
           
        }

        /// <summary>
        /// 下载文本资源
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="resultCallback">下载结果回调</param>
        /// <param name="progressCallback">下载进度回调</param>
        /// <returns></returns>
        public static void DownLoadTextAsset(string url, UnityAction<string> resultCallback, UnityAction<float> progressCallback = null)
        {
            MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IEDownLoadTextAsset(url, resultCallback, progressCallback));
        }
        /// <summary>
        /// 下载文本资源
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="resultCallback">下载结果回调</param>
        /// <param name="progressCallback">下载进度回调</param>
        private static IEnumerator IEDownLoadTextAsset(string url, UnityAction<string> resultCallback, UnityAction<float> progressCallback=null)
        {                      
            UnityWebRequest uwr = CreateWebRequest(url);//创建请求     
            UnityWebRequestAsyncOperation ao = uwr.SendWebRequest();//发送下载请求请求
            while (true)
            {            
                progressCallback?.Invoke(uwr.downloadProgress);//1.远程下载进度
                if (uwr.isDone && uwr.downloadHandler.isDone)//1.与远程建立通信是否成功  2.给予远程的任务是否完成
                { break;}
                yield return uwr.downloadProgress;
            }
            progressCallback?.Invoke(1);//回调1
            resultCallback?.Invoke(Encoding.GetEncoding(GB2312).GetString(uwr.downloadHandler.data));//回调下载结果         
        }

        /// <summary>
        /// 创建Web请求
        /// </summary>
        /// <param name="url">绝对路径地址</param>
        /// <returns></returns>
        private static UnityWebRequest CreateWebRequest(string url)
        {
            return UnityWebRequest.Get(url);
        }
    }
}
