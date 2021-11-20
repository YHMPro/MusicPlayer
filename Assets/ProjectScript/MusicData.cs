using UnityEngine;
using UnityEngine.Events;
using Farme.Net;
using System.IO;
using System;
using System.Text;

namespace MusicPlayer
{
    /// <summary>
    /// 歌曲数据
    /// </summary>
    public class MusicData
    {
        /// <summary>
        /// 歌曲的数量
        /// </summary>
        private static int m_MusicNum = 0;//歌曲的数量
        /// <summary>
        /// 歌曲的数量
        /// </summary>
        public static int MusicNum
        {
            get
            {
                if(m_MusicPaths!=null)
                {
                    m_MusicNum = m_MusicPaths.Length;
                }
                return m_MusicNum;
            }
        }
        /// <summary>
        /// 歌曲路径数组
        /// </summary>
        private static string[] m_MusicPaths = new string[100];
        /// <summary>
        /// 歌曲路径数组
        /// </summary>
        public static string[] MusicPaths
        {
            get
            {
                return m_MusicPaths;
            }
        }



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
        /// 是否有歌词
        /// </summary>
        private bool m_IsHaveLrc;
        /// <summary>
        /// 是否有封面
        /// </summary>
        private bool m_IsHaveCover;
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
        /// 初始化歌曲数据
        /// </summary>
        /// <param name="musicUrl">歌曲绝对路径</param>
        /// <param name="lrcUrl">歌词绝对路径</param>
        /// <param name="resultCallback">结果回调</param>
        /// <param name="progressCallback">加载进度回调</param>
        public void InitMusicData(string musicUrl,string lrcUrl,UnityAction<MusicData> resultCallback,UnityAction <float> progressCallback=null)
        {
            //if(musicUrl==null|| musicUrl)
            DownLoad.DownLoadAudioClipAsset(musicUrl, (ac) =>
             {
                  Debug.Log("歌曲音频加载成功");
                  m_Ac = ac;//音频                  
                  //LrcInfo.GetLrc(lrcUrl, (lrcInfo) =>
                  //{
                  //    Debug.Log("歌曲歌词加载成功");
                  //    m_LrcInfo = lrcInfo;//歌词信息
                  //    m_IsHaveLrc = true;                                                                       
                  //});
                 GetAlbumCover(musicUrl, (cover) =>
                 {
                     Debug.Log("歌曲封面加载成功");
                     m_Cover = cover;
                     m_IsHaveCover = true;
                 });//封面       
                 resultCallback?.Invoke(this);
             },progressCallback);
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
