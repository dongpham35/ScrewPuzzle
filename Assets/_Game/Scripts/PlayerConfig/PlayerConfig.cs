using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public int    Level;
    public int    Gold;
    public int    Energy;
    public int    AvatarId;
    public int    FrameId;
    public Sprite AvatarSprite;
    
    public Player()
    {
        Level        = 1;
        Gold         = 0;
        Energy       = 10;
        AvatarId     = 0;
        FrameId      = 0;
        AvatarSprite = null;
    }
    
    public Player(PlayerData playerData)
    {
        Level = playerData.Level;
        Gold = playerData.Gold;
        Energy = playerData.Energy;
        AvatarId = playerData.AvatarId;
        FrameId = playerData.FrameId;
        AvatarSprite = playerData.AvatarSprite;
    }
}

[Serializable]
public enum ItemType
{
    Avatar,
    Frame,
    Support
}