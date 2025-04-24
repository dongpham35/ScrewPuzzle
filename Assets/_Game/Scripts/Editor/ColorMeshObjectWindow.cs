#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class ColorMeshObjectWindow : EditorWindow
{
    private static GameObject _selectedObject;
    private        WoolControl _woolControl;
    
    private        int        _totalLayer;
    private        Color      _highestColor;
    private        bool       _onvalueChange;
    
    private MaterialPropertyBlock _materialPropertyBlock;
    
    
    [MenuItem("Tools/ColorMeshObject")]
    public static void ShowWindow()
    {
        GetWindow<ColorMeshObjectWindow>("Color Mesh Object");
    }
    
    private void OnEnable()
    {
        _onvalueChange =  false;
        _materialPropertyBlock = new MaterialPropertyBlock();
        Selection.selectionChanged += Repaint;
    }
    
    private void OnDisable()
    {
        Selection.selectionChanged -= Repaint;
    }

    private void OnGUI()
    {
        GUILayout.Label("Color Mesh Object", EditorStyles.boldLabel);
        var newSelectionObject = Selection.activeGameObject;
        if(newSelectionObject == null) return;

        if (_selectedObject != newSelectionObject)
        {
            _selectedObject = newSelectionObject;
            _onvalueChange  = false;
        }

        if (!_onvalueChange && newSelectionObject.TryGetComponent(out _woolControl))
        {
            _onvalueChange = true;
            _totalLayer    = _woolControl.MeshObjectData.TotalLayer;
            _highestColor  = _woolControl.MeshObjectData.HightestColor;
        }
        else if (_woolControl == null)
        {
            _onvalueChange = false;
            return;
        }
        
        EditorGUILayout.LabelField("Name", _woolControl.name);
        
         _totalLayer   = EditorGUILayout.IntField("Total Layer", _totalLayer);
         _highestColor = EditorGUILayout.ColorField("Highest Color",_highestColor);
         
         _woolControl.TopMeshRenderer.sharedMaterial.SetColor(ShaderPropertiesLib.Color, _highestColor);
        if (GUILayout.Button("Save", GUILayout.Height(30)))
        {
            _woolControl.MeshObjectData.TotalLayer    = _totalLayer;
            _woolControl.MeshObjectData.HightestColor = _highestColor;
            _onvalueChange = false;
        }
    }
}
#endif