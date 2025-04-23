using System;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public static class RenderHelper
{
    public struct MeshApplyJob : IJob
    {
        public struct VertexData
        {
            public float3 Position;
            public float3 Normal;
            public Color Color;
            public float4 UV1;
        }

        public NativeList<VertexData> _vertexData;
        public NativeList<ushort>     _indices;
        public Mesh.MeshDataArray     _meshDataArray;
        public void Execute()
        {
            if (_vertexData.Length >= UInt16.MaxValue) Debug.Log("Please use IndexFormat.<color=green>UInt32</color> instead of IndexFormat.<color=red>UInt16</color> on the mesh!");

            if (_indices.Length > 0)
            {
                Mesh.MeshData _meshData = _meshDataArray[0];
                _meshData.SetIndexBufferParams(_indices.Length, IndexFormat.UInt16);
                _meshData.SetVertexBufferParams(_vertexData.Length, vertexAttributeDescriptor);
                _meshData.GetIndexData<ushort>().CopyFrom(_indices.AsArray());
                _meshData.GetVertexData<VertexData>().CopyFrom(_vertexData.AsArray());
                _meshData.subMeshCount = 1;
                _meshData.SetSubMesh(0, new SubMeshDescriptor(0, _indices.Length));
                _meshData.subMeshCount = 1;
            }
        }

        public Mesh ApplyMeshAndDispose()
        {
            if (_indices.Length == 0)
            {
                _indices.Dispose();
                _vertexData.Dispose();
                Debug.Log(1);
                return null;
            }
            Mesh mesh = new Mesh();
            mesh.indexFormat = IndexFormat.UInt16;
            mesh.name = "Tetrahedron";
            Mesh.ApplyAndDisposeWritableMeshData(_meshDataArray, mesh, MeshUpdateFlags.DontValidateIndices | MeshUpdateFlags.DontNotifyMeshUsers);
            mesh.RecalculateBounds();
            _indices.Dispose();
            _vertexData.Dispose();
            return mesh;
        }

        static readonly VertexAttributeDescriptor[] vertexAttributeDescriptor =
       {
            new(VertexAttribute.Position, VertexAttributeFormat.Float32, dimension: 3, stream: 0),
            new(VertexAttribute.Normal, VertexAttributeFormat.Float32, dimension: 3, stream: 0),
            new(VertexAttribute.Color, VertexAttributeFormat.Float32, dimension: 4, stream: 0),
            new(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, dimension: 4, stream: 0)
        };

    }    
}