using System;
using UnityEngine;

public class ValveMover : MonoBehaviour
{
    private void Update()
    {
        if (ValvesHandler.totalValves <= 0)
        {
            Destroy(gameObject);
        }
    }
}
