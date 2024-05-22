using UnityEngine;
using Cursor = Features.Building.Scripts.Grid.Cursor;

namespace Features.Building.Scripts.Demos
{
    public class CursorRotationDemo : MonoBehaviour
    {
        [SerializeField] private Cursor _cursor;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
                _cursor.Rotate(Input.GetKeyDown(KeyCode.Q) ? -1 : 1);
        }
    }
}