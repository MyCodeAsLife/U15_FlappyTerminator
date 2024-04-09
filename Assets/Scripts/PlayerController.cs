using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _force;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _minRotationZ;
    [SerializeField] private float _maxRotationZ;
    [SerializeField] private float _bulletSpeed;

    private Rigidbody2D _rigidbody;
    private PlayerInputActions _inputActions;
    private ProjectileController _projectileController;
    private Quaternion _minRotation;
    private Quaternion _maxRotation;
    private Vector2 _basePosition;

    public event Action Hited;

    private void Awake()
    {
        _projectileController = GetComponentInParent<ProjectileController>();
        _inputActions = new PlayerInputActions();
        _rigidbody = GetComponent<Rigidbody2D>();
        _basePosition = new Vector2(-6f, 0f);
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Move.FlyUp.performed += AddForce;
        _inputActions.Fight.Shoot.performed += Shoot;
    }

    private void OnDisable()
    {
        _inputActions.Move.FlyUp.performed -= AddForce;
        _inputActions.Fight.Shoot.performed -= Shoot;
        _inputActions.Disable();
    }

    private void Start()
    {
        _minRotation = Quaternion.Euler(0, 0, _minRotationZ);
        _maxRotation = Quaternion.Euler(0, 0, _maxRotationZ);
    }

    private void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, _minRotation, _rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Hited?.Invoke();
        transform.position = _basePosition;
    }

    private void AddForce(InputAction.CallbackContext obj)
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.velocity = new Vector2(0, _force);
        transform.rotation = _maxRotation;
    }

    private void Shoot(InputAction.CallbackContext obj)
    {
        const float Half = 0.5f;
        var bullet = _projectileController.GetAmmo() as Bullet;
        float newPosX = transform.position.x + GetComponent<CapsuleCollider2D>().size.x * Half + bullet.Collider.size.x;
        Vector2 startBulletPosition = new Vector2(newPosX, transform.position.y);

        bullet.Mover.SetStartData(startBulletPosition, transform.rotation, _bulletSpeed);
    }
}
