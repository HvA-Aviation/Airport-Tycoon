using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;

    [Header("Movement settings")]
    [SerializeField, Range(1, 10)]
    private float _cameraSpeed;

    [SerializeField, Range(1, 10)]
    private float _zoomSpeed;

    [Header("Movement limits")]
    [SerializeField]
    private Vector2Int _BoundingBoxOffset;

    [SerializeField]
    private Vector2Int _BoundingBoxSize;

    [Header("Zoom limits")]
    [SerializeField]
    private float _minZoom;

    [SerializeField]
    private float _maxZoom;

    public void Move(Vector2 offset)
    {
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = (Vector2)_BoundingBoxSize * .5f + _BoundingBoxOffset;
        Gizmos.DrawWireCube(center, (Vector2)_BoundingBoxSize);
    }
}
