using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CustomGravity : MonoBehaviour
{
    [SerializeField] private float _gravityScale = 1f;
    [SerializeField, Range(0, 0.05f)] private float _centerCoefficient = 0.03f;

    private Rigidbody2D _rb;

    private void OnEnable()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (_rb.simulated == true)
            CalcGravity(Vector2.zero);
    }

    private void CalcGravity(Vector2 center)
    {
        Vector2 gravityDir = (center - _rb.position).normalized;
        Vector2 gravity = _gravityScale * 9.8f * gravityDir;

        Vector2 vel = _rb.velocity;
        Vector2 nextPos = _rb.position + vel;

        Vector2 parallelVel = Vector3.Project(vel, gravityDir);
        Vector2 perpendicularVel = vel - parallelVel;

        Vector2 newPerpendicularVel = perpendicularVel + (Vector2)Vector3.Project(-perpendicularVel, (nextPos - center));
        perpendicularVel = Vector2.Lerp(perpendicularVel, newPerpendicularVel, _centerCoefficient);

        _rb.velocity = perpendicularVel + parallelVel + gravity * Time.fixedDeltaTime;
    }
}
