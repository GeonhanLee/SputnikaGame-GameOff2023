using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField : MonoBehaviour
{
    [SerializeField] private float _radius = 4f;
    [SerializeField] private float _tolerance = 0.1f;
    [Header("Warning Indicator")]
    [SerializeField] private SpriteRenderer _warningSprite;
    [SerializeField, Range(0.5f,1)] private float _warningStartRadiusScale = 0.8f;
    [SerializeField] private Gradient _warningGradient;
    [SerializeField] private float _warningAlphaScale;
    [Header("Gravity VFX")]
    [SerializeField] private float _gravityVfxDuration = 1f;
    [SerializeField] private float _gravityVfxInterval = 3f;
    [SerializeField] private Transform _gravityVFX;
    [SerializeField] private Color _gravityVfxInitialColor;

    public float Radius => _radius;

    private void Awake()
    {
        transform.localScale = Vector3.one * _radius * 2f;
    }

    public bool IsIn(Planet planet)
    {
        var planetRadius = planet.GetData().radius;
        var planetPos = planet.transform.position;

        return (Vector3.Distance(planetPos, transform.position) + planetRadius <= _radius + _tolerance);
    }

    private float _maxPlanetDistance = 0f;
    public void SetDistanceFromCenter(Planet planet)
    {
        var planetPos = planet.transform.position;
        var distance = Vector3.Distance(planetPos, transform.position);
        distance += planet.GetData().radius;
        _maxPlanetDistance = Mathf.Max(_maxPlanetDistance, distance);
    }

    private void Update()
    {
        var t = Mathf.InverseLerp(_radius * _warningStartRadiusScale, _radius, _maxPlanetDistance);

        var color = _warningGradient.Evaluate(t);
        color.a *= _warningAlphaScale;
        _warningSprite.color = color;

        _maxPlanetDistance = 0f;


        float x = Time.time % _gravityVfxInterval;
        x = Mathf.Lerp(1, 0, x / _gravityVfxDuration);

        _gravityVFX.localScale = Vector3.one * x;

        var newColor = _gravityVfxInitialColor;
        newColor.a *= x;
        _gravityVFX.GetComponent<SpriteRenderer>().color = newColor;
    }

    private void OnValidate() => Awake();
}
