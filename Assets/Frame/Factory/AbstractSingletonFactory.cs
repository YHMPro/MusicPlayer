namespace Farme
{
    /// <summary>
    /// 抽象单例工厂
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AbstractSingletonFactory<T> where T:class
    {
        protected AbstractSingletonFactory() { }
        #region 字段
        /// <summary>
        /// 申请的单例实例
        /// </summary>
        protected static T _instance = null;
        /// <summary>
        /// 防止因线程安全导致的实例未初始化就被引用
        /// (线程安全详情:一个对象的实例过程分为三步:1.开辟内存 2.建立内存引用联系 3.初始化
        /// 线程并发操作会导致对象未初始化就被拿取)
        /// </summary>
        protected static readonly object _threadLock = new object();
        #endregion
        #region 属性
        /// <summary>
        /// 实例是否存在
        /// </summary>
        protected static bool InstanceExist { get { return _instance == null ? false : true; } }
        #endregion
        #region 方法        
        /// <summary>
        /// 清除实例
        /// </summary>
        protected static void ClearInstance()
        {        
            _instance = null;
        }
        #endregion
    }
}
