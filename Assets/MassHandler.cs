using UnityEngine;

public class MassHandler : MonoBehaviour
{
    [SerializeField] private Collider2D boxCollider2D;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    public void DisableMass()
    {
        boxCollider2D.enabled = false;
        spriteRenderer.enabled = false;
    }
}
