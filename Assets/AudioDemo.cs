using UnityEngine;

public class AudioDemo : MonoBehaviour
{
    [SerializeField]
    private AudioManager _manager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            _manager.PlaySFX("Complete");

        if (Input.GetKeyDown(KeyCode.W))
            _manager.PlaySFX("Bling");

        if (Input.GetKeyDown(KeyCode.E))
            _manager.PlaySFX("Tab");

        if (Input.GetKeyDown(KeyCode.R))
            _manager.PlaySFX("Bonus");
    }
}
