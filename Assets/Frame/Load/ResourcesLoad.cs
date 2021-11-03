using System.Collections;
using UnityEngine;
using UnityEngine.Events;
namespace Farme
{
    /// <summary>
    /// Res加载
    /// </summary>
    public class ResourcesLoad
    {
        protected ResourcesLoad() { }
        #region//同步加载资源
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resPath">资源路径</param>
        /// <param name="result">结果</param>
        /// <returns></returns>
        public static bool Load<T>(string resPath,out T result) where T : Object
        {
            result = Resources.Load<T>(resPath);        
            if(result==null)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 加载资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resPath">资源路径</param>
        /// <param name="result">结果</param>
        /// <returns></returns>
        public static T Load<T>(string resPath) where T : Object
        {
           return Resources.Load<T>(resPath);           
        }
        #endregion
        #region//异步加载资源
        /// <summary>
        /// 异步加载资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="resPath">资源路径</param>
        /// <param name="callBack">回调</param>
        public static void LoadAsync<T>(string resPath, UnityAction<T> callBack) where T : Object
        {
            //开启协程进行资源加载
            MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IELoadAsync(resPath, callBack));
        }
        /// <summary>
        /// 协程启动异步加载Res资源
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="resPath">资源路径</param>
        /// <param name="callBack">回调</param>
        /// <returns></returns>
        private static IEnumerator IELoadAsync<T>(string resPath, UnityAction<T> callBack) where T : Object
        {
            //异步加载资源
            ResourceRequest rr = Resources.LoadAsync<T>(resPath);
            yield return rr;//等待请求完成
            callBack?.Invoke(rr.asset as T);
        }
        #endregion
    }
}
