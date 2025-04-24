using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueTargetControl : MonoBehaviour
{
    #region PROPERTIES

    public Transform WoolTransform;

    private Color _currentColor;
    private bool  _isActive;

    #endregion


    #region MAIN_METHODS

    public bool AddChild(Color color)
    {
        if (_isActive) return false;
        _currentColor = color;
        _isActive = true;
        return true;
    }

    public Color GetCurrentColor()
    {
        _isActive = false;
        return _currentColor;
    }
    #endregion
}