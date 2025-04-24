using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTargetControl : MonoBehaviour
{

    #region PROPERTIES

    public List<Transform> TargetChildren;
    public GameObject      UnLock;

    private int _indexChild = 0;
    private Color _currentColor;

    private const int TotalChild = 3;
    #endregion


    #region MAIN_METHODS

    public void AddChild(int indexCube)
    {
        _indexChild++;
        
        if (_indexChild + 1 == TotalChild)
        {
            GamePlaySystem.Instance.GenNewCube(indexCube);
            _indexChild = 0;
        }
    }

    public void SetActiveCubeTarget(bool active)
    {
        UnLock.SetActive(active);
    }

    public bool CheckColor(Color color)
    {
        return color == _currentColor;
    }

    #endregion
}
