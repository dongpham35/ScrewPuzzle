using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public static class PlayerConfig
{

    #region PROPERTIES

    public static Player player;

    #endregion


    #region MAIN_METHODS

    #endregion
}

[Serializable]
public class Player
{
    public string Name;
    public int    Level;
    public int    Gold;
    public int    Energy;
    public int    AvatarId;
    public int    FrameId;
    public Sprite AvatarSprite;
    public Sprite FrameSprite;
    
    public Player()
    {
        Name         = $"Hehe_{Random.Range(0, 100)}";
        Level        = 1;
        Gold         = 0;
        Energy       = 10;
        AvatarId     = 0;
        FrameId      = 0;
        AvatarSprite = null;
        FrameSprite  = null;
    }
    
    public Player(PlayerData playerData)
    {
        Level = playerData.Level;
        Gold = playerData.Gold;
        Energy = playerData.Energy;
        AvatarId = playerData.AvatarId;
        FrameId = playerData.FrameId;
        AvatarSprite = playerData.AvatarSprite;
        FrameSprite = playerData.FrameSprite;
    }
}

[Serializable]
public enum ItemType
{
    Avatar,
    Frame,
    Support
}