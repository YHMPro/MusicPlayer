using System.Collections.Generic;
using UnityEngine.Events;
namespace Farme
{   
    /// <summary>
    /// 消息托管
    /// </summary>
    public class MesgMgr
    {
        protected MesgMgr() { }
        #region 字段      
        private static Dictionary<string, IMesg> m_MesgDic = null;
        #endregion

        #region 属性
        /// <summary>
        /// 消息容器
        /// </summary>
        private static Dictionary<string,IMesg> MesgDic
        {
            get
            {
                if(m_MesgDic==null)
                {
                    m_MesgDic = new Dictionary<string, IMesg>();
                }
                return m_MesgDic;
            }
        }
        #endregion
        #region 方法
        /// <summary>
        /// 消息监听
        /// </summary>
        /// <param name="mesgName">消息名</param>
        /// <param name="mesgAction">消息</param>
        public static void MesgListen(string mesgName, UnityAction mesgAction)
        {          
            if (MesgDic.TryGetValue(mesgName, out var mA))
            {
                (mA as Mesg).mesgAction += mesgAction;
            }
            else
            {
                MesgDic.Add(mesgName, new Mesg(mesgAction));
            }
        }
        /// <summary>
        /// 消息派送
        /// </summary>
        /// <typeparam name="T">传递信息类型</typeparam>
        /// <param name="mesgName">消息名</param>
        /// <param name="mesgAction">消息</param>
        public static void MesgListen<T>(string mesgName, UnityAction<T> mesgAction)
        {       
            if (MesgDic.TryGetValue(mesgName, out var mA))
            {
                (mA as Mesg<T>).mesgAction += mesgAction;
            }
            else
            {
                MesgDic.Add(mesgName, new Mesg<T>(mesgAction));
            }
        }
        /// <summary>
        /// 消息触发
        /// </summary>
        /// <param name="mesgName">消息名</param>
        public static void MesgTirgger(string mesgName)
        {          
            if (MesgDic.TryGetValue(mesgName, out var mA))
            {
                (mA as Mesg).mesgAction?.Invoke();
            }
        }
        /// <summary>
        /// 消息触发
        /// </summary>
        /// <typeparam name="T">传递信息类型</typeparam>
        /// <param name="mesgName">消息名称</param>
        /// <param name="mesgInfo">消息信息</param>
        public static void MesgTirgger<T>(string mesgName, T mesgInfo)
        {        
            if (MesgDic.TryGetValue(mesgName, out var mA))
            {
                (mA as Mesg<T>).mesgAction?.Invoke(mesgInfo);
            }
        }
        /// <summary>
        /// 消息断开监听
        /// </summary>
        /// <param name="mesgName">消息名</param>
        /// <param name="mesgAction">消息</param>
        public static void MesgBreakListen(string mesgName, UnityAction mesgAction)
        {
            if (MesgDic.TryGetValue(mesgName, out var mA))
            {
                (mA as Mesg).mesgAction -= mesgAction;
            }
        }
        /// <summary>
        /// 消息断开监听
        /// </summary>
        /// <typeparam name="T">传递信息类型</typeparam>
        /// <param name="mesgName">消息名</param>
        /// <param name="mesgAction">消息</param>
        public static void MesgBreakListen<T>(string mesgName, UnityAction<T> mesgAction)
        {
            if (MesgDic.TryGetValue(mesgName, out var mA))
            {
                (mA as Mesg<T>).mesgAction -= mesgAction;
            }
        }
        #endregion
        /// <summary>
        /// 创建一个接口
        /// 基于里氏转换原则来实现IMesg接口对自身派生类及自身派生类的重载类的转变
        /// </summary>
        protected interface IMesg { }
        /// <summary>
        /// Mesg类
        /// </summary>
        protected class Mesg : IMesg
        {
            public UnityAction mesgAction;
            public Mesg(UnityAction mesgAction)
            {
                this.mesgAction += mesgAction;
            }        
        }
        /// <summary>
        /// Mesg泛型重载类
        /// </summary>
        /// <typeparam name="T">消息信息类型</typeparam>
        protected class Mesg<T> : IMesg
        {     
            public UnityAction<T> mesgAction;
            public Mesg(UnityAction<T> mesgAction)
            {
                this.mesgAction += mesgAction;
            }
   
        }


    }
}
