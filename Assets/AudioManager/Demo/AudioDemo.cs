using Managers;
using UnityEngine;

public class AudioDemo : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            GameManager.Instance.AudioManager.PlaySFX("Complete");

        if (Input.GetKeyDown(KeyCode.W))
            GameManager.Instance.AudioManager.PlaySFX("Bling");

        if (Input.GetKeyDown(KeyCode.E))
            GameManager.Instance.AudioManager.PlaySFX("Tab");

        if (Input.GetKeyDown(KeyCode.R))
            GameManager.Instance.AudioManager.PlaySFX("Bonus");
    }
}
