using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

public static class T_Utilities
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

    public static void CreateNewPlayer(PlayerData playerData, Sprite avatarSprite = null)
    {
        PlayerConfig.player = new Player();
        PlayerPrefs.SetString(PlayerPrefKey.InitializedPlayer, "true");
        playerData.Level        = PlayerConfig.player.Level;
        playerData.Gold         = PlayerConfig.player.Gold;
        playerData.Energy       = PlayerConfig.player.Energy;
        playerData.AvatarId     = PlayerConfig.player.AvatarId;
        playerData.FrameId      = PlayerConfig.player.FrameId;
        playerData.Energy       = DefaultValue.MaxEnergyDefault;
        playerData.AvatarSprite = avatarSprite;
    }

    public static void LoadAddressableData()
    {
        Addressables.LoadAssetsAsync<ItemProperty>("dataSO", so =>
        {
        }).Completed += handle =>
        {
            List<ItemProperty> allSO = new List<ItemProperty>(handle.Result);
            foreach (var item in allSO)
            {
                switch ((int)item.ItemType)
                {
                    case 0:
                    {
                        DataManager.AvatarList.Add(item);
                        break;
                    }
                    case 1:
                    {
                        DataManager.FrameList.Add(item);
                        break;
                    }
                    case 2:
                    {
                        DataManager.SupportList.Add(item);
                        break;
                    }
                }
            }
        };
    }

    #endregion
}

public static class DefaultValue
{
    public const int MaxEnergyDefault = 10; 
}

public static class PlayerPrefKey
{
    public static string InitializedPlayer = "InitializedPlayer";
}