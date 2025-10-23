using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class CameraController : MonoBehaviour
{
    CinemachineCamera _virtualCamera;
    public Transform target;

    [Header("PostProcces")]
    public Volume volume;
    ChromaticAberration chromatic;

    [Header("Zoom")]
    [SerializeField] float _zoomSpeed = 2f;
    [SerializeField] float _minZoom = 4f;
    [SerializeField] float _maxZoom = 12f;

    [Header("Rotation")]
    [SerializeField] float _rotationSpeed = 180f;
    [SerializeField] float _rotationChangeAmount = 45f;
    private float _currentRotationY;
    private bool _isRotating = false;
    private float _targetRotationY;

    private void Start()
    {
        _virtualCamera = GetComponent<CinemachineCamera>();
        _currentRotationY = transform.eulerAngles.y;
        _targetRotationY = _currentRotationY;

        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        _virtualCamera.Follow = target;
        _virtualCamera.LookAt = target;

        // PostProces
        volume.profile.TryGet<ChromaticAberration>(out chromatic);
    }

    private void Update()
    {
        HandleZoom();
        HandleInput();
        UpdateRotation();

        PostProcesDeneme();
    }

    public void PostProcesDeneme()
    {
        if (PlayerStats.Instance.currentSanity < 200)
        {
            chromatic.intensity.value = Mathf.PingPong(Time.time, 1f);            
        }
        else
            chromatic.intensity.value -= 0.1f * Time.deltaTime;
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Approximately(scroll, 0f)) return;

        float ortho = _virtualCamera.Lens.OrthographicSize;
        ortho -= scroll * _zoomSpeed;
        ortho = Mathf.Clamp(ortho, _minZoom, _maxZoom);
        _virtualCamera.Lens.OrthographicSize = ortho;
    }

    private void HandleInput()
    {
        if (_isRotating) return;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartRotation(+_rotationChangeAmount);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            StartRotation(-_rotationChangeAmount);
        }
    }

    private void StartRotation(float deltaDegrees)
    {
        _targetRotationY = _targetRotationY + deltaDegrees;
        _targetRotationY = Mathf.Repeat(_targetRotationY, 360f);
        _isRotating = true;
    }

    private void UpdateRotation()
    {
        if (!_isRotating) return;

        _currentRotationY = Mathf.MoveTowardsAngle(_currentRotationY, _targetRotationY, _rotationSpeed * Time.deltaTime);

        Vector3 e = transform.eulerAngles;
        e.y = _currentRotationY;
        transform.eulerAngles = e;

        if (Mathf.Abs(Mathf.DeltaAngle(_currentRotationY, _targetRotationY)) < 0.1f)
        {
            _currentRotationY = _targetRotationY;
            _isRotating = false;
        }
    }
}
