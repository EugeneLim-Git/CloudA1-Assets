using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayFabUserMgt : MonoBehaviour
{
    [SerializeField]InputField userEmail,userPassword,userName;
    [SerializeField]Text Msg;
    public void OnButtonRegUser(){ //for button click
        var registerRequest=new RegisterPlayFabUserRequest
        {
            Email=userEmail.text,
            Password=userPassword.text,
            Username=userName.text
        };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest,OnRegSuccess,OnError);   
    }
    void OnRegSuccess(RegisterPlayFabUserResult r){
        //Debug.Log("Register Success!");        
        UpdateMsg("Registration success!");

    }
    void OnError(PlayFabError e){
        //Debug.Log("Error"+e.GenerateErrorReport());
        UpdateMsg("Error"+e.GenerateErrorReport());
    }
    void UpdateMsg(string msg){ //to display in console and messagebox
        Debug.Log(msg);
        Msg.text=msg;
    }
    public void OnButtonLogin(){
        var loginRequest=new LoginWithEmailAddressRequest{
            Email=userEmail.text,
            Password=userPassword.text
        };
        PlayFabClientAPI.LoginWithEmailAddress(loginRequest,OnLoginSuccess,OnError);
    }
    void OnLoginSuccess(LoginResult r){
       UpdateMsg("Login Success!");
       SceneManager.LoadScene("GameScene");
    }
    public void OnButtonLogout(){
        PlayFabClientAPI.ForgetAllCredentials();
        Debug.Log("logged out");
        SceneManager.LoadScene("LoginScn");

    }
    public void PasswordResetRequest(){
        var req=new SendAccountRecoveryEmailRequest{
            Email=userEmail.text,
            TitleId=PlayFabSettings.TitleId //no need hardcode!
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(req,OnPasswordReset,OnError);

    
    }
        void OnPasswordReset(SendAccountRecoveryEmailResult r){
        Msg.text="Password reset email sent.";
    }
      public void OnButtonDeviceLogin(){ //login with device id
        var req0=new LoginWithCustomIDRequest{
            CustomId=SystemInfo.deviceUniqueIdentifier,CreateAccount=true,
            InfoRequestParameters=new GetPlayerCombinedInfoRequestParams{
                GetPlayerProfile=true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(req0,OnLoginSuccess,OnError);
    }
}

