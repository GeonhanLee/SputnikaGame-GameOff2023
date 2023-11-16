using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlanetManager", menuName = "ScriptableObjects/PlanetManager")]
public class PlanetSettings : ScriptableObject
{
    [SerializeField] private float _massCalculateMultiplier = 1f;
    [SerializeField] private PlanetData[] _planetData;

    private void OnEnable()
    {
        for (int i = 0; i < _planetData.Length; i++)
        {
            _planetData[i].id = (uint)i;
        }
    }

    public PlanetData GetPlanetData(uint id)
    {
        id %= (uint)_planetData.Length;
        return _planetData[id];
    }

    public bool IsLastPlanet(PlanetData data)
    {
        return data.id == _planetData.Length - 1;
    }

    [ContextMenu("Set Mass By Radius")]
    private void SetMassByRadius()
    {
        for(int i = 0; i < _planetData.Length; i++)
        {
            _planetData[i].mass = _planetData[i].radius * _planetData[i].radius * Mathf.PI * _massCalculateMultiplier;
        }
    }
}
