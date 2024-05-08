using System;
using UnityEngine;

namespace Building.Demos
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