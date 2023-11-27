using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using TMPro;

public class ScoreboardHandle : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreboardText;
    [SerializeField] GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
        //scoreboardText.text = "Haiii";
    }

    private void Awake()
    {
        scoreboardText.enabled = false;
        //OnGetLeaderboard();
    }

    void UpdateScoreBoard(string msg) //to display in console and messagebox
    {

        Debug.Log(msg);
        scoreboardText.text =msg+'\n';
        scoreboardText.enabled = true;
    }

    public void gameIsOver()
    {
        OnSendLeaderboard();
    }

    public void OnSendLeaderboard()
    {
        var req = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>{ //playfab leaderboard statistic name
                new StatisticUpdate{
                    StatisticName="highscore",
                    Value=gameController.score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(req, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult r)
    {
        string LeaderboardStr = "Leaderboard\n";
        foreach (var item in r.Leaderboard)
        {
            string onerow = (item.Position + 1) + ". " + item.DisplayName + " - " + item.StatValue + "\n";
            LeaderboardStr += onerow; //combine all display into one string 1.
        }
        Debug.Log(LeaderboardStr);
        UpdateScoreBoard(LeaderboardStr);
    }


    public void OnGetLeaderboard()
    {
        var inv = new GetUserInventoryRequest
        {

        };
        var lbreq = new GetLeaderboardRequest
        {
            StatisticName = "highscore", //playfab leaderboard statistic name
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(lbreq, OnLeaderboardGet, OnError);
    }

    void OnError(PlayFabError e) //report any errors here!
    {
        Debug.Log("Error" + e.GenerateErrorReport());
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult r)
    {
        OnGetLeaderboard();
        Debug.Log("Successful leaderboard sent:" + r.ToString());
    }

}
