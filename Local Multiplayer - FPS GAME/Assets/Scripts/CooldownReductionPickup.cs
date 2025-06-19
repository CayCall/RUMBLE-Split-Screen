using UnityEngine;
using System.Collections;

public class CooldownReductionPickup : MonoBehaviour
{
    [Header("Cooldown Reduction Settings")]
    public float reducedCooldown = 1f;
    public float boostDuration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        TileController tileController = other.GetComponent<TileController>();

        if (tileController != null)
        {
            StartCoroutine(ApplyCooldownReduction(tileController));
            gameObject.SetActive(false); // Hide or disable the pickup visually
        }
    }

    private IEnumerator ApplyCooldownReduction(TileController tileController)
    {
        // Store the original cooldown
        float originalCooldown = tileController.GetCooldown();

        // Apply reduced cooldown
        tileController.SetCooldown(reducedCooldown);

        // Wait for duration
        yield return new WaitForSeconds(boostDuration);

        // Restore original cooldown
        tileController.SetCooldown(originalCooldown);

        Destroy(gameObject); // Optional: destroy the powerup object
    }
}
