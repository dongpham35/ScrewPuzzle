using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(MeshFilter))]
public class SpiralPathBaker : MonoBehaviour
{
    [SerializeField] private SpiralPathData _spiralPathData;
    [SerializeField] private MeshFilter _meshFilter;

    private void OnValidate()
    {
        if (_meshFilter == null)
        {
            _meshFilter = GetComponent<MeshFilter>();
        }
    }

    [ContextMenu("Bake")]
    private void Bake()
    {
        if (_spiralPathData == null || _meshFilter == null || _meshFilter.sharedMesh == null)
        {
            Debug.LogError("Missing required components for baking spiral path");
            return;
        }

        Mesh mesh = _meshFilter.sharedMesh;
        Vector3[] vertices = mesh.vertices;
        
        if (vertices.Length == 0)
        {
            Debug.LogError("Mesh has no vertices");
            return;
        }

        // Convert vertices to world space
        List<Vector3> worldVertices = new List<Vector3>(vertices.Length);
        for (int i = 0; i < vertices.Length; i++)
        {
            worldVertices.Add(transform.TransformPoint(vertices[i]));
        }

        // Group vertices by height (rounded to handle floating point precision)
        float heightPrecision = 0.01f;
        var verticesByHeight = worldVertices
            .Select((v, index) => new { Vertex = v, Index = index })
            .GroupBy(x => Mathf.Round(x.Vertex.y / heightPrecision) * heightPrecision)
            .OrderByDescending(g => g.Key) // Sort from highest to lowest
            .ToList();

        List<Vector3> spiralPath = new List<Vector3>();
        
        // For each height layer, sort vertices in a circular pattern
        foreach (var heightGroup in verticesByHeight)
        {
            // Calculate the center of this height group
            Vector3 center = Vector3.zero;
            foreach (var item in heightGroup)
            {
                center += item.Vertex;
            }
            center /= heightGroup.Count();
            
            // Sort vertices in this height group by angle around the center
            var sortedVertices = heightGroup
                .Select(x => new 
                {
                    Vertex = x.Vertex,
                    Angle = Mathf.Atan2(x.Vertex.z - center.z, x.Vertex.x - center.x)
                })
                .OrderBy(x => x.Angle)
                .Select(x => x.Vertex)
                .ToList();
            
            spiralPath.AddRange(sortedVertices);
        }
        
        // Save the result
        _spiralPathData.PathPoints = spiralPath;
        
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(_spiralPathData);
        UnityEditor.AssetDatabase.SaveAssets();
        Debug.Log($"Spiral path baked with {spiralPath.Count} points");
#endif
    }
}
