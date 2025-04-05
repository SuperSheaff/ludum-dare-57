using UnityEngine;

public class CollisionSenses : CoreComponent
{
    [Header("Ground Check Settings")]
    [SerializeField] private Transform _groundCheckPointA;
    [SerializeField] private Transform _groundCheckPointB;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private LayerMask _whatIsGround;

    [Header("Obstacle Settings")]
    [SerializeField] private LayerMask _whatIsObstacle;

    public Transform GroundCheckPointA => _groundCheckPointA;
    public Transform GroundCheckPointB => _groundCheckPointB;
    public float GroundCheckRadius => _groundCheckRadius;
    public LayerMask WhatIsGround => _whatIsGround;

    public bool Ground
    {
        get
        {
            Vector2 pointA = _groundCheckPointA.position;
            Vector2 pointB = _groundCheckPointB.position;
            return Physics2D.OverlapArea(pointA, pointB, _whatIsGround);
        }
    }

    private void OnDrawGizmos()
    {
        if (_groundCheckPointA == null || _groundCheckPointB == null)
            return;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(_groundCheckPointA.position, new Vector3(_groundCheckPointA.position.x, _groundCheckPointB.position.y, 0));
        Gizmos.DrawLine(_groundCheckPointB.position, new Vector3(_groundCheckPointB.position.x, _groundCheckPointA.position.y, 0));
        Gizmos.DrawLine(_groundCheckPointA.position, _groundCheckPointB.position);
    }
}
