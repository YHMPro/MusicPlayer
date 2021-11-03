using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Farme
{
    /// <summary>
    /// Audio2D托管
    /// </summary>
    public class Audio2DMgr : BaseAudioMgr
    {
        protected Audio2DMgr() { }
           
        protected override BaseAudio GetAudioInstance()
        {
            return MonoFactory<Audio2D>.GetInstance();
        }    
    }
}
