using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickaxeAnimator : MonoBehaviour
{


    [SerializeField] private float diggingAnimAngle;
    [SerializeField] private float diggingRetractAnimAngle;
    [SerializeField] private float idleAnimAngle;


    [SerializeField] private AnimationCurve startAnimCurve;
    [SerializeField] private AnimationCurve mineAnimCurve;
    [SerializeField] private float startAnimSpeed;
    [SerializeField] private float mineAnimSpeed;

    public Action PickaxeHit;
    
    private float currentAnimTime;
    
    public enum MiningAnimState
    {
        Idle,
        Starting,
        MiningOut,
        MiningIn,
        Stopping,
    }

    [SerializeField] private MiningAnimState animState;

    private bool isLastMineFlag = false;
    
    public MiningAnimState AnimState
    {
        get => animState;
        set
        {
            currentAnimTime = 0;
            animState = value;
        }
    }

    public void SoftStart()
    {
        AnimState = MiningAnimState.Starting;//TODO: this isn't soft lol
    }

    public void SoftStop()
    {
        isLastMineFlag = true;
    }

    private void Start()
    {
        AnimState = MiningAnimState.Idle;
        currentAnimTime = 0;
    }



    private void Update()
    {
        HandleMiningAnim();
    }

    private void HandleMiningAnim()
    {
        currentAnimTime += Time.deltaTime;
        switch (AnimState)
        {
            case PickaxeAnimator.MiningAnimState.Idle:
                HandelIdleAnim();
                break;
            case PickaxeAnimator.MiningAnimState.Starting:
                HandelStartingAnim();
                break;
            case PickaxeAnimator.MiningAnimState.MiningOut:
                HandelMiningAnim(PickaxeAnimator.MiningAnimState.MiningOut);
                break;
            case PickaxeAnimator.MiningAnimState.MiningIn:
                HandelMiningAnim(PickaxeAnimator.MiningAnimState.MiningIn);
                break;
            case PickaxeAnimator.MiningAnimState.Stopping:
                HandelStoppingAnim();
                break;
        }
    }

    private void HandelIdleAnim()
    {
        
    }

    private void SetPickaxeRotation(Vector3 angles)
    {
        //Debug.Log(angles);
        this.transform.rotation = Quaternion.Euler(angles);
    }

    private void HandelStartingAnim()
    {
        var time = currentAnimTime / startAnimSpeed;
        Vector3 desiredRotation = this.transform.rotation.eulerAngles;
        
        if (time > 1)
        {
            PickaxeHit();
            if (isLastMineFlag)
            {
                
                isLastMineFlag = false;
                AnimState = MiningAnimState.Stopping;
            }
            else
            {
                AnimState = MiningAnimState.MiningOut;
            }
            desiredRotation.x = diggingAnimAngle;
        }
        else
        {
            desiredRotation.x = Mathf.Lerp(idleAnimAngle,diggingAnimAngle,startAnimCurve.Evaluate(currentAnimTime/startAnimSpeed));
        }

        SetPickaxeRotation(desiredRotation);
    }
    
    private void HandelMiningAnim(MiningAnimState state)
    {
        var time = currentAnimTime / mineAnimSpeed;
        
        Vector3 desiredRotation = this.transform.rotation.eulerAngles;
        
        if (time > 1)
        {
            if (state == MiningAnimState.MiningOut)
            {
                AnimState = MiningAnimState.MiningIn;
                desiredRotation.x = diggingRetractAnimAngle;
            }
            else
            {
                desiredRotation.x = diggingAnimAngle;
                PickaxeHit();
                if (isLastMineFlag)
                {
                    isLastMineFlag = false;
                    AnimState = MiningAnimState.Stopping;
                }
                else
                {
                    AnimState = MiningAnimState.MiningOut;
                }
            }
        }
        else
        {
            if (state == MiningAnimState.MiningOut)
            {
                desiredRotation.x = Mathf.Lerp(diggingAnimAngle,diggingRetractAnimAngle,mineAnimCurve.Evaluate( currentAnimTime/startAnimSpeed));
            }
            else
            {
                desiredRotation.x = Mathf.Lerp(diggingRetractAnimAngle,diggingAnimAngle,mineAnimCurve.Evaluate(currentAnimTime/startAnimSpeed));
            }
        }
        
        SetPickaxeRotation(desiredRotation);
    }
    
    private void HandelStoppingAnim()
    {
        var time = currentAnimTime / startAnimSpeed;
        Vector3 desiredRotation = this.transform.rotation.eulerAngles;
        
        if (time > 1)
        {
            AnimState = MiningAnimState.Idle;
            desiredRotation.x = idleAnimAngle;
        }
        else
        {
            desiredRotation.x = Mathf.Lerp(diggingAnimAngle,idleAnimAngle,startAnimCurve.Evaluate(currentAnimTime/startAnimSpeed));
        }
        
        SetPickaxeRotation(desiredRotation);
    }
}
