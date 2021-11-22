using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Farme.Tool;
namespace MusicPlayer
{
    public class MusicPlayerTool 
    {
        /// <summary>
        /// 获取文件夹下所有子文件名称
        /// </summary>
        /// <param name="floderPath">文件夹路径</param>
        /// <param name="fileSuffix">提取的子文件类型(后缀)</param>
        /// <returns></returns>
        public static string[] GetAllChildFileNames(string floderPath, string fileSuffix)
        {
            if (Directory.Exists(floderPath))//判断该文件夹是否存在
            {                
                DirectoryInfo floder = new DirectoryInfo(floderPath);//文件夹信息
                FileInfo[] files = floder.GetFiles(fileSuffix);//所以子文件信息(fileSuffix类型
                string[] chileFileNames = new string[files.Length];//子文件名称数组
                for (int index = 0; index < chileFileNames.Length; index++)
                {
                    //去除文件后缀名   方面后续处理mp3与lrc文件
                    string fileName = files[index].Name.Split('.')[0];
                    chileFileNames[index] = fileName;
                }
                Debuger.Log("获取新的音乐文件数组成功");
                return chileFileNames;
            }
            Debuger.Log("获取新的音乐文件数组失败");
            return null;
        }
    }
}
