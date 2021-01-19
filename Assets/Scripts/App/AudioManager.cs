using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    // Singleton
    private static AudioManager current;
    public static AudioManager Instance { get => current; }
    // --------------------------------------------------------

    // Audio Clip
    [Header("Music")]
    public AudioClip musicClip; // Main Background Clip
    public List<AudioClip> mapBackgroundClips; // Background clip for each map

    [Header("Player")]
    public AudioClip punchClip; // Lightning Clip (Maybe correct in this current scenario)

    [Header("Voice")]
    public AudioClip jumpVoiceClip;
    public AudioClip deadVoiceClip;

    // Mix Audio Source
    [Header("Mixer Group")]
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup playerGroup;
    public AudioMixerGroup voiceGroup;

    // Audio Source 
    private AudioSource musicSource;
    private AudioSource playerSource;
    private AudioSource voiceSource;

    private void Awake()
    {
        // Singleton Pattern 
        if(current != null && current != this)
        {
            Destroy(this);
            return;
        }
        current = this;
        DontDestroyOnLoad(this);

        //Generate the Audio Source "channels" for our game's audio
        musicSource = gameObject.AddComponent<AudioSource>();
        playerSource = gameObject.AddComponent<AudioSource>();
        voiceSource = gameObject.AddComponent<AudioSource>();

        //Assign each audio source to its respective mixer group so that it is
        //routed and controlled by the audio mixer
        musicSource.outputAudioMixerGroup = musicGroup;
        playerSource.outputAudioMixerGroup = playerGroup;
        voiceSource.outputAudioMixerGroup = voiceGroup;

        // Load Sound From File
        LoadSoundSettingFromFile();

        // Play Start Game Music
        PlayStartGameMusic();
    }

    public void PlayStartGameMusic()
    {
        musicSource.clip = musicClip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlayMapBackgroundMusic()
    {
        if (mapBackgroundClips.Count <= 0)
            return;
        int randomIndex = UnityEngine.Random.Range(0, mapBackgroundClips.Count - 1);
        musicSource.clip = mapBackgroundClips[randomIndex];
        musicSource.loop = true;
        musicSource.Play();
    }

    public void LoadSoundSettingFromFile()
    {
        Debug.Log("Load Sound Setting From File");
    }
}