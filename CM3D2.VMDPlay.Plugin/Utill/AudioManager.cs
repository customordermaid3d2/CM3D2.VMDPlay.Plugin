using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace COM3D2.Lilly.Plugin.Utill
{
    // AudioManager.audioMgr
    // AudioManager.audiosource
    public class AudioManager
    {
        public static bool isLoaded = false;
        //public static AudioSourceMgr audioMgr;
        public static AudioSource audiosource;
        public static SoundMgr soundMgr;
        public static int danceVolumeBackup;

        public static bool Load(string filePath, bool loop = false)
        {
            if (soundMgr == null)
            {
                soundMgr = GameMain.Instance.SoundMgr;
            }

            Debug.Log("LoadAndPlayAudioClip" + filePath);
            string extension = Path.GetExtension(filePath);
            if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath) || (extension != ".ogg" && extension != ".wav"))
            {
                if (!string.IsNullOrEmpty(filePath))
                {
                    Console.WriteLine(string.Format("{0}または{1}ファイルを指定してください。{2}", ".ogg", ".wav", filePath));
                }
                return false;
            }
            isLoaded = false;
            using (WWW www = new WWW("file:///" + filePath))
            {
                int num = 0;
                while (!www.isDone)
                {
                    Thread.Sleep(100);
                    num += 100;
                    if (30000 < num)
                    {
                        Console.WriteLine("音声読込タイムアウトのため処理を中止します。");
                        return false;
                    }
                }
                AudioClip audioClip = www.GetAudioClip();
                if (audioClip.loadState == AudioDataLoadState.Loaded)
                {
                    if (audiosource != null)
                    {
                        GameMain.Destroy(audiosource);
                        audiosource = null;
                    }
                    if (audiosource == null)
                    {
                        //AudioSourceMgr[] componentsInChildren = GameMain.Instance.MainCamera.gameObject.GetComponentsInChildren<AudioSourceMgr>();
                        //audioMgr = componentsInChildren.FirstOrDefault((AudioSourceMgr a) => a.SoundType == AudioSourceMgr.Type.Bgm);
                        audiosource = GameMain.Instance.MainCamera.gameObject.AddComponent<AudioSource>();
                        //audiosource = audioMgr.audiosource;
                    }
                    if (audiosource != null)
                    {
                        audiosource.clip = audioClip;
                        audiosource.loop = loop;
                        isLoaded = true;
                    }
                }
            }
            return isLoaded;
        }

        public static void SetTime(float bgmTime)
        {
            audiosource.time = bgmTime;
        }

        public static void SetVolume(int volume)
        {
            SoundMgr soundMgr = GameMain.Instance.SoundMgr;
            audiosource.outputAudioMixerGroup = soundMgr.mix_mgr[AudioMixerMgr.Group.Dance];
            audiosource.volume = (float)volume;
            audiosource.mute = false;
            soundMgr.SetVolumeDance(volume);
            soundMgr.Apply();
        }

        public static float GetTime()
        {
            return audiosource.time;
        }

        public static float GetLength()
        {
            return audiosource.clip.length;
        }

        public static void Play(bool isRepeat = false)
        {
            Debug.Log("Play "+ audiosource != null);
            if (audiosource != null)
            {
                GameMain.Instance.SoundMgr.StopAll();
                danceVolumeBackup = soundMgr.GetVolumeDance();
                audiosource.Stop();
                audiosource.Play();
                audiosource.loop = isRepeat;
                SetTime(0f);
                SetVolume(danceVolumeBackup);
                Debug.Log("Play " + GetLength());
                Debug.Log("Play " + GetTime());
                Debug.Log("Play " + danceVolumeBackup);
            }
        }

        public static void Play(string bgmName, bool isRepeat = false)
        {
            if (Load(bgmName))
            {
                //if (!isRepeat)
                //{
                //    soundMgr.StopBGM(0f);
                //}                
                Play();               
            }
        }

        public static void Stop()
        {
            Debug.Log("Stop");
            if (audiosource != null)
            {
                audiosource.Stop();
            }
        }

        public static void Pause(bool isRepeat = false)
        {
            if (audiosource != null)
            {
                if (isPlay())
                {
                    audiosource.Pause();
                }
                else
                {
                    audiosource.UnPause();
                }
                audiosource.loop = isRepeat;
            }
        }
        /*
        public static void UnPause(bool isRepeat = false)
        {
            if (audioMgr != null)
            {
                audioMgr.audiosource.UnPause();
                audioMgr.audiosource.loop = isRepeat;
            }
        }*/

        public AudioManager()
        {
        }


        public static bool isPlay()
        {
            if (audiosource != null)
            {
                return audiosource.isPlaying;
            }
            return false;
        }

        public static void SetLoop(bool loop = true)
        {
            if (audiosource != null)
            {
                audiosource.loop = loop;
            }
        }
    }
}
