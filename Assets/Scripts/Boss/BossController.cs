using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    private enum BossStage
    {
        Hooked, // braccio agganciato
        Attacking, // in fase di attacco
        HookEngaging, // in fase di aggancio
        Exposed, // boss esposto (attaccabile)
        Stunned
    }
    [SerializeField] private BossStage stage;

    [SerializeField] private List<Transform> enviromentGrips;
    [SerializeField] private List<Rigidbody2D> hairHands;

    [SerializeField]
    private float hairHandSpeed;

    float timeToReach = 0;



    const float hookedTime = 2; //quantità di tempo in cui il boss resta hookato al grip

    const float attackingTime = 3; //tempo in fase di attacco
    const float exposedTime = 1.5f; //tempo in cui è esposto

    const float stunnedTime = 1.5f; //tempo in cui è esposto


    /* 
    const float hookedTime = 0;//2; //quantità di tempo in cui il boss resta hookato al grip

    const float attackingTime = 0;//3; //tempo in fase di attacco
    const float exposedTime = 0;//1.5f; //tempo in cui è esposto

    const float stunnedTime = 0;//1.5f; //tempo in cui è esposto 
    */



    private int movingArm = 1;
    private int fixedArm = 0;

    private int selectedGrip = 1;

    public void Start()
    {
        
        stage = BossStage.Hooked;
        movingArm = 1;
        fixedArm = 0;
        selectedGrip = 1;
        ChangeGripWithRandom();
        hairHands[fixedArm].constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
        timeToReach = Time.time + hookedTime;
    }



    public void FixedUpdate()
    {

        if (stage == BossStage.Hooked || stage == BossStage.Attacking || stage == BossStage.Exposed || stage == BossStage.Stunned)
        {

            if (Time.time < timeToReach) {
                HandleTimedBehaviour();
            } else
            {
                ChangeStage();
            }
        } else {
            HandleUntimedBehaviour();
        }
    }

    private void ChangeStage()
    {
        switch (stage)
        {
            case BossStage.Hooked:
                timeToReach = Time.time + attackingTime;
                stage = BossStage.Attacking;
                break;
            case BossStage.Attacking:
                timeToReach = Time.time + exposedTime;
                stage = BossStage.Exposed;
                break;
            case BossStage.HookEngaging:
                timeToReach = Time.time + hookedTime;
                stage = BossStage.Hooked;
                break;
            case BossStage.Exposed:
                stage = BossStage.HookEngaging;
                break;
            case BossStage.Stunned:
                stage = BossStage.HookEngaging;
                break;
        }

        Debug.Log(stage);
    }

    public void ForceStunning()
    {
        stage = BossStage.Stunned;
        timeToReach = Time.time + stunnedTime;
    }

    private void HandleTimedBehaviour()
    {
        switch (stage)
        {
            case BossStage.Hooked:

                break;
            case BossStage.Attacking:

                break;
            case BossStage.Exposed:

                break;
            case BossStage.Stunned:

                break;
        }
    }

    private void HandleUntimedBehaviour()
    {
        if(stage == BossStage.HookEngaging)
        {
            if(Vector2.Distance(enviromentGrips[selectedGrip].position, hairHands[movingArm].transform.position) < 0.1f)
            {

                

                hairHands[movingArm].constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
                hairHands[fixedArm].constraints = RigidbodyConstraints2D.None;

                Debug.Log("reached");

                ChangeGripWithRandom();
                SwitchArm();
                ChangeStage();
            } else
            {

                Vector3 newPosition = Vector3.MoveTowards(
                    hairHands[movingArm].position, enviromentGrips[selectedGrip].position,
                    hairHandSpeed * Time.fixedDeltaTime);
                hairHands[movingArm].MovePosition(newPosition);
            }
            
        }
    }


    private void SwitchArm()
    {
        int t = fixedArm;
        fixedArm = movingArm;
        movingArm = t;
    }

    private void ChangeGripWithRandom()
    {

        if(selectedGrip == enviromentGrips.Count - 1)
        {
            selectedGrip = selectedGrip - 2;
        } else if (selectedGrip == 0)
        {
            selectedGrip = 1;
        } else
        {
            System.Random rand = new System.Random();
            int result;
            do
            {
                result = rand.Next(-1, 1);
            } while (result == 0);

            selectedGrip = selectedGrip + (result);
        }
        //Debug.Log(selectedGrip);
    }
}
