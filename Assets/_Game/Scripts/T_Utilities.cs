using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static partial  class T_Utilities
{
    #region MAIN_METHODS

    public static bool GetPlayerData(PlayerData playerData)
    {
        PlayerConfig.player = new Player(playerData);
        if (PlayerConfig.player == null)
        {
#if UNITY_EDITOR
            Debug.LogError("PlayerConfig.player is null");
#endif
            return false;
        }
        return true;
    }

    public static void CreateNewPlayer(PlayerData playerData)
    {
        PlayerConfig.player = new Player();
        PlayerPrefs.SetString(PlayerPrefKey.InitializedPlayer, "true");
        SavePlayerData(playerData);
    }

    public static void SavePlayerData(PlayerData playerData)
    {
        playerData.name     = PlayerConfig.player.Name;
        playerData.Level    = PlayerConfig.player.Level;
        playerData.Gold     = PlayerConfig.player.Gold;
        playerData.Energy   = PlayerConfig.player.Energy;
        playerData.AvatarId = PlayerConfig.player.AvatarId;
        playerData.FrameId  = PlayerConfig.player.FrameId;
        playerData.Energy   = DefaultValue.MaxEnergyDefault;
        if (DataManager.AvatarList.TryGetValue(playerData.AvatarId, out var avatarInfor))
        {
            playerData.AvatarSprite          = avatarInfor.Sprite;
            PlayerConfig.player.AvatarSprite = avatarInfor.Sprite;
        }
        if(DataManager.FrameList.TryGetValue(playerData.FrameId, out var frameInfor))
        {
            playerData.FrameSprite          = frameInfor.Sprite;
            PlayerConfig.player.FrameSprite = frameInfor.Sprite;
        }
    }
    
    public static async Task LoadAddressableData()
    {
        var handle = Addressables.LoadAssetsAsync<ItemPropertySO>( DefaultValue.AddressableItemLable, null);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            var soData = new List<ItemPropertySO>(handle.Result);
            foreach (var item in soData)
            {
                switch ((int)item.ItemType)
                {
                    case 0:
                    {
                        DataManager.AvatarList.Add(item.Id, item);
                        break;
                    }
                    case 1:
                    {
                        DataManager.FrameList.Add(item.Id, item);
                        break;
                    }
                    case 2:
                    {
                        DataManager.SupportList.Add(item.Id, item);
                        break;
                    }
                }
            }
        }
    }
    

    #endregion
}

public static class DefaultValue
{
    public const int MaxEnergyDefault = 10;
    
    public const float MaxZomInDefault = 5f;
    public const float MinZomInDefault = 1f;
    
    public const string AddressableItemLable = "ItemProperty";
}

public static class PlayerPrefKey
{
    public static string InitializedPlayer = "InitializedPlayer";
}

public static class AnimatorHashKey
{
    public const string IndexBottomHashKey = "IndexBottom";
}

[Serializable]
public enum MenuType
{
    Shop,
    Home,
    Rank
}