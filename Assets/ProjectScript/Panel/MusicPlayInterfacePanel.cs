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
    /// 音乐界面面板
    /// </summary>
    public class MusicPlayInterfacePanel : BasePanel
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
            string[] btnNameArray = new string[] { "MusicList", "Set"};//后续会改成读取配置表
            foreach (var btnName in btnNameArray)
            {
                if (GetComponent(btnName, out btn))
                {
                    switch (btnName)
                    {
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
                    if (callback != null)
                    {
                        btn.onClick.AddListener(callback);
                    }

                }
            }
        }


        private void SetEvent()
        {
            Debuger.Log("设置事件");
            if(MonoSingletonFactory<WindowRoot>.SingletonExist)
            {
                WindowRoot root = MonoSingletonFactory<WindowRoot>.GetSingleton();

                StandardWindow window = root.GetWindow("Controller");
                if(window!=null)
                {
                    MusicPlaySetPanel panel = window.GetPanel<MusicPlaySetPanel>("SetPanel");
                    if(panel==null)
                    {
                        window.CreatePanel<MusicPlaySetPanel>("Panel\\MusicPlaySetPanel","SetPanel", EnumPanelLayer.SYSTEM, (instance) =>
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
                    panel.SetState(m_MusicListEnable?EnumPanelState.Show:EnumPanelState.Hide);
                }
            }

        }

    }
}
