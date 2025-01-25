using UnityEngine;

public class DamageBobbles : MonoBehaviour, IBobble
{

    private void OnParticleCollision(GameObject other)
    {
        if (!other.CompareTag(Constants.ENEMY_TAG))
            return;

        ApplyEffect();   
    }


    public void ApplyEffect()
    {
                
    }
}
