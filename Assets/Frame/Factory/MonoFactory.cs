using UnityEngine;
namespace Farme
{
    /// <summary>
    /// MonoBehaviour工厂
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoFactory<T> where T : MonoBehaviour
    {
        protected MonoFactory() { }
        /// <summary>
        /// 获取实例
        /// </summary>
        /// <param name="result">结果</param>
        /// <param name="applyTarget">申请对象</param>
        /// <param name="isDontDestroyOnLoad">加载新场景时是否销毁目标对象</param>
        public static void GetInstance(out T result, GameObject applyTarget = null, bool isDontDestroyOnLoad = true)
        {
            if (applyTarget == null)
            {
                applyTarget = new GameObject(typeof(T).Name);
            }          
            if (!isDontDestroyOnLoad)
            {
                Object.DontDestroyOnLoad(applyTarget);
            }
            result = applyTarget.AddComponent<T>();
        }
        /// <summary>
        /// 获取实例
        /// </summary>
        /// <param name="applyTarget">申请对象</param>
        /// <param name="isDontDestroyOnLoad">加载新场景时是否销毁目标对象</param>
        /// <returns>实例</returns>
        public static T GetInstance(GameObject applyTarget = null, bool isDontDestroyOnLoad = true)
        {
            if (applyTarget == null)
            {
                applyTarget = new GameObject(typeof(T).Name);
            }            
            if (!isDontDestroyOnLoad)
            {
                Object.DontDestroyOnLoad(applyTarget);
            }
            return applyTarget.AddComponent<T>();
        }
    }
}
