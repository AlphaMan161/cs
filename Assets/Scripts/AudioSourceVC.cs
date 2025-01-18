// ILSpyBased#2
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceVC : MonoBehaviour
{
    static Dictionary<string, int> _003C_003Ef__switch_0024map7;
    private AudioSource audioSource;

    public string SoundType = "effect";

    private bool paused;

    private void Start()
    {
        this.audioSource = base.GetComponent<AudioSource>();
    }

    private void LateUpdate()
    {
        if (OptionsManager.SoundIsMute || OptionsManager.SoundVolumeEffect <= 0f)
        {
            this.audioSource.volume = 0f;
            if (!this.paused)
            {
                this.audioSource.Pause();
                this.paused = true;
            }
        }
        else
        {
            if (this.paused)
            {
                this.audioSource.Play();
                this.paused = false;
            }
            string soundType = this.SoundType;
            if (soundType != null)
            {
                if (AudioSourceVC._003C_003Ef__switch_0024map7 == null)
                {
                    Dictionary<string, int> dictionary = new Dictionary<string, int>(2);
                    dictionary.Add("effect", 0);
                    dictionary.Add("music", 1);
                    AudioSourceVC._003C_003Ef__switch_0024map7 = dictionary;
                }
                int num = default(int);
                if (AudioSourceVC._003C_003Ef__switch_0024map7.TryGetValue(soundType, out num))
                {
                    switch (num)
                    {
                        case 0:
                            if (this.audioSource.volume != OptionsManager.SoundVolumeEffect)
                            {
                                this.audioSource.volume = OptionsManager.SoundVolumeEffect;
                            }
                            break;
                        case 1:
                            if (this.audioSource.volume != OptionsManager.SoundVolumeMusic)
                            {
                                this.audioSource.volume = OptionsManager.SoundVolumeMusic;
                            }
                            break;
                    }
                }
            }
        }
    }
}


