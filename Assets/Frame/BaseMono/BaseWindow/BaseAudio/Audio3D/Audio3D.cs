using UnityEngine;
namespace Farme
{
    /// <summary>
    /// Audio3D
    /// </summary>
    public class Audio3D : BaseAudio
    {
        protected Audio3D() { }

        #region 生命周期函数
        protected override void Awake()
        {
            base.Awake();
            m_As.spatialBlend = 1f;
        }
        #endregion
        #region 字段
        /// <summary>
        /// 空间混合度  2D-3D音效的转换
        /// </summary>
        private float m_SpatialBlend =1f;
        #endregion
        #region 属性
        /// <summary>
        /// 空间回响  0(2D) ->  1(3D)
        /// </summary>
        public float SpatialBlend
        {
            set
            {
                if (m_As != null)
                {
                    m_SpatialBlend = Mathf.Clamp(value, 0, 1);
                    m_As.spatialBlend = m_SpatialBlend;
                }
            }
            get
            {
                return m_SpatialBlend;
            }
        }
        #endregion
        #region 方法
        protected override BaseAudioMgr GetRelyOnAudioMgr()
        {
            Audio3DMgr audio3DMgr = MonoSingletonFactory<Audio3DMgr>.GetSingleton();
            transform.SetParent(audio3DMgr.transform);
            return audio3DMgr;
        }
        #endregion
    }
}
