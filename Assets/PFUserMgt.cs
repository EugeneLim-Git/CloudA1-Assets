using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

public class PFUserMgt : MonoBehaviour
{
    [SerializeField] TMP_Text msgbox;
    [SerializeField] TMP_InputField if_username, if_email, if_password;

    public void OnButtonRegUser() // for button click
    {
        var regReq = new RegisterPlayFabUserRequest // Creates request object
        {
            Email = if_email.text,
            Password = if_password.text,
            Username = if_username.text
        };
        //Executes request by calling the playfab API
        PlayFabClientAPI.RegisterPlayFabUser(regReq, OnRegSucc, OnError);
    }

    public void OnButtonLogin()
    {
        var loginReq = new LoginWithEmailAddressRequest // Creates request object
        {
            Email = if_email.text,
            Password = if_password.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(loginReq, OnLoginSucc, OnError);
    }

    void OnRegSucc(RegisterPlayFabUserResult r) // function to handle success
    {
        msgbox.text = "Register Success! " + r.PlayFabId;
    }
    
    void OnLoginSucc(LoginResult r)
    {
        msgbox.text = "Login Success! " + r.PlayFabId;
    }

    void OnError(PlayFabError e) // function to handle error
    {
        msgbox.text = "Error" + e.GenerateErrorReport();
    }
}
