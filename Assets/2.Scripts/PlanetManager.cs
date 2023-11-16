using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetManager : MonoBehaviour
{
    [Header("Planet Settings")]
    [SerializeField] private PlanetSettings _planetSettings;
    [SerializeField] private GameObject _planetPrefab;
    [Header("GravityField")]
    [SerializeField] private GravityField _gravityField;

    private static PlanetManager _instance;
    private static PlanetManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PlanetManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    public static GravityField GravityField => Instance._gravityField;
    public static PlanetData GetPlanetData(uint id)
    {
        return Instance.GetPlanetDataImpl(id);
    }

    public static Planet Spawn(PlanetData data, Vector2 pos)
    {
        return Instance.SpawnImpl(data, pos);
    }

    public static bool IsLastPlanet(PlanetData data)
    {
        return Instance.IsLastPlanetImpl(data);
    }

    private PlanetData GetPlanetDataImpl(uint id)
    {
        return _planetSettings.GetPlanetData(id);
    }
    private Planet SpawnImpl(PlanetData data, Vector2 pos)
    {
        var planet = Instantiate(_planetPrefab, pos, Quaternion.identity).GetComponent<Planet>();
        planet.SetData(data);

        return planet;
    }
    private bool IsLastPlanetImpl(PlanetData data)
    {
        return _planetSettings.IsLastPlanet(data);
    }
}
