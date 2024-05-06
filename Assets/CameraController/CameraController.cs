using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;

    [Header("Movement settings")]
    [SerializeField, Range(1, 10)]
    private float _cameraSpeed;

    [SerializeField, Range(1, 100)]
    private float _zoomSpeed;

    [Header("Movement limits")]
    [SerializeField]
    private Rect _boundingBox;

    [Header("Zoom limits")]
    [SerializeField]
    private float _minZoom;

    [SerializeField]
    private float _maxZoom;

    public void Move(Vector2 offset)
    {
        Vector2 delta = offset * _cameraSpeed;
        Vector3 newPosition = transform.position + (Vector3)delta;
        newPosition.x = Mathf.Clamp(newPosition.x, _boundingBox.position.x, _boundingBox.position.x + _boundingBox.size.x); 
        newPosition.y = Mathf.Clamp(newPosition.y, _boundingBox.position.y, _boundingBox.position.y + _boundingBox.size.y); 
        transform.position = newPosition;
    }

    public void Zoom(float offset)
    {
        float delta = offset * _zoomSpeed;
        float newZoom = _camera.fieldOfView + delta;
        newZoom = Mathf.Clamp(newZoom, _maxZoom, _minZoom);
        _camera.fieldOfView = newZoom;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_boundingBox.center, _boundingBox.size);
    }
}
