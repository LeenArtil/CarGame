// 🧱 ObstacleCollisionHandler.cs
using UnityEngine;

public class ObstacleCollisionHandler : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("💥 Player hit obstacle!");

            // Stop score and compare
            ScoreManager.Instance.StopAndCompare();

            // Return to menu (optional)
            MainMenuReturner.Instance.ReturnToMainMenu();
        }
    }
}
