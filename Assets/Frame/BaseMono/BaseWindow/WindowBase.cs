using UnityEngine;
namespace Farme
{   
    /// <summary>
    /// 窗口基类
    /// </summary>
    public abstract class BaseWindow : BaseMono
    {
        protected BaseWindow() { }
        protected override void Awake()
        {
            base.Awake();
            _ = MRectTrans;
            _ = RelyCanvas;
        }
        #region 字段
        /// <summary>
        /// 矩形转换
        /// </summary>
        protected RectTransform _mRectTrans = null;
        /// <summary>
        /// 依赖的画布
        /// </summary>
        protected Canvas _relyCanvas = null;
        #endregion
        #region 属性
        /// <summary>
        /// 窗口依赖的画布
        /// </summary>
        public Canvas RelyCanvas
        {
            get
            {
                if(_relyCanvas)
                {
                    _relyCanvas = transform.parent.parent.GetComponent<Canvas>();
                }
                return _relyCanvas;
            }
        }
        /// <summary>
        /// 矩形转换
        /// </summary>
        public RectTransform MRectTrans 
        {
            get
            {
                if (_mRectTrans == null)
                {
                    _mRectTrans = transform as RectTransform;
                }
                return _mRectTrans;
            }
        }
        #endregion
        #region 方法            
        /// <summary>
        /// 窗口风格
        /// </summary>
        /// <param name="styleIndex">风格索引</param>
        public abstract void WindowStyle(int styleIndex);      
        #endregion
    }
}
