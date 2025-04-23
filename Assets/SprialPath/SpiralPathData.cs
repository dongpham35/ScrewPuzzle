using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpiralPathData", menuName = "ScriptableObjects/SpiralPathData")]
public class SpiralPathData : ScriptableObject
{
    public List<Vector3> PathPoints;
    private float[] _cumulativeDistances;
    private float _totalPathLength;
    private bool _isDirty = true;

    /// <summary>
    /// Returns a position on the path based on progress (0-1)
    /// </summary>
    /// <param name="progress">Progress value between 0 and 1</param>
    /// <returns>Interpolated position on the path</returns>
    public Vector3 GetPositionAtProgress(float progress)
    {
        if (PathPoints == null || PathPoints.Count == 0)
        {
            Debug.LogWarning("Path has no points");
            return Vector3.zero;
        }

        if (PathPoints.Count == 1)
        {
            return PathPoints[0];
        }

        // Clamp progress to 0-1 range
        progress = Mathf.Clamp01(progress);

        // Calculate the index based on progress
        float indexFloat = progress * (PathPoints.Count - 1);
        int index = Mathf.FloorToInt(indexFloat);
        
        // Make sure we don't exceed array bounds
        if (index >= PathPoints.Count - 1)
        {
            return PathPoints[PathPoints.Count - 1];
        }

        // Calculate the interpolation factor between the two points
        float t = indexFloat - index;
        
        // Linearly interpolate between the two points
        return Vector3.Lerp(PathPoints[index], PathPoints[index + 1], t);
    }

    /// <summary>
    /// Returns a position on the path based on progress (0-1) using actual distance interpolation
    /// </summary>
    /// <param name="progress">Progress value between 0 and 1</param>
    /// <returns>Interpolated position on the path based on actual distance</returns>
    public Vector3 GetPositionByDistance(float progress)
    {
        if (PathPoints == null || PathPoints.Count == 0)
        {
            Debug.LogWarning("Path has no points");
            return Vector3.zero;
        }

        if (PathPoints.Count == 1)
        {
            return PathPoints[0];
        }

        // Calculate distances if needed
        if (_isDirty)
        {
            CalculateDistances();
        }

        // Clamp progress to 0-1 range
        progress = Mathf.Clamp01(progress);
        
        // Find target distance
        float targetDistance = progress * _totalPathLength;
        
        // Find the segment where the target distance falls
        int index = 0;
        while (index < _cumulativeDistances.Length - 1 && _cumulativeDistances[index] < targetDistance)
        {
            index++;
        }
        
        // If we're at the beginning of the path
        if (index == 0)
        {
            float t = targetDistance / _cumulativeDistances[0];
            return Vector3.Lerp(PathPoints[0], PathPoints[1], t);
        }
        
        // Calculate interpolation factor
        float segmentStart = _cumulativeDistances[index - 1];
        float segmentLength = _cumulativeDistances[index] - segmentStart;
        float t2 = (targetDistance - segmentStart) / segmentLength;
        
        // Linearly interpolate between the two points
        return Vector3.Lerp(PathPoints[index - 1], PathPoints[index], t2);
    }

    /// <summary>
    /// Calculate and cache the cumulative distances along the path
    /// </summary>
    private void CalculateDistances()
    {
        int count = PathPoints.Count;
        _cumulativeDistances = new float[count];
        
        _cumulativeDistances[0] = 0f;
        for (int i = 1; i < count; i++)
        {
            float segmentLength = Vector3.Distance(PathPoints[i-1], PathPoints[i]);
            _cumulativeDistances[i] = _cumulativeDistances[i-1] + segmentLength;
        }
        
        _totalPathLength = _cumulativeDistances[count - 1];
        _isDirty = false;
    }

    /// <summary>
    /// Mark the path as dirty when points change to recalculate distances
    /// </summary>
    public void SetDirty()
    {
        _isDirty = true;
    }

    /// <summary>
    /// Get the total length of the path
    /// </summary>
    public float GetTotalLength()
    {
        if (_isDirty)
        {
            CalculateDistances();
        }
        return _totalPathLength;
    }
}