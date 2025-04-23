using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GamePlaySystem : MonoBehaviour
{
    #region <====================| Properties |====================>

    [Header("Objects")] 
    public Transform SpawnPoint;
    public GameObject ModelPrefab;

    [Header("Data relation rotation object")]
    public float Friction = 3f;                                  // The speed of decay of inertia
    public  Vector2 RotationSensitivity = new Vector2(1f,   1f); // Giới hạn tốc độ xoay
    public  Vector2 AccelerationRange   = new Vector2(0.1f, 1f); // Giới hạn tốc độ xoay
    public  float   RotationSpeed       = 5f;                    // Tốc độ xoay
    public  float   SmoothingTime       = 0.05f;
    private float   _acceleration;
    private float   _timerAfterMouseUp   = 0f;
    private float   _timerAfterMouseDown = 0f;
    private bool    _isDragging          = false;
    private Vector2 _previousDelta       = Vector2.zero;
    private Vector3 _lastMousePosition;

    private Camera     _mainCamera;
    private GameObject _targetObject;
    
    private LevelData _levelData;
    private GamePlayMeshController _meshController;

    private List<Color> _currentColorList      = new ();
    public  int RemainColor { get; private set; }
    public  int TotalColor  { get; private set; }
    
    private const float SmoothyImpactTimeAfterMouseUp = 0.5f; // Thời gian tác động sau khi nhả chuột
    private const float ObjectImpactTimeAfterMouseDown = 0.2f; // Thời gian tác động sau khi nhả chuột

    #endregion <=============================================>


    #region UNITY_METHODS

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        LoadLevel();
        GenRandomColor();
        _timerAfterMouseUp = 0;
        _acceleration      = AccelerationRange.x;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Bắt đầu kéo khi nhấn chuột trái
        {
            _isDragging        = true;
            _previousDelta     = Vector2.zero;
            _lastMousePosition = Input.mousePosition;
            
            Ray ray = _mainCamera.ScreenPointToRay(_lastMousePosition);// dùng luôn _lastMousePosition để lấy mouse position không cần get lại
            if (Physics.Raycast(ray, out var hit))
            {
                _timerAfterMouseDown = Time.time;
                _targetObject        = hit.collider.gameObject;
            }
        }

        if (Input.GetMouseButtonUp(0)) // Thả chuột
        {
            _timerAfterMouseUp = SmoothyImpactTimeAfterMouseUp;
            _isDragging        = false;
            
            if(Time.time - _timerAfterMouseDown <= ObjectImpactTimeAfterMouseDown)
            {
                _targetObject.GetComponent<WoolControl>()?.WoolRotation();
                _timerAfterMouseUp = 0f;
            }
            _timerAfterMouseDown = 0f;
        }
        Rotation(_isDragging);
    }

    #endregion


    #region MAIN_METHODS
    
    private void Rotation(bool isDragging)
    {
        if (!isDragging && _timerAfterMouseUp <= 0f)
        {
            SpawnPoint.Rotate(0f,  _acceleration * RotationSensitivity.x, 0f, Space.World);
            return;
        }
        Vector3 mousePos = Input.mousePosition;
        Vector3 rawDelta = mousePos - _lastMousePosition;

        // Tính toán acceleration
        _acceleration = Mathf.Clamp(rawDelta.magnitude * Time.deltaTime, AccelerationRange.x, AccelerationRange.y);

        // Làm mượt delta bằng SmoothDamp
        Vector2 smoothDelta = Vector2.zero;
        Vector2 delta = Vector2.SmoothDamp(
                new Vector2(_previousDelta.x, _previousDelta.y), 
                new Vector2(rawDelta.x,      rawDelta.y),
                ref smoothDelta,
                SmoothingTime // ví dụ: 0.05f
            );

        // Áp dụng ma sát
        delta.x = Mathf.Lerp(delta.x, 0f, Mathf.Clamp(Friction, 1, 50) * Time.deltaTime);
        delta.y = Mathf.Lerp(delta.y, 0f, Mathf.Clamp(Friction, 1, 50) * Time.deltaTime);

        // Tính toán xoay
        float rotX = delta.y * RotationSpeed * _acceleration * RotationSensitivity.x;
        float rotY = -delta.x * RotationSpeed * _acceleration * RotationSensitivity.x;

        // Điều chỉnh tương quan giữa rotX và rotY
        if (Mathf.Abs(rotX) > Mathf.Abs(rotY)) rotY *= 0.5f;
        if (Mathf.Abs(rotY) > Mathf.Abs(rotX)) rotX *= 0.5f;

        // Thực hiện xoay
        ModelPrefab.transform.Rotate(rotX, rotY, 0, Space.World);

        // Cập nhật vị trí chuột và delta cũ
        _lastMousePosition = mousePos;
        _previousDelta     = delta;
        _timerAfterMouseUp -= Time.deltaTime;
    }

    private void LoadLevel()
    {
        if (DataManager.LevelList.TryGetValue(PlayerConfig.player.Level, out var level))
        {
            _levelData = level;
        }
        if(_levelData == null)
        {
            #if UNITY_EDITOR
            Debug.LogError($"LevelId {PlayerConfig.player.Level} not found in LevelList");
            #endif
            return;
        }
        foreach (Color t in _levelData.ColorList)
        {
            for(int j = 0; j< _levelData.ColorCountList.Count;j++)
            {
                _currentColorList.Add(t);
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
        for (int i = 0; i < _meshController.MeshObjectDatas.Count; i++)
        {
            if(_meshController.MeshObjectDatas[i].ColorStack.Count == _meshController.MeshObjectDatas[i].TotalLayer) continue;
            _meshController.MeshObjectDatas[i].ColorStack.Push(_currentColorList[i]);
        }
    }

    #endregion
}
