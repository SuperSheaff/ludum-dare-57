using UnityEngine;

public class Core : MonoBehaviour
{
    public Movement Movement                { get; private set; }
    public CollisionSenses CollisionSenses  { get; private set; }

    private Movement _movement;
    private CollisionSenses _collisionSenses;

    private void Awake()
    {
        _movement = GetComponentInChildren<Movement>();
        _collisionSenses = GetComponentInChildren<CollisionSenses>();

        if (_movement == null)
            Debug.LogWarning("Movement component missing in Core.");
        if (_collisionSenses == null)
            Debug.LogWarning("CollisionSenses component missing in Core.");

        Movement = _movement;
        CollisionSenses = _collisionSenses;
    }

    public void Update()
    {
        Movement?.Update();
    }
}
