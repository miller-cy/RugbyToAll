using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{
    public  static PlayfabManager instance;
    public string playerName;

    public List<int> scoreList;
    public List<string> nameList;
 

    private void Awake()
    {
        if (instance == null)
        
            instance = this;

       
         
        else
        
            
            Destroy(this.gameObject);

        DontDestroyOnLoad(this);
    
        Login();
    }
    #region 登入信息
    private void Login()
    {
       // var request = new LoginWithEmailAddressRequest()
   // {
      //  Email = "user@example.com", // 替换为用户的电子邮件地址
       // Password = "userPassword",  // 替换为用户的密码
       // TitleId = "YourTitleId"     // 替换为你的PlayFab Title ID
   // };

        // PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    //}

    //private void OnLoginSuccess(LoginResult result)
    //{
    //Debug.Log("登录成功！");
    // 处理登录成功后的逻辑
   // }



        var request = new LoginWithCustomIDRequest();
        request.CustomId = SystemInfo.deviceUniqueIdentifier;
        request.CreateAccount = true;
        request .InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
        {
            GetPlayerProfile = true,
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }  

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("登录成功");
        if (result .InfoResultPayload.PlayerProfile != null)
        {
          playerName = result.InfoResultPayload.PlayerProfile.DisplayName;
          Debug.Log("玩家昵称：" + playerName);
        }
    }
    #endregion
     private void OnError(PlayFabError error)
    {
        Debug.Log("登录失败：");
        Debug.Log(error.GenerateErrorReport());
    }   
    
    
    
    public void SendLeaderboard(int score)
    {   var request = new UpdatePlayerStatisticsRequest();
        request.Statistics = new List<StatisticUpdate> 
        {    
         new StatisticUpdate
           {
               StatisticName ="RugbyToAll(HighScores)",
               Value = score
           }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }
    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("排行榜更新成功！");
        GetLeaderboardData ();
    }
    public void GetLeaderboardData()
    {
        var request = new GetLeaderboardRequest();
        request.StatisticName = "RugbyToAll(HighScores)";
        request.StartPosition = 0;
        request.MaxResultsCount = 7;

        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboard, OnError);
    }    

    
    private void OnGetLeaderboard(GetLeaderboardResult result)
    {
        scoreList = new List<int>();
        nameList = new List<string>();

        foreach (var item in result .Leaderboard)
        {
          //Debug.Log(item.Position + "  " +item .DisplayName+"   "+item.StatValue);
          scoreList .Add(item.StatValue);
          nameList.Add(item.DisplayName);   
        }
    }
     

   public void SubmitName(string name)
   { 
       if (string.IsNullOrEmpty(name))
       {
        Debug.LogError("昵称不能为空");
        return;
       }
       var request = new UpdateUserTitleDisplayNameRequest
       {   
           
           DisplayName = name,
           
       };
       
       PlayFabClientAPI.UpdateUserTitleDisplayName(request,OnDisplayNameUpdate, OnApiError);


   }
   private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
   {
    playerName = result.DisplayName;
    Debug.Log("昵称更新成功！");
   }

    private void OnApiError(PlayFabError error)
    {
        Debug.Log("api 登录失败：");
        Debug.Log(error.GenerateErrorReport());
    }   
  
}

    
