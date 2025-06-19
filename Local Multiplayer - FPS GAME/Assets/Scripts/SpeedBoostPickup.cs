using UnityEngine;

public class SpeedBoostPickup : MonoBehaviour
{
    [Header("Speed Boost Settings")]
    public float speedBoostAmount = 3f;
    public float boostDuration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        MonoBehaviour playerScript = null;

        // Check if the collider has PlayerController
        PlayerController player1 = other.GetComponent<PlayerController>();
        if (player1 != null)
        {
            playerScript = player1;
        }

        // Check if the collider has Player2Controller
        Player2Controller player2 = other.GetComponent<Player2Controller>();
        if (player2 != null)
        {
            playerScript = player2;
        }

        // If we found a valid player script, apply the boost
        if (playerScript != null)
        {
            StartCoroutine(ApplySpeedBoost(playerScript));
            gameObject.SetActive(false); // Disable pickup after use
        }
    }

    private System.Collections.IEnumerator ApplySpeedBoost(MonoBehaviour playerScript)
    {
        float originalSpeed = 0f;

        // Try to call the right Get/Set methods based on type
        if (playerScript is PlayerController player1)
        {
            originalSpeed = player1.GetMoveSpeed();
            player1.SetMoveSpeed(originalSpeed + speedBoostAmount);
            yield return new WaitForSeconds(boostDuration);
            player1.SetMoveSpeed(originalSpeed);
        }
        else if (playerScript is Player2Controller player2)
        {
            originalSpeed = player2.GetMoveSpeed();
            player2.SetMoveSpeed(originalSpeed + speedBoostAmount);
            yield return new WaitForSeconds(boostDuration);
            player2.SetMoveSpeed(originalSpeed);
        }

        Destroy(gameObject); // Optional: remove pickup completely
    }
}
