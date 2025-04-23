using System;
using System.Collections.Generic;
using UnityEngine;

public static partial class T_Utitilies
{
    
}

[Serializable]
public class MeshObjectData
{
    public int          TotalLayer;
    public Stack<Color> ColorStack = new Stack<Color>();
}

[Serializable]
public class CubeTargetData
{
    public int TargetFill;
    public Color TargetColor;
}

[Serializable]
public class SupportData
{
    public int    SupportId;
    public int    SupportPrice;
    public int    SupportLevelUnlock;
    public Sprite SupportSprite;
}

[Serializable]
public class LevelData
{
    public int LevelId;
    public int CurrentcyLevel;
    public List<MeshObjectData> MeshObjectList = new List<MeshObjectData>();
    public List<Color> ColorList = new List<Color>();
    public List<int> ColorCountList = new List<int>();
}