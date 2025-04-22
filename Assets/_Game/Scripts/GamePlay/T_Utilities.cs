using System;
using System.Collections.Generic;
using UnityEngine;

public static partial class T_Utitilies
{
    
}

[Serializable]
public class ComponentMesh
{
    public Stack<Color> ColorStack = new Stack<Color>();
    public MeshRenderer MeshRenderer;
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
    public List<ComponentMesh> ComponentMeshList = new List<ComponentMesh>();
}