using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public ChunkCoord coord;
    GameObject chunkObject;
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    int vertexIndex = 0;
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uvs = new List<Vector2>();

    byte[,,] voxelMap = new byte[VoxelData.chunkWidth, VoxelData.chunkHeight, VoxelData.chunkWidth];
    World world;

    public Chunk(ChunkCoord _coord,World _world)
    {
        world = _world;
        coord = _coord;
        chunkObject = new GameObject();

        meshFilter = chunkObject.AddComponent<MeshFilter>();
        meshRenderer = chunkObject.AddComponent<MeshRenderer>();
        meshRenderer.material = world.material;
        chunkObject.transform.SetParent(world.transform);
        chunkObject.transform.position = new Vector3(coord.x * VoxelData.chunkWidth, 0f, coord.z * VoxelData.chunkWidth);
        chunkObject.name="Chunk "+coord.x+", " + coord.z;

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
                    voxelMap[x, y, z] = world.GetVoxel(new Vector3(x,y,z)+position);
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
    public bool isActive
    {
        get { return chunkObject.activeSelf; }
        set { chunkObject.SetActive(value); }
    }
    public Vector3 position
    {
        get { return chunkObject.transform.position; }
    }
    bool isVoxelInChunk(int x, int y, int z)
    {
        if (x < 0 || x > VoxelData.chunkWidth - 1 || y < 0 || y > VoxelData.chunkHeight - 1 || z < 0 || z > VoxelData.chunkWidth - 1)
            return false;
        else
        {
            return true;
        }
    }
    bool CheckVoxel(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x);
        int y = Mathf.FloorToInt(pos.y);
        int z = Mathf.FloorToInt(pos.z);
        if (!isVoxelInChunk(x,y,z))
            return world.blockTypes[world.GetVoxel(pos + position)].isSolid;
        return world.blockTypes[voxelMap[x,y,z]].isSolid;
    }
    void AddDataToChunk(Vector3 pos)
    {
        for (int j = 0; j < 6; j++)
        {
            if (!CheckVoxel(pos + VoxelData.faceChecks[j]))
            {
                byte blockID = voxelMap[(int)pos.x, (int)pos.y, (int)pos.z];

                vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[j, 0]]);
                vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[j, 1]]);
                vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[j, 2]]);
                vertices.Add(pos + VoxelData.voxelVerts[VoxelData.voxelTris[j, 3]]);
                AddTexure(world.blockTypes[blockID].GetTextureID(j));
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
    void AddTexure(int textureID)
    {
        float y = textureID / VoxelData.textureAtlasSizeInBlocks;
        float x = textureID - (y * VoxelData.textureAtlasSizeInBlocks);
        x *= VoxelData.normalizeBlockTextureSize;
        y *= VoxelData.normalizeBlockTextureSize;
        y = 1f - y - VoxelData.normalizeBlockTextureSize;
        uvs.Add(new Vector2(x, y));
        uvs.Add(new Vector2(x, y + VoxelData.normalizeBlockTextureSize));
        uvs.Add(new Vector2(x + VoxelData.normalizeBlockTextureSize, y));
        uvs.Add(new Vector2(x + VoxelData.normalizeBlockTextureSize, y + VoxelData.normalizeBlockTextureSize));
    }
}
public class ChunkCoord
{
    public int x;
    public int z;
    public ChunkCoord(int _x,int _z)
    {
        x = _x;
        z = _z;
    }
    public bool Equals(ChunkCoord other)
    {
        if (other == null)
        {
            return false;
        }
        else if (other.x == this.x && other.z == this.z)
        {
            return true;
        }
        return false;
    }
}
