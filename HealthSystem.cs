using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;
    public bool isAlive = true;

    [Header("UI Reference")]
    public UIManager uiManager;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (!isAlive) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (uiManager != null)
            uiManager.UpdateHealthUI(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (!isAlive) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (uiManager != null)
            uiManager.UpdateHealthUI(currentHealth, maxHealth);
    }

    public void Die()
    {
        isAlive = false;
        GetComponent<PlayerController>().enabled = false;
        GetComponent<Animator>().SetTrigger("Death");
        Invoke("DestroyPlayer", 3f);
    }

    void DestroyPlayer()
    {
        Destroy(gameObject);
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public float GetHealthPercent()
    {
        return (currentHealth / maxHealth) * 100f;
    }
}
