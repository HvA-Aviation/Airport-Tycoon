using UnityEngine;

namespace Features.UI
{
    public class UIToggle : MonoBehaviour
    {
        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}