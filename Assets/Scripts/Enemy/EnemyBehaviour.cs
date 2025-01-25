using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemy
{
    public class EnemyBehaviour : MonoBehaviour
    {
        private const float distanceEpsilon = 15f;
        
        public enum EnemyState
        {
            IDLE,
            AIMING,
            ATTACK
        }
        
        private EnemyState state;
        
        [SerializeField] private float speed;

        private Transform target;

        private bool isDead;

        private void Start()
        {
            target = GameObject.FindWithTag("Player").transform;
        }

        private void Update()
        {
            var distanceToPlayer = Vector2.Distance(transform.position, target.position);
            if (distanceToPlayer < distanceEpsilon)
            {
                // Raycast line and see if there's a wall between the player and the enemy
                var hit = Physics2D.Raycast(transform.position, target.position - transform.position, distanceToPlayer);
                if (hit.collider != null && hit.collider.CompareTag("Wall"))
                {
                    ChangeState(EnemyState.IDLE);
                }
                else
                {
                    ChangeState(EnemyState.AIMING);
                }
            }
            else
            {
                // Move towards the player
                var direction = (target.position - transform.position).normalized;
                transform.position += direction * (speed * Time.deltaTime);
            }
        }

        private void OnDrawGizmos()
        {
            // Draw circle debug
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, distanceEpsilon);
        }
        
        private void ChangeState(EnemyState newState)
        {
            state = newState;
        }
    }
}