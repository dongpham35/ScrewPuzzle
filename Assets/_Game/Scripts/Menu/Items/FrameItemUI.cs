using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrameItemUI : MonoBehaviour, IitemUI
{
    public Image FrameImage;
    public Image SelectedImage;
    
    public Button ClickButton;

    private void OnEnable()
    {
        ClickButton.onClick.AddListener(Selected);
    }
    
    private void OnDisable()
    {
        ClickButton.onClick.RemoveListener(Selected);
    }

    public ItemPropertySO Item { get; set; }

    public void Init(ItemPropertySO itemPropertySo)
    {
        FrameImage.sprite = itemPropertySo.Sprite;
        SelectedImage.sprite  = PlayerConfig.player.FrameSprite;
        Item = itemPropertySo;
    }

    public void DisSelected()
    {
        SelectedImage.gameObject.SetActive(false);
    }

    public void Selected()
    {
        SelectedImage.gameObject.SetActive(true);
        ProfileUISystem.Instance.ChangeFrame(Item.Id);
    }
}
