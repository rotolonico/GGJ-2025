using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float _initHealth;

    [SerializeField] private UnityEvent _onDamage;
    [SerializeField] private UnityEvent _onDeath;

    private float _health;
    private bool _isAlive => _health == 0;

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
            _onDamage?.Invoke();
        if (_health <= 0)
        {
            _health = 0;
            _onDeath?.Invoke();
        }
    }
}
