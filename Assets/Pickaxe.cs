using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PickaxeAnimator))]
public class Pickaxe : MonoBehaviour
{
    [SerializeField] private float miningSpeed = 1;
    [SerializeField] private Vector3 postionUpdateOffset;
    private BlockObject selectedBlock;

    private bool startMiningAnim = false;
    private bool MiningAnim = false;

    private PickaxeAnimator animator;
    
    private void Awake()
    {
        animator = GetComponent<PickaxeAnimator>();
        animator.PickaxeHit += TryBreakBlock;
    }

    public bool StartMine(BlockObject block)
    {
        if (block.Minable == false)
        {
            return false;
        }

        UpdatePos(block.transform.position);
        animator.SoftStart();
        selectedBlock = block;
        return true;
    }

    private bool waitForLastHitThenStopAnim = false;
    
    public void StopMine()
    {
        animator.SoftStop();
        
    }


    public void Mine()
    {
        if (selectedBlock == null)
        {
            StopMine();
            Debug.LogError("Pixkaxe: mining but selectedBlock ss null");//TODO: tu może być obsłużony drag z jednego już wykopanego na nowy
            return;
        }

        float mineDmg = miningSpeed * Time.deltaTime;
        
        selectedBlock.Mine(mineDmg);
    }

    public void TryBreakBlock()
    {
        if (selectedBlock != null && selectedBlock.TryBreak())
        {
            animator.SoftStop();
            selectedBlock = null;
        }
    }

    private void UpdatePos(Vector3 blockPos)
    {
        this.transform.position = blockPos + postionUpdateOffset;
    }
}
