using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUI : MonoBehaviour
{

    #region PROPERTIES
    [Header("Currentcy")]
    public TMP_Text GoldCount;
    public Button TopUpGoldBtn;

    [Header("Setting")]
    public Button SettingBtn;
    public GameObject SettingUI;

    [Header("Target")] 
    public TMP_Text   TargetPass;
    public GameObject QueueTarget;
    public GameObject CurrentTarget;

    [Header("ZomButtonGroup")] 
    public Button ResetBtn;
    public Button ZomInBtn;
    public Button ZomOutBtn;
    
    [Header("Support")]
    public List<Image> SupportImage = new();
    public List<Button> SupportBuyBtn = new();
    public List<Button> SupportUsedBtn = new();

    private int    _totalTargetPassed = 0;
    private Camera _mainCamenra;
    #endregion
    
    #region UNITY_METHODS

    private void Awake()
    {
        _mainCamenra = Camera.main;
    }

    private void OnEnable()
    {
        Init();
        
        
        RegisterButton();
    }

    private void OnDisable()
    {
        
        UnRegisterButton();
    }

    #endregion

    #region MAIN_METHODS

    private void Init()
    {
        GoldCount.text = PlayerConfig.player.Gold.ToString();
    }

    private void StartLevel()
    {
        _totalTargetPassed = 0;
    }
    
    public void UpdateTarget(int targetPassed)
    {
        _totalTargetPassed += targetPassed;
        TargetPass.text = _totalTargetPassed.ToString();
        InitializeCubeTarget();
        UseQueueTarget();
    }

    private void InitializeCubeTarget()
    {
        
    }

    private void UseQueueTarget()
    {
        
    }
    #endregion

    #region HELPER
    private void RegisterButton()
    {
        TopUpGoldBtn.onClick.AddListener(OnTopUpGold);
        SettingBtn.onClick.AddListener(OnSettingButton);
        ZomInBtn.onClick.AddListener(OnZomInButton);
        ResetBtn.onClick.AddListener(OnResetButton);
        ZomOutBtn.onClick.AddListener(OnZomOutButton);
        for (int i = 0; i < SupportBuyBtn.Count; i++)
        {
            int index = i;
            SupportBuyBtn[i].onClick.AddListener(OnBuySupportButton);
            SupportUsedBtn[i].onClick.AddListener(OnSupportUsedButton);
        }
    }
    
    private void UnRegisterButton()
    {
        TopUpGoldBtn.onClick.RemoveListener(OnTopUpGold);
        SettingBtn.onClick.RemoveListener(OnSettingButton);
        ZomInBtn.onClick.RemoveListener(OnZomInButton);
        ResetBtn.onClick.RemoveListener(OnResetButton);
        ZomOutBtn.onClick.RemoveListener(OnZomOutButton);
        for (int i = 0; i < SupportBuyBtn.Count; i++)
        {
            SupportBuyBtn[i].onClick.RemoveListener(OnBuySupportButton);
            SupportUsedBtn[i].onClick.RemoveListener(OnSupportUsedButton);
        }
    }

    #endregion
    
    #region BUTTON_EVENTS
    
    private void OnTopUpGold()
    {
        
    }

    private void OnSettingButton()
    {
        SettingUI.SetActive(true);
        Time.timeScale = 0f;
    }
    
    private void OnResetButton()
    {
        
    }
    private void OnZomInButton()
    {
        
    }
    
    private void OnZomOutButton()
    {
        
    }
    
    private void OnSupportUsedButton()
    {
        
    }

    private void OnBuySupportButton()
    {
        
    }
    #endregion
}