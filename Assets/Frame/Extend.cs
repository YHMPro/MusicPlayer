using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;
using Farme.UI;
namespace Farme.Extend
{
    /// <summary>
    /// 扩展
    /// </summary>
    public static class Extend
    {
        #region 为GameObject类扩展相关功能
        /// <summary>
        /// 回收
        /// </summary>
        /// <param name="target"></param>
        /// <param name="reuseGroup">复用组</param>
        /// <param name="delay">延迟时长</param>
        public static void Recycle(this GameObject target,string reuseGroup, float delay = 0)
        {
            MonoSingletonFactory<ShareMono>.GetSingleton().DelayAction(delay, () =>
             {
                 if(target!=null)
                 {
                     GoReusePool.Put(reuseGroup, target);
                 }
             });         
        }
        /// <summary>
        /// 检测组件是否存在，有则直接返该类型组件，没有则添加该类型组件在返回
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="target">发起对象</param>
        public static T InspectComponent<T>(this GameObject target) where T : Component
        {
            T t = target.GetComponent<T>();
            //判断是否已经含有T类型组件
            if (t == null)
                return target.AddComponent<T>();
            else
                return t;
        }
        #endregion
        #region  为String类扩展相关功能
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
        #region 为UIBehaviour类扩展相关功能
        /// <summary>
        /// UI事件注册
        /// </summary>
        /// <param name="taeget"></param>
        /// <param name="eTType">消息类型</param>
        /// <param name="callBack">添加的回调</param>
        public static void UIEventRegistered(this UIBehaviour target, EventTriggerType eTType, UnityAction<BaseEventData> callBack)
        {
            WindowRoot.UIEventRegistered(target, eTType, callBack);
        }
        /// <summary>
        /// UI事件移除      
        /// </summary>
        /// <param name="taeget"></param>
        /// <param name="eTType">消息类型</param>
        /// <param name="callBack">添加的回调</param>
        public static void UIEventRemove(this UIBehaviour target, EventTriggerType eTType, UnityAction<BaseEventData> callBack)
        {
            WindowRoot.UIEventRemove(target, eTType, callBack);
        }
        #endregion
        #region 为flaot类扩展相关功能
        /// <summary>
        /// 取整(四舍五入形式)
        /// </summary>
        /// <param name="f"></param>
        /// <param name="Index">对小数点后第几位取整 默认为0</param>
        /// <returns></returns>
        public static int Round(this float f, int Index = 0)
        {
            return (int)Math.Round(f, Index);
        }
        #endregion
        #region 为Animator扩展相关功能
        /// <summary>
        /// 获取动画器中动画剪辑的时长
        /// </summary>
        /// <param name="target">发起对象</param>
        /// <param name="clipName">剪辑名称</param>
        /// <returns></returns>
        public static float AnimatorClipTimeLength(this Animator target, string clipName)
        {
            //获取所有剪辑
            var clips = target.runtimeAnimatorController.animationClips;
            //遍历所有剪辑
            foreach (var clip in clips)
            {
                //找到匹配的剪辑
                if (clip.name == clipName)
                {
                    //返回剪辑时长
                    return clip.length;
                }
            }
            //没找到返回-1作为标记
            return -1;
        }
        #endregion
    }
}
