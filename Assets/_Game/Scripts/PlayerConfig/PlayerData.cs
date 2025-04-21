using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects/PlayerData", menuName = "ScriptableObjects/PlayerData")]
public class PlayerData :ScriptableObject
{
    public int    Level;
    public int    Gold;
    public int    Energy;
    public int    AvatarId;
    public int    FrameId;
    public Sprite AvatarSprite;
}
