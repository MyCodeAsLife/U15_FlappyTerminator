using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Bullet : BaseObject
{
    private const float MaxLifeDistance = 15f;

    private CapsuleCollider2D _capsuleCollider;

    public System.Action<Bullet> Disabled;

    public CapsuleCollider2D Collider => _capsuleCollider;

    public override Mover Mover
    {
        get
        {
            CreateComponentMover();
            return _mover;
        }
    }

    private void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        CreateComponentMover();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, Vector2.zero) > MaxLifeDistance)
            Disabled?.Invoke(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Disabled?.Invoke(this);
    }

    private void CreateComponentMover()
    {
        if (_mover == null)
            _mover = this.AddComponent<Mover>();
    }
}
