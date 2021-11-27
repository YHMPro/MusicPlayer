using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MusicPlayer;
using Farme.Net;
using Farme.Audio;
public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string path = "C:\\Users\\XiaoHeTao\\Desktop\\Music\\";

        //WebDownloadTool.WebDownloadText(path + "123我爱你.lrc", (str) =>
        //  {


        WebDownloadTool.WebDownLoadAudioClipMP3(path + "M2M - Pretty Boy.mp3", (clip) =>
          {
              Audio audio = AudioManager.ApplyForAudio();
              audio.Loop = true;
              audio.Clip = clip;
              audio.Play();
          });


        //  });

        //LrcInfo.GetLrc(path + "不想长大（治愈版）.lrc", (info) =>
        // {
        //     Debug.Log(info.Album);
        //     foreach (var str in info.LrcWord.Values)
        //     {
        //         Debug.Log(str);
        //     }
        // });
        //string[] strArray = MusicPlayerData.GetAllChildFileNames(path, "*mp3");
        //foreach (var str in strArray)
        //{
        //    Debug.Log(str);

        //}
        //if (Directory.Exists(path))
        //{
        //    string[] strArray=  Directory.GetFiles(path,"*.mp3");
        //    foreach (var str in strArray)
        //    {


        //    }
        //}
        //string []str = DirectoryInfo.
        // File.Op("C:\\Users\\XiaoHeTao\\Desktop\\新建文件夹\\");


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
