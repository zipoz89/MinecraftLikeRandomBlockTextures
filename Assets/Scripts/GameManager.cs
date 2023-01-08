using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //A TERAZ
    //CZAS NA
    //BLOKI
    
    //NASZE BLOKI
    
    //SĄ
    
    //...

    [SerializeField] private Camera cemera;
    
    private List<BlockLayer> LayerRows = new ();
    
    [SerializeField] private int ySpawnableRows = 2;
    [SerializeField] private int xlSpawnableRows = 1;
    [SerializeField] private int xrSpawnableRows = 1;
    [SerializeField] private int SpawnableRows = 1;
    
    private SpawningMethod _spawningMethod = new AllStoneSpawningMethod();
    
    [SerializeField] private Vector3 worldOrigin;
    [SerializeField] private float blockSize;
    
    [SerializeField] private GameObject blockPrefab;
    
    [SerializeField] private Transform[] layerParents;

    [SerializeField] private Pickaxe pickaxe;
    
    void Start()
    {
        LayerRows.Add(new BlockLayer(-1,-1,3,3,_spawningMethod));
        LayerRows.Add(new BlockLayer(-1,-1,3, 3,_spawningMethod));
        
        
        SpawnLayer(LayerRows,0,0,layerParents[0],true);
        SpawnLayer(LayerRows,1,1,layerParents[1],false);

    }

    private void SpawnLayer(List<BlockLayer> LayerRows,int idInLayersInRow,int worldZ,Transform layerParent,bool minable)
    {
        LayerRows[idInLayersInRow].SpawnLayer(worldZ);
        for (int y = 0; y < LayerRows[0].ySize; y++)
        {
            for (int x = 0; x < LayerRows[0].xSize; x++)
            {
                SpawnBlock(x, y, LayerRows[0].Grid[x, y], layerParent,minable);
            }
        }
    }
    
    private void SpawnBlock(int x, int y, Block block,Transform layerParent,bool minable)
    {
        float xPos = x * blockSize;
        float yPos = y * blockSize;

        BlockObject o = Instantiate(blockPrefab, new Vector3(xPos, yPos), Quaternion.identity, layerParent).GetComponent<BlockObject>();
        o.transform.localPosition = new Vector3(xPos, yPos);
        o.Minable = minable;
    }


    private void Update()
    {
        HandlePickaxe();
    }

    private bool isMining;
    
    private void HandlePickaxe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cemera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out BlockObject block))
                {
                    isMining = pickaxe.StartMine(block);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0) && isMining)
        {
            isMining = false;
            pickaxe.StopMine();
        }
        else if (Input.GetMouseButton(0) && isMining)
        {
            pickaxe.Mine();
        }
    }

    //TERAZ MI SIĘ NIE CHCE TO NA KIDYŚ TERAZ ZA DUŻO ROBOTY
    // [SerializeField] private Transform  cemraTransform;
    // [SerializeField] private Transform pickaxeRotation;
    //
    // private void Update()
    // {
    //     Input.gyro.enabled = true;
    //     cemraTransform.Rotate(0, -Input.gyro.rotationRateUnbiased.y, 0);
    //     pickaxeRotation.Rotate( 0,0,Input.gyro.rotationRateUnbiased.z);
    // }
}

public class BlockLayer
{
    public Block[,] Grid;
    public int xSize;
    public int ySize;
    public int xWorldPos;
    public int yWorldPos;
    public int zWorldPos;
    public SpawningMethod SpawningMethod;

    public BlockLayer(int xWorldPos, int yWorldPos,int xSize, int ySize, SpawningMethod spawningMethod)
    {
        this.xWorldPos = xWorldPos;
        this.yWorldPos = yWorldPos;
        this.xSize = xSize;
        this.ySize = ySize;
        SpawningMethod = spawningMethod;
    }

    public void SpawnLayer(int worldZIndex)
    {
        zWorldPos = worldZIndex;
        Grid = SpawningMethod.SpawnLayer(xSize,ySize,xWorldPos,yWorldPos,zWorldPos);
    }
}

public abstract class SpawningMethod
{
    public int seed;
    
    public abstract Block[,] SpawnLayer(int x, int y,int yWorldPos, int xWorldPos, int zWorldPos);
}

public class AllStoneSpawningMethod : SpawningMethod
{
    public override Block[,] SpawnLayer(int xToSpawn, int yToSpawn,int yWorldPos, int xWorldPos, int zWorldPos)
    {
        Block[,] layer = new Block[xToSpawn, yToSpawn];
        for (int y = 0; y < yToSpawn; y++)
        {
            for (int x= 0; x < xToSpawn; x++)
            {
                layer[x, y] = new Block(0);
            }
        }

        return layer;
    }
}