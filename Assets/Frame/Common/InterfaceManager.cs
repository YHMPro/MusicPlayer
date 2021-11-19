using System.Collections.Generic;
namespace Farme
{  
    /// <summary>
    /// 接口管理器
    /// </summary>
    public class InterfaceManager
    {
        protected InterfaceManager() { }
        #region 字段
        /// <summary>
        /// 接口容器  key:接口分组   value:接口
        /// </summary>
        private static Dictionary<string, List<IInterfaceBase>> m_InterfaceDic;
        #endregion
        #region 方法
        /// <summary>
        /// 添加接口托管
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="interfaceGroup">接口分组</param>
        /// <param name="i">接口</param>
        public static void AddInterface<T>(string interfaceGroup,T i)where T: IInterfaceBase
        {
            if(m_InterfaceDic == null)
            {
                m_InterfaceDic = new Dictionary<string, List<IInterfaceBase>>();
            }
            if(m_InterfaceDic.TryGetValue(interfaceGroup,out List<IInterfaceBase> _interfaceLi))
            {
                _interfaceLi.Add(i);
            }
            else
            {
                m_InterfaceDic.Add(interfaceGroup, new List<IInterfaceBase>() { i });
            }
        }
        /// <summary>
        /// 移除接口托管
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="interfaceGroup">接口分组</param>
        /// <param name="i">接口</param>
        public static void RemoveInterface<T>(string interfaceGroup,T i)where T:IInterfaceBase
        {
            if (m_InterfaceDic == null)
            {
                m_InterfaceDic = new Dictionary<string, List<IInterfaceBase>>();
            }
            if (m_InterfaceDic.TryGetValue(interfaceGroup, out List<IInterfaceBase> _interfaceLi))
            {
                if(i!=null)
                {
                    _interfaceLi.Remove(i);
                }
            }          
        }
        /// <summary>
        /// 获取接口列表
        /// </summary>
        /// <typeparam name="T">接口类型</typeparam>
        /// <param name="interfaceGroup">接口分组</param>
        /// <param name="iLi">接口列表</param>
        /// <returns></returns>
        public static bool GetInterfaceLi<T>(string interfaceGroup,out List<T> iLi) where T: IInterfaceBase
        {
            iLi = null;
            if (m_InterfaceDic == null)
            {
                m_InterfaceDic = new Dictionary<string, List<IInterfaceBase>>();
            }
            if (m_InterfaceDic.TryGetValue(interfaceGroup, out List<IInterfaceBase> _interfaceLi))
            {
                iLi = new List<T>();
                foreach (T i in _interfaceLi)
                {
                    if (i != null)
                    {
                        iLi.Add(i);
                    }
                }
            }
            if(iLi==null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
    }
}
