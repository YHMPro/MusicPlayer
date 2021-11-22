using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Farme;
using Farme.UI;
using Farme.Tool;
using UnityEngine.Events;
namespace MusicPlayer.Panel
{
    /// <summary>
    /// 音乐播放控制面板
    /// </summary>
    public class MusicPlayControllerPanel : BasePanel
    {

        protected override void Awake()
        {
            base.Awake();
            RegisterComponentsTypes<Button>();//注册按钮类型
        }

        protected override void Start()
        {
            base.Start();
            Button btn = null;
            UnityAction callback = null;
            string[] btnNameArray = new string[] { "PlayType", "Last", "PlayAndPause", "Next", "Volume","MusicList","Set" };//后续会改成读取配置表
            foreach(var btnName in btnNameArray)
            {
                if (GetComponent(btnName, out btn))
                {
                    switch(btnName)
                    {
                        case "PlayType":
                            {
                                callback = MusicPlayTypeEvent;
                                break;
                            }
                        case "Last":
                            {
                                callback = MusicLastEvent;
                                break;
                            }
                        case "PlayAndPause":
                            {
                                callback = MusicPlayAndPauseEvent;
                                break;
                            }
                        case "Next":
                            {
                                callback = MusicNextEvent;
                                break;
                            }
                        case "Volume":
                            {
                                callback = MusicVolumeEvent;
                                break;
                            }
                        case "MusicList":
                            {
                                callback = OpenMusicListEvent;
                                break;
                            }
                        case "Set":
                            {
                                callback = SetEvent;
                                break;
                            }
                    }
                    if(callback!=null)
                    {
                        btn.onClick.AddListener(callback);
                    }

                }
            }           
        }

        private void MusicPlayTypeEvent()
        {
            Debuger.Log("触发更新播放方式事件");
        }

        private void MusicPlayAndPauseEvent()
        {
            Debuger.Log("触发播放或暂停事件");
        }

        private void MusicNextEvent()
        {
            Debuger.Log("触发播放下一首事件");
        }

        private void MusicLastEvent()
        {
            Debuger.Log("触发播放上一首事件");
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
                        window.CreatePanel<MusicPlaySetPanel>("Panel\\MusicPlaySetPanel", "SetPanel", EnumPanelLayer.SYSTEM, (instance) =>
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
                        window.CreatePanel<MusicListPanel>("Panel\\MusicListPanel", "MusicListPanel", EnumPanelLayer.BOTTOM, (instance) =>
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
    }
}
