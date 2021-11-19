using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Farme.UI
{
    /// <summary>
    /// 面板状态
    /// </summary>
    public enum EnumPanelState
    {
        /// <summary>
        /// 无
        /// </summary>
        None,
        /// <summary>
        /// 显示
        /// </summary>
        Show,
        /// <summary>
        /// 隐藏
        /// </summary>
        Hide,
        /// <summary>
        /// 销毁
        /// </summary>
        Destroy
    }
    /// <summary>
    /// 面板基类
    /// </summary>
    public abstract class BasePanel : BaseMono
    {

        protected override void Awake()
        {
            base.Awake();
        }
        #region 字段     
        /// <summary>
        /// 依赖的窗口
        /// </summary>
        public StandardWindow relyWindow = null;
        #endregion
        #region 属性
        /// <summary>
        /// 矩形转换
        /// </summary>
        public RectTransform RectTransform
        {
            get
            {
                return transform as RectTransform;
            }
        }
        #endregion
        #region 方法                   
        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="state">状态</param>
        /// <param name="callback">回调</param>
        public void SetState(EnumPanelState state,UnityAction callback=null)
        {
            switch(state)
            {
                case EnumPanelState.Show:
                    {
                        gameObject.SetActive(true);
                        callback?.Invoke();
                        break;
                    }
                case EnumPanelState.Hide:
                    {
                        callback?.Invoke();
                        gameObject.SetActive(false);
                        break;
                    }
                case EnumPanelState.Destroy:
                    {                 
                        if(relyWindow!=null)
                        {
                            relyWindow.RemovePanel(gameObject.name);
                        }
                        callback?.Invoke();
                        Destroy(gameObject);
                        break;
                    }
                case EnumPanelState.None:
                    {
                        callback?.Invoke();
                        break;
                    }
            }
        }          
        #endregion
    }
    /// <summary>
    /// 提供无功能的面板
    /// </summary>
    public class EmptyPanel : BasePanel
    {
        
    }
}
