using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Farme;
using Farme.Tool;
namespace MusicPlayer.Visual
{
    public class VisualStyle 
    {
        /// <summary>
        /// 建设原点
        /// </summary>
        private static Vector3 m_BuliderOrigin = Vector3.zero;
        /// <summary>
        /// 虚拟矩形体
        /// </summary>
        public static List<VisualRectangle> VisualRectangleLi = new List<VisualRectangle>();

        #region 直线型
        /// <summary>
        /// 直线状
        /// </summary>
        public static void BuilderLine()
        {
            ReCycle();
            GameObject go;
            VisualRectangle vr;
            Vector3 pos = new Vector3(-31, 0, 0);
            for (int i=0;i<63;i++)
            {
                if(!GoReusePool.Take("VisualRectangle",out go))
                {
                    if(!GoLoad.Take("VisualRectangle",out go,null))
                    {
                        Debuger.LogWarning("虚拟矩形体加载失败");
                        return;
                    }
                }
                vr = go.GetComponent<VisualRectangle>();
                VisualRectangleLi.Add(vr);
                //初始化数据            
                vr.Position = pos+ m_BuliderOrigin;
                pos.x++;
            }
        }
        #endregion
        #region 圆环型
        static float a = 15;
        static float b = 15;
        /// <summary>
        /// 圆环状
        /// </summary>
        public static void BuilderCircle()
        {
            ReCycle();
            GameObject go;
            VisualRectangle vr;
            float angle = 0;
            Vector3 pos = Vector3.zero;
            Vector3 euler = Vector3.zero;
            for (int i = 0; i < 63; i++)
            {
                if (!GoReusePool.Take("VisualRectangle", out go))
                {
                    if (!GoLoad.Take("VisualRectangle", out go, null))
                    {
                        Debuger.LogWarning("虚拟矩形体加载失败");
                        return;
                    }
                }
                angle = 360f / 63f * i;
                pos.x = Mathf.Cos(angle * Mathf.PI / 180f)*a;
                pos.y = Mathf.Sin(angle * Mathf.PI / 180f)*b;
                euler.z = angle;
                vr = go.GetComponent<VisualRectangle>();
                //初始化数据
                vr.Position=pos+ m_BuliderOrigin;
                vr.Euler = euler;
            }
            
        }
        #endregion
        /// <summary>
        /// 球体状
        /// </summary>
        public static void BuilderSphere()
        {

        }


        public static void ReCycle()
        {
            foreach(var vr in VisualRectangleLi)
            {
                GoReusePool.Put("VisualRectangle", vr.gameObject);
            }
            
        }
    }
}
