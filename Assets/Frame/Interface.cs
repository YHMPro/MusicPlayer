using UnityEngine;
using UnityEngine.Audio;
namespace Farme
{
    /// <summary>
    /// 接口基类
    /// </summary>
    public interface IInterfaceBase { }

    #region 数据接口
    /// <summary>
    /// 数据接口
    /// </summary>
    public interface IData:IInterfaceBase
    {
        /// <summary>
        /// 数据初始化
        /// </summary>
        void DataInit();
    }
    #endregion

    #region 特殊接口
    /// <summary>
    /// 位置同步接口
    /// </summary>
    public interface ILocationSync: IInterfaceBase
    {
        /// <summary>
        /// 位置同步
        /// </summary>
        /// <param name="targetSync">同步目标</param>
        void LocationSync(Transform targetSync);
    }
    #endregion

    #region 指针事件接口  ( 独立约束 -> 具体过程 )
    /// <summary>
    /// 事件接口
    /// </summary>
    public interface IEvent : IInterfaceBase
    {

    }
    /// <summary>
    /// 指针事件接口
    /// </summary>
    public interface IPointerEvent : IEvent
    {

    }
    /// <summary>
    /// 指针事件屏蔽接口
    /// </summary>
    public interface IPointerEventShield : IPointerEvent
    {
        /// <summary>
        /// 指针事件屏蔽
        /// </summary>
        /// <typeparam name="T">依据类型</typeparam>
        /// <param name="gist">依据</param>
        void PointerEventShield<T>(T gist);
    }
    /// <summary>
    /// 指针事件重置接口
    /// </summary>
    public interface IPointerEventReset : IPointerEvent
    {
        /// <summary>
        /// 指针事件重置
        /// </summary>
        /// <typeparam name="T">依据类型</typeparam>
        /// <param name="gist">依据</param>
        void PointerEventReset<T>(T gist);
    }
    #endregion

    #region Audio接口
    public interface IAudio: IInterfaceBase
    {

    }
    /// <summary>
    /// 音效控制接口
    /// </summary>
    public interface IAudioControl : IAudio
    {
        /// <summary>
        /// 播放
        /// </summary>
        /// <returns>是否成功播放</returns>
        bool Play();
        /// <summary>
        /// 暂停
        /// </summary>
        /// <returns>是否暂停成功</returns>
        bool Pause();
        /// <summary>
        /// 重新播放
        /// </summary>
        /// <returns>是否重播成功</returns>
        bool RePlay();
        /// <summary>
        /// 停止
        /// </summary>
        /// <returns>是否停止成功</returns>
        bool Stop();         
        /// <summary>
        /// 设置循环
        /// </summary>
        /// <param name="loop">循环</param>
        /// <returns>是否设置成功</returns>
        bool SetLoop(bool loop);
        /// <summary>
        /// 设置音效混合器组
        /// </summary>
        /// <param name="group">组</param>
        /// <returns>是否设置成功</returns>
        bool SetAudioMixerGroup(AudioMixerGroup group);
        /// <summary>
        /// 设置已播放的时长
        /// </summary>
        /// <param name="playedTime">已播放时长</param>
        /// <returns>是否设置成功</returns>
        bool SetPlayedTime(float playedTime);
        /// <summary>
        /// 设置音效剪辑
        /// </summary>
        /// <param name="clip">剪辑</param>
        /// <param name="inheritTime">继承时间</param>
        /// <returns>是否设置成功</returns>
        bool SetAudioClip(AudioClip clip,bool inheritTime=false);
        /// <summary>
        /// 获取音效混合器组
        /// </summary>
        /// <param name="group">组</param>
        /// <returns>是否获取成功</returns>
        bool GetAudioMixerGroup(out AudioMixerGroup group);
        /// <summary>
        /// 获取已播放的时长
        /// </summary>
        float GetPlayedTime();
        /// <summary>
        /// 获取音效剪辑
        /// </summary>
        AudioClip GetAudioClip();
        /// <summary>
        /// 是否暂停
        /// </summary>
        /// <returns></returns>
        bool IsPause();
        /// <summary>
        /// 是否停止
        /// </summary>
        /// <returns></returns>
        bool IsStop();
        /// <summary>
        /// 是否播放
        /// </summary>
        /// <returns></returns>
        bool IsPlay();
        /// <summary>
        /// 是否循环
        /// </summary>
        /// <returns></returns>
        bool IsLoop();
    }
    /// <summary>
    /// 音效信息接口
    /// </summary>
    public interface IAudioInfo : IAudio
    {
        
    }
    #endregion

    #region UGUI扩展接口
    #region CheckBox指针事件接口  ( 集中约束  ->  具体 )
    /// <summary>
    /// Box指针事件接口
    /// </summary>
    public interface ICheckBoxPointerEvent: IPointerEventShield, IPointerEventReset
    {

    }
    #endregion
    #endregion


}
