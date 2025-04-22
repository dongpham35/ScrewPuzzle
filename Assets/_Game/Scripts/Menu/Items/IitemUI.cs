using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IitemUI
{
    public ItemPropertySO Item { set; }
    void Init(ItemPropertySO itemPropertySo);
    void DisSelected();
    void Selected();
}
