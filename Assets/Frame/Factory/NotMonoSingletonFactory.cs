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
            if (_instance == null)
            {
                lock (_threadLock)
                {
                    if (_instance == null)
                    {
                        NotMonoFactory<T>.GetInstance(out _instance);
                    }
                }
            }
            result = _instance;
        }
        /// <summary>
        /// 获取单例
        /// </summary>
        /// <returns></returns>
        public static T GetSingleton()
        {
            if (_instance == null)
            {
                lock (_threadLock)
                {
                    if (_instance == null)
                    {
                        NotMonoFactory<T>.GetInstance(out _instance);
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
            ClearInstance();
        }
        #endregion
    }
}
