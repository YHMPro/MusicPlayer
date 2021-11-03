using UnityEngine;
namespace Farme
{
    public class BaseWorldWindowMgr : BaseWindowMgr,ILocationSync
    {
        protected BaseWorldWindowMgr() { }

        protected override void Awake()
        {
            base.Awake();
            _interfaceGroup = GetType().Name;
            if (_bindCamera == null)
            {
                _bindCamera = RootWorldWindowGo.GetComponent<Camera>();
            }
            GetComponent<Canvas>().worldCamera = _bindCamera;
            InterfaceMgr.AddInterface(_interfaceGroup, this);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            InterfaceMgr.RemoveInterface(_interfaceGroup, this);
        }
        #region 字段
        /// <summary>
        /// 接口托管组
        /// </summary>
        protected string _interfaceGroup = "";
        /// <summary>
        /// 绑定的相机
        /// </summary>
        protected static Camera _bindCamera = null;
        /// <summary>
        /// 根节点屏幕窗口转换
        /// </summary>
        protected static GameObject _rootWorldWindowGo = null;
        #endregion
        #region 属性
        /// <summary>
        /// 接口托管组
        /// </summary>
        public string InterfaceGroup
        {
            get
            {
                if(_interfaceGroup==null || _interfaceGroup=="")
                {
                    _interfaceGroup = GetType().Name;
                }
                return _interfaceGroup;
            }
        }
        /// <summary>
        /// 世界窗口根节点(可移动)
        /// </summary>
        public static GameObject RootWorldWindowGo
        {
            get
            {
                if(_rootWorldWindowGo==null)
                {
                    if (!GoLoad.Take("YHMFarmeLockFile/RootWorldWindow", out _rootWorldWindowGo))
                    {
                        Debug.Log("错误!");
                    }
                }
                return _rootWorldWindowGo;
            }
        }
        /// <summary>
        /// 绑定的相机
        /// </summary>
        public static Camera BindCamera
        {
            get 
            { 
                if(_bindCamera == null)
                {
                    _bindCamera = RootWorldWindowGo.GetComponent<Camera>();
                }
                return _bindCamera; 
            } 
        }
        #endregion
        #region 方法
        /// <summary>
        /// 创建单例窗口
        /// </summary>
        /// <typeparam name="T">窗口类型</typeparam>
        /// <param name="windowURL">窗口路径</param>
        /// <param name="result">结果</param>
        /// <returns></returns>
        public bool CreateSingletonWindow<T>(string windowURL, out T result) where T : BaseWorldWindow
        {         
            result = null;
            if (MonoSingletonFactory<T>.SingletonExist)
            {
                MonoSingletonFactory<T>.GetSingleton(out result);
            }
            else
            {
                if (GoLoad.Take(windowURL, out GameObject go,transform))
                {
                    MonoSingletonFactory<T>.GetSingleton(out result, go);
                }
            }
            if (result == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 世界全局坐标转屏幕全局坐标
        /// 基于屏幕中心点(0,0)
        /// </summary>
        /// <param name="v">转换坐标</param>
        public static void WorldGlobalToScreenGlobalPositionBaseScreenCenter(Vector3 v, out Vector2 result)
        {
            Vector3 resultV3 = BindCamera.WorldToScreenPoint(v);
            resultV3.x -= BindCamera.pixelWidth / 2;
            resultV3.y -= BindCamera.pixelHeight / 2;
            resultV3.z = 0;
            result = resultV3;
        }
        /// <summary>
        /// 位置同步接口实现
        /// </summary>
        /// <param name="targetSync">同步目标</param>
        public virtual void LocationSync(Transform targetSync)
        {
            if(targetSync==null|| _bindCamera!=null)
            {
                return;
            }
            _bindCamera.transform.position = targetSync.position;
            _bindCamera.transform.rotation = targetSync.rotation;
        }
        #endregion
    }
}
