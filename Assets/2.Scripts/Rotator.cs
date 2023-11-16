using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed = 10f;

    private void Update()
    {
        transform.Rotate(_rotateSpeed * Time.deltaTime * Vector3.forward);
    }
}
