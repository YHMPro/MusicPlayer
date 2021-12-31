using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MusicPlayer.Manager;
namespace MusicPlayer.Visual
{
    public class VisualTool
    {     
        public static void SampleAllot()
        {
            Vector3 scale;
            for (int i=0,k=0;k<63;k++)
            {
                scale = new Vector3(0, 1, 1);
                for (int j=0;j<1;j++)//共分为63组  每组4个采样数据
                {
                    scale.x += MusicController.MusicSample[i];
                    i++;
                }
                scale.x *= 30f;
                if (VisualStyle.VisualRectangleLi.Count > k)
                {
                    VisualStyle.VisualRectangleLi[k].Scale = scale;
                }
            }
        }






    }
}
