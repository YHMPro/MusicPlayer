using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MusicPlayer;
using Farme.Net;
using Farme.Audio;
using MusicPlayer.Visual;
public class Test : MonoBehaviour
{
    //public Transform img;
    public bool enable=false;
    //private Vector3 m_SelfToImgDir;
    //private float Dis = 5;
    //float m_y;
    // Start is called before the first frame update
    void Start()
    {
        //VisualStyle.BuilderLine();
        //VisualStyle.BuilderCircle();
        //m_y = transform.position.y - img.position.y;
        //m_SelfToImgDir=(img.position - transform.position).normalized;
        // m_SelfToImgDir = img.position - transform.position;
        //Dis = m_SelfToImgDir.magnitude;
        //string path = "C:\\Users\\XiaoHeTao\\Desktop\\Music\\";

        //WebDownloadTool.WebDownloadText(path + "123我爱你.lrc", (str) =>
        //  {


        //WebDownloadTool.WebDownLoadAudioClipMP3(path + "M2M - Pretty Boy.mp3", (clip) =>
        //  {
        //      Audio audio = AudioManager.ApplyForAudio();
        //      audio.Loop = true;
        //      audio.Clip = clip;
        //      audio.Play();
        //  });


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

        //_= Sort(array_);
        //foreach(var value in array_)
        //{
        //    Debug.Log(value);
        //}

        VisualStyle.BuilderLine();

    }
    //private int[] array_ = new int[] { 5, 2, 8, 4, 9, 1 };
    //private int[] Sort(int[] array)
    //{
    //    for (int index_1 = 0; index_1 < array.Length; index_1++)
    //    {
    //        for (int index_2 = index_1+1; index_2 < array.Length; index_2++)
    //        {
    //            if (array[index_1] > array[index_2])
    //            {
    //                //交换两者的元素值
    //                int value = array[index_1];
    //                array[index_1] = array[index_2];
    //                array[index_2 ] = value;
    //            }
    //        }
    //    }
    //    return array;
    //}
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            VisualStyle.BuilderLine();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            VisualStyle.BuilderCircle();
        }
        if (enable)
        {
            VisualTool.SampleAllot();
        }
        //Vector3 selfToImgDir = (img.position - transform.position).normalized;

        //Vector3 pos= transform.position + m_SelfToImgDir * Dis;

        //pos.y = m_y;
        //float angle = Vector3.SignedAngle(Vector3.forward, transform.forward,transform.up);

        //pos.x = -Mathf.Cos(angle * Mathf.PI / 180f);
        //pos.z = Mathf.Sin(angle * Mathf.PI / 180f);


        //Vector3 r = img.eulerAngles;
        //r.y = -90+transform.eulerAngles.y;
        //img.eulerAngles = r;




        //img.position =transform.position+ pos.normalized*Dis;

        //img.LookAt(transform);


        //transform.forward

        //m_SelfToImgDir = img.position - transform.position;
        //Vector3 v1 = transform.forward.normalized;
        //v1.x = 0;
        //Vector3 v = img.position;
        //v.y = 0;
        //v.y = 0;

        //Vector3 r = img.eulerAngles;
        //r.x = 0;
        //img.eulerAngles = r;
        //img.position = v;
    }
}
