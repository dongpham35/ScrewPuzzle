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
    public MeshRenderer TopMeshRenderer;
    public MeshRenderer HideMeshRenderer;
    public Collider  BoxCollider;
        
    public Animator Animator;
    
    public float WoolRotationTotalTime = 5f;
    public float WoolRotationSpeed = 1f;
    
    private MaterialPropertyBlock _topMaterialPropertyBlock;
    private MaterialPropertyBlock _hideMaterialPropertyBlock;

    private Color _currentColor;
    #endregion

    
    


    #region MAIN_METHODS
    public void InitMesh(MeshObjectData meshObjectDataData)
    {
        BoxCollider.enabled        = true;
        MeshObjectData             = meshObjectDataData;
        _currentColor              = meshObjectDataData.HightestColor;
        _topMaterialPropertyBlock  = new MaterialPropertyBlock();
        _hideMaterialPropertyBlock = new MaterialPropertyBlock();
        TopMeshRenderer.GetPropertyBlock(_topMaterialPropertyBlock);
        HideMeshRenderer.GetPropertyBlock(_hideMaterialPropertyBlock);
    }

    public void WoolRotation()
    {
        _ = AsyncWoolRotation();
    }

    public void SetColor(Color color)
    {
        MeshObjectData.ColorStack.Push(color);
        MeshObjectData.TotalLayer++;
    }

    private async Task AsyncWoolRotation()
    {
        GamePlaySystem.Instance.OnClickMesh(_currentColor);
        Color nextColor = Color.black;
        if (MeshObjectData.ColorStack.Count != 0)
        {
            nextColor = MeshObjectData.ColorStack.Pop();
            _hideMaterialPropertyBlock.SetColor(ShaderPropertiesLib.Color, nextColor);
            _hideMaterialPropertyBlock.SetFloat(ShaderPropertiesLib.Display, 1);
        }else _hideMaterialPropertyBlock.SetFloat(ShaderPropertiesLib.Display, 0);
        HideMeshRenderer.SetPropertyBlock(_hideMaterialPropertyBlock);
        var timer = WoolRotationTotalTime;
        while(timer > 0)
        {
            _topMaterialPropertyBlock.SetFloat(ShaderPropertiesLib.Display, timer / WoolRotationTotalTime);
            TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);
            timer -= Time.deltaTime * WoolRotationSpeed;
        }
        // Animator.CrossFade(AnimatorHashKey.ShowHideWool, 0.1f);
        if (nextColor != Color.black)
        {
            _topMaterialPropertyBlock.SetColor(ShaderPropertiesLib.Color, nextColor);
            _topMaterialPropertyBlock.SetFloat(ShaderPropertiesLib.Display, 1);
            _currentColor = nextColor;
        }else BoxCollider.enabled = false;
        TopMeshRenderer.SetPropertyBlock(_topMaterialPropertyBlock);
        await Task.Yield();
    }
    #endregion
    
}
