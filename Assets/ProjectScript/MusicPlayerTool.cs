using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Farme.Tool;
using System;
using System.Text;
using UnityEngine.Events;
using Farme.Net;
using MusicPlayer.Manager;

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

        /// <summary>
        /// 处理信息
        /// </summary>
        /// <param name="line"></param>
        /// <returns>返回基础信息</returns>
        public static string SplitInfo(string line)
        {
            return line.Substring(line.IndexOf(":") + 1).TrimEnd(']');
        }
        /// <summary>
        /// 分割字符串（换行符）
        /// </summary>
        /// <param name="content">字符串</param>
        /// <returns></returns>
        public static string[] SplitString(string content)
        {
            return content.Split(Environment.NewLine.ToCharArray());//new string[] { LINE }, StringSplitOptions.None);
        }

        /// <summary>
        /// 获取音乐封面
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static void GetAlbumCover(string path, UnityAction<Texture2D> resultCallback)
        {
            FileStream fs = new FileStream(path, FileMode.Open);
            try
            {
                byte[] header = new byte[10]; //标签头
                int offset = 0;
                bool haveAPIC = false;
                fs.Read(header, 0, 10);
                offset += 10;
                string head = Encoding.Default.GetString(header, 0, 3);
                if (head.Equals("ID3"))
                {
                    int sizeAll = header[6] * 0x200000 //获取该标签的尺寸，不包括标签头
                    + header[7] * 0x4000
                    + header[8] * 0x80
                    + header[9];
                    int size = 0;
                    byte[] body = new byte[10]; //数据帧头,这里认为数据帧头不包括编码方式
                    fs.Read(body, 0, 10);
                    offset += 10;
                    head = Encoding.Default.GetString(body, 0, 4);
                    while (offset < sizeAll) //当数据帧不是图片的时候继续查找
                    {
                        if (("APIC".Equals(head))) { haveAPIC = true; break; }
                        size = body[size + 4] * 0x1000000 //获取该数据帧的尺寸(不包括帧头)
                        + body[size + 5] * 0x10000
                        + body[size + 6] * 0x100
                        + body[size + 7];
                        body = new byte[size + 10];
                        fs.Read(body, 0, size + 10);
                        offset += size + 10;
                        head = Encoding.Default.GetString(body, size, 4);
                    }
                    if (haveAPIC)
                    {
                        size = body[size + 4] * 0x1000000
                        + body[size + 5] * 0x10000
                        + body[size + 6] * 0x100
                        + body[size + 7];
                        byte[] temp = new byte[9];
                        byte[] temptype = new byte[10];
                        fs.Seek(1, SeekOrigin.Current);
                        fs.Read(temp, 0, 9);
                        int i = 0;
                        switch (Encoding.Default.GetString(temp))
                        {
                            case "image/jpe":

                                while (i < size) //jpeg开头0xFFD8
                                {
                                    if (temptype[0] == 0 && temptype[1] == 255 && temptype[2] == 216)
                                    {
                                        break;
                                    }
                                    fs.Seek(-2, SeekOrigin.Current);
                                    fs.Read(temptype, 0, 3);
                                    i++;
                                }
                                fs.Seek(-2, SeekOrigin.Current);
                                break;
                            case "image/jpg":

                                while (i < size) //jpg开头0xFFD8
                                {
                                    if (temptype[0] == 0 && temptype[1] == 255 && temptype[2] == 216)
                                    {
                                        break;
                                    }
                                    fs.Seek(-2, SeekOrigin.Current);
                                    fs.Read(temptype, 0, 3);
                                    i++;
                                }
                                fs.Seek(-2, SeekOrigin.Current);
                                break;
                            case "image/gif":

                                while (i < size) //gif开头474946
                                {
                                    if (temptype[0] == 71 && temptype[1] == 73 && temptype[2] == 70)
                                    {
                                        break;
                                    }
                                    fs.Seek(-2, SeekOrigin.Current);
                                    fs.Read(temptype, 0, 3);
                                    i++;
                                }
                                fs.Seek(-3, SeekOrigin.Current);
                                break;
                            case "image/bmp":

                                while (i < size) //bmp开头424d
                                {
                                    if (temptype[0] == 66 && temptype[1] == 77)
                                    {
                                        break;
                                    }
                                    fs.Seek(-1, SeekOrigin.Current);
                                    fs.Read(temptype, 0, 2);
                                    i++;
                                }
                                fs.Seek(-2, SeekOrigin.Current);
                                break;
                            case "image/png":
                                while (i < size) //png开头89 50 4e 47 0d 0a 1a 0a
                                {
                                    if (temptype[0] == 137 && temptype[1] == 80 && temptype[2] == 78 && temptype[3] == 71 && temptype[4] == 13)
                                    {
                                        break;
                                    }
                                    fs.Seek(-9, SeekOrigin.Current);
                                    fs.Read(temptype, 0, 10);
                                    i++;
                                }
                                fs.Seek(-10, SeekOrigin.Current);
                                break;
                            default://FFFB为音频的开头
                                break;
                        }
                        byte[] imageBytes = new byte[size];
                        fs.Read(imageBytes, 0, size);
                        Texture2D texture2D = new Texture2D(128, 128);
                        texture2D.LoadImage(imageBytes);
                        resultCallback?.Invoke(texture2D);
                    }
                }

            }
            catch (Exception ex)
            {
                Debug.LogWarning(ex.Message);
            }
            finally
            {
                fs.Close();
            }
        }
        /// <summary>
        /// 去除字符串中的空字符
        /// </summary>
        /// <param name="target">字符串对象</param>
        /// <returns>结果</returns>
        public static string RemoveStringSpaceChar(string target)
        {
            return target.Replace(" ", "");
        }
        
    }
}
