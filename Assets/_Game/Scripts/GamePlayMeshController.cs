using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

public class GamePlayMeshController : MonoBehaviour
{

    #region PROPERTIES
    [Header("Config LevelId before press button SetDataLevel")]
    public int LevelId;
    public bool IrgnoreLevelId = true;
    public LevelData            LevelData;
    public List<MeshObjectData> MeshObjectDatas;
    public List<WoolControl>    WoolControls;
    
    private List<Color> _currentColorList      = new ();
    public  int         RemainColor { get; private set; }
    public  int         TotalColor  { get; private set; }
    #endregion
    
    #region MAIN_METHODS

    public void LoadcolorForMesh()
    {
        LoadLevel();
        GenRandomColor();
    }

    #if UNITY_EDITOR
    [Button]
    [UsedImplicitly]
    public void SetDataLevel()
    {
        if (!IrgnoreLevelId)
        {
            if(DataManager.LevelList == null || DataManager.LevelList.Count == 0)
            {
                Debug.LogError("LevelList is null or empty");
                return;
            }
            if(!DataManager.LevelList.TryGetValue(LevelId, out var levelData))
            {
                Debug.LogError($"LevelId {LevelId} not found in LevelList");
                return;
            }
            LevelData = levelData;
        }
        WoolControls = new List<WoolControl>();
        MeshObjectDatas = new List<MeshObjectData>();
        var gameobj = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (var meshRenderer in gameobj)
        {
            if(meshRenderer.transform.childCount == 0)  continue;
                var meshObjectData = new MeshObjectData();//Tạm thời set tay layer nên sẽ không khởi tạo Layer ở taại đây => set tay
            if (meshRenderer.gameObject.TryGetComponent(typeof(WoolControl), out var woolControl))
            {
                WoolControls.Add(woolControl as WoolControl);
                meshObjectData.HightestColor = (woolControl as WoolControl).MeshObjectData.HightestColor;
                meshObjectData.TotalLayer   = (woolControl as WoolControl).MeshObjectData.TotalLayer;
            }
            else WoolControls.Add(meshRenderer.gameObject.AddComponent<WoolControl>());
            MeshObjectDatas.Add(meshObjectData);
        }
    }
    
    private void LoadLevel()
    {
//         if (DataManager.LevelList.TryGetValue(PlayerConfig.player.Level, out var level))
//         {
//             _levelData = level;
//         }
//         if (_levelData == null)
//         {
// #if UNITY_EDITOR
//             Debug.LogError($"LevelId {PlayerConfig.player.Level} not found in LevelList");
// #endif
//             return;
//         }
        for (int i = 0;i < LevelData.ColorList.Count; i++)
        {
            for (int j = 0; j < LevelData.ColorCountList[i]; j++)
            {
                _currentColorList.Add(LevelData.ColorList[i]);
            }
        }
        //Load mesh object
    }

    private void GenRandomColor()
    {
        //Merge color list
        for (int i = 0; i < _currentColorList.Count; i++)
        {
            int randomIndex = Random.Range(i, _currentColorList.Count);
            (_currentColorList[i], _currentColorList[randomIndex]) = (_currentColorList[randomIndex], _currentColorList[i]);
        }
        for (int i = 0; i < _currentColorList.Count; i++)
        {
            if (MeshObjectDatas[i % MeshObjectDatas.Count].ColorStack.Count + 1 == MeshObjectDatas[i % MeshObjectDatas.Count].TotalLayer) continue;
                MeshObjectDatas[i % MeshObjectDatas.Count]
               .ColorStack
               .Push(_currentColorList[i]);
        }
        for (int i = 0; i < MeshObjectDatas.Count; i++)
        {
            WoolControls[i].InitMesh(MeshObjectDatas[i]);
        }
    }
    
    #endif
    #endregion 
}
