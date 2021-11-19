using System.Collections.Generic;
using UnityEngine;
namespace Farme
{
    /// <summary>
    /// Mono基类
    /// </summary>
    public abstract class BaseMono : MonoBehaviour
    {
        protected BaseMono() { }
        #region 字段
        private Dictionary<string, List<Component>> m_ComponentDic;
        protected Dictionary<string,List<Component>> ComponentDic
        {
            get
            {
                if(m_ComponentDic==null)
                {
                    m_ComponentDic = new Dictionary<string, List<Component>>();
                }
                return m_ComponentDic;
            }
        }

        #endregion
        #region 生命周期
        /// <summary>
        /// 组件初始化完成之后被调用一次
        /// </summary>
        protected virtual void Awake() { }    
        /// <summary>
        /// 组件被激活时调用一次
        /// </summary>
        protected virtual void OnEnable()
        {
            Invoke("LateOnEnable", 0);
        }
        /// <summary>
        /// 组件初始化完成之后执行一次(注:在Awake之后执行)
        /// </summary>
        protected virtual void Start() { }      
        /// <summary>
        /// 自身禁用后执行一次
        /// </summary>
        protected virtual void OnDisable() { }     
        /// <summary>
        /// 自身销毁时自动调用一次(通过Destroy(Object target)形式)
        /// </summary>
        protected virtual void OnDestroy()
        {
            m_ComponentDic = null;
        }
#if UNITY_EDITOR
        protected virtual void OnValidate() { }      
#endif
#endregion
        #region 方法     
        /// <summary>
        /// 注册组件类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="includeInactive">不活跃的对象是否查找</param>
        protected void RegisterComponentsTypes<T>(bool includeInactive=true) where T : Component
        {                     
            T[] tS = GetComponentsInChildren<T>(includeInactive);
            foreach (T t in tS)
            {             
                if (ComponentDic.TryGetValue(t.name,out List<Component> componentLi))
                {
                    if(!componentLi.Contains(t))
                    {
                        componentLi.Add(t);
                    }
                }
                else
                {
                    ComponentDic.Add(t.name, new List<Component>() { t });
                }               
            }
        }     
        /// <summary>
        /// 获取组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetName">对象名称</param>
        /// <param name="result">组件</param>
        public virtual bool GetComponent<T>(string targetName,out T result) where T : Component
        {
            result = null;          
            if(ComponentDic.TryGetValue(targetName,out List<Component> componentLi))
            {
                foreach(var component in componentLi)
                {
                    if(component is T)
                    {
                        result= component as T;
                        break;
                    }                   
                }
            }    
            if(result==null)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 获取组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="targetName">对象名称</param>
        /// <returns></returns>
        public virtual T GetComponent<T>(string targetName) where T : Component
        {          
            if (ComponentDic.TryGetValue(targetName, out List<Component> componentLi))
            {
                foreach (var component in componentLi)
                {
                    if (component is T)
                    {
                        return component as T;
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 获取多个同类型组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="targetName">对象名称</param>
        /// <param name="resultLi">组件列表</param>
        public virtual void GetComponents<T>(string targetName,out List<T> resultLi) where T : Component
        {
            resultLi = new List<T>();      
            if (ComponentDic.TryGetValue(targetName, out List<Component> componentLi))
            {
                foreach (var component in componentLi)
                {
                    if (component is T)
                    {
                        resultLi.Add(component as T);
                    }
                }
            }                    
        }
        /// <summary>
        /// 获取多个同类型组件
        /// </summary>
        /// <typeparam name="T">组件类型</typeparam>
        /// <param name="targetName">对象名称</param>
        /// <returns></returns>
        public virtual List<T> GetComponents<T>(string targetName) where T:Component
        {
            List<T> ts = new List<T>();          
            if (ComponentDic.TryGetValue(targetName, out List<Component> componentLi))
            {
               
                foreach (var component in componentLi)
                {
                    if (component is T)
                    {
                        ts.Add(component as T);
                    }
                }             
            }
            return ts;
        }
        /// <summary>
        /// 自动调用  在OnEnable之后调用且慢于Start调用快于Update的第一帧调用
        /// OnEnable函数不能被子类完全重写
        /// </summary>
        protected virtual void LateOnEnable() {  }    
        #endregion
    }
}
