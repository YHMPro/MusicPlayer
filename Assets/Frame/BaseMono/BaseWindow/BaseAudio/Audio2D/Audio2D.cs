namespace Farme
{

    /// <summary>
    /// Audio2D
    /// </summary>
    public class Audio2D : BaseAudio
    {
        protected Audio2D() { }
        #region 生命周期函数
       
        #endregion
        #region 方法       
        protected override BaseAudioMgr GetRelyOnAudioMgr()
        {
            return MonoSingletonFactory<Audio2DMgr>.GetSingleton();
        }

        #endregion
    }
}
