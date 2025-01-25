using System.Collections;
using UnityEngine;

namespace Enemy
{
    public class EnemyBehaviour : MonoBehaviour
    {
        private const float DETECTION_RADIUS = 15f;
        private const float AIM_TIME = 1f;
        private const float ATTACK_TIME = 1f;
        private const float TIRED_TIME = 1f;

        private enum EnemyState { IDLE, AIMING, ATTACK, TIRED }

        [SerializeField] private float attackSpeed = 5f;
        [SerializeField] private float idleSpeed = 2f;

        private Transform player;
        private Rigidbody2D rb;
        private EnemyState state = EnemyState.IDLE;
        private Vector2 idleDirection;

        private void Start()
        {
            player = GameObject.FindWithTag("Player")?.transform;
            rb = GetComponent<Rigidbody2D>();
            SetRandomIdleDirection();
        }

        private void Update()
        {
            if (player == null || rb == null) return;

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (state == EnemyState.IDLE && distanceToPlayer < DETECTION_RADIUS)
            {
                if (!Physics2D.Raycast(transform.position, player.position - transform.position, distanceToPlayer, LayerMask.GetMask("Wall")))
                {
                    StartCoroutine(AimingAndAttackSequence());
                }
            }

            HandleMovement();
        }

        private void HandleMovement()
        {
            switch (state)
            {
                case EnemyState.IDLE:
                    rb.linearVelocity = idleDirection * idleSpeed;
                    break;
                case EnemyState.AIMING:
                case EnemyState.TIRED:
                    rb.linearVelocity = Vector2.zero;
                    break;
                case EnemyState.ATTACK:
                    rb.linearVelocity = (player.position - transform.position).normalized * attackSpeed;
                    break;
            }
        }

        private IEnumerator AimingAndAttackSequence()
        {
            state = EnemyState.AIMING;
            yield return new WaitForSeconds(AIM_TIME);
            state = EnemyState.ATTACK;
            yield return new WaitForSeconds(ATTACK_TIME);
            state = EnemyState.TIRED;
            yield return new WaitForSeconds(TIRED_TIME);
            state = EnemyState.IDLE;
        }

        private void SetRandomIdleDirection()
        {
            float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
            idleDirection = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, DETECTION_RADIUS);
        }
    }
}