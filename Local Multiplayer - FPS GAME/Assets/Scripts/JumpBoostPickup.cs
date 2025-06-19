using UnityEngine;

public class JumpBoostPickup : MonoBehaviour
{
    [Header("Jump Boost Settings")]
    public float jumpBoostAmount = 5f;
    public float boostDuration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        MonoBehaviour playerScript = null;

        // Check for PlayerController
        PlayerController player1 = other.GetComponent<PlayerController>();
        if (player1 != null)
        {
            playerScript = player1;
        }

        // Check for Player2Controller
        Player2Controller player2 = other.GetComponent<Player2Controller>();
        if (player2 != null)
        {
            playerScript = player2;
        }

        if (playerScript != null)
        {
            StartCoroutine(ApplyJumpBoost(playerScript));
            gameObject.SetActive(false); // Disable pickup after use
        }
    }

    private System.Collections.IEnumerator ApplyJumpBoost(MonoBehaviour playerScript)
    {
        float originalJump = 0f;

        if (playerScript is PlayerController player1)
        {
            originalJump = player1.GetJumpForce();
            player1.SetJumpForce(originalJump + jumpBoostAmount);
            yield return new WaitForSeconds(boostDuration);
            player1.SetJumpForce(originalJump);
        }
        else if (playerScript is Player2Controller player2)
        {
            originalJump = player2.GetJumpForce();
            player2.SetJumpForce(originalJump + jumpBoostAmount);
            yield return new WaitForSeconds(boostDuration);
            player2.SetJumpForce(originalJump);
        }

        Destroy(gameObject); // Optional: delete the pickup
    }
}
