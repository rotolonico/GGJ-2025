using UnityEngine;

public class DamageBobbles : MonoBehaviour, IBobble
{

    [SerializeField] private float _dmgPerParticle;

    private EnemyHealth _enemy;

    private void OnParticleCollision(GameObject other)
    {
        if (!other.CompareTag(Constants.ENEMY_TAG))
            return;

        _enemy = other.GetComponent<EnemyHealth>();
        ApplyEffect();   
    }


    public void ApplyEffect()
    {
        _enemy.TakeDamage(_dmgPerParticle);
    }
}
