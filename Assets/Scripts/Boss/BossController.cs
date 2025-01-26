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
        Stunned,
        Died
    }
    [SerializeField] private BossStage stage;

    [Header("References")]
    [SerializeField] private List<Transform> enviromentGrips;
    [SerializeField] private List<Rigidbody2D> hairHands;
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private SpriteRenderer currentEye;

    [SerializeField] private Animation pupilAnimation;

    [SerializeField] private Transform exclamationMark;

    [Header("Parameters")]
    [SerializeField] private float hairHandSpeed;
    [SerializeField] private float projectileAngle = 30f; 
    [SerializeField] private float projectileSpeed;

    [Header("Sprites")]
    [SerializeField] private Sprite halfOpenedEye;
    [SerializeField] private Sprite openedEye;

    [Header("Prefabs")]
    [SerializeField] private GameObject projectilePrefab;

    float timeToReach = 0;

    const float hookedTime = 2; //quantità di tempo in cui il boss resta hookato al grip

    const float attackingTime = 1; //tempo in fase di attacco
    const float exposedTime = 2.5f; //tempo in cui è esposto

    const float stunnedTime = 2f; //tempo in cui è esposto


    // fasi del boss
    int bossPhases = 3;
    int currentBossPhases = 0;


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


        // mantiene punto esclamativo
        exclamationMark.transform.position = bodyTransform.position + new Vector3(0, 18, 0);

        // Fissa la rotazione, assicurando che rimanga sempre dritta
        exclamationMark.transform.rotation = Quaternion.identity;
    }

    private void ChangeStage()
    {
        switch (stage)
        {
            case BossStage.Hooked:
                timeToReach = Time.time + attackingTime;
                stage = BossStage.Attacking;
                exclamationMark.gameObject.SetActive(false);
                currentEye.sprite = halfOpenedEye;
                pupilAnimation.gameObject.SetActive(false);
                SpawnProjectiles();
                break;
            case BossStage.Attacking:
                timeToReach = Time.time + exposedTime;
                stage = BossStage.Exposed;
                exclamationMark.gameObject.SetActive(false);
                currentEye.sprite = openedEye;
                pupilAnimation.gameObject.SetActive(true);
                pupilAnimation.Rewind();
                pupilAnimation.Stop();
                break;
            case BossStage.HookEngaging:
                timeToReach = Time.time + hookedTime;
                stage = BossStage.Hooked;
                exclamationMark.gameObject.SetActive(true);
                currentEye.sprite = halfOpenedEye;
                pupilAnimation.gameObject.SetActive(false);
                break;
            case BossStage.Exposed:
                stage = BossStage.HookEngaging;
                exclamationMark.gameObject.SetActive(false);
                currentEye.sprite = halfOpenedEye;
                pupilAnimation.gameObject.SetActive(false);
                break;
            case BossStage.Stunned:
                stage = BossStage.HookEngaging;
                exclamationMark.gameObject.SetActive(false);
                currentEye.sprite = halfOpenedEye;
                pupilAnimation.gameObject.SetActive(false);
                break;
        }

        Debug.Log(stage);
    }

    public void ForceStunning()
    {
        timeToReach = Time.time + stunnedTime;
        stage = BossStage.Stunned;
        currentEye.sprite = openedEye;
        pupilAnimation.gameObject.SetActive(true);
        pupilAnimation.Rewind();
        pupilAnimation.Play();
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

    Vector3[] GetShootingDirections()
    {
        // Direzione globale verso il basso
        Vector3 downDirection = Vector3.down;

        // Rotazione per ottenere le direzioni oblique
        Quaternion leftRotation = Quaternion.Euler(0, 0, projectileAngle);
        Quaternion rightRotation = Quaternion.Euler(0, 0, -projectileAngle);

        Vector3 leftDirection = leftRotation * downDirection;  // Inclinata a sinistra
        Vector3 rightDirection = rightRotation * downDirection; // Inclinata a destra

        return new Vector3[]
        {
            downDirection,   // Giù dritto
            leftDirection,   // Obliqua sinistra
            rightDirection   // Obliqua destra
        };
    }

    void SpawnProjectiles()
    {
        // Otteniamo le tre direzioni
        Vector3[] directions = GetShootingDirections();

        foreach (Vector3 direction in directions)
        {
            // Instanzia il proiettile nella posizione dell'oggetto
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.transform.position = bodyTransform.position;

            projectile.GetComponent<BossProjectile>().SetProjectile(direction, projectileSpeed);


        }
    }

    void OnDrawGizmos()
    {
        Vector3[] directions = GetShootingDirections();
        Gizmos.color = Color.green;

        foreach (Vector3 direction in directions)
        {
            Gizmos.DrawLine(bodyTransform.position, bodyTransform.position + direction * 17f);  // Linea più lunga
        }
    }

    [ContextMenu("IncrementPhase")]
    public void IncrementPhase() {

        currentBossPhases++;
        ForceStunning();
        if (currentBossPhases == bossPhases)
        {
            Debug.Log("boss is death");
            stage = BossStage.Died;

            //caduta boss
            hairHands[0].constraints = RigidbodyConstraints2D.None;
            hairHands[1].constraints = RigidbodyConstraints2D.None;
        }
    }
}
