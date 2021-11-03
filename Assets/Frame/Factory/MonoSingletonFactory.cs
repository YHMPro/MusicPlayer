using UnityEngine;
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
            if (_instance == null)
            {
                lock (_threadLock)
                {
                    if (_instance == null)
                    {
                        T[] instances = FindSingletons();
                        if (instances.Length==0)
                        {
                            MonoFactory<T>.GetInstance(out _instance, applyTarget, isDontDestroyOnLoad);
                        }
                        else
                        {
                            _instance = instances[0];
                            if (instances.Length > 1)
                            {
                                Debug.LogWarning("场景内存在多个" + typeof(T).Name + "实例");
                            }                            
                        }                      
                    }
                }
            }
            result = _instance;         
        }
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <param name="applyTarget">申请对象</param>
        /// <param name="isDontDestroyOnLoad">加载新场景时是否销毁目标对象。</param>
        /// <returns></returns>
        public static T GetSingleton(GameObject applyTarget = null, bool isDontDestroyOnLoad = true)
        {
            if (_instance == null)
            {
                lock (_threadLock)
                {
                    if (_instance == null)
                    {
                        T[] instances = FindSingletons();
                        if (instances.Length == 0)
                        {
                            _instance= MonoFactory<T>.GetInstance(applyTarget, isDontDestroyOnLoad);
                        }
                        else
                        {
                            _instance = instances[0];
                            if (instances.Length > 1)
                            {
                                Debug.LogWarning("场景内存在多个" + typeof(T).Name + "实例");
                            }
                        }                       
                    }
                }
            }
            return _instance;
        }
        /// <summary>
        /// 清除单例
        /// </summary>
        public static void ClearSingleton()
        {
            if (_instance != null)
            {
                Object.Destroy(_instance.gameObject);
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
