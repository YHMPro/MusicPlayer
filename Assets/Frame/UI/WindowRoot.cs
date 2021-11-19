using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Farme.Tool;
using UnityEngine.Events;
using Farme.Extend;
namespace Farme.UI
{
    /// <summary>
    /// 窗口根节点
    /// </summary>
    public class WindowRoot : MonoBehaviour
    {
        protected WindowRoot() { }
        #region 字段
        private readonly string m_WindowModelPath = "FarmeLockFile\\StandardWindow";
        private EventSystem m_ES = null;
        private StandaloneInputModule m_InputModule = null;
        private Camera m_Camera = null;
        private Dictionary<string, StandardWindow> m_WindowModelDic = null;            
        #endregion
        #region 属性
        /// <summary>
        /// 事件系统
        /// </summary>
        public EventSystem ES
        {
            get
            {
                if(m_ES==null)
                {
                    m_ES = GetComponent<EventSystem>();
                }
                return m_ES;
            }
        }      
        /// <summary>
        /// 相机
        /// </summary>
        public Camera Camera
        {
            get
            {
                if(m_Camera==null)
                {
                    m_Camera = GetComponent<Camera>();
                }
                return m_Camera;
            }
        }
        #endregion
        #region 生命周期函数
        private void Awake()
        {
            m_WindowModelDic = new Dictionary<string, StandardWindow>();
            m_ES = GetComponent<EventSystem>();
            m_InputModule = GetComponent<StandaloneInputModule>();
            m_Camera = GetComponent<Camera>();
        }
        #endregion
        #region 方法
        /// <summary>
        /// 创建窗口
        /// </summary>
        /// <param name="windowName">窗口名称</param>
        /// <param name="renderMode">渲染模式</param>
        /// <param name="callback">回调</param>
        public void CreateWindow(string windowName, RenderMode renderMode=RenderMode.ScreenSpaceOverlay,UnityAction<StandardWindow> callback=null)
        {
            if(m_WindowModelDic.ContainsKey(windowName))
            {
                Debuger.LogWarning("窗口:"+ windowName+"反复创建。");
                return;
            }
            if (GoLoad.Take(m_WindowModelPath,out GameObject go))
            {
                go.transform.SetParent(transform);
                StandardWindow windowModel = go.InspectComponent<StandardWindow>();
                windowModel.gameObject.name = windowName;          
                windowModel.Canvas.renderMode = renderMode;
                windowModel.Canvas.worldCamera = m_Camera;             
                switch (renderMode)
                {
                    case RenderMode.ScreenSpaceOverlay:
                        {                           
                            break;
                        }
                    case RenderMode.ScreenSpaceCamera:
                        {                          
                            break;
                        }
                    case RenderMode.WorldSpace:
                        {
                            windowModel.transform.localScale = Vector3.one * 0.001f;
                            break;
                        }
                }
                m_WindowModelDic.Add(windowName, windowModel);
                callback?.Invoke(windowModel);
            }                                      
        }
        /// <summary>
        /// 获取窗口
        /// </summary>
        /// <param name="windowName">窗口名称</param>
        /// <returns>窗口</returns>
        public StandardWindow GetWindow(string windowName)
        {
            if(m_WindowModelDic.TryGetValue(windowName,out StandardWindow windowModel))
            {
                return windowModel;
            }
            return null;
        }
        /// <summary>
        /// 移除窗口
        /// </summary>
        /// <param name="windowName">窗口名称</param>
        public void RemoveWindow(string windowName)
        {
            if(m_WindowModelDic.ContainsKey(windowName))
            {
                m_WindowModelDic.Remove(windowName);
            }          
        }       
        /// <summary>
        /// UI事件注册
        /// 注:指针事件触发条件(1:UI透明的>=0.1 2:UI的RaycastTarget为True)
        /// </summary>
        /// <param name="uiB">ui对象</param>
        /// <param name="eTType">添加的事件触发类型</param>
        /// <param name="callBack">添加的回调</param>
        public static void UIEventRegistered(UIBehaviour uiB, EventTriggerType eTType, UnityAction<BaseEventData> callBack)
        {
            if (uiB == null)
            {
                Debuger.Log("申请对象为NULL");
                return;
            }
            if (callBack == null)
            {
                Debuger.Log("回调为NULL");
                return;
            }
            //获取UI对象身下的EventTrigger组件实例
            EventTrigger eT = uiB.gameObject.InspectComponent<EventTrigger>();
            //便利已添加的事件
            foreach (var t in eT.triggers)
            {
                //判断需要申请的事件类型是否已添加
                if (t.eventID == eTType)
                {
                    //添加回调监听
                    t.callback.AddListener(callBack);
                    return;
                }
            }
            //实例事件入口用于添加监听的事件
            EventTrigger.Entry e = new EventTrigger.Entry();
            //赋予事件类型
            e.eventID = eTType;
            //添加对该类型事件的监听
            e.callback.AddListener(callBack);
            //实例对象添加对该事件类型的监听
            eT.triggers.Add(e);
        }
        /// <summary>
        /// UI事件移除
        /// </summary>
        /// <param name="uiB">UI对象</param>
        /// <param name="eTType">移除事件触发类型</param>
        /// <param name="callBack">移除的回调</param>
        public static void UIEventRemove(UIBehaviour uiB, EventTriggerType eTType, UnityAction<BaseEventData> callBack)
        {
            if (uiB == null)
            {
                Debuger.Log("申请对象为NULL");
                return;
            }
            if (callBack == null)
            {
                Debuger.Log("回调为NULL");
                return;
            }
            //获取UI对象身下的EventTrigger组件实例
            if (!uiB.gameObject.TryGetComponent(out EventTrigger eT))
            {
                return;
            }
            //遍历已添加的监听事件
            foreach (var t in eT.triggers)
            {
                //找到对应事件类型
                if (t.eventID == eTType)
                {
                    //移除该事件类型下的回调
                    t.callback.RemoveListener(callBack);
                    break;//跳出foreach
                }
            }
        }
        #endregion
    }
}
