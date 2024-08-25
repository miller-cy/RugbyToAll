using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// 该类用于管理游戏中的音频，处理背景音乐和音效播放。
/// </summary>
public class AudioManager : MonoBehaviour
{
    // 静态的AudioManager实例，用于确保游戏中只有一个AudioManager存在（单例模式）。
    public static AudioManager instance;
    

    [Header("Audio Mixer")]
    public AudioMixer mixer;

    // 在Unity的Inspector中，使用Header对字段进行分组，方便查看和管理。
    [Header("音频剪辑")]

    // 背景音乐剪辑。
    public AudioClip bgmClip;

    // 玩家跳跃时的音效剪辑。
    public AudioClip jumpClip;

    // 玩家长跳时的音效剪辑。
    public AudioClip longJumpClip;

    // 玩家死亡或游戏结束时的音效剪辑。
    public AudioClip deadClip;

    // 在Unity的Inspector中，使用Header对音频源进行分组。
    [Header("音频源")]

    // 用于播放背景音乐的AudioSource。
    public AudioSource bgmMusic;

    // 用于播放音效的AudioSource。
    public AudioSource fx;
    public bool isMusicPlaying = true;
    public bool isAudioOn = true;

    // Awake在脚本实例被加载时调用。
    // 该方法确保只有一个AudioManager实例存在，并且该实例在场景切换时不会被销毁。
    private void Awake()
    {
        // 如果当前没有实例，则将此对象设为唯一实例。
        if (instance == null)
            instance = this;
        else
            // 如果已经存在一个实例，销毁这个新的对象，以强制实现单例模式。
            Destroy(this.gameObject);

        // 保持当前的AudioManager对象在加载新场景时不被销毁。
        DontDestroyOnLoad(this);

        bgmMusic.enabled = true;
        bgmMusic.clip = bgmClip;
        
        PlayMusic();
        if (AudioManager.instance.isMusicPlaying)
       {
        AudioManager.instance.PlayMusic();
       }
    }

    private void OnEnable()
    {
        // 订阅游戏结束事件，确保事件发生时调用 OnGameOverEvent 方法
        EventHandler.GameOverEvent += OnGameOverEvent;
    }

    private void OnDisable()
    {
        // 取消订阅游戏结束事件，避免内存泄漏或多次订阅
        EventHandler.GameOverEvent -= OnGameOverEvent;
    }

    // 事件处理方法，当游戏结束时会调用此方法
    private void OnGameOverEvent()
    {   
        fx.clip = deadClip;
        fx.Play();
    }

    /// <summary>
    /// 设置跳跃音效剪辑
    /// </summary>
    /// <param name="type">0: 小跳 1: 大跳</param>
    public void SetJumpClip(int type)
    {
        // 根据type的值选择不同的跳跃音效
        switch (type)
        {
            // 当type为0时，选择小跳的音效
            case 0:
                fx.clip = jumpClip;
                break;
            // 当type为1时，选择大跳的音效
            case 1:
                fx.clip = longJumpClip;
                break;
        }
    }

    /// <summary>
    /// 播放跳跃音效
    /// </summary>
    public void PlayJumpFX()
    {
        // 播放当前设置的跳跃音效
        fx.Play();
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    public void PlayMusic()
    {
        // 如果背景音乐没有在播放中，则播放背景音乐
        if (!bgmMusic.isPlaying)
        {
            bgmMusic.Play();
        }
    }

    public void ToggleAudio(bool isOn)
    {   isAudioOn = isOn;
        if (isOn)
        {
            mixer.SetFloat("masterVolume", 0);
        }
        else
        {
            mixer.SetFloat("masterVolume", -80);
        }
    }
    
   
}