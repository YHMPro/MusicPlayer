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
    /// 设置面板
    /// </summary>
    public class MusicPlaySetPanel : BasePanel
    {
        protected override void Awake()
        {
            base.Awake();
            RegisterComponentsTypes<Button>();
            RegisterComponentsTypes<InputField>();
            RegisterComponentsTypes<Toggle>();
        }

        protected override void Start()
        {
            base.Start();
            Button btn = null;
            UnityAction callback = null;
            string[] btnNameArray = new string[] { "CloseBtn"};//后续会改成读取配置表
            foreach (var btnName in btnNameArray)
            {
                if (GetComponent(btnName, out btn))
                {
                    switch (btnName)
                    {
                        case "CloseBtn":
                            {
                                callback = CloseEvent;
                                break;
                            }                      
                    }
                    if (callback != null)
                    {
                        btn.onClick.AddListener(callback);
                    }

                }
            }
            if(GetComponent("PathSet",out Toggle toggle))
            {
                toggle.onValueChanged.AddListener(PathSetEvent);
            }
        }
        /// <summary>
        /// 路径设置事件
        /// </summary>
        /// <param name="isOn"></param>
        private void PathSetEvent(bool isOn)
        {
            if(GetComponent("PathInput", out InputField input))
            {
                input.interactable = !isOn;
                input.readOnly = isOn;
            }
        }

        private void CloseEvent()
        {
            SetState(EnumPanelState.Hide);
        }
        /// <summary>
        /// 歌曲文件路径
        /// </summary>
        /// <returns></returns>
        public string MusicFilePath()
        {
            if (GetComponent("PathInput", out InputField input))
            {
                return input.text;
            }
            return "";
        }
    }
}
