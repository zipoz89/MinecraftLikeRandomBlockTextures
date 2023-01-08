using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlockObject : MonoBehaviour
{
    [SerializeField] private Block block;
    

    [SerializeField]  private MeshRenderer renderer;

    private int breakingProcentage = 0;

    [SerializeField] private float blockLife = 1;
    [SerializeField] private float blockHardness = 1;

    [SerializeField] private AnimationCurve breakingAnimCurve;

    private bool minable = true;

    public bool Minable
    {
        get => minable;
        set => minable = value;
    }


    private bool readyToBeDestroyed = false;

    public bool ReadyToBeDestroyed
    {
        get => readyToBeDestroyed;
        set => readyToBeDestroyed = value;
    }


    private bool isBeingMined = false;
    private bool isBeingMinedLastFrame = false;
    
    private Material breakingMaterialInstance;

    private void Start()
    {
        breakingMaterialInstance = renderer.materials[0];
    }

    private void Update()
    {
        HandleMining();
    }


    private void HandleMining()
    {
        if (isBeingMined)
        {
            
        }
        
        isBeingMinedLastFrame = isBeingMined;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isBeingMined = true;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            isBeingMined = false;
        }
        
    }
    
    public void Mine(float mineDmg)
    {
        if (!minable)
        {
            return;
        }

        blockLife -= mineDmg;

        breakingMaterialInstance.SetFloat("_CircleSize", 1-blockLife);
        
        if (blockLife <= 0)
        {
            Debug.Log("Block mined");
            minable = false;
            readyToBeDestroyed = true;
            blockLife = 0;//TODO: tu może być liczony jakiś overshoot i jak będzie za dużo to możę nie wykopać kamieni tylko zniszczyć albo wgl zrobiuć jakiś proszek
        }

    }

    public bool TryBreak()
    {
        if (readyToBeDestroyed)
        {
            Destroy(this.gameObject);
            return true;
        }

        return false;
    }

}
