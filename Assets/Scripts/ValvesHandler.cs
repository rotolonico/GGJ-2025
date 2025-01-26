using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ValvesHandler : MonoBehaviour
{
    public static int maxValves = 4;
    public static int totalValves = 4;
    
    [SerializeField] private TextMeshProUGUI valvesText;
    
    private List<Collider2D> hitColliders = new List<Collider2D>();

    private void UpdateValveText() => valvesText.text = $"{maxValves - totalValves}/{maxValves}";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Valve"))
        {
            if (hitColliders.Contains(other)) return;
            hitColliders.Add(other);
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
