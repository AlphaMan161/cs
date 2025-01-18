// ILSpyBased#2
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;

    public AudioClip jetSound;

    public AudioClip jetOffSound;

    public AudioClip jetOnSound;

    public AudioClip machineGunShotSound;

    public AudioClip laserShotSound;

    public AudioClip railSound;

    public AudioClip reloadSound;

    public AudioClip damageSound;

    public AudioClip deathSound;

    public AudioClip killEnemySound;

    public AudioClip pickupAmmoSound;

    public AudioClip pickupHealthPackSound;

    public AudioClip matchStartsSound;

    public AudioClip matchEndsSound;

    private Dictionary<string, AudioClip> sounds;

    public static SoundManager Instance
    {
        get
        {
            return SoundManager.instance;
        }
    }

    private void Awake()
    {
        SoundManager.instance = this;
        this.sounds = new Dictionary<string, AudioClip>();
    }

    public void PlayShot(AudioSource src, int shotType)
    {
        if (!OptionsManager.SoundIsMute && !(OptionsManager.SoundVolumeEffect <= 0f))
        {
            switch (shotType)
            {
                case 2:
                    break;
                case 4:
                    break;
                case 5:
                    break;
                case 1:
                    src.PlayOneShot(this.machineGunShotSound);
                    MonoBehaviour.print("MachineSound");
                    break;
                case 3:
                    src.PlayOneShot(this.railSound);
                    break;
                case 6:
                    src.PlayOneShot(this.jetOnSound);
                    break;
                case 7:
                    src.PlayOneShot(this.jetOffSound);
                    break;
                case 8:
                    src.PlayOneShot(this.laserShotSound);
                    MonoBehaviour.print("LaserSound");
                    break;
            }
        }
    }

    public void Play(AudioSource src, string snd, AudioPlayMode playMode)
    {
        if (!OptionsManager.SoundIsMute && !(OptionsManager.SoundVolumeEffect <= 0f))
        {
            string path = "Sounds/" + snd;
            if (src.gameObject.activeSelf && src.gameObject.activeInHierarchy)
            {
                if (!this.sounds.ContainsKey(snd))
                {
                    this.sounds[snd] = (AudioClip)Resources.Load(path);
                }
                switch (playMode)
                {
                    case AudioPlayMode.Play:
                        src.PlayOneShot(this.sounds[snd]);
                        break;
                    case AudioPlayMode.PlayLoop:
                        src.clip = this.sounds[snd];
                        if (!src.isPlaying)
                        {
                            src.Play();
                            src.loop = true;
                        }
                        break;
                    case AudioPlayMode.PlayStop:
                        src.clip = null;
                        src.Stop();
                        src.PlayOneShot(this.sounds[snd]);
                        break;
                    case AudioPlayMode.Stop:
                        src.clip = null;
                        src.Stop();
                        break;
                }
            }
        }
    }

    public void PlayAfterSeconds(AudioSource src, string snd, AudioPlayMode playMode, float sec)
    {
        base.StartCoroutine(this.DelayedPlayRoutine(src, snd, playMode, sec));
    }

    private IEnumerator DelayedPlayRoutine(AudioSource src, string snd, AudioPlayMode playMode, float sec)
    {
        yield return (object)new WaitForSeconds(sec);
        this.Play(src, snd, playMode);
    }

    public void PlayReload(AudioSource src)
    {
        if (!OptionsManager.SoundIsMute && !(OptionsManager.SoundVolumeEffect <= 0f))
        {
            src.PlayOneShot(this.reloadSound);
        }
    }

    public void PlayDamage(AudioSource src)
    {
        if (!OptionsManager.SoundIsMute && !(OptionsManager.SoundVolumeEffect <= 0f))
        {
            src.PlayOneShot(this.damageSound);
        }
    }

    public void PlayDeath(AudioSource src)
    {
        if (!OptionsManager.SoundIsMute && !(OptionsManager.SoundVolumeEffect <= 0f))
        {
            src.PlayOneShot(this.deathSound);
        }
    }

    public void PlayKillEnemy(AudioSource src)
    {
        if (!OptionsManager.SoundIsMute && !(OptionsManager.SoundVolumeEffect <= 0f))
        {
            src.PlayOneShot(this.killEnemySound);
        }
    }

    public void PlayPickupAmmo(AudioSource src)
    {
        if (!OptionsManager.SoundIsMute && !(OptionsManager.SoundVolumeEffect <= 0f))
        {
            src.PlayOneShot(this.pickupAmmoSound);
        }
    }

    public void PlayPickupHealthPack(AudioSource src)
    {
        if (!OptionsManager.SoundIsMute && !(OptionsManager.SoundVolumeEffect <= 0f))
        {
            src.PlayOneShot(this.pickupHealthPackSound);
        }
    }
}


