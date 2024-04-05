using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Bullet : MonoBehaviour
{
    private CapsuleCollider2D _capsuleCollider;
    private Mover _mover;

    public System.Action<Bullet> Hited;

    public CapsuleCollider2D Collider => _capsuleCollider;

    public Mover Mover
    {
        get
        {
            CreateComponent();
            return _mover;
        }
    }

    private void Awake()
    {
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        CreateComponent();
    }

    private void Update()
    {
        //if (transform.position)
        //{
            
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hited?.Invoke(this);

        Debug.Log(collision.name);
    }

    private void CreateComponent()
    {
        if (_mover == null)
            _mover = this.AddComponent<Mover>();
    }
}
