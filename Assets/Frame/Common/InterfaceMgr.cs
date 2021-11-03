namespace Farme
{
    using System.Collections.Generic;
    /// <summary>
    /// 接口托管
    /// </summary>
    public class InterfaceMgr
    {
        protected InterfaceMgr() { }
        #region 字段
        /// <summary>
        /// 接口容器  key:接口分组   value:接口
        /// </summary>
        private static Dictionary<string, List<IInterfaceBase>> _interfaceDic;
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
            if(_interfaceDic==null)
            {
                _interfaceDic = new Dictionary<string, List<IInterfaceBase>>();
            }
            if(_interfaceDic.TryGetValue(interfaceGroup,out List<IInterfaceBase> _interfaceLi))
            {
                _interfaceLi.Add(i);
            }
            else
            {
                _interfaceDic.Add(interfaceGroup, new List<IInterfaceBase>() { i });
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
            if (_interfaceDic == null)
            {
                _interfaceDic = new Dictionary<string, List<IInterfaceBase>>();
            }
            if (_interfaceDic.TryGetValue(interfaceGroup, out List<IInterfaceBase> _interfaceLi))
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
            if (_interfaceDic == null)
            {
                _interfaceDic = new Dictionary<string, List<IInterfaceBase>>();
            }
            if (_interfaceDic.TryGetValue(interfaceGroup, out List<IInterfaceBase> _interfaceLi))
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
