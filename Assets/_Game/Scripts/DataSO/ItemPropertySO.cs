using System;
using System.Collections;
using System.Collections.Generic;using UnityEditor.Build.Content;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObjects/ItemPropery", menuName = "ScriptableObjects/ItemPropery")]
public class ItemPropertySO : ScriptableObject
{
    public int      Id;
    public Sprite   Sprite;
    public ItemType ItemType;
    public int      Price;
}
