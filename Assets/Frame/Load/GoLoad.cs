using UnityEngine;
using UnityEngine.Events;
namespace Farme
{
    /// <summary>
    /// Go加载
    /// </summary> 
    public class GoLoad
    {
        protected GoLoad() { }
        #region 方法
        /// <summary>
        /// 拿取Go实例
        /// 基于Resources加载方式
        /// </summary>
        /// <param name="goPath">go路径</param>
        /// <param name="result">拿取的Go实例</param>
        /// <param name="parent">父级</param>
        /// <returns></returns>
        public static bool Take(string goPath,out GameObject result, Transform parent = null)
        {
            if (ResourcesLoad.Load(goPath, out result))
            {               
                string goName = result.name;
                if (parent != null)
                {
                    result=Object.Instantiate(result, parent);
                }
                else
                {
                    result=Object.Instantiate(result);
                }
                result.name = goName;
                return true;
            }
            return false;
        }
        /// <summary>
        /// 拿取Go实例
        /// 基于Resources加载方式
        /// </summary>
        /// <param name="goPath">go路径</param>
        /// <param name="parent">父级</param>
        /// <param name="callBack">回调</param>
        public static void TakeAsync(string goPath, Transform parent = null,UnityAction<GameObject> callBack=null)
        {
            ResourcesLoad.LoadAsync<GameObject>(goPath, (go) =>
             {
                 if (go != null)
                 {
                     string goName = go.name;
                     if (parent != null)
                     {
                         go=Object.Instantiate(go, parent);
                     }
                     else
                     {
                         go=Object.Instantiate(go);
                     }
                     go.name = goName;
                     callBack?.Invoke(go);
                 }             
             });                           
        }
        #endregion
    }
}
