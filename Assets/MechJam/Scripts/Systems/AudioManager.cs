using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HelpURL("https://www.youtube.com/watch?v=QuXqyHpquLg&t=7s")]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("References")]
    [SerializeField] private AudioSource CellBlock;
    [SerializeField] private AudioSource CholesterolBlock;
    [SerializeField] private AudioSource RedBloodCell;
    [SerializeField] private AudioSource WhiteBloodCell;
    [SerializeField] private AudioSource A_Virus;
    [SerializeField] private AudioSource C_Virus;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource musicSource;

    private Dictionary<string, AudioSource> audioDict;
    private Dictionary<string, AudioClip[]> sfxDict;

    [Header("Music")]
    [SerializeField] private AudioClip[] musicList;

    [Header("SFX")]
    [SerializeField] private AudioClip[] CellBlockSFX;
    [SerializeField] private AudioClip[] CholesterolBlockSFX;
    [SerializeField] private AudioClip[] RedBloodCellSFX;
    [SerializeField] private AudioClip[] WhiteBloodCellSFX;
    [SerializeField] private AudioClip[] A_VirusSFX;
    [SerializeField] private AudioClip[] C_VirusSFX;

    [Header("ButtonSFX")]
    [SerializeField] private AudioClip[] buttonSfxList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioDict();
            InitializeSFXDict();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudioDict()
    {
        audioDict = new Dictionary<string, AudioSource>
        {
            { "CellBlock", CellBlock },
            { "CholesterolBlock", CholesterolBlock },
            { "RedBloodCell", RedBloodCell },
            { "WhiteBloodCell", WhiteBloodCell },
            { "A_Virus", A_Virus },
            { "C_Virus", C_Virus},
            { "General", audioSource }
        };
    }

    private void InitializeSFXDict()
    {
        sfxDict = new Dictionary<string, AudioClip[]>
        {
            { "CellBlock", CellBlockSFX },
            { "CholesterolBlock", CholesterolBlockSFX },
            { "RedBloodCell", RedBloodCellSFX },
            { "WhiteBloodCell", WhiteBloodCellSFX },
            { "A_Virus", A_VirusSFX },
            { "C_Virus", C_VirusSFX }
        };
    }

    public void PlayMusic(int musicNumber, float volume = 1)
    {
        musicSource.clip = musicList[musicNumber];
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void PlayRandomSFX(string audioKey, float volume = 1)
    {
        if (audioDict.TryGetValue(audioKey, out AudioSource source) && sfxDict.TryGetValue(audioKey, out AudioClip[] clips))
        {
            if (clips.Length == 0)
            {
                Debug.LogWarning($"No AudioClips found for key '{audioKey}'.");
                return;
            }

            int randomIndex = Random.Range(0, clips.Length);
            source.clip = clips[randomIndex];
            source.volume = volume;
            source.Play();
        }
        else
        {
            Debug.LogWarning($"AudioSource or AudioClips with key '{audioKey}' not found.");
        }
    }

    public void PlaySFX(string audioKey, int index)
    {
        if (audioDict.TryGetValue(audioKey, out AudioSource source) && sfxDict.TryGetValue(audioKey, out AudioClip[] clips))
        {
            if (clips.Length == 0)
            {
                Debug.LogWarning($"No AudioClips found for key '{audioKey}'.");
                return;
            }
            source.clip = clips[index];
            source.Play();
        }
        else
        {
            Debug.LogWarning($"AudioSource or AudioClips with key '{audioKey}' not found.");
        }
    }

    public void PlayButtonSFX(int buttonNumber, float volume = 1)
    {
        audioSource.clip = buttonSfxList[buttonNumber];
        audioSource.volume = volume;
        audioSource.Play();
    }
}