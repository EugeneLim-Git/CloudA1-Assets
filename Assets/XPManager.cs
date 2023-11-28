using PlayFab.ClientModels;
using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;
using TMPro;
using UnityEditor.PackageManager;

public class XPManager : MonoBehaviour
{
    [SerializeField] GameController gameController;
    [SerializeField] TMP_Text XPDisplay;
    [SerializeField] TMP_Text LevelDisplay;
    private int playerXPAmount;
    private int playerLevel;

    private void Awake()
    {
        GetUserData();
    }

    public void SetUserData()
    {
        playerXPAmount += gameController.score;
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
                {"XP", (playerXPAmount.ToString())}
            }
        },
        result => Debug.Log("Successfully updated user data"),
        error => {
            Debug.Log("Got error setting user data XP");
            Debug.Log(error.GenerateErrorReport());
        });

        XPDisplay.text = "XP: " + playerXPAmount;

        playerLevel = playerXPAmount / 10;

        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
                {"Level", (playerLevel.ToString())}
            }
        },
        result => Debug.Log("Successfully updated user data"),
        error => {
            Debug.Log("Got error setting user data Level");
            Debug.Log(error.GenerateErrorReport());
        });

        var currencyUpdate = new AddUserVirtualCurrencyRequest
        {
            VirtualCurrency = "CN",
            Amount = gameController.score

        };
        PlayFabClientAPI.AddUserVirtualCurrency(currencyUpdate, OnCurrencyUpdate, OnError);



    }
    public void GetUserData(/*string myPlayFabId*/)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            // PlayFabId = myPlayFabId,
            // Keys = null
        }, result => {
            Debug.Log("Got user data:");
            if (result.Data == null || !result.Data.ContainsKey("XP")) Debug.Log("No XP");
            else
            {
                Debug.Log("XP: " + result.Data["XP"].Value);
                XPDisplay.text = "XP: " + result.Data["XP"].Value;
                playerXPAmount = int.Parse(result.Data["XP"].Value);

            }
            if (result.Data == null || !result.Data.ContainsKey("Level")) Debug.Log("No Level");
            else
            {
                Debug.Log("Level: " + result.Data["Level"].Value);
                LevelDisplay.text = "Level: " + result.Data["Level"].Value;
                playerLevel = playerXPAmount / 10;

            }
        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });


    }

    void OnCurrencyUpdate(ModifyUserVirtualCurrencyResult r)
    {
        Debug.Log("Successful currency update!: " + r.ToString());
    }

    void OnError(PlayFabError e) //report any errors here!
    {
        Debug.Log("Error" + e.GenerateErrorReport());
    }
}
