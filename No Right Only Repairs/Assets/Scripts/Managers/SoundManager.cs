using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SoundName
{
    AsteroidBreaks,
    Death1,
    Death2,
    Death3,
    Death4,
    ElectionHasHappened,
    Notice,
    PickUpRoom,
    PirateShipShoots,
    PirateShipShootsAlternate,
    PlaceRoom,
    PlayerShipThrusters,
    TurretShots,
    UIMouseOver,
    Warning
}
public class SoundManager : MonoBehaviourSingleton<SoundManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;

    [Header("Sound Clips")]
    public AudioClip asteroidBreaks;
    public AudioClip death1;
    public AudioClip death2;
    public AudioClip death3;
    public AudioClip death4;
    public AudioClip electionHasHappened;
    public AudioClip notice;
    public AudioClip pickUpRoom;
    public AudioClip pirateShipShoots;
    public AudioClip pirateShipShootsAlternate;
    public AudioClip placeRoom;
    public AudioClip playerShipThrusters;
    public AudioClip turretShots;
    public AudioClip uiMouseOver;
    public AudioClip warning;

    [Header("Background Music")]
    public AudioClip battleBGM;
    public AudioClip shopBGM;
    public AudioClip normalBGM;

    [Header("Sound Management")]
    [SerializeField] private int maxSimultaneousSounds = 5;
    private Queue<AudioSource> audioSourcePool;
    private List<AudioSource> activeAudioSources;

    private float _sfxVolume = 1f;
    private float _musicVolume = 1f;

    private void Start()
    {
        _sfxVolume = Settings.Instance.SFXVolumn;
        _musicVolume = Settings.Instance.BGMVolumn;

        InitializeAudioSourcePool();
    }

    private void Update()
    {
        if (GameObject.FindWithTag("Enemy") != null)
        {
            if (GameManager.Instance.inBattle == false)
            {
                PlayBattleBGM();
            }
            GameManager.Instance.inBattle = true;
        }
        else
        {
            Debug.Log(2);
            if (GameManager.Instance.inBattle == true)
            {
                PlayNormalBGM();
            }
            GameManager.Instance.inBattle = false;
        }
    }

    private void InitializeAudioSourcePool()
    {
        audioSourcePool = new Queue<AudioSource>();
        activeAudioSources = new List<AudioSource>();

        for (int i = 0; i < maxSimultaneousSounds; i++)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.volume = _sfxVolume; 
            audioSourcePool.Enqueue(audioSource);
        }
    }

    private AudioSource GetAvailableAudioSource()
    {
        if (audioSourcePool.Count > 0)
        {
            return audioSourcePool.Dequeue();
        }
        else
        {
            if (activeAudioSources.Count > 0)
            {
                AudioSource oldestSource = activeAudioSources[0];
                activeAudioSources.RemoveAt(0);
                oldestSource.Stop();
                return oldestSource;
            }
            return null;
        }
    }

    public void PlaySound(SoundName soundName)
    {
        AudioClip clip = GetAudioClip(soundName);

        if (clip != null)
        {
            AudioSource audioSource = GetAvailableAudioSource();
            if (audioSource != null)
            {
                audioSource.clip = clip;
                audioSource.volume = _sfxVolume;
                audioSource.Play();
                activeAudioSources.Add(audioSource);

                StartCoroutine(ReturnAudioSourceToPool(audioSource, clip.length));
            }
            else
            {
                Debug.LogWarning("No available AudioSource to play: " + soundName);
            }
        }
        else
        {
            Debug.LogWarning("AudioClip not found: " + soundName);
        }
    }

    private IEnumerator ReturnAudioSourceToPool(AudioSource audioSource, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.Stop();
        activeAudioSources.Remove(audioSource);
        audioSourcePool.Enqueue(audioSource);
    }

    private AudioClip GetAudioClip(SoundName soundName)
    {
        switch (soundName)
        {
            case SoundName.AsteroidBreaks: return asteroidBreaks;
            case SoundName.Death1: return death1;
            case SoundName.Death2: return death2;
            case SoundName.Death3: return death3;
            case SoundName.Death4: return death4;
            case SoundName.ElectionHasHappened: return electionHasHappened;
            case SoundName.Notice: return notice;
            case SoundName.PickUpRoom: return pickUpRoom;
            case SoundName.PirateShipShoots: return pirateShipShoots;
            case SoundName.PirateShipShootsAlternate: return pirateShipShootsAlternate;
            case SoundName.PlaceRoom: return placeRoom;
            case SoundName.PlayerShipThrusters: return playerShipThrusters;
            case SoundName.TurretShots: return turretShots;
            case SoundName.UIMouseOver: return uiMouseOver;
            case SoundName.Warning: return warning;
            default: return null;
        }
    }

    public void PlayMusic(AudioClip clip, bool loop = true)
    {
        musicSource.clip = clip;
        musicSource.loop = loop;
        musicSource.volume = _musicVolume;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayBattleBGM()
    {
        PlayMusic(battleBGM);
        Debug.Log("BattleBGM");
    }

    public void PlayShopBGM()
    {
        PlayMusic(shopBGM);
        Debug.Log("ShopBGM");
    }

    public void PlayNormalBGM()
    {
        PlayMusic(normalBGM);
        Debug.Log("NormalBGM");
    }

    public void SetSFXVolume(float volume)
    {
        _sfxVolume = Mathf.Clamp01(volume);
        Settings.Instance.SFXVolumn = _sfxVolume;
        // Update the volume for all active and pooled audio sources
        foreach (var audioSource in activeAudioSources)
        {
            audioSource.volume = _sfxVolume;
        }

        foreach (var audioSource in audioSourcePool)
        {
            audioSource.volume = _sfxVolume;
        }
    }

    public void SetMusicVolume(float volume)
    {
        _musicVolume = Mathf.Clamp01(volume);
        Settings.Instance.BGMVolumn = _musicVolume;
        musicSource.volume = _musicVolume;
    }

    public float GetSFXVolume()
    {
        return _sfxVolume;
    }

    public float GetMusicVolume()
    {
        return _musicVolume;
    }
}
