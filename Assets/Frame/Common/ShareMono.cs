using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Farme.Tool;
namespace Farme
{ 
    /// <summary>
    /// 更新活动方式
    /// </summary>
    public enum EnumUpdateAction
    {
        /// <summary>
        /// 固定
        /// </summary>
        Fixed,
        /// <summary>
        /// 后一帧
        /// </summary>
        Late,
        /// <summary>
        /// 标准
        /// </summary>
        Standard
    }
    /// <summary>
    /// 共享Mono 用于非MonoBehaviour派生类提供MonoBehaviour功能
    /// </summary>
    public class ShareMono : MonoBehaviour
    {
        #region 生命周期 
        /// <summary>
        /// 持续更新
        /// </summary>
        private void Update()
        {
            m_Callback?.Invoke();
            
        }
        /// <summary>
        /// 延迟更新
        /// </summary>
        private void LateUpdate()
        {
            m_LateCallback?.Invoke();
        }
        /// <summary>
        /// 固定更新
        /// </summary>
        private void FixedUpdate()
        {
            m_FixCallback?.Invoke();
        }
        #endregion
        protected ShareMono() { }       
        #region 事件       
        /// <summary>
        /// 回调
        /// </summary>
        private event UnityAction m_Callback;
        /// <summary>
        /// 回调
        /// </summary>
        private event UnityAction m_LateCallback;
        /// <summary>
        /// 回调
        /// </summary>
        private event UnityAction m_FixCallback;
        #endregion
        #region 方法      
        /// <summary>
        /// 申请Update行为
        /// </summary>
        /// <param name="updateAction">行为方式</param>
        /// <param name="callback">回调</param>
        public void ApplyUpdateAction(EnumUpdateAction updateAction,UnityAction callback)
        {
            switch(updateAction)
            {
                case EnumUpdateAction.Standard:
                    {
                        m_Callback += callback;
                        break;
                    }
                case EnumUpdateAction.Fixed:
                    {
                        m_FixCallback += callback;
                        break;
                    }
                case EnumUpdateAction.Late:
                    {
                        m_LateCallback += callback;
                        break;
                    }           
            }
        }
        /// <summary>
        /// 移除Update行为
        /// </summary>
        /// <param name="updateAction">行为方式</param>
        /// <param name="callback">回调</param>
        public void RemoveUpdateAction(EnumUpdateAction updateAction, UnityAction callback)
        {
            switch (updateAction)
            {
                case EnumUpdateAction.Standard:
                    {
                        m_Callback -= callback;
                        break;
                    }
                case EnumUpdateAction.Fixed:
                    {
                        m_FixCallback -= callback;
                        break;
                    }
                case EnumUpdateAction.Late:
                    {
                        m_LateCallback -= callback;
                        break;
                    }
            }
        }             
        /// <summary>
        /// 延迟执行(无参数传递) 
        /// </summary>
        /// <param name="delayTime">延迟时长</param>
        /// <param name="callback">回调</param>
        /// <returns></returns>
        public Coroutine DelayAction(float delayTime, UnityAction callback)
        {
            return StartCoroutine(IEDelayAction(Mathf.Clamp(delayTime, 0, delayTime), callback));
        }
        /// <summary>
        /// 延迟执行(含参数传递) 
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="delayTime">延迟时长</param>
        /// <param name="tInfo">信息</param>
        /// <param name="callback">回调</param>
        /// <returns></returns>
        public Coroutine DelayAction<T>(float delayTime, T tInfo, UnityAction<T> callback)
        {
            return StartCoroutine(IEDelayAction(Mathf.Clamp(delayTime, 0, delayTime), tInfo, callback));
        }
        /// <summary>
        /// 延迟执行(含参数传递) 
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <typeparam name="K">参数类型</typeparam>
        /// <param name="delayTime">延迟时长</param>
        /// <param name="tInfo">信息</param>
        /// <param name="kInfo">信息</param>
        /// <param name="callback">回调</param>
        /// <returns></returns>
        public Coroutine DelayAction<T, K>(float delayTime, T tInfo, K kInfo, UnityAction<T, K> callback)
        {
            return StartCoroutine(IEDelayAction(Mathf.Clamp(delayTime, 0, delayTime), tInfo, kInfo, callback));
        }
        /// <summary>
        /// 协程延迟(无参数传递)
        /// </summary>
        /// <param name="delayTime">延迟时长</param>
        /// <param name="callback">回调</param>
        /// <returns></returns>
        private IEnumerator IEDelayAction(float delayTime, UnityAction callback)
        {
            yield return new WaitForSeconds(delayTime);
            callback?.Invoke();
        }
        /// <summary>
        /// 协程延迟(含参数传递)
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="delayTime">延迟时长</param>
        /// <param name="tInfo">信息</param>
        /// <param name="callback">回调</param>
        /// <returns></returns>
        private IEnumerator IEDelayAction<T>(float delayTime, T tInfo, UnityAction<T> callback)
        {
            yield return new WaitForSeconds(delayTime);
            callback?.Invoke(tInfo);
        }
        /// <summary>
        /// 协程延迟(含参数传递)
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <typeparam name="K">参数类型</typeparam>
        /// <param name="delayTime">延迟时长</param>
        /// <param name="tInfo">信息</param>
        /// <param name="kInfo">信息</param>
        /// <param name="callback">回调</param>
        /// <returns></returns>
        private IEnumerator IEDelayAction<T, K>(float delayTime, T tInfo, K kInfo, UnityAction<T, K> callback)
        {
            yield return new WaitForSeconds(delayTime);
            callback?.Invoke(tInfo, kInfo);
        }       
        /// <summary>
        /// 移除所有Update委托
        /// </summary>
        public void ClearUpdate()
        {
            m_Callback = null;
        }
        /// <summary>
        /// 移除所有FixUpdate委托
        /// </summary>
        public void ClearFixedUpdate()
        {
            m_FixCallback = null;
        }
        /// <summary>
        /// 移除所有LateUpdate委托
        /// </summary>
        public void ClearLateUpdate()
        {
            m_LateCallback = null;
        }
        #endregion
    }
}
