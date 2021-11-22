using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farme.UI;
using Farme;
using MusicPlayer.Panel;
using Farme.Tool;
namespace MusicPlayer
{
    public class MusicPlayerEntry : MonoBehaviour
    {
        private void Awake()
        {           





            //加载控制面板
            if(GoLoad.Take("FarmeLockFile\\WindowRoot", out GameObject go))
            {
                WindowRoot root = MonoSingletonFactory<WindowRoot>.GetSingleton(go);
                root.CreateWindow("Controller", RenderMode.ScreenSpaceOverlay, (window) => 
                {
                    window.CreatePanel<MusicPlayControllerPanel>("Panel\\MusicPlayControllerPanel", "ControllerPanel", EnumPanelLayer.TOP, (panel) => 
                    {
                        Debuger.Log("加载控制面板成功");
                    });
                    //window.CreatePanel<MusicPlayInterfacePanel>("Panel\\MusicPlayInterfacePanel", "InterfacePanel", EnumPanelLayer.MIDDLE, (panel) =>
                    //{
                    //    Debuger.Log("加载界面面板成功");
                    //});
                });                
            }
            

            


        }
    }
}
