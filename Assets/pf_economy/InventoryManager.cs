using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Msg;
    [SerializeField] TextMeshProUGUI coinCount;
    bool ownedMusicPass;
    private void Awake()
    {
        GetPlayerInventory();
        GetVirtualCurrencies();
    }

    void UpdateCoinsText(string msg)
    {
        Debug.Log("Updating coin text");
        coinCount.text = msg;
    }

    void UpdateMsg(string msg) //to display in console and messagebox
    {
        Debug.Log(msg);
        Msg.text+=msg+'\n';
    }
    void OnError(PlayFabError e)
    {
        UpdateMsg(e.GenerateErrorReport());
    }
        public void LoadScene(string scn){
        UnityEngine.SceneManagement.SceneManager.LoadScene(scn);
    }
    public void GetVirtualCurrencies(){
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
            r =>
            {
                int coins = r.VirtualCurrency["CN"]; //replace CN with your currency
                UpdateCoinsText("Coins:" + coins);
            }, OnError);
    }
    
    public void GetCatalog(){
        var catreq=new GetCatalogItemsRequest{
            CatalogVersion="terranweapons" //update catalog name
        };
        PlayFabClientAPI.GetCatalogItems(catreq,
        result=>{
            List<CatalogItem> items=result.Catalog;
            UpdateMsg("Catalog Items");
            foreach(CatalogItem i in items){
                UpdateMsg(i.DisplayName+","+i.VirtualCurrencyPrices["CN"]); 
                //change "CN" to virtual currency type
            }
            },OnError);
    }

    public void GetPlayerInventory(){
        var UserInv=new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(UserInv,
        result=>{
            List<ItemInstance> ii=result.Inventory;
            UpdateMsg("Player Inventory");
            foreach(ItemInstance i in ii){
                UpdateMsg(i.DisplayName+","+i.ItemId+","+i.ItemInstanceId);

                if (i.ItemId == "musicPass")
                {
                    ownedMusicPass = true;
                }
            }
            },OnError);

    }

    public void BuyMusicPass(){
        
        if (InventoryCheck() == false)
        {
            var buyreq = new PurchaseItemRequest
            {
                //current sample is hardcoded, should make it more dynamic
                CatalogVersion = "terranweapons",
                ItemId = "musicPass", //replace with your item id
                VirtualCurrency = "CN",
                Price = 20
            };
            PlayFabClientAPI.PurchaseItem(buyreq,
                result => { UpdateMsg("Bought!"); },
                OnError);
        }
        else
        {
            UpdateMsg("Already owned!");
        }
    }

    public bool InventoryCheck()
    {

        var UserInv = new GetUserInventoryRequest();
        PlayFabClientAPI.GetUserInventory(UserInv,
        result => {
            List<ItemInstance> ii = result.Inventory;
            UpdateMsg("Player Inventory");
            foreach (ItemInstance i in ii)
            {
                if (i.ItemId == "musicPass")
                {
                    ownedMusicPass = true;
                    Debug.Log(ownedMusicPass);
                    break;
                }
            }
        }, OnError);

        return ownedMusicPass;

    }

    public void BackButton()
    {
        SceneManager.LoadScene("Menu");
    }

}
//Sell items (use cloud scripts)
//https://github.com/MicrosoftDocs/playfab-docs/blob/docs/playfab-docs/features/data/playerdata/player-inventory.md

