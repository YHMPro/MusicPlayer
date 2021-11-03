using UnityEngine;
namespace Farme
{
    /// <summary>
    /// 屏幕窗口基类
    /// </summary>
    public abstract class BaseScreenWindowMgr : BaseWindowMgr
    {
        protected BaseScreenWindowMgr() { }
        /// <summary>
        /// 窗口层级
        /// </summary>
        public enum WindowLayer
        {
            /// <summary>
            /// 底层
            /// </summary>
            BOTTOM,
            /// <summary>
            /// 中层
            /// </summary>
            MIDDLE,
            /// <summary>
            /// 顶层
            /// </summary>
            TOP,
            /// <summary>
            /// 系统层
            /// </summary>
            SYSTEM
        }
        protected override void Awake()
        {
            base.Awake();
            if (_bindCamera == null)
            {
                _bindCamera = RootScreenWindowGo.GetComponent<Camera>();
            }
            GetComponent<Canvas>().worldCamera = _bindCamera;         
            if (!GetComponent("Bottom", out _tranBottom))
            {
                Debug.Log("错误!");
            }
            if (!GetComponent("Middel", out _tranMiddle))
            {
                Debug.Log("错误!");
            }
            if (!GetComponent("Top", out _tranTop))
            {
                Debug.Log("错误!");
            }
            if (!GetComponent("System", out _tranSystem))
            {
                Debug.Log("错误!");
            }
        }
        #region 字段
        /// <summary>
        /// 屏幕窗口根节点
        /// </summary>
        protected static GameObject _rootScreenWindowGo = null;
        /// <summary>
        /// 画布绑定的相机
        /// </summary>
        protected static Camera _bindCamera = null;
        /// <summary>
        /// 底层
        /// </summary>
        protected Transform _tranBottom = null;
        /// <summary>
        /// 中层
        /// </summary>
        protected Transform _tranMiddle = null;
        /// <summary>
        /// 顶层
        /// </summary>
        protected Transform _tranTop = null;
        /// <summary>
        /// 系统层(最高层)
        /// </summary>
        protected Transform _tranSystem = null;
        #endregion
        #region 属性
        /// <summary>
        /// 屏幕窗口根节点
        /// </summary>
        public static GameObject RootScreenWindowGo
        {
            get
            {
                if (_rootScreenWindowGo == null)
                {
                    if(!GoLoad.Take("YHMFarmeLockFile/RootScreenWindow",out _rootScreenWindowGo))
                    {
                        Debug.Log("错误!");
                    }
                }
                return _rootScreenWindowGo;
            }
        }
        /// <summary>
        /// 绑定的相机
        /// </summary>
        public static Camera BindCamera
        {
            get
            {
                if(_bindCamera==null)
                {
                    _bindCamera = RootScreenWindowGo.GetComponent<Camera>();
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
        /// <param name="windowLayer">窗口层级</param>
        /// <returns></returns>
        public bool CreateSingletonWindow<T>(string windowURL, out T result,WindowLayer windowLayer=WindowLayer.BOTTOM) where T:BaseScreenWindow
        {
            result = null;
            if (MonoSingletonFactory<T>.SingletonExist)
            {
                 MonoSingletonFactory<T>.GetSingleton(out result);
            }
            else
            {
                if(GoLoad.Take(windowURL,out GameObject go,GetWindowParent(windowLayer)))
                {
                    MonoSingletonFactory<T>.GetSingleton(out result,go);
                }
            }
            if(result == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 获取窗口父级(由层级决定)
        /// </summary>
        /// <param name="windowLayer">窗口层级</param>
        /// <returns></returns>   
        private Transform GetWindowParent(WindowLayer windowLayer)
        {
            Transform screenWindowParent;
            //设置父级
            switch (windowLayer)
            {
                case WindowLayer.BOTTOM: screenWindowParent = _tranBottom; break;

                case WindowLayer.MIDDLE: screenWindowParent = _tranMiddle; break;

                case WindowLayer.TOP: screenWindowParent = _tranTop; break;

                case WindowLayer.SYSTEM: screenWindowParent = _tranSystem; break;

                default: screenWindowParent=transform; break;
            }        
            return screenWindowParent;
        }
        /// <summary>
        /// 世界全局坐标转屏幕全局坐标
        /// 基于屏幕中心点(0,0)
        /// </summary>
        /// <param name="v">转换坐标</param>
        public static void WorldGlobalToScreenGlobalPositionBaseScreenCenter(Vector3 v,out Vector2 result)
        {
            Vector3 resultV3 = BindCamera.WorldToScreenPoint(v);
            resultV3.x -= BindCamera.pixelWidth / 2;
            resultV3.y -= BindCamera.pixelHeight / 2;
            resultV3.z = 0;
            result = resultV3;
        }
        #endregion
    }
}
