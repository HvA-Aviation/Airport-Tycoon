using UnityEngine;

namespace Features.AudioManager.Demo
{
    public class CameraDemo : MonoBehaviour
    {
        [SerializeField]
        private CameraController.CameraController _controller;

        void Update()
        {
            int xMovement = 0;
            if (Input.GetKey(KeyCode.D)) xMovement++;
            if (Input.GetKey(KeyCode.A)) xMovement--;

            int yMovement = 0;
            if(Input.GetKey(KeyCode.W)) yMovement++;
            if(Input.GetKey(KeyCode.S)) yMovement--;

            _controller.Move(new Vector2(xMovement, yMovement) * Time.deltaTime);

            _controller.Zoom(-Input.mouseScrollDelta.y * Time.deltaTime);
        }
    }
}
