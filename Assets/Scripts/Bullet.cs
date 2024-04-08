using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Bullet : MonoBehaviour
{
    private const float MaxLifeDistance = 15f;

    private CapsuleCollider2D _capsuleCollider;
    private Mover _mover;

    public System.Action<Bullet> Hited;

    public CapsuleCollider2D Collider => _capsuleCollider;

    public Mover Mover
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
            Hited?.Invoke(this);            // Добавить еще событие для устаревания снаряда или изменить текущее?
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hited?.Invoke(this);
    }

    private void CreateComponentMover()
    {
        if (_mover == null)
            _mover = this.AddComponent<Mover>();
    }
}
