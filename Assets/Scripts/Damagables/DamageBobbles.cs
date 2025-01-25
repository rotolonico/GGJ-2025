using UnityEngine;

public class DamageBobbles : MonoBehaviour, IBobble
{


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other != null) { }

        ApplyEffect();   
    }
    public void ApplyEffect()
    {
        
    }
}
