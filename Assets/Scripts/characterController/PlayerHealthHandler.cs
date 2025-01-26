using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(-100)]
public class PlayerHealthHandler : MonoBehaviour
{
    public int maxHealth = 5;
    public int currentHealth = 5;
    
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private SpriteRenderer[] spriteRenderers;

    [SerializeField] public UnityEvent onDeathEvent;

    private bool invincibilityFrames = false;
    private bool onPlatform;

    public static UnityAction onDeath;

    private void Awake()
    {
        onDeath = null;
    }

    private void Update()
    {
        healthText.text = $"{currentHealth}/{maxHealth}";
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Health"))
        {
            Heal(1);
            Destroy(other.gameObject);
        }
        
        if (other.gameObject.CompareTag("Platform"))
        {
            onPlatform = true;
        }
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Void") && !onPlatform)
        {
            TakeDamage(1);
            transform.position = AnchorInstance.AnchorPosition;
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            onPlatform = false;
        }
    }

    public void TakeDamage(int damage)
    {
        if (invincibilityFrames)
            return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }

        StartCoroutine(ActivateInvincibilityFrames());
    }

    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        StartCoroutine(HealFlashing());
    }

    private IEnumerator HealFlashing()
    {
        for (var i = 0; i < 5; i++)
        {
            foreach (var spriteRenderer in spriteRenderers) spriteRenderer.color = new Color(0, 1, 0, 1);
            yield return new WaitForSeconds(0.1f);
            foreach (var spriteRenderer in spriteRenderers) spriteRenderer.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator ActivateInvincibilityFrames()
    {
        invincibilityFrames = true;

        // flash the player sprite
        for (var i = 0; i < 5; i++)
        {
            foreach (var spriteRenderer in spriteRenderers) spriteRenderer.color = new Color(1, 1, 1, 0);
            yield return new WaitForSeconds(0.1f);
            foreach (var spriteRenderer in spriteRenderers) spriteRenderer.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.1f);
        }

        invincibilityFrames = false;
    }

    private void Die()
    {
        onDeathEvent?.Invoke();
        onDeath?.Invoke();
        gameObject.SetActive(false);
        // TODO, connect to gamehandler.Die
    }
}