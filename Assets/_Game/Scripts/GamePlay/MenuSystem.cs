using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuSystem : MonoBehaviour
{
    
    #region PROPERTIES
    
    [Header("PlayerData")]
    public PlayerData playerData;
    
    [Header("Player Information UI")]
    public Image avatarImage;
    public TMP_Text GoldCount;
    public TMP_Text EnergyCount;

    [Header("Bottom UI")] 
    public TMP_Text ShopText;
    public TMP_Text HomeText;
    public TMP_Text RankText;
        
    #endregion

    #region UNITY_METHODS

    public void Awake()
    {
        LoadPlayerData();
        DisplayPlayerData();
    }

    #endregion


    #region MAIN_METHODS

    private void LoadPlayerData()
    {
        if (PlayerPrefs.HasKey(PlayerPrefKey.InitializedPlayer))
            T_Utilities.GetPlayerData(playerData);
        else
            T_Utilities.CreateNewPlayer(playerData);
    }
    
    private void DisplayPlayerData()
    {
        GoldCount.text = playerData.Gold.ToString();
        EnergyCount.text = playerData.Energy.ToString();
    }

    #endregion
}
