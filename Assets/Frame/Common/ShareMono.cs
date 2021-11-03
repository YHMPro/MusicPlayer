using System.Collections;
using UnityEngine;
using UnityEngine.Events;
namespace Farme
{ 
    /// <summary>
    /// 共享Mono 用于非MonoBehaviour派生类提供MonoBehaviour功能
    /// </summary>
    public class ShareMono : MonoBehaviour
    {
        protected ShareMono() { }
        #region 事件
        /// <summary>
        /// 回调
        /// </summary>
        private event UnityAction _callBack;
        /// <summary>
        /// 回调
        /// </summary>
        private event UnityAction _lateCallBack;
        /// <summary>
        /// 回调
        /// </summary>
        private event UnityAction _fixCallBack;
        #endregion
        #region 方法      
        /// <summary>
        /// 持续更新
        /// </summary>
        private void Update()
        {
            _callBack?.Invoke();
        }
        /// <summary>
        /// 迟与Update更新
        /// </summary>
        private void LateUpdate()
        {
            _lateCallBack?.Invoke();
        }
        /// <summary>
        /// 固定更新
        /// </summary>
        private void FixedUpdate()
        {
            _fixCallBack?.Invoke();
        }
        /// <summary>
        /// 添加Update行为
        /// </summary>
        /// <param name="callBack">回调</param>
        public void AddUpdateUAction(UnityAction callBack)
        {
            this._callBack += callBack;
        }
        /// <summary>
        /// 移除Update行为
        /// </summary>
        /// <param name="callBack">回调</param>
        public void RemoveUpdateUAction(UnityAction callBack)
        {
            this._callBack -= callBack;
        }
        /// <summary>
        /// 移除LateUpdate行为
        /// </summary>
        /// <param name="callBack">回调</param>
        public void RemoveLateUpdateUAction(UnityAction callBack)
        {
            this._lateCallBack -= callBack;
        }
        /// <summary>
        /// 添加LateUpdate行为
        /// </summary>
        /// <param name="callBack">回调</param>
        public void AddLateUpdateUAction(UnityAction callBack)
        {
            this._lateCallBack += callBack;
        }
        /// <summary>
        /// 移除FixUpdate行为
        /// </summary>
        /// <param name="callBack">回调</param>
        public void RemoveFixUpdateUAction(UnityAction callBack)
        {
            this._fixCallBack -= callBack;
        }
        /// <summary>
        /// 添加FixUpdate行为
        /// </summary>
        /// <param name="callBack">回调</param>
        public void AddFixUpdateUAction(UnityAction callBack)
        {
            this._fixCallBack += callBack;
        }
        /// <summary>
        /// 延迟执行(无参数传递) 
        /// </summary>
        /// <param name="delayTime">延迟时长</param>
        /// <param name="callBack">回调</param>
        /// <returns></returns>
        public Coroutine DelayUAction(float delayTime, UnityAction callBack)
        {
            return StartCoroutine(IEDelayUAction(delayTime, callBack));
        }
        /// <summary>
        /// 延迟执行(含参数传递) 
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="delayTime">延迟时长</param>
        /// <param name="tInfo">信息</param>
        /// <param name="callBack">回调</param>
        /// <returns></returns>
        public Coroutine DelayUAction<T>(float delayTime, T tInfo, UnityAction<T> callBack)
        {
            return StartCoroutine(IEDelayUAction(delayTime, tInfo, callBack));
        }
        /// <summary>
        /// 延迟执行(含参数传递) 
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <typeparam name="K">参数类型</typeparam>
        /// <param name="delayTime">延迟时长</param>
        /// <param name="tInfo">信息</param>
        /// <param name="kInfo">信息</param>
        /// <param name="callBack">回调</param>
        /// <returns></returns>
        public Coroutine DelayUAction<T, K>(float delayTime, T tInfo, K kInfo, UnityAction<T, K> callBack)
        {
            return StartCoroutine(IEDelayUAction(delayTime, tInfo, kInfo, callBack));
        }
        /// <summary>
        /// 协程延迟(无参数传递)
        /// </summary>
        /// <param name="delayTime">延迟时长</param>
        /// <param name="callBack">回调</param>
        /// <returns></returns>
        private IEnumerator IEDelayUAction(float delayTime, UnityAction callBack)
        {
            yield return new WaitForSeconds(delayTime);
            callBack?.Invoke();
        }
        /// <summary>
        /// 协程延迟(含参数传递)
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <param name="delayTime">延迟时长</param>
        /// <param name="tInfo">信息</param>
        /// <param name="callBack">回调</param>
        /// <returns></returns>
        private IEnumerator IEDelayUAction<T>(float delayTime, T tInfo, UnityAction<T> callBack)
        {
            yield return new WaitForSeconds(delayTime);
            callBack?.Invoke(tInfo);
        }
        /// <summary>
        /// 协程延迟(含参数传递)
        /// </summary>
        /// <typeparam name="T">参数类型</typeparam>
        /// <typeparam name="K">参数类型</typeparam>
        /// <param name="delayTime">延迟时长</param>
        /// <param name="tInfo">信息</param>
        /// <param name="kInfo">信息</param>
        /// <param name="callBack">回调</param>
        /// <returns></returns>
        private IEnumerator IEDelayUAction<T, K>(float delayTime, T tInfo, K kInfo, UnityAction<T, K> callBack)
        {
            yield return new WaitForSeconds(delayTime);
            callBack?.Invoke(tInfo, kInfo);
        }
        /// <summary>
        /// 移除所有Update委托
        /// </summary>
        public void ClearUA()
        {
            _callBack = null;
        }
        /// <summary>
        /// 移除所有FixUpdate委托
        /// </summary>
        public void ClearFixUA()
        {
            _fixCallBack = null;
        }
        /// <summary>
        /// 移除所有LateUpdate委托
        /// </summary>
        public void ClearLateUA()
        {
            _lateCallBack = null;
        }
        #endregion
    }
}
