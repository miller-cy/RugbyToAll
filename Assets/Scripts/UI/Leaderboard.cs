// 引入必要的库
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 定义一个排行榜类，继承自 MonoBehaviour
public class Leaderboard : MonoBehaviour
{
    // 这是一个用来存储分数记录的列表
    public List<ScoreRecord> scoreRecords;

    // 用来存储整数分数的列表
    private List<int> scoreList;

    private List<string> nameList;

    // 当这个脚本启用时，会自动调用这个方法
    private void OnEnable()
    {
        // 从游戏管理器获取分数列表并保存到 scoreList 里
        scoreList = GameManager.instance.GetScorelistData();
        PlayfabManager.instance.GetLeaderboardData();
        SetLeaderboardData();

    }

    // 在游戏开始时会调用这个方法
    //private void Start()
    //{
        // 设置排行榜数据
       // SetLeaderboardData();
   // }

    // 这是一个用来设置排行榜数据的方法
    public void SetLeaderboardData()
    {   
        nameList = PlayfabManager.instance.nameList;
        // 遍历每个分数记录
        for (int i = 0; i < scoreRecords.Count; i++)
        {
            // 如果分数列表中的数据够用
            if (i < scoreList.Count)
            {
                
                scoreRecords[i].SetScoreText(scoreList[i]);
                
                 

                scoreRecords[i].SetName(nameList[i]);
                // 显示这个分数记录
                scoreRecords[i].gameObject.SetActive(true);
            }
            else
            {
                // 如果没有足够的分数，隐藏多余的分数记录
                scoreRecords[i].gameObject.SetActive(false);
            }
        }
    }
}