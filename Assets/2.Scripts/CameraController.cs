using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private UIManager _uiManager;
    [Header("Camera Setting")]
    [SerializeField] private float _defaultSize = 7f;
    [SerializeField] private float _zoomSpeed = 5f;
    [Space(10)]
    [Header("Planet Distance Setting")]
    [SerializeField] private float _zoomOutStartDistance = -1f;
    [Header("Sound Setting")]
    [SerializeField] private float _volumeMinDist = 3f;

    private Camera _camera;

    private float _targetSize;
    private float _curMaxPlanetDistance = 0f;
    private Transform _currentMaxDistancePlanet;

    // singleton
    private static CameraController _instance;
    public static CameraController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<CameraController>();
            }

            return _instance;
        }
    }

    private CameraConfigurer _cameraConfigurer;


    
    private void Start()
    {
        _camera = Camera.main;
        _curMaxPlanetDistance = -99f;
    }

    private void SetCamSize()
    {
        _defaultSize = _cameraConfigurer.DefaultSize;
        ResetCamTargetSize();
    }

    private void LateUpdate()
    {
        if(_curMaxPlanetDistance > _zoomOutStartDistance)
        {
            _targetSize = _defaultSize + _curMaxPlanetDistance - _zoomOutStartDistance;
        }
        else
        {
            _targetSize = _defaultSize;
        }

        _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, _targetSize, Time.deltaTime * _zoomSpeed);

        if(!GameManager.Instance.IsGameOver)
            SoundManager.Instance.SetMasterVolume(Mathf.InverseLerp(_defaultSize + _volumeMinDist, _defaultSize, _targetSize));

        _uiManager.SetUIPos();
    }

    public void SetPlanetDistance(Transform planet)
    {
        float distance = _cameraConfigurer.DistanceFromBorder(planet);
        //print(distance);
        if (_currentMaxDistancePlanet == null || (distance > _curMaxPlanetDistance))
        {
            _currentMaxDistancePlanet = planet;
            _curMaxPlanetDistance = distance;
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.OnReload += ResetCamTargetSize;

        _cameraConfigurer = GetComponent<CameraConfigurer>();
        _cameraConfigurer.OnAspectChanged += SetCamSize;
    }

    private void OnDisable()
    {
        if(GameManager.Instance != null)
            GameManager.Instance.OnReload -= ResetCamTargetSize; 

        if (_cameraConfigurer != null)
            _cameraConfigurer.OnAspectChanged -= SetCamSize;
    }

    public void ResetCamTargetSize(PlanetData _ = null)
    {
        _currentMaxDistancePlanet = null;
        _curMaxPlanetDistance = -99f;
    }
}
