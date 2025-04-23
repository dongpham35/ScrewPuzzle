using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class WoolControl : MonoBehaviour
{

    #region PROPERTIES
    [Header("Mesh Object")]
    public MeshObjectData MeshObjectData;
    public MeshRenderer MeshRenderer;

    public Animator Animator;
    
    public float WoolRotationTotalTime = 5f;
    public float WoolRotationSpeed = 1f;
    
    private MaterialPropertyBlock _materialPropertyBlock;

    private int _currentLayerIndex;
    #endregion


    #region UNITY_METHODS

    #endregion


    #region MAIN_METHODS
    public void InitMesh(MeshObjectData meshObjectDataData)
    {
        MeshObjectData         = meshObjectDataData;
        _currentLayerIndex     = 0;
        _materialPropertyBlock = new MaterialPropertyBlock();
        MeshRenderer.GetPropertyBlock(_materialPropertyBlock);
    }

    public void WoolRotation()
    {
        _ = AsyncWoolRotation();
    }

    public void SetColor(Color color)
    {
        MeshObjectData.ColorStack.Push(color);
        MeshObjectData.TotalLayer++;
        _currentLayerIndex++;
    }

    private async Task AsyncWoolRotation()
    {
        var timer = WoolRotationTotalTime;
        _materialPropertyBlock = new MaterialPropertyBlock();
        MeshRenderer.GetPropertyBlock(_materialPropertyBlock);
        while(timer > 0)
        {
            _materialPropertyBlock.SetFloat(ShaderPropertiesLib.Display, timer / WoolRotationTotalTime);
            MeshRenderer.SetPropertyBlock(_materialPropertyBlock);
            timer -= Time.deltaTime * WoolRotationSpeed;
        }
        // Animator.CrossFade(AnimatorHashKey.ShowHideWool, 0.1f);
        await Task.Yield();
    }
    #endregion
    
}
