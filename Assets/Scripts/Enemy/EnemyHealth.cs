using System.Collections;
using Enemy;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float _initHealth;

    [SerializeField] private UnityEvent _onDamage;
    [SerializeField] private UnityEvent _onDeath;

    [SerializeField] private SpriteRenderer sr;

    private float _health;
    private bool _isAlive => _health != 0;

    private void Start()
    {
        _health = _initHealth;
    }

    public void TakeDamage(float damage)
    {
        if (!_isAlive)
            return;



        Debug.Log("Oh no!");
        _health -= damage;

        if (_health > 0)
        {
            _onDamage?.Invoke();
            StartCoroutine(BlinkRed());
        }
        
        if (_health <= 0)
        {
            _health = 0;
            _onDeath?.Invoke();
            GetComponent<EnemyBehaviour>().Die();
        }
    }
    
    private IEnumerator BlinkRed()
    {
        for (int i = 0; i < 3; i++)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Revive()
    {
        _health = _initHealth;
    }
}
