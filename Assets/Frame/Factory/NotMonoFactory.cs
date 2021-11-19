namespace Farme
{
    /// <summary>
    /// 非MonoBehaviour工厂
    /// </summary>
    public class NotMonoFactory<T> where T:class,new()
    {
        protected NotMonoFactory() { }
        /// <summary>
        /// 获取实例
        /// </summary>
        /// <param name="result">结果</param>     
        public static void GetInstance(out T result)
        {
            result = new T();
        }
        /// <summary>
        /// 获取实例
        /// </summary>      
        /// <returns>实例</returns>
        public static T GetInstance()
        {
            return new T();
        }
    }
}
