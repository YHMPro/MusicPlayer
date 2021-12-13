using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Farme;
using Farme.UI;
using MusicPlayer.Manager;
namespace MusicPlayer.Panel
{
    /// <summary>
    /// 设置面板
    /// </summary>
    public class MusicPlaySetPanel : BasePanel
    {
        private Toggle m_PathSet = null;
        private InputField m_PathInput = null;
        private Button m_CloseBtn = null;
        private Slider m_MainVolume = null;
        private Slider m_MusicVolume = null;
        private Slider m_ButtonVolume = null;
        protected override void Awake()
        {
            base.Awake();
            RegisterComponentsTypes<Button>();
            RegisterComponentsTypes<InputField>();
            RegisterComponentsTypes<Toggle>();
            RegisterComponentsTypes<Slider>();
            m_CloseBtn = GetComponent<Button>("CloseBtn");
            m_PathSet = GetComponent<Toggle>("PathSet");
            m_PathInput = GetComponent<InputField>("PathInput");
            m_MainVolume = GetComponent<Slider>("MainVolumeSlider");
            m_MusicVolume = GetComponent<Slider>("MusicVolumeSlider");
            m_ButtonVolume = GetComponent<Slider>("ButtonVolumeSlider");
        }
        protected override void Start()
        {
            base.Start();
            m_CloseBtn.onClick.AddListener(CloseEvent);
            m_PathSet.onValueChanged.AddListener(LockMusicFilePath);
            m_PathInput.onEndEdit.AddListener(MusicFilePathEditeEndEvent);
            m_MainVolume.onValueChanged.AddListener(MainVolumeChangeEvent);
            m_MusicVolume.onValueChanged.AddListener(MusicVolumeChangeEvent);
            m_ButtonVolume.onValueChanged.AddListener(ButtonVolumeChangeEvent);


            MainVolumeChangeEvent(1);
            MusicVolumeChangeEvent(0.8f);
            ButtonVolumeChangeEvent(0.8f);
        }
        protected override void OnDestroy()
        {
            m_CloseBtn.onClick.RemoveListener(CloseEvent);
            m_PathSet.onValueChanged.RemoveListener(LockMusicFilePath);
            m_PathInput.onEndEdit.RemoveListener(MusicFilePathEditeEndEvent);
            m_MainVolume.onValueChanged.RemoveListener(MainVolumeChangeEvent);
            m_MusicVolume.onValueChanged.RemoveListener(MusicVolumeChangeEvent);
            m_ButtonVolume.onValueChanged.RemoveListener(ButtonVolumeChangeEvent);
            base.OnDestroy();
        }

        public void RefreshPanel()
        {
            
        }


        /// <summary>
        /// 锁定音乐文件夹路径
        /// </summary>
        /// <param name="isOn">是否锁定</param>
        private void LockMusicFilePath(bool isOn)
        {
            if (GetComponent("PathInput", out InputField input))
            {
                input.interactable = !isOn;
                input.readOnly = isOn;
            }
        }
        /// <summary>
        /// 音乐文件路径编辑结束时间
        /// </summary>
        /// <param name="value">路径</param>
        private void MusicFilePathEditeEndEvent(string value)
        {
            MusicPlayerData.MusicFilePath = value;
        }

        private void CloseEvent()
        {
            SetState(EnumPanelState.Hide);
        }
        /// <summary>
        /// 歌曲文件路径
        /// </summary>
        /// <returns></returns>
        public string MusicFilePath()
        {
            if (GetComponent("PathInput", out InputField input))
            {
                return input.text;
            }
            return "";
        }


        #region 音量修改事件
        private void MainVolumeChangeEvent(float value)//主音量
        {
            MusicController.MainVolume = value;
        }
        private void MusicVolumeChangeEvent(float value)//音乐音量
        {
            MusicController.MusicVolume = value;
        }
        private void ButtonVolumeChangeEvent(float value)//按钮音量
        {
            MusicController.ButtonVolume = value;
        }
        #endregion
    }
}
