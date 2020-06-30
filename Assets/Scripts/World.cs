using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public int seed;

    public Transform player;
    public Vector3 spawnPosition;

    public Material material;
    public BlockType[] blockTypes;

    Chunk[,] chunks = new Chunk[VoxelData.worldSizeInChunks, VoxelData.worldSizeInChunks];
    List<ChunkCoord> activeChunks = new List<ChunkCoord>();
    ChunkCoord playerLastChunkCoord;
    ChunkCoord playerChunkCoord;
    private void Start()
    {
        Random.InitState(seed);
        playerLastChunkCoord=GetChunkCoordFromVector3(player.position);
        spawnPosition = new Vector3((VoxelData.worldSizeInChunks * VoxelData.chunkWidth) / 2f, VoxelData.chunkHeight+2f, (VoxelData.worldSizeInChunks * VoxelData.chunkWidth) / 2f);
        player.position = spawnPosition;
        GenerateWorld();
    }
    private void Update()
    {
        playerChunkCoord = GetChunkCoordFromVector3(player.position);
        if (!playerChunkCoord.Equals(playerLastChunkCoord))
        {
            CheckViewDistance();
            playerLastChunkCoord = playerChunkCoord; //delete this if bugged
        }
    }
    ChunkCoord GetChunkCoordFromVector3(Vector3 pos)
    {
        int x = Mathf.FloorToInt(pos.x / VoxelData.chunkWidth);
        int z = Mathf.FloorToInt(pos.z / VoxelData.chunkWidth);
        return new ChunkCoord(x, z);
    }
    void CheckViewDistance()
    {
        ChunkCoord coord= GetChunkCoordFromVector3(player.position);

        List<ChunkCoord> previouslyActiveChunks = new List<ChunkCoord>(activeChunks);
        activeChunks.Clear();

        for (int x = coord.x - VoxelData.viewDistanceInChunks; x < coord.x + VoxelData.viewDistanceInChunks; x++)
        {
            for (int z = coord.z - VoxelData.viewDistanceInChunks; z < coord.z + VoxelData.viewDistanceInChunks; z++)
            {
                if (IsChunkInWorld(new ChunkCoord(x,z)))
                {
                    if (chunks[x, z] == null)
                    {
                        CreateNewChunk(x, z);
                    }
                    else if (!chunks[x, z].isActive)
                    {
                        chunks[x, z].isActive = true;
                        activeChunks.Add(new ChunkCoord(x, z));
                    }
                }
                for (int i = 0; i < previouslyActiveChunks.Count; i++)
                { 
                    if(previouslyActiveChunks[i].Equals(new ChunkCoord(x, z)))
                    {
                        previouslyActiveChunks.RemoveAt(i);
                    }
                }
            }
        }
        foreach(var c in previouslyActiveChunks)
        {
            chunks[c.x,c.z].isActive=false;
        }
    }
    void GenerateWorld()
    {
        for (int x = (VoxelData.worldSizeInChunks /2)-VoxelData.viewDistanceInChunks; x < (VoxelData.worldSizeInChunks / 2) + VoxelData.viewDistanceInChunks; x++)
        {
            for (int z = (VoxelData.worldSizeInChunks / 2) - VoxelData.viewDistanceInChunks; z < (VoxelData.worldSizeInChunks / 2) + VoxelData.viewDistanceInChunks; z++)
            {
                CreateNewChunk(x, z);
            }
        }
    }
    public byte GetVoxel(Vector3 pos)
    {
        if (!IsVoxelInWorld(pos))
        {
            return 0;
        }
        if (pos.y < 1)
        {
            return 1;
        }
        else if (pos.y == VoxelData.chunkHeight - 1)
        {
            float tempNoise = Noise.Get2Dperlin(new Vector2(pos.x, pos.y), 0, 0.1f);
            if (tempNoise < 0.5)
            {
                return 3;
            }
            else return 2;
        }
        else return 4;
    }
    void CreateNewChunk(int x, int z)
    {
        chunks[x,z]= new Chunk(new ChunkCoord(x, z), this);
        activeChunks.Add(new ChunkCoord(x, z));
    }
    bool IsChunkInWorld (ChunkCoord coord)
    {
        if (coord.x > 0&&coord.x<VoxelData.worldSizeInChunks-1&&coord.z>0&&coord.z<VoxelData.worldSizeInChunks)
        {
            return true;
        }
        else return false;
    }
    bool IsVoxelInWorld(Vector3 pos)
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
