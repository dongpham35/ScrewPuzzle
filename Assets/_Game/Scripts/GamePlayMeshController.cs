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
    #endregion


    #region MyRegion

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
            MeshObjectDatas.Add(meshObjectData);
            if (meshRenderer.gameObject.TryGetComponent(typeof(WoolControl), out var woolControl))
                WoolControls.Add(woolControl as WoolControl);
            else WoolControls.Add(meshRenderer.gameObject.AddComponent<WoolControl>());
        }
    }
    #endif
    #endregion 
}
