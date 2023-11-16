using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetShooter : MonoBehaviour
{
    [SerializeField] private Transform _shootStartPos;
    [Header("Shoot Force")]
    [SerializeField] private float _forceScale = 1f;
    [SerializeField] private float _minForce = 1f;
    [SerializeField] private float _maxForce = 5f;
    [SerializeField] private float _maxHorizontalForce = 3f;
    [Header("Drag and Drop Setting")]
    [SerializeField] private float _minDragDistance = 0.5f;
    [SerializeField] private float _maxDragDistance = 2f;
    [Header("Trajectory Line")]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private float _lineLengthScale = 0.4f;
    [SerializeField] private float _lineWidth = 0.1f;
    [Header("Planet move on drag")]
    [SerializeField] private float _planetMoveScale = 0.5f;
    [SerializeField] private float _planetMoveMaxDistance = 0.2f;
    [Header("Planet Angular Vel")]
    [SerializeField] private float _planetMaxAngularVelocity = 10f;

    private Planet _planetToShoot;
    private bool _isShootAble = true;

    private void Start()
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.startWidth = _lineWidth;
    }

    bool _isDragging = false;
    Vector2 _dragStartPos;
    Vector2 _dragEndPos;

    float _angularVelocity = 0f;

    private void Update()
    {
        if (!_isShootAble) return;

        if (Input.GetMouseButtonDown(0) && _isShootAble)
        {
            _dragStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _isDragging = true;
        }

        if (Input.GetMouseButtonDown(1))
        {
            _isDragging = false;
        }
        
        if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            _isDragging = false;
            _dragEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 dragVector = _dragEndPos - _dragStartPos;

            if (dragVector.magnitude > _minDragDistance)
            {
                var force = CalcShootForce();

                ShootPlanet(force);
                _isShootAble = false;
            }
        }
    
        Vector2 planetDragTargetPos = _shootStartPos.position;
        if (_isDragging)
        {
            var force = CalcShootForce();

            // draw trajectory line
            _lineRenderer.enabled = true;
            var lineVec = force * _lineLengthScale;

            _lineRenderer.SetPosition(0, _shootStartPos.position);
            _lineRenderer.SetPosition(1, (Vector2)_shootStartPos.position + lineVec);

            // move planet
            Vector2 planetDir = -force * _planetMoveScale;
            planetDir = Vector2.ClampMagnitude(planetDir, _planetMoveMaxDistance);

            planetDragTargetPos = (Vector2)_shootStartPos.position + planetDir;
        }
        else
        {
            _lineRenderer.enabled = false;
        }

        _planetToShoot.transform.position = planetDragTargetPos;

        _planetToShoot.GetComponent<Rigidbody2D>().angularVelocity = _angularVelocity;
    }



    private Vector2 CalcShootForce()
    {
        _dragEndPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 dragVector = _dragEndPos - _dragStartPos;
        Vector2 dragDir = dragVector.normalized;

        if (dragVector.magnitude <= _minDragDistance) return Vector2.zero;

        dragVector = Vector2.ClampMagnitude(dragVector, _maxDragDistance);
        float normalizedForce = Mathf.InverseLerp(_minDragDistance, _maxDragDistance, dragVector.magnitude);
        float power = Mathf.Lerp(_minForce, _maxForce, normalizedForce) * _forceScale;

        var force = dragDir * power;
        force.x = Mathf.Clamp(force.x, -_maxHorizontalForce, _maxHorizontalForce);

        return force;
    }

    private void ShootPlanet(Vector2 force)
    {
        var planetRb = _planetToShoot.GetComponent<Rigidbody2D>();
        planetRb.constraints = RigidbodyConstraints2D.None;
        planetRb.velocity = force;

        SoundManager.Instance.PlayShootSound();
    }
    
    private void OnEnable()
    {
        GameManager.Instance.OnReload += ReloadPlanet;
        GameManager.Instance.OnGameOver += StopShooting;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnReload -= ReloadPlanet;
            GameManager.Instance.OnGameOver -= StopShooting;
        }
    }

    private void ReloadPlanet(PlanetData data)
    {
        _planetToShoot = PlanetManager.Spawn(data, _shootStartPos.position);

        var planetRb = _planetToShoot.GetComponent<Rigidbody2D>();
        planetRb.constraints = RigidbodyConstraints2D.FreezePosition;

        _angularVelocity = Random.Range(-_planetMaxAngularVelocity, _planetMaxAngularVelocity);

        _isShootAble = true;
    }

    private void StopShooting()
    {
        _isShootAble = false;
    }

}
