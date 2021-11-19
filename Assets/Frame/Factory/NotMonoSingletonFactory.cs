namespace Farme
{
    /// <summary>
    /// 非Mono单例工厂
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NotMonoSingletonFactory<T> : AbstractSingletonFactory<T> where T : class, new()
    {
        protected NotMonoSingletonFactory() { }
        #region 属性
        /// <summary>
        /// 单例是否存在
        /// </summary>
        public static bool SingletonExist{ get { return InstanceExist; } }
        #endregion
        #region 方法
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <param name="result">结果</param>
        public static void GetSingleton(out T result)
        {
            if (m_Instance == null)
            {
                lock (m_ThreadLock)
                {
                    if (m_Instance == null)
                    {
                        NotMonoFactory<T>.GetInstance(out m_Instance);
                    }
                }
            }
            result = m_Instance;
        }
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns>实例</returns>
        public static T GetSingleton()
        {
            if (m_Instance == null)
            {
                lock (m_ThreadLock)
                {
                    if (m_Instance == null)
                    {
                        NotMonoFactory<T>.GetInstance(out m_Instance);
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
            ClearInstance();
        }
        #endregion
    }
}
