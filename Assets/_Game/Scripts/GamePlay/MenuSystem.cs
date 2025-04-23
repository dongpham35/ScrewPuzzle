using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSystem : Singleton<MenuSystem>
{
    
    #region PROPERTIES
    [Header("Animator")]
    public Animator BottomAnimator;
    
    [Header("PlayerData")]
    public PlayerData playerData;
    
    [Header("Player Information UI")]
    public Image frameImage;
    public Image avatarImage;
    public TMP_Text GoldCount;
    public TMP_Text EnergyCount;

    [Header("Bottom UI")] 
    public TMP_Text ShopText;
    public TMP_Text HomeText;
    public TMP_Text RankText;
    
    [Header("Btn Group")]
    public Button AvatarBtn;
    public Button SettingBtn;
    public Button ShopBtn;
    public Button HomeBtn;
    public Button RankBtn;
    public Button PlayBtn;
    
    [Header("UI")]
    public GameObject AvatarUI;
    public GameObject SettingUI;
    
    private MenuType _menuType;
        
    #endregion

    #region UNITY_METHODS

    private void OnEnable()
    {
        AvatarBtn.onClick.AddListener(ChangeInformationUI);
        SettingBtn.onClick.AddListener(ChangeSettingUI);
        ShopBtn.onClick.AddListener(ShopButton);
        HomeBtn.onClick.AddListener(HomeButton);
        RankBtn.onClick.AddListener(RankButton);
        PlayBtn.onClick.AddListener(PlayButton);
        ChangeMenu(MenuType.Home);
    }

    private async void Start()
    {
        await T_Utilities.LoadAddressableData();
        
        LoadPlayerData();         
        DisplayPlayerData();
    }

    private void OnDisable()
    {
        AvatarBtn.onClick.RemoveListener(ChangeInformationUI);
        SettingBtn.onClick.RemoveListener(ChangeSettingUI);
        ShopBtn.onClick.RemoveListener(ShopButton);
        HomeBtn.onClick.RemoveListener(HomeButton);
        RankBtn.onClick.RemoveListener(RankButton);
        PlayBtn.onClick.RemoveListener(PlayButton);
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
    
    public void DisplayPlayerData()
    {
        frameImage.sprite = PlayerConfig.player.FrameSprite;
        avatarImage.sprite = PlayerConfig.player.AvatarSprite;
        GoldCount.text = PlayerConfig.player.Gold.ToString();
        EnergyCount.text = PlayerConfig.player.Energy.ToString();
        
        T_Utilities.SavePlayerData(playerData);
    }

    public void ChangeMenu(MenuType menuType)
    {
        _menuType = menuType;
        BottomAnimator.SetInteger(AnimatorHashKey.IndexBottomHashKey, (int)menuType);
    }

    #endregion


    #region BUTTON_EVENT

    private void ChangeInformationUI()
    {
        AvatarUI.SetActive(true);
    }

    private void ChangeSettingUI()
    {
        SettingUI.SetActive(true);
    }
    
    private void ShopButton()
    {
        ChangeMenu(MenuType.Shop);
    }
    
    private void HomeButton()
    {
        ChangeMenu(MenuType.Home);
    }
    
    private void RankButton()
    {
        ChangeMenu(MenuType.Rank);
    }
    
    private void PlayButton()
    {
        SceneManager.LoadScene(1);
    }
    #endregion
}
