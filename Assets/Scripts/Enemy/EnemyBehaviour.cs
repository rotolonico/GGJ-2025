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

        [SerializeField] private Animator animator;
        [SerializeField] private Animation attackAnimation;
        [SerializeField] private Animation tiredAnimation;
        [SerializeField] private Animation idleAnimation;
        [SerializeField] private Animation aimingAnimation;

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

        [field: SerializeField] private Transform target { get; set; }
        private Rigidbody2D rb;
        private bool isDead;

        private Vector2 idleDirection;
        private Vector2 attackDirection;

        private void Start()
        {
            //target = GameObject.FindWithTag("Player").transform;
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
            if (target == null)
                return;
            
            if (isDead)
                return;

            var distanceToPlayer = Vector2.Distance(transform.position, target.position);

            if (state == EnemyState.IDLE)
            {
                if (distanceToPlayer < distanceEpsilon && IsSameRoomAsPlayer())
                {
                    StartAiming();
                }
            }

            switch (state)
            {
                case EnemyState.IDLE:
                    rb.linearVelocity = idleDirection * idleSpeed;
                    transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(idleDirection.y, idleDirection.x) * Mathf.Rad2Deg);
                    sr.color = Color.white;
                    break;
                case EnemyState.AIMING:
                    rb.linearVelocity = Vector2.zero; // Stop movement while aiming
                    attackDirection = (target.position - transform.position).normalized;
                    transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg);
                    break;
                case EnemyState.ATTACK:
                    rb.linearVelocity = attackDirection * attackSpeed;
                    break;
                case EnemyState.TIRED:
                    rb.linearVelocity = Vector2.zero; // Stop moving when tired
                    break;
            }
        }

        private bool IsSameRoomAsPlayer()
        {
            var playerRoom = CameraRoomFollower.GetRoomClampedPosition(target.position, RoomHandler.RoomSize);
            var enemyRoom = CameraRoomFollower.GetRoomClampedPosition(transform.position, RoomHandler.RoomSize);

            return playerRoom == enemyRoom;
        }

        private void StartAiming()
        {
            StartCoroutine(AimingSequence());
        }

        private IEnumerator AimingSequence()
        {
            state = EnemyState.AIMING;
            animator.Play("RatAiming");
            yield return new WaitForSecondsRealtime(aimingTime);
            state = EnemyState.ATTACK;
            animator.Play("RatAttack");
            yield return new WaitForSecondsRealtime(attackingTime);
            state = EnemyState.TIRED;
            animator.Play("RatTired");
            yield return new WaitForSecondsRealtime(tiredTime);
            state = EnemyState.IDLE;
            animator.Play("RatIdle");
            SetRandomIdleDirection();
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
                // Reflect 90 degrees

                if (state == EnemyState.IDLE)
                {
                    idleDirection = Vector2.Reflect(idleDirection, other.GetContact(0).normal);
                }
                else if (state == EnemyState.ATTACK)
                {
                    attackDirection = Vector2.Reflect(attackDirection, other.GetContact(0).normal);
                }
            }
        }

        public void SetTarget(Transform target)
        {
            this.target = target;
        }

        public void Die()
        {
            Destroy(gameObject);
            
            if (isDead)
                return;

            isDead = true;
            animator.Play("RatDead");
            
            StartCoroutine(Revive());
        }
        
        private IEnumerator Revive()
        {
            yield return new WaitForSeconds(5);
            animator.Play("RatIdle");
            GetComponent<EnemyHealth>().Revive();
            isDead = false;
        }
    }
}