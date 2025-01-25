using UnityEngine;

public interface IBobble 
{
    public Color color { get; set; }  
    public void ApplyEffect(bool isShooting);
}
