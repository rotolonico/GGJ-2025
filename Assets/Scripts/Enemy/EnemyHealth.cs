using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float _initHealth;
    [SerializeField] private float _health;

    [SerializeField] private UnityEvent _onDeath;

    private bool _isAlive => _health < 0;

    private void Start()
    {
        _health = _initHealth;
    }

    public void TakeDamage(float damage)
    {
        if (_isAlive)
            return;

        _health -= damage;

        if (_health < 0)
            _onDeath?.Invoke();
    }
}
