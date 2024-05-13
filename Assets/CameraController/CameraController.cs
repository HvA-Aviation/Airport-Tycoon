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
    [SerializeField, Tooltip("The furthest the camera can zoom out (Highest FOV).")]
    private float _minZoom;

    [SerializeField, Tooltip("The furthest the camera can zoom in (Lowest FOV).")]
    private float _maxZoom;

    /// <summary>
    /// Moves the camera within the movement boundaries.
    /// </summary>
    /// <param name="offset">The amount of movement to apply.</param>
    public void Move(Vector2 offset)
    {
        Vector2 delta = offset * _cameraSpeed;
        Vector3 newPosition = transform.position + (Vector3)delta;
        newPosition.x = Mathf.Clamp(newPosition.x, _boundingBox.position.x, _boundingBox.position.x + _boundingBox.size.x); 
        newPosition.y = Mathf.Clamp(newPosition.y, _boundingBox.position.y, _boundingBox.position.y + _boundingBox.size.y); 
        transform.position = newPosition;
    }

    /// <summary>
    /// Zooms the camera within the zoom boundaries.
    /// </summary>
    /// <param name="offset">The amount of zoom to apply.</param>
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
