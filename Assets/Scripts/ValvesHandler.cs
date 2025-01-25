using System;
using TMPro;
using UnityEngine;

public class ValvesHandler : MonoBehaviour
{
    public static int totalValves = 4;
    
    [SerializeField] private TextMeshPro valvesText;

    private void UpdateValveText() => valvesText.text = $"{totalValves}";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Valve"))
        {
            totalValves--;
            other.GetComponent<ValveHandler>().ActivateValve();
            UpdateValveText();
        }
    }
    
    private void Start()
    {
        UpdateValveText();
    }
    
    public bool AllValvesClosed() => totalValves <= 0;
}
