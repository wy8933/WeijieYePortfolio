using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth;
    private float currentHealth;

    public void Awake()
    {
        currentHealth = maxHealth;
        PlayerCurrentHealthUpdate(0);
    }

    /// <summary>
    /// update the max health of player Pod
    /// </summary>
    /// <param name="amount">the amount of changes to player Pod's max health</param>
    public void PlayerMaxHealthUpdate(int amount) {
        maxHealth += amount;
        currentHealth += amount;
    }

    /// <summary>
    /// update the current health of player Pod, and check if player Pod is dead
    /// </summary>
    /// <param name="amount">the amount of changes to player Pod's current health</param>
    public void PlayerCurrentHealthUpdate(float amount) {
        currentHealth += amount;

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            HUD.Instance.UpdateHealth(maxHealth, currentHealth);
            PlayerDestroyed();
        }
        else if(currentHealth >= maxHealth) {
            currentHealth = maxHealth;
        }
        HUD.Instance.UpdateHealth(maxHealth, currentHealth);

    }

    public void PlayerDestroyed() {
        Settings.Instance.GoodwillOverall = GoodWillManager.Instance.Goodwill;
        SceneManager.LoadScene("BadEnd");
    }
}
