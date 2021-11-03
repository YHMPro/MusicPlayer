using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Farme
{
    /// <summary>
    /// Audio3D托管
    /// </summary>
    public class Audio3DMgr : BaseAudioMgr
    {
        protected Audio3DMgr() { }

        protected override BaseAudio GetAudioInstance()
        {
            return MonoFactory<Audio3D>.GetInstance();
        }
    }
}
