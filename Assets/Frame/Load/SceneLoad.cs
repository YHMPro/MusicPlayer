using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
namespace Farme
{
    /// <summary>
    /// 场景加载
    /// </summary>
    public class SceneLoad
    {
        #region//异步加载场景
        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="startLoadCallback">开始加载回调</param>
        /// <param name="endLoadCallback">结束加载回调</param>
        /// <param name="loadProgressCallback">加载进度回调</param>
        public static void LoadSceneAsync(string sceneName, UnityAction startLoadCallback = null, UnityAction endLoadCallback = null, UnityAction<float> loadProgressCallback = null)
        {
            //开启协程
            MonoSingletonFactory<ShareMono>.GetSingleton().StartCoroutine(IELoadScene(sceneName, startLoadCallback, endLoadCallback, loadProgressCallback));
        }
        /// <summary>
        /// 协程加载场景
        /// </summary>
        /// <param name="sceneName">场景名称</param>
        /// <param name="startLoadCallback">回调切换场景前</param>
        /// <param name="endLoadCallback">回调切换场景后</param>
        /// <param name="loadProgressCallback">加载进度回调</param>
        /// <returns></returns>
        private static IEnumerator IELoadScene(string sceneName, UnityAction startLoadCallback = null, UnityAction endLoadCallback = null, UnityAction<float> loadProgressCallback = null)
        {
            //用于处理加载前场景数据
            startLoadCallback?.Invoke();
            AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
            while (!ao.isDone)//等待场景加载完成
            {
                loadProgressCallback?.Invoke(ao.progress);
                yield return ao.progress;
            }
            //用于初始化数据
            endLoadCallback?.Invoke();
        }
        #endregion

    }
}
