using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Material material;
    public BlockType[] blockTypes;

    Chunk[,] chunks = new Chunk[VoxelData.worldSizeInChunks, VoxelData.worldSizeInChunks];
    private void Start()
    {
        generateWorld();
    }
    void generateWorld()
    {
        for (int x = 0; x < VoxelData.worldSizeInChunks; x++)
        {
            for (int z = 0; z < VoxelData.worldSizeInChunks; z++)
            {
                createNewChunk(x, z);
            }
        }
    }
    public byte getVoxel(Vector3 pos)
    {
        if (!isVoxelInWorld(pos))
        {
            return 0;
        }
        if (pos.y < 1)
        {
            return 1;
        }
        else if (pos.y < 10)
        {
            return 2;
        }
        else if (pos.y < 14)
        {
            return  4;
        }
        else
        {
            return  3;
        }
    }
    void createNewChunk(int x, int z)
    {
        chunks[x,z]= new Chunk(new ChunkCoord(x, z), this);
    }
    bool isChunkInWorld (ChunkCoord coord)
    {
        if (coord.x > 0&&coord.x<VoxelData.worldSizeInChunks-1&&coord.z>0&&coord.z<VoxelData.worldSizeInChunks)
        {
            return true;
        }
        else return false;
    }
    bool isVoxelInWorld(Vector3 pos)
    {
        if (pos.x >= 0 && pos.x < VoxelData.worldSizeInVoxels && pos.y >= 0 && pos.y < VoxelData.chunkHeight && pos.z >= 0 && pos.z < VoxelData.worldSizeInVoxels)
        {
            return true;
        }
        else return false;
    }
}
[System.Serializable]
public class BlockType
{
    public string name;
    public bool isSolid;

    [Header("Texture Values")]
    //back, front, top, bottom, left, right
    public int backFaceTexure;
    public int frontFaceTexure;
    public int topFaceTexure;
    public int bottomFaceTexure;
    public int leftFaceTexure;
    public int rightFaceTexure;


    public int GetTextureID(int faceIndex)
    {
        switch (faceIndex)
        {
            case 0:
                return backFaceTexure;
            case 1:
                return frontFaceTexure;
            case 2:
                return topFaceTexure;
            case 3:
                return bottomFaceTexure;
            case 4:
                return leftFaceTexure;
            case 5:
                return rightFaceTexure;
            default:
                Debug.Log("Error in GetTexrureID");
                return 0;
        }
    }
}
