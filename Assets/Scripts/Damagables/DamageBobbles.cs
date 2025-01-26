using UnityEngine;
using UnityEngine.TextCore;

public class DamageBobbles : MonoBehaviour, IBobble
{
    [SerializeField] private float _dmgPerParticle;
    [SerializeField] private ParticleSystem _particles;

    private EnemyHealth _enemy;

    public Color color { get; set; } = Color.white;

    private void OnParticleCollision(GameObject other)
    {
        if (!other.CompareTag(Constants.ENEMY_TAG))
            return;

        _enemy = other.GetComponent<EnemyHealth>();
        _enemy ??= other.GetComponentInParent<BossController>().currentEnemy;
        _enemy.TakeDamage(_dmgPerParticle);
    }

    public void ApplyEffect(bool isShooting)
    {
        if (isShooting)
            _particles.Play();
        else
            _particles.Stop();
    }
}
