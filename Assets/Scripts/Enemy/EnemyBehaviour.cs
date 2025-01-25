using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class EnemyBehaviour : MonoBehaviour
    {
        private const float distanceEpsilon = 15f;
        private const float aimingTime = 1f;
        private const float attackingTime = 1f;
        private const float tiredTime = 1f;

        private bool delay;

        private SpriteRenderer sr;

        public enum EnemyState
        {
            IDLE,
            AIMING,
            ATTACK,
            TIRED
        }

        private EnemyState state;

        [SerializeField] private float attackSpeed;
        [SerializeField] private float idleSpeed;

        private Transform target;
        private Rigidbody2D rb;
        private bool isDead;

        private Vector2 idleDirection;
        private Vector2 attackDirection;

        private void Start()
        {
            target = GameObject.FindWithTag("Player").transform;
            rb = GetComponent<Rigidbody2D>();
            sr = GetComponent<SpriteRenderer>();

            if (rb == null)
            {
                Debug.LogError("Rigidbody2D component is missing.");
            }

            SetRandomIdleDirection();
        }

        private void SetRandomIdleDirection()
        {
            float randomAngle = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad;
            idleDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)).normalized;
        }

        private void Update()
        {
            var distanceToPlayer = Vector2.Distance(transform.position, target.position);

            if (state == EnemyState.IDLE)
            {
                if (distanceToPlayer < distanceEpsilon)
                {
                    // Raycast line and see if there's a wall between the player and the enemy
                    var hit = Physics2D.Raycast(transform.position, target.position - transform.position, distanceToPlayer);
                    if (hit.collider == null || !hit.collider.CompareTag("Wall"))
                    {
                        StartAiming();
                    }
                }
            }

            switch (state)
            {
                case EnemyState.IDLE:
                    rb.linearVelocity = idleDirection * idleSpeed;
                    sr.color = Color.white;
                    break;
                case EnemyState.AIMING:
                    rb.linearVelocity = Vector2.zero; // Stop movement while aiming
                    attackDirection = (target.position - transform.position).normalized;
                    sr.color = Color.blue;
                    break;
                case EnemyState.ATTACK:
                    rb.linearVelocity = attackDirection * attackSpeed;
                    sr.color = Color.red;
                    break;
                case EnemyState.TIRED:
                    rb.linearVelocity = Vector2.zero; // Stop moving when tired
                    sr.color = Color.yellow;
                    break;
            }
        }

        private void StartAiming()
        {
            StartCoroutine(AimingSequence());
        }

        private IEnumerator AimingSequence()
        {
            state = EnemyState.AIMING;
            yield return new WaitForSecondsRealtime(aimingTime);
            state = EnemyState.ATTACK;
            yield return new WaitForSecondsRealtime(attackingTime);
            state = EnemyState.TIRED;
            yield return new WaitForSecondsRealtime(tiredTime);
            state = EnemyState.IDLE;
        }

        private void OnDrawGizmos()
        {
            // Draw circle debug
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, distanceEpsilon);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            // If wall is hit, reverse idle direction
            if (!other.collider.CompareTag("Player"))
            {
                idleDirection = -idleDirection;
            }
        }
    }
}