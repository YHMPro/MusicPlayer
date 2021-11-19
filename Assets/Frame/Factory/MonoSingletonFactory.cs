using UnityEngine;
using Farme.Tool;
namespace Farme
{    
    /// <summary>
    /// Mono单例工厂
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MonoSingletonFactory<T> : AbstractSingletonFactory<T> where T : MonoBehaviour
    {
        protected MonoSingletonFactory() { }
        #region 属性
        /// <summary>
        /// 单例是否存在
        /// </summary>
        public static bool SingletonExist { get { return InstanceExist; } }
        #endregion
        #region 方法     
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <param name="result">结果</param>
        /// <param name="applyTarget">申请目标</param>
        /// <param name="isDontDestroyOnLoad">加载新场景时是否销毁目标对象</param>
        public static void GetSingleton(out T result, GameObject applyTarget = null, bool isDontDestroyOnLoad = true)
        {
            if (m_Instance == null)
            {
                lock (m_ThreadLock)
                {
                    if (m_Instance == null)
                    {
                        T[] instances = FindSingletons();
                        if (instances.Length==0)
                        {
                            MonoFactory<T>.GetInstance(out m_Instance, applyTarget, isDontDestroyOnLoad);
                        }
                        else
                        {
                            m_Instance = instances[0];
                            if (instances.Length > 1)
                            {
                                Debuger.LogWarning("场景内存在多个" + typeof(T).Name + "实例");
                            }                            
                        }                      
                    }
                }
            }
            result = m_Instance;         
        }
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <param name="applyTarget">申请对象</param>
        /// <param name="isDontDestroyOnLoad">加载新场景时是否销毁目标对象。</param>
        /// <returns></returns>
        public static T GetSingleton(GameObject applyTarget = null, bool isDontDestroyOnLoad = true)
        {
            if (m_Instance == null)
            {
                lock (m_ThreadLock)
                {
                    if (m_Instance == null)
                    {
                        T[] instances = FindSingletons();
                        if (instances.Length == 0)
                        {
                            m_Instance = MonoFactory<T>.GetInstance(applyTarget, isDontDestroyOnLoad);
                        }
                        else
                        {
                            m_Instance = instances[0];
                            if (instances.Length > 1)
                            {
                                Debuger.LogWarning("场景内存在多个" + typeof(T).Name + "实例");
                            }
                        }                       
                    }
                }
            }
            return m_Instance;
        }
        /// <summary>
        /// 清除单例
        /// </summary>
        public static void ClearSingleton()
        {
            if (m_Instance != null)
            {
                Object.Destroy(m_Instance.gameObject);
            }
            ClearInstance();
        }
        /// <summary>
        /// 查找单例
        /// </summary>
        /// <returns></returns>
        private static T[] FindSingletons()
        {
            return Object.FindObjectsOfType<T>();
        }
        #endregion
    }
}
