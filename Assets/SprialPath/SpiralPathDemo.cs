using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class SpiralPathDemo : MonoBehaviour
{
    [Header("Path Settings")]
    [SerializeField] private SpiralPathData spiralPathData;
    [SerializeField] private bool useDistanceInterpolation = true;
    [SerializeField] private bool autoLoop = true;
    
    [Header("Movement Settings")]
    [SerializeField] private float speed = 0.2f;
    [SerializeField] private float startProgress = 0f;
    [SerializeField] private bool playOnStart = true;
    
    [Header("Debug")]
    [SerializeField] private bool showPathGizmos = true;
    [SerializeField] private Color pathColor = Color.yellow;
    [SerializeField] private float gizmoSize = 0.05f;

    [SerializeField] private Material wollenMaterial;
    private                  float    _progress;
    private                  bool     _isPlaying = false;
    private static readonly  int      DisplayID   = Shader.PropertyToID("_Display");

    private void Start()
    {
        if (spiralPathData == null)
        {
            Debug.LogError("SpiralPathData is not assigned!", this);
            return;
        }
        
        _progress = startProgress;
        _isPlaying = playOnStart;
    }
    
    private void Update()
    {
        if (!_isPlaying || spiralPathData == null) return;
        
        // Update progress
        _progress += speed * Time.deltaTime;
        
        // Handle looping
        if (_progress >= 1f)
        {
            if (autoLoop)
            {
                _progress %= 1f;
            }
            else
            {
                _progress = 1f;
                _isPlaying = false;
            }
        }
        
        // Update position
        transform.position = useDistanceInterpolation 
            ? spiralPathData.GetPositionByDistance(_progress) 
            : spiralPathData.GetPositionAtProgress(_progress);
        
        wollenMaterial.SetFloat(DisplayID, 1 - _progress);
    }
    
    private void OnDrawGizmos()
    {
        if (spiralPathData == null || !showPathGizmos) return;
        
        Gizmos.color = pathColor;
        
        // Draw path points
        for (int i = 0; i < spiralPathData.PathPoints.Count; i++)
        {
            Gizmos.DrawSphere(spiralPathData.PathPoints[i], gizmoSize);
            
            // Draw lines between points
            if (i > 0)
            {
                Gizmos.DrawLine(spiralPathData.PathPoints[i-1], spiralPathData.PathPoints[i]);
            }
        }
        
        // Draw current position
        if (Application.isPlaying && _isPlaying)
        {
            Vector3 currentPos = useDistanceInterpolation 
                ? spiralPathData.GetPositionByDistance(_progress) 
                : spiralPathData.GetPositionAtProgress(_progress);
                
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(currentPos, gizmoSize * 1.5f);
        }
    }
    
    // Public methods for external control
    
    public void Play()
    {
        _isPlaying = true;
    }
    
    public void Pause()
    {
        _isPlaying = false;
    }
    
    public void Stop()
    {
        _isPlaying = false;
        _progress = startProgress;
        
        // Update position immediately
        if (spiralPathData != null)
        {
            transform.position = useDistanceInterpolation 
                ? spiralPathData.GetPositionByDistance(_progress) 
                : spiralPathData.GetPositionAtProgress(_progress);
        }
    }
    
    public void SetProgress(float progress)
    {
        _progress = Mathf.Clamp01(progress);
        
        // Update position immediately
        if (spiralPathData != null)
        {
            transform.position = useDistanceInterpolation 
                ? spiralPathData.GetPositionByDistance(_progress) 
                : spiralPathData.GetPositionAtProgress(_progress);
        }
    }
    
    public float GetProgress()
    {
        return _progress;
    }
    
    public float GetTotalPathLength()
    {
        return spiralPathData != null ? spiralPathData.GetTotalLength() : 0f;
    }
} 