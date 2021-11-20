
using NAudio.Wave;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
public static class NAudioPlayer
{
    public static void FromMp3Data(byte[] data,UnityAction<AudioClip> callback)
    {      
        // Load the data into a stream
        using (MemoryStream mp3stream = new MemoryStream(data))
        {
            Mp3FileReader mp3audio = new Mp3FileReader(mp3stream);
            WaveStream waveStream = null;
            using (waveStream = WaveFormatConversionStream.CreatePcmStream(mp3audio))
            {
                WAV wav = new WAV(AudioMemStream(waveStream).ToArray());
                AudioClip audioClip = AudioClip.Create("Music", wav.SampleCount, 1, wav.Frequency, false);               
                audioClip.SetData(wav.LeftChannel, 0);
                callback?.Invoke(audioClip);
            }
        }                     
    }
    private static MemoryStream AudioMemStream(WaveStream waveStream)
    {
        MemoryStream outputStream = new MemoryStream();
        using (WaveFileWriter waveFileWriter = new WaveFileWriter(outputStream, waveStream.WaveFormat))
        {
            byte[] bytes = new byte[waveStream.Length];
            waveStream.Position = 0;
            waveStream.Read(bytes, 0, Convert.ToInt32(waveStream.Length));
            waveFileWriter.Write(bytes, 0, bytes.Length);
            waveFileWriter.Flush();
        }
        return outputStream;
    } 
}