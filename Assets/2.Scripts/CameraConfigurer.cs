using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraConfigurer : MonoBehaviour
{
    public System.Action OnAspectChanged;
    [SerializeField] private GravityField _gravityField;
    [SerializeField] private Transform _shootPos;
    [SerializeField] private float _shootPosRadius = 0.5f;
    [SerializeField] private float _padding = 1f;

    private float _currentAspect;
    private float _defaultSize;
    Vector2 _cameraHalfSize;

    public float DefaultSize => _defaultSize;

    private float ConfigureCamSettings()
    {
        var cam = Camera.main;

        var minLongLength = (Vector3.Distance(_gravityField.transform.position, _shootPos.position) + _gravityField.Radius + +_shootPosRadius + _padding * 2) / 2;
        var minShortLength = _gravityField.Radius + _padding;

        float shortLen;

        if (cam.aspect >= 1) // is landscape
        {
            shortLen = minLongLength / cam.aspect;
            if(shortLen < minShortLength)
            {
                shortLen = minShortLength;
                minLongLength = shortLen * cam.aspect;
            }
            cam.transform.rotation = Quaternion.identity;
            _defaultSize = minLongLength / cam.aspect;
        }
        else // is portrait
        {
            shortLen = minLongLength * cam.aspect;
            if (shortLen < minShortLength)
            {
                shortLen = minShortLength;
                minLongLength = shortLen / cam.aspect;
            }
            cam.transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            _defaultSize = minLongLength;
        }

        float centerX = (_gravityField.transform.position.x + _gravityField.Radius + _shootPos.position.x - _shootPosRadius) / 2;
        cam.transform.position = new Vector3(centerX, 0f, -10f);

        cam.orthographicSize = _defaultSize;

        _cameraHalfSize = cam.ViewportToWorldPoint(new Vector3(1, 1));
        _cameraHalfSize = cam.transform.InverseTransformPoint(_cameraHalfSize);

        return _defaultSize;
    }

    public float DistanceFromBorder(Transform pos)
    {
        var cam = Camera.main;
        Vector2 posInCamSpace = cam.transform.InverseTransformPoint(pos.position);
        posInCamSpace = new Vector2(Mathf.Abs(posInCamSpace.x), Mathf.Abs(posInCamSpace.y));

        float distance = Mathf.Max(posInCamSpace.x - _cameraHalfSize.x, posInCamSpace.y - _cameraHalfSize.y); 

        return distance;
    }

    private void Update()
    {
        if(_currentAspect != Camera.main.aspect)
        {
            _currentAspect = Camera.main.aspect;
            ConfigureCamSettings();
            OnAspectChanged?.Invoke();
        }
    }
}
