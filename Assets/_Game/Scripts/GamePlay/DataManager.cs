using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataManager
{
    public static Dictionary<int, ItemPropertySO> AvatarList  = new Dictionary<int, ItemPropertySO>();
    public static Dictionary<int, ItemPropertySO> FrameList   = new Dictionary<int, ItemPropertySO>();
    public static Dictionary<int, ItemPropertySO> SupportList = new Dictionary<int, ItemPropertySO>();
    
    public static Dictionary<int, LevelData> LevelList = new Dictionary<int, LevelData>();
}
