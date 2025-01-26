using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private bool isBoss = false;
    [SerializeField] private float _initHealth;

    [SerializeField] private UnityEvent _onDamage;
    [SerializeField] private UnityEvent _onDeath;

    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private List<SpriteRenderer> renderers = new List<SpriteRenderer>();

    Coroutine blinking = null;

    private float _health;
    private bool _isAlive => _health != 0;

    private void OnEnable()
    {
        _health = _initHealth;
    }

    public void TakeDamage(float damage)
    {
        if (!_isAlive)
            return;

        _health -= damage;

        if (_health > 0)
        {
            _onDamage?.Invoke();

            if (blinking == null)
                blinking = StartCoroutine(BlinkRed());
        }

        if (_health <= 0)
        {
            _health = 0;

            _onDeath?.Invoke();
            GetComponent<EnemyBehaviour>()?.Die();
        }
    }

    private IEnumerator BlinkRed()
    {
        for (int i = 0; i < 3; i++)
        {
            if (isBoss)
            {
                foreach (var item in renderers)
                    item.color = Color.red;
            }
            else
                sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);


            if (isBoss)
            {
                foreach (var item in renderers)
                    item.color = Color.white;
            }
            else
                sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }

        blinking = null;
    }

    public void Revive()
    {
        _health = _initHealth;
    }
}
