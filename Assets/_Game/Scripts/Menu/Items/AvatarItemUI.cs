using UnityEngine;
using UnityEngine.UI;

public class AvatarItemUI : MonoBehaviour, IitemUI
{
    public Image AvatarImage;
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

    public void Init(ItemPropertySO item)
    {
        AvatarImage.sprite = item.Sprite;
        SelectedImage.sprite  = PlayerConfig.player.FrameSprite;
        Item = item;
    }

    public void DisSelected()
    {
        SelectedImage.gameObject.SetActive(false);
    }

    public void Selected()
    {
        SelectedImage.gameObject.SetActive(true);
        ProfileUISystem.Instance.ChangeAvatar(Item.Id);
    }
    
}
