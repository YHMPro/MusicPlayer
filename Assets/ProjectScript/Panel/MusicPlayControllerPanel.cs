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
            string[] btnNameArray = new string[] { "PlayType", "Last", "PlayAndPause", "Next", "Volume" };//后续会改成读取配置表
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
    }
}
