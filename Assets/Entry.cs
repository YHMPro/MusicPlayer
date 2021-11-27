

using UnityEngine;
using UnityEngine.UI;

using Farme;
using Farme.Audio;
using Farme.Net;

namespace MusicPlayer
{
    public class Entry : MonoBehaviour
    {

        public Transform Cube;
        public RectTransform img1;
        public RectTransform img2;
        string MP3path = "C:\\Users\\XiaoHeTao\\Music\\Wisp X - Stand With Me.mp3";
        //"花たん (花糖) - only my railgun.mp3";
        //"S:\\Unity Pro 2019.3.7f1\\MyGitProject\\MusicPlayer\\Music\\Christophe Beck - Paperman.mp3";
        string Lrcpath = "C:\\Users\\XiaoHeTao\\Music\\Wisp X - Stand With Me.lrc";// +
                                                                                   //"花たん (花糖) - only my railgun.lrc";
                                                                                   //"S:\\Unity Pro 2019.3.7f1\\MyGitProject\\MusicPlayer\\Music\\Christophe Beck -Paperman.lrc";
        public Toggle toggle;
        public InputField URLInput;
        public Image img;
        Audio m_audio;
        public float[] samples;
        private readonly int LINERENDER_POINT_CNT = 32;



        private void Awake()
        {
            WebDownloadTool.WebDownloadTexture(@"F:\YHM\项目\景德镇陶瓷拉胚项目\协作记录\模型协作\负责人(祖)\2021.11.26\陶艺拉坯姿势需求\姿势截图\1.png", (texture2D) =>
            {
                img.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);

            });
            //m_audio = AudioManager.ApplyForAudio();
            //m_audio.AbleRecycle = false;
            //samples = new float[64];
            ////InvokeRepeating("Con", 0, 0.2f);
        }

        public void SetURLEnd()
        {
            MP3path = URLInput.text;
            //Lrcpath=
        }
        Audio audio;
        string path = "C:\\Users\\XiaoHeTao\\Desktop\\Music\\";
        public void PlayEvent()
        {
            //if(audio!=null)
            //{
            //    audio.Clip.UnloadAudioData();
            //}
            //WebDownloadTool.WebDownLoadAudioClipMP3(path + MP3path+".mp3", (clip) =>
            //{
            //    if (audio == null)
            //    {
            //        audio = AudioManager.ApplyForAudio();
            //        audio.Loop = true;
            //    }
            //    audio.Clip = clip;
            //    audio.Play();
            //});
            //NotMonoFactory<MusicData>.GetInstance().InitMusicData(MP3path, Lrcpath, (musicData) =>
            //{
            //    img.sprite = Sprite.Create(musicData.Cover, new Rect(0, 0, musicData.Cover.width, musicData.Cover.height), Vector2.zero);
            //    if (m_audio.Clip != null)
            //    {
            //        m_audio.Clip.UnloadAudioData();
            //    }
            //    m_audio.Clip = musicData.Ac;
            //    m_audio.Play();
            //    MonoSingletonFactory<ShareMono>.GetSingleton().ApplyUpdateAction(EnumUpdateAction.Standard, Con);
            //});
        }

        public void PauseEvent()
        {
           // MonoSingletonFactory<ShareMono>.GetSingleton().RemoveUpdateAction(EnumUpdateAction.Standard, Con);
            //m_audio.Pause();
        }

        public void LoopEvent()
        {
            //m_audio.Loop = toggle.isOn;
        }
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

            //NotMonoFactory<MusicData>.GetInstance().InitMusicData(MP3path, Lrcpath, (musicData) =>
            // {
                          
            //     audio =AudioManager.ApplyForAudio();
                 
                
            //     //AS.clip = musicData.Ac;
                 
            //     //InvokeRepeating("Con", 0,0.1f);
            //     ////text.text = musicData.LrcInfo.LrcBy;
            //     ////Debug.Log(musicData.LrcInfo.LrcBy);
            //     ////Debug.Log(musicData.LrcInfo.Title);
            //     ////Debug.Log(musicData.LrcInfo.Album);
            //     ////Debug.Log(musicData.LrcInfo.Artist);
            //     //AS.Play();
            //     //samples = new float[1024];
            //     //LR.positionCount = LINERENDER_POINT_CNT;
            //     //LR.startWidth = 0.02f;
            //     //LR.endWidth = 0.02f;

            // });
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

        public void Con()
        {
            if(audio!=null)
            {
                //audio.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
                //float num = 0;
                //foreach(var value in samples)
                //{
                //    num += value;
                //}
                //img1.localScale = Vector3.one * num*10;
                //img2.localScale = Vector3.one * num*10;

            }
        }

        public void Update()
        {
            
            //if(Input.GetKey(KeyCode.Space))
            //{
            //    //AS.GetSpectrumData(samples, 0, FFTWindow.BlackmanHarris);
            //    //for (int i = 0, cnt = LINERENDER_POINT_CNT; i < cnt; ++i)
            //    //{
            //    //    var v = samples[i];
            //    //    LR.SetPosition(i, new Vector3((i - LINERENDER_POINT_CNT / 2) * 0.2f, v * 20, -5));
            //    //}
            //}

        }

    }
   
}

