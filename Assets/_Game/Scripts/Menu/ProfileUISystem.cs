using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileUISystem : Singleton<ProfileUISystem>
{
    #region PROPERTIES
    [Header("Header")]
    public Image AvatarImage;
    public Image FrameImage;
    public TMP_Text  NameText;
    
    [Header("Buttons")]
    public Button TurnOffButton;
    public Button AvatarButton;
    public Button FrameButton;
    public Button SaveButton;
    public Image AvatarButtonImage;
    public Image FrameButtonImage;

    [Header("Content")]
    public GameObject ContentParent;
    public GameObject AvatarObj;
    public GameObject FrameObj;
    
    private List<AvatarItemUI> AvatarList = new List<AvatarItemUI>();
    private List<FrameItemUI>  FrameList  = new List<FrameItemUI>();
    private bool               _isActiveAvatar;

    private bool _isInit             = false;
    private int  _currentIndexAvatar = 0;
    private int  _currentIndexFrame  = 0;
    
    #endregion

    #region UNITY_METHODS
    private void OnEnable()
    {
        Init();
        
        TurnOffButton.onClick.AddListener(TurnOff);
        AvatarButton.onClick.AddListener(OnAvatarButtonClick);
        FrameButton.onClick.AddListener(OnFrameButtonClick);
        SaveButton.onClick.AddListener(OnSaveButtonClick);
    }

    private void OnDisable()
    {
        TurnOffButton.onClick.RemoveListener(TurnOff);
        AvatarButton.onClick.RemoveListener(OnAvatarButtonClick);
        FrameButton.onClick.RemoveListener(OnFrameButtonClick);
        SaveButton.onClick.RemoveListener(OnSaveButtonClick);
        AvatarList[_currentIndexAvatar].DisSelected();
        FrameList[_currentIndexFrame].DisSelected();
        if(!_isActiveAvatar)
            (AvatarButtonImage.color, FrameButtonImage.color) = (FrameButtonImage.color, AvatarButtonImage.color);
    }

    #endregion


    #region MAIN_METHODS

    private void Init()
    {
        NameText.text       = PlayerConfig.player.Name;
        AvatarImage.sprite  = PlayerConfig.player.AvatarSprite;
        FrameImage.sprite   = PlayerConfig.player.FrameSprite;
        _currentIndexAvatar = PlayerConfig.player.AvatarId;
        _currentIndexFrame  = PlayerConfig.player.FrameId;
        LoadAvatar(true);
        OnLoadFrame(false);
        _isInit             = true;
        _isActiveAvatar = true;
    }
    private void LoadAvatar(bool active)
    {
        for(int i = 0, count = DataManager.AvatarList.Count; i < count; i++)
        {
            if (_isInit)
            {
                AvatarList[i].gameObject.SetActive(active);
                if(i == PlayerConfig.player.AvatarId)
                    AvatarList[i].Selected();
                continue;
            }
            if(!DataManager.AvatarList.TryGetValue(i, out var avatarItem)) continue;
            var avatarObj    = Instantiate(AvatarObj, ContentParent.transform);
            var avatarItemUI = avatarObj.GetComponent<AvatarItemUI>();
            avatarItemUI.Init(avatarItem);
            avatarObj.transform.SetParent(ContentParent.transform);
            avatarObj.SetActive(active);
            AvatarList.Add(avatarItemUI);
            if(i == PlayerConfig.player.AvatarId) avatarItemUI.Selected();
        }
        _currentIndexAvatar = PlayerConfig.player.AvatarId;
    }
    
    private void OnLoadFrame(bool active)
    {
        for(int i = 0, count = DataManager.FrameList.Count; i < count; i++)
        {
            if (_isInit)
            {
                FrameList[i].gameObject.SetActive(active);
                if(i == PlayerConfig.player.FrameId)
                    FrameList[i].Selected();
                continue;
            }
            if(!DataManager.FrameList.TryGetValue(i, out var frameItem)) continue;
            var frameObj    = Instantiate(FrameObj, ContentParent.transform);
            var frameItemUI = frameObj.GetComponent<FrameItemUI>();
            frameItemUI.Init(frameItem);
            frameObj.transform.SetParent(ContentParent.transform);
            frameObj.SetActive(active);
            FrameList.Add(frameItemUI);
            if(i == PlayerConfig.player.FrameId) frameItemUI.Selected();
        }
        _currentIndexFrame = PlayerConfig.player.FrameId;
    }

    public void ChangeAvatar(int avatarId)
    {
        if(!DataManager.AvatarList.TryGetValue(avatarId, out var avatarItem)) return;
        if(avatarId == _currentIndexAvatar) return;
        var avatarItemUI = AvatarList[_currentIndexAvatar];
        avatarItemUI.DisSelected();
        AvatarImage.sprite = avatarItem.Sprite;
        _currentIndexAvatar = avatarId;
    }
    
    public void ChangeFrame(int frameId)
    {
        if(!DataManager.FrameList.TryGetValue(frameId, out var frameItem)) return;
        if(frameId == _currentIndexFrame) return;
        var frameItemUI = FrameList[_currentIndexFrame];
        frameItemUI.DisSelected();
        FrameImage.sprite = frameItem.Sprite;
        _currentIndexFrame = frameId;
    }
    #endregion


    #region BUTTON_EVENTS

    private void TurnOff()
    {
        gameObject.SetActive(false);
    }
    
    private void OnAvatarButtonClick()
    {
        if(_isActiveAvatar) return;
        LoadAvatar(true);
        OnLoadFrame(false);
        (AvatarButtonImage.color, FrameButtonImage.color) = (FrameButtonImage.color, AvatarButtonImage.color);
        _isActiveAvatar                                   = true;
    }
    
    private void OnFrameButtonClick()
    {
        if(!_isActiveAvatar) return;
        LoadAvatar(false);
        OnLoadFrame(true);
        (AvatarButtonImage.color, FrameButtonImage.color) = (FrameButtonImage.color, AvatarButtonImage.color);
        _isActiveAvatar                                   = false;
    }
    
    private void OnSaveButtonClick()
    {
        PlayerConfig.player.AvatarSprite = AvatarImage.sprite;
        PlayerConfig.player.FrameSprite  = FrameImage.sprite;
        PlayerConfig.player.Name         = NameText.text;
        PlayerConfig.player.AvatarId     = _currentIndexAvatar;
        PlayerConfig.player.FrameId      = _currentIndexFrame;
        MenuSystem.Instance.DisplayPlayerData();
    }
   
    #endregion
}
