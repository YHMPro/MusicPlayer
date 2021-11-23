using UnityEngine;
using UnityEngine.Events;
using Farme.Net;
using System.IO;
using System;
using System.Text;
using Farme.UI;
using Farme;
using MusicPlayer.Panel;
using Farme.Tool;
using System.Collections.Generic;

namespace MusicPlayer
{
    /// <summary>
    /// 歌曲数据
    /// </summary>
    public class MusicData
    {       
        private bool m_IsLoadLrc = false;

        private bool m_IsLoadAudio = false;

        private bool m_IsLoadCover = false;
        /// <summary>
        /// 音频
        /// </summary>
        private AudioClip m_Ac;
        /// <summary>
        /// 封面
        /// </summary>
        private Texture2D m_Cover;
        /// <summary>
        /// 歌词信息
        /// </summary>
        private LrcInfo m_LrcInfo;        
        /// <summary>
        /// 歌曲音频
        /// </summary>
        public AudioClip Ac
        {
            get
            {
                return m_Ac;
            }
        }
        /// <summary>
        /// 封面
        /// </summary>
        public Texture2D Cover
        {
            get
            {
                return m_Cover;
            }
        }
        /// <summary>
        /// 歌词信息
        /// </summary>
        public LrcInfo LrcInfo
        {
            get
            {                 
                return m_LrcInfo;
            }
        }

        /// <summary>
        /// 歌曲文件名称
        /// </summary>
        public void LoadAudio(string musicFileName)
        {
            if(m_IsLoadAudio)
            {
                return;
            }
            m_IsLoadAudio = true;
            DownLoad.DownLoadAudioClipAsset(MusicPlayerData.MusicFilePath + "\\" + musicFileName + ".mpc", (clip) =>
             {
                 m_Ac = clip;//音频
                 Debuger.Log("加载歌曲音频成功");
                 GetAlbumCover(MusicPlayerData.MusicFilePath + "\\" + musicFileName + ".mpc", (cover) =>
                 {
                     Debug.Log("歌曲封面加载成功");
                     m_Cover = cover;
                 });//封面       
             });
        }
        /// <summary>
        /// 歌曲文件名称
        /// </summary>
        /// <param name="musicFileName"></param>
        public void LoadLrc(string musicFileName,UnityAction callback)
        {
            if(m_IsLoadLrc)
            {
                callback?.Invoke();
                return;
            }
            m_IsLoadLrc = true;
            LrcInfo.GetLrc(MusicPlayerData.MusicFilePath + "\\" + musicFileName + ".lrc",(lrc)=> 
            {
                if (lrc != null)
                {
                    m_LrcInfo = lrc;
                    Debuger.Log("加载歌词成功");
                }
                else
                {
                    m_LrcInfo = new LrcInfo();
                    Debuger.Log("加载歌词失败,待初始值");
                }
                callback?.Invoke();
                   
            });
        }       
        /// <summary>
        /// 清除数据
        /// </summary>
        public void ClearData()
        {
            if(m_Ac!=null)
            {
                m_Ac.UnloadAudioData();
            }
            UnityEngine.Object.Destroy(m_Ac);
            UnityEngine.Object.Destroy(m_Cover);
            m_LrcInfo = null;
        }
        /// <summary>
        /// 获取音乐封面
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static void GetAlbumCover(string path,UnityAction<Texture2D> resultCallback )
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
    }
}
