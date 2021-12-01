using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Farme;
using Farme.UI;
using Farme.Tool;
using MusicPlayer.Manager;
using UnityEngine.EventSystems;
using Farme.Extend;
namespace MusicPlayer.Panel
{
    /// <summary>
    /// 播放类型
    /// </summary>
    public enum ENUM_PlayType
    {
        SingleLoop,
        ListLoop,
        One
    }
    /// <summary>
    /// 音乐播放控制面板
    /// </summary>
    public class MusicPlayControllerPanel : BasePanel
    {
        private InputField m_MusicName = null;
        private Image m_MusicAlbum = null;
        private Button m_PlayType = null;
        private Button m_Last = null;
        private Button m_PlayOrPause = null;
        private Button m_Next = null;
        private Button m_Volume = null;
        private Button m_MusicList = null;
        private Button m_Set = null;
        private Button m_OpenOrClose = null;
        private Animator m_SelfAnim = null;
        protected override void Awake()
        {            
            base.Awake();
            RegisterComponentsTypes<Button>();//注册按钮类型
            RegisterComponentsTypes<Image>();//注册按钮类型
            RegisterComponentsTypes<InputField>();
            m_SelfAnim = GetComponent<Animator>();
            m_PlayType = GetComponent<Button>("PlayType");
            m_PlayType.onClick.AddListener(MusicPlayTypeEvent);
            m_Last = GetComponent<Button>("Last");
            m_Last.onClick.AddListener(MusicLastEvent);
            m_PlayOrPause = GetComponent<Button>("PlayOrPause");
            m_PlayOrPause.onClick.AddListener(MusicPlayOrPauseEvent);
            m_Next = GetComponent<Button>("Next");
            m_Next.onClick.AddListener(MusicNextEvent);
            m_Volume = GetComponent<Button>("Volume");
            m_Volume.onClick.AddListener(MusicVolumeEvent);
            m_MusicList = GetComponent<Button>("MusicList");
            m_MusicList.onClick.AddListener(OpenMusicListEvent);
            m_Set = GetComponent<Button>("Set");
            m_Set.onClick.AddListener(SetEvent);
            m_OpenOrClose = GetComponent<Button>("OpenOrClose");
            m_OpenOrClose.onClick.AddListener(OpenOrCloseSelf);
            m_MusicAlbum = GetComponent<Image>("MusicAlbum");
            m_MusicName = GetComponent<InputField>("MusicNameInput");
        }

        protected override void Start()
        {
            base.Start();

            #region 注册m_MusicName上的指针进入移出事件
            m_MusicName.UIEventRegistered(EventTriggerType.PointerEnter, MusicNamePointerEnter);
            m_MusicName.UIEventRegistered(EventTriggerType.PointerExit, MusicNamePointerExit);
            #endregion
            //m_AlbumRotateTween = m_MusicAlbum.transform.DORotate(new Vector3(0, 0, 360), 6f, RotateMode.WorldAxisAdd).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);          
        }

        protected override void OnDestroy()
        {
            m_MusicName.UIEventRemove(EventTriggerType.PointerEnter, MusicNamePointerEnter);
            m_MusicName.UIEventRemove(EventTriggerType.PointerExit, MusicNamePointerExit);
            base.OnDestroy();
        }
        private void MusicPlayTypeEvent()
        {
            Debuger.Log("触发更新播放方式事件");

        }

        private void MusicPlayOrPauseEvent()
        {
            Debuger.Log("触发播放或暂停事件");
            MusicController.MusicPlayOrPause();
        }

        public void RefreshPanel()
        {
            m_Next.interactable = MusicPlayerData.NowPlayMusicIsEnd;
            m_Last.interactable = MusicPlayerData.NowPlayMusicIsStart;
            m_MusicAlbum.transform.eulerAngles = Vector3.zero;
            m_MusicAlbum.sprite = null;
            MusicInfo musicInfo = MusicInfoManager.GetMusicInfo(MusicPlayerData.MusicFileNames[MusicPlayerData.NowPlayMusicIndex]);
            if(musicInfo!=null)
            {             
                m_MusicAlbum.sprite = musicInfo.Album;
                Debuger.Log("更新歌曲封面");
            }
            m_MusicName.text = MusicPlayerData.MusicFileNames[MusicPlayerData.NowPlayMusicIndex];//更新音乐文件名称
        }
        private void MusicNextEvent()
        {
            Debuger.Log("触发播放下一首事件");
            //更新数据
            ++MusicPlayerData.NowPlayMusicIndex;
            MusicController.MusicPlay();         
        }

        private void MusicLastEvent()
        {
            Debuger.Log("触发播放上一首事件");
            //更新数据
            --MusicPlayerData.NowPlayMusicIndex;
            MusicController.MusicPlay();          
        }

        private void MusicVolumeEvent()
        {
            Debuger.Log("触发音量调节事件");

        }
        private void SetEvent()
        {
            Debuger.Log("设置事件");
            if (MonoSingletonFactory<WindowRoot>.SingletonExist)
            {
                WindowRoot root = MonoSingletonFactory<WindowRoot>.GetSingleton();

                StandardWindow window = root.GetWindow("Controller");
                if (window != null)
                {
                    MusicPlaySetPanel panel = window.GetPanel<MusicPlaySetPanel>("SetPanel");
                    if (panel == null)
                    {
                        window.CreatePanel<MusicPlaySetPanel>(@"Panel\MusicPlaySetPanel", "SetPanel", EnumPanelLayer.SYSTEM, (instance) =>
                        {
                            Debuger.Log("创建设置面板成功");
                            instance.SetState(EnumPanelState.Show);
                        });
                        return;
                    }
                    panel.SetState(EnumPanelState.Show);
                }
            }
        }
        /// <summary>
        /// 音乐列表是否激活
        /// </summary>
        bool m_MusicListEnable = false;
        private void OpenMusicListEvent()
        {
            m_MusicListEnable = !m_MusicListEnable;
            Debuger.Log("打开音乐列表事件");
            if (MonoSingletonFactory<WindowRoot>.SingletonExist)
            {
                WindowRoot root = MonoSingletonFactory<WindowRoot>.GetSingleton();
                StandardWindow window = root.GetWindow("Controller");
                if (window != null)
                {
                    MusicListPanel panel = window.GetPanel<MusicListPanel>("MusicListPanel");
                    if (panel == null)
                    {
                        window.CreatePanel<MusicListPanel>(@"Panel\MusicListPanel", "MusicListPanel", EnumPanelLayer.BOTTOM, (instance) =>
                        {
                            Debuger.Log("创建音乐列表面板成功");
                            instance.SetState(EnumPanelState.Show);
                        });
                        return;
                    }
                    panel.SetState(m_MusicListEnable ? EnumPanelState.Show : EnumPanelState.Hide);
                }
            }

        }

        /// <summary>
        /// 音乐控制面板是否打开
        /// </summary>
        bool m_SelfEnable = false;
        private void OpenOrCloseSelf()
        {
            m_SelfEnable = !m_SelfEnable;
            if (m_SelfEnable)
            {
                m_SelfAnim.SetTrigger("IsOpen");
                //封面不为空的情况下
                MonoSingletonFactory<ShareMono>.GetSingleton().ApplyUpdateAction(EnumUpdateAction.Standard, OpenOrCloseUpdate);
            }
            else
            {
                m_SelfAnim.SetTrigger("IsClose");
                //封面不为空的情况下
                MonoSingletonFactory<ShareMono>.GetSingleton().RemoveUpdateAction(EnumUpdateAction.Standard, OpenOrCloseUpdate);
            }
        }
        private void OpenOrCloseUpdate()
        {
            if (m_MusicAlbum.sprite != null)
            {
                m_MusicAlbum.transform.Rotate(0, 0, 0.05f);
            }
        }

        #region 鼠标光标相关的事件


        #region MusicNamePointer
        private void MusicNamePointerEnter(BaseEventData bEData)
        {
            MonoSingletonFactory<ShareMono>.GetSingleton().ApplyUpdateAction(EnumUpdateAction.Standard, MusicNamePointerUpdate);      
        }
        private void MusicNamePointerExit(BaseEventData bEData)
        {
            MonoSingletonFactory<ShareMono>.GetSingleton().RemoveUpdateAction(EnumUpdateAction.Standard, MusicNamePointerUpdate);
        }
        private void MusicNamePointerUpdate()
        {
            //激活后文字向左偏移  往复循环


        }
        #endregion
        #endregion
    }
}
