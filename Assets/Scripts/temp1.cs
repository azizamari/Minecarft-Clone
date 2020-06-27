using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class temp : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    int vertexIndex = 0;
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uvs = new List<Vector2>();

    bool[,,] voxelMap = new bool[VoxelData.chunkWidth, VoxelData.chunkHeight, VoxelData.chunkWidth];
    private void Start()
    {
        PopulateVoxelMap();
        CreateMeshData();
        CreateMesh();
    }
    void PopulateVoxelMap()
    {
        for (int y = 0; y < VoxelData.chunkHeight; y++)
        {
            for (int x = 0; x < VoxelData.chunkWidth; x++)
            {
                for (int z = 0; z < VoxelData.chunkWidth; z++)
                {
                    voxelMap[x, y, z] = true;
                }
            }
        }
    }
    void CreateMeshData()
    {
        for (int y = 0; y < VoxelData.chunkHeight; y++)
        {
            for (int x = 0; x < VoxelData.chunkWidth; x++)
            {
                for (int z = 0; z < VoxelData.chunkWidth; z++)
                {
                    AddDataToChunk(new Vector3(x, y, z));
                }
            }
        }
    }
    bool CheckVoxel(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x);
        int y = Mathf.FloorToInt(pos.y);
        int z = Mathf.FloorToInt(pos.z);
        if (x < 0 || x > VoxelData.chunkWidth - 1 || y < 0 || y > VoxelData.chunkHeight - 1 || z < 0 || z > VoxelData.chunkWidth - 1)
            return false;
        return voxelMap [x,y,z];
    }
    void AddDataToChunk(Vector3 pos)
    {
        for (int j = 0; j < 6; j++)
        {
            if (!CheckVoxel(pos + VoxelData.faceChecks[j])) 
            {
                vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[j, 0]]);
                vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[j, 1]]);
                vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[j, 2]]);
                vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[j, 3]]);
                uvs.Add(VoxelData.voxelUvs[0]);
                uvs.Add(VoxelData.voxelUvs[1]);
                uvs.Add(VoxelData.voxelUvs[2]);
                uvs.Add(VoxelData.voxelUvs[3]);
                triangles.Add(vertexIndex);
                triangles.Add(vertexIndex+1);
                triangles.Add(vertexIndex+2);
                triangles.Add(vertexIndex+2);
                triangles.Add(vertexIndex+1);
                triangles.Add(vertexIndex+3);
                vertexIndex += 4;
            }
        }
    }
    void CreateMesh()
    {
        Mesh mesh = new Mesh
        {
            vertices = vertices.ToArray(),
            triangles = triangles.ToArray(),
            uv = uvs.ToArray()
        };
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }
}
