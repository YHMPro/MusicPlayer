using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farme.UI;
using Farme;
using MusicPlayer.Panel;
using Farme.Tool;
using MusicPlayer.Manager;
namespace MusicPlayer
{
    public class MusicPlayerEntry : MonoBehaviour
    {
        private void Awake()
        {
            MusicController.Init();
            //加载控制面板
            if(GoLoad.Take(@"FarmeLockFile\WindowRoot", out GameObject go))
            {
                WindowRoot root = MonoSingletonFactory<WindowRoot>.GetSingleton(go);
                root.CreateWindow("Controller", RenderMode.ScreenSpaceOverlay, (window) => 
                {
                    window.CreatePanel<MusicPlayControllerPanel>(@"Panel\MusicPlayControllerPanel", "ControllerPanel", EnumPanelLayer.TOP, (panel) => 
                    {
                        Debuger.Log("加载控制面板成功");
                    });
                    window.CreatePanel<MusicLyricPanel>(@"Panel\MusicLyricPanel", "LyricPanel", EnumPanelLayer.BOTTOM, (panel) =>
                    {                  
                        Debuger.Log("加载歌词面板成功");
                    });
                });                
            }
            if (ResourcesLoad.Load("SkyBox/Purple_4K_Resolution",out Material material))
            {
                RenderSettings.skybox = material;
            }
            //设置天空盒
           
        }
        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                MusicPlayerData.MusicFilePath = @"C:\Users\XiaoHeTao\Desktop\Music";

                //foreach(var name in MusicPlayerData.MusicFileNames)
                //{
                //    Debuger.Log(name);
                //}
            }
        }
    }
}
