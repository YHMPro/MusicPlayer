using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
namespace Farme
{
    /// <summary>
    /// 扩展
    /// </summary>
    public static class Extend
    {
        #region GameObject类扩展
        /// <summary>
        /// 回收
        /// </summary>
        /// <param name="target"></param>
        /// <param name="reuseGroup">复用组</param>
        /// <param name="delay">延迟时长</param>
        public static void Recycle(this GameObject target,string reuseGroup, float delay = 0)
        {
            MonoSingletonFactory<ShareMono>.GetSingleton().DelayUAction(delay, () =>
             {
                 if(target!=null)
                 {
                     GoReusePool.Put(reuseGroup, target);
                 }
             });         
        }
        #endregion
        #region  String类扩展
        /// <summary>
        /// 指定字符截取字符串
        /// 截取目标字符串中指定字符Aimchar出现第(Max-Count)次之后的字符串  Count默认为1
        /// </summary>
        /// <param name="target">发起对象</param>
        /// <param name="aimChar">目标字符</param>
        /// <param name="count">次数</param>
        public static string AssignCharExtract(this string target, char aimChar, int count = 1)
        {
            return target.Substring(target.LastIndexOf(aimChar) + count);
        }

        #endregion
        #region UIBehaviour类扩展
        /// <summary>
        /// UI事件注册
        /// </summary>
        /// <param name="taeget"></param>
        /// <param name="eTType">消息类型</param>
        /// <param name="callBack">添加的回调</param>
        public static void UIEventRegistered(this UIBehaviour target, EventTriggerType eTType, UnityAction<BaseEventData> callBack)
        {
            BaseWindowMgr.UIEventRegistered(target, eTType, callBack);
        }
        /// <summary>
        /// UI事件移除      
        /// </summary>
        /// <param name="taeget"></param>
        /// <param name="eTType">消息类型</param>
        /// <param name="callBack">添加的回调</param>
        public static void UIEventRemove(this UIBehaviour target, EventTriggerType eTType, UnityAction<BaseEventData> callBack)
        {
            BaseWindowMgr.UIEventRemove(target, eTType, callBack);
        }
        #endregion
    }
}
