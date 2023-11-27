using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Msg;
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
                UpdateMsg("Coins:" + coins);
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
            }
            },OnError);

    }

    public void BuyItem(){
        var buyreq=new PurchaseItemRequest{
            //current sample is hardcoded, should make it more dynamic
            CatalogVersion="terranweapons", 
            ItemId="Weap01LC", //replace with your item id
            VirtualCurrency="CN",
            Price=2
        };
        PlayFabClientAPI.PurchaseItem(buyreq,
            result=>{UpdateMsg("Bought!");},
            OnError);

    }
}
//Sell items (use cloud scripts)
//https://github.com/MicrosoftDocs/playfab-docs/blob/docs/playfab-docs/features/data/playerdata/player-inventory.md

