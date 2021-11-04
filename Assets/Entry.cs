

using UnityEngine;
using UnityEngine.UI;

using Farme;
namespace MusicPlayer
{
    public class Entry : MonoBehaviour
    {
        string MP3path = "S:\\Unity Pro 2019.3.7f1\\MyGitProject\\MusicPlayer\\Music\\Christophe Beck - Paperman.mp3";
        string Lrcpath = "S:\\Unity Pro 2019.3.7f1\\MyGitProject\\MusicPlayer\\Music\\Christophe Beck -Paperman.lrc";
        public Image img;
        public Text text;
        public AudioSource AS;
        public LineRenderer LR;
        public float[] samples;
        private readonly int LINERENDER_POINT_CNT = 32;
        private void Start()
        {

            //LrcInfo.GetLrc(Lrcpath, (info) =>
            // {
            //     text.text = info.Title;
            // });

            //UnityWebRequest unityWebRequest = UnityWebRequest.Get(MP3path);
            //DownloadHandlerAudioClip downloadHandlerAudioClip = new DownloadHandlerAudioClip(MP3path, AudioType.UNKNOWN);
            //unityWebRequest.downloadHandler = downloadHandlerAudioClip;
            //var s = unityWebRequest.SendWebRequest();

            NotMonoFactory<MusicData>.GetInstance().InitMusicData(MP3path, Lrcpath, (musicData) =>
             {
                 AS.clip = musicData.Ac;
                 img.sprite = Sprite.Create(musicData.Cover, new Rect(0, 0, musicData.Cover.width, musicData.Cover.height), Vector2.zero);
                 //text.text = musicData.LrcInfo.LrcBy;
                 //Debug.Log(musicData.LrcInfo.LrcBy);
                 //Debug.Log(musicData.LrcInfo.Title);
                 //Debug.Log(musicData.LrcInfo.Album);
                 //Debug.Log(musicData.LrcInfo.Artist);
                 AS.Play();
                 samples = new float[1024];
                 LR.positionCount = LINERENDER_POINT_CNT;
                 LR.startWidth = 0.02f;
                 LR.endWidth = 0.02f;

             });
            //while (true)
            //{
            //    if(s.isDone)
            //    {
            //        break;
            //    }
            //}
            //AS.clip = NAudioPlayer.FromMp3Data(downloadHandlerAudioClip.data);
            //AS.Play();

            //LoadAssetsTools.LoadFinishAudioClip(audiouwr);

            //StartCoroutine(Load());

            //DownLoad.DownLoadTextAsset(Lrcpath, (t) => 
            //{
            //    Debug.Log(t);
            //});
            //UnityWebRequest uwrLRC = LoadAssetsTools.StartLoadTextAsset(Lrcpath);
            //while (true)
            //{
            //    if (LoadAssetsTools.LoadingTextAsset(uwrLRC))
            //    {
            //        break;
            //    }
            //}
            //string content = LoadAssetsTools.LoadFinishTextAsset(uwrLRC);
            //Debug.Log(content);
            //string[] lrcLines = SplitString(content);
            //foreach (var lrcLine in lrcLines)
            //{
            //    text.text += lrcLine;
            //    text.text += "\n";
            //}
        }

      

        public void Update()
        {
            if(Input.GetKey(KeyCode.Space))
            {
                AS.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
                for (int i = 0, cnt = LINERENDER_POINT_CNT; i < cnt; ++i)
                {
                    var v = samples[i];
                    LR.SetPosition(i, new Vector3((i - LINERENDER_POINT_CNT / 2) * 0.2f, v * 20, -5));
                }
            }

        }

    }
   
}

