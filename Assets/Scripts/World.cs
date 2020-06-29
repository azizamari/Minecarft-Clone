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
    void createNewChunk(int x, int z)
    {
        chunks[x,z]= new Chunk(new ChunkCoord(x, z), this);
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
