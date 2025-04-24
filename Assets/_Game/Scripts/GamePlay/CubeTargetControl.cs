using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTargetControl : MonoBehaviour
{

    #region PROPERTIES

    public List<Transform> TargetChildren;

    private int _indexChild = 0;

    private const int TotalChild = 3;
    #endregion


    #region MAIN_METHODS

    public void AddChild()
    {
        _indexChild++;

        if (_indexChild + 1 == TotalChild)
        {
            GamePlaySystem.Instance.GenNewCube();
            _indexChild = 0;
        }
    }

    #endregion
}
