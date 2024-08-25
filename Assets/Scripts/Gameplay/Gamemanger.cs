using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool isGlobal;
    public List<int> scoreList; // 用于存储排行榜分数
    private int score; // 当前分数

    private string dataPath; // 数据保存路径

    // 在 Awake 中初始化单例模式并读取分数数据
    private void Awake()
    {
        // 设置数据路径为持久化数据路径
        dataPath = Application.persistentDataPath + "/leaderboard.json";
        //scoreList = GetScorelistData();

        // 实现单例模式
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        DontDestroyOnLoad(this);
    }

    // 在 Enable 时注册事件监听器
    private void OnEnable()
    {
        EventHandler.GameOverEvent += OnGameOverEvent;
        EventHandler.GetPointEvent += OnGetPointEvent;
    }

    // 在 Disable 时移除事件监听器
    private void OnDisable()
    {
        EventHandler.GameOverEvent -= OnGameOverEvent;
        EventHandler.GetPointEvent -= OnGetPointEvent;
    }
    private void Start()
    {
        scoreList = GetScorelistData();
    }

    // 获取得分时的回调函数
    private void OnGetPointEvent(int point)
    {
        score = point;
    }

    // 游戏结束时的回调函数
    private void OnGameOverEvent()
    {
        if (!isGlobal )
        {
            // 如果列表中没有当前分数，则添加到排行榜
            if (!scoreList.Contains(score))
            {
                scoreList.Add(score);
            }
    
        
    
            // 排序并倒序显示分数列表
            scoreList.Sort();
            scoreList.Reverse();
    
            // 保存分数列表到文件
            File.WriteAllText(dataPath, JsonConvert.SerializeObject(scoreList));

        }
        else    
        //send to Playerfab
             PlayfabManager.instance.SendLeaderboard(score);
    }

    /// <summary>
    /// 读取保存的分数列表数据
    /// </summary>
    /// <returns>返回分数列表</returns>
    public List<int> GetScorelistData()
    {   
        if(!isGlobal )
        {
        // 检查文件是否存在，如果存在则读取并反序列化
           if (File.Exists(dataPath))
           {
            string jsonData = File.ReadAllText(dataPath);
            return JsonConvert.DeserializeObject<List<int>>(jsonData);
           }
           return new List<int>();
        }
        else
        {  
           return PlayfabManager.instance.scoreList;
          

        }    
           
    }
}
