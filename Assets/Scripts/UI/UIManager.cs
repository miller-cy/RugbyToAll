using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

// UIManager类继承自MonoBehaviour，用于管理UI相关的功能
public class UIManager : MonoBehaviour
{
    // 公开的Text对象，用于显示分数
    public Text scoreText;
    public GameObject  gameOverPanel;

    public GameObject leaderboardPanel;
    // 当脚本启用时，注册OnGetPointEvent方法到GetPointEvent事件
    private void OnEnable()
    {   Time.timeScale = 1;
        EventHandler.GetPointEvent += OnGetPointEvent;
        EventHandler.GameOverEvent += OnGameOverEvent;
    }


    // 当获取分数事件触发时，调用此方法，参数是当前分数


    // 当脚本禁用时，从GetPointEvent事件中移除OnGetPointEvent方法
    private void OnDisable()
    {
        EventHandler.GetPointEvent -= OnGetPointEvent;
        EventHandler.GameOverEvent -= OnGameOverEvent;
    }

   

    // Start方法在脚本启动时调用，初始化分数文本为 "00"
    private void Start()
    {
        scoreText.text = "00";
    }
      private void OnGetPointEvent(int point)
    {
        // 在此处可以处理传递进来的分数
        scoreText.text = point.ToString (); 
    }
     private void OnGameOverEvent()
    {
        gameOverPanel.SetActive(true);
        
        if (gameOverPanel .activeInHierarchy)
        {
           //暂停
            Time.timeScale = 0;
        }
    }
    public void RestartGame()
    {  
        gameOverPanel.SetActive(false);
        TransitionManager.instance .Transition ("Gameplay");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
 
    public void BackToMenu()
{
    // 隐藏游戏结束面板
    gameOverPanel.SetActive(false);

    // 保存当前的音频状态
    AudioManager.instance.isAudioOn = AudioManager.instance.bgmMusic.isPlaying;

    // 调试输出当前音频状态
    Debug.Log("BackToMenu: AudioManager isAudioOn set to " + AudioManager.instance.isAudioOn);
    Debug.Log("BackToMenu: bgmMusic isPlaying = " + AudioManager.instance.bgmMusic.isPlaying);

    // 切换到标题屏幕
    TransitionManager.instance.Transition("Title");
    Debug.Log("BackToMenu: Transition to Title screen initiated.");

    // 在标题屏幕初始化背景音乐
    InitializeBackgroundMusic();
}

private void InitializeBackgroundMusic()
{
    // 确保AudioManager实例存在
    if (AudioManager.instance != null)
    {
        // 根据之前保存的音频状态重新设置背景音乐
        AudioManager.instance.bgmMusic.clip = AudioManager.instance.bgmClip;
        if (AudioManager.instance.isMusicPlaying)
        {
            AudioManager.instance.PlayMusic();
        }
        else
        {
            AudioManager.instance.ToggleAudio(false); // 根据需要设置音频开关状态
        }

        // 调试输出背景音乐初始化状态
        Debug.Log("InitializeBackgroundMusic: bgmMusic initialized and playback state set.");
    }
}


    public void OpenLeaderBoard()
    {
        leaderboardPanel.SetActive (true);
    }
}