using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VoxelData
{
    public static readonly int chunkWidth=5;
    public static readonly int chunkHeight=15;
    public static readonly int worldSizeInChunks=100;

    public static int worldSizeInVoxels
    {
        get { return worldSizeInChunks * chunkWidth; }
    }

    public static readonly int viewDistanceInChunks=5;

    public static readonly int textureAtlasSizeInBlocks = 4;
    public static float normalizeBlockTextureSize { 
        get { return 1.0f / (float)textureAtlasSizeInBlocks; }
    }
    public static readonly Vector3[] voxelVerts = new Vector3[8]
    {
        new Vector3(0f,0f,0f),
        new Vector3(1f,0f,0f),
        new Vector3(1f,1f,0f),
        new Vector3(0f,1f,0f),
        new Vector3(0f,0f,1f),
        new Vector3(1f,0f,1f),
        new Vector3(1f,1f,1f),
        new Vector3(0f,1f,1f)
    };
    public static readonly Vector3[] faceChecks = new Vector3[6]
    {
        new Vector3(0f,0f,-1f),
        new Vector3(0f,0f,1f),
        new Vector3(0f,1f,0f),
        new Vector3(0f,-1f,0f),
        new Vector3(-1f,0f,0f),
        new Vector3(1f,0f,0f),
    };
    public static readonly int[,] voxelTris = new int[6, 4]
    {
        {0, 3, 1,2}, // Back Face
        {5, 6, 4,7}, // Front Face
        {3, 7, 2,6}, // Top Face
        {1, 5, 0,4}, // Bottom Face
        {4, 7, 0,3}, // Left Face
        {1, 2, 5,6}  // Right Face
    };
    public static readonly Vector2[] voxelUvs = new Vector2[4]
    {
        new Vector2(0.0f,0.0f),
        new Vector2(0.0f,1.0f),
        new Vector2(1.0f,0.0f),
        new Vector2(1.0f,1.0f)
    };
}
