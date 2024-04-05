using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _force;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _minRotationZ;
    [SerializeField] private float _maxRotationZ;

    private ProjectileController _projectileController;
    private Quaternion _minRotation;
    private Quaternion _maxRotation;

    private PlayerInputActions _inputActions;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _projectileController = GetComponentInParent<ProjectileController>();
        _inputActions = new PlayerInputActions();
        _rigidbody = GetComponent<Rigidbody2D>();
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
        Debug.Log(collision.gameObject.name);
    }

    private void AddForce(InputAction.CallbackContext obj)
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.velocity = new Vector2(0, _force);
        transform.rotation = _maxRotation;
    }

    private void Shoot(InputAction.CallbackContext obj)                                // Стрельба тут
    {
        var bullet = _projectileController.GetAmmo();
        float newPosX = transform.position.x + GetComponent<CapsuleCollider2D>().size.x + bullet.Collider.size.x;
        Vector2 startBulletPosition = new Vector2(newPosX, transform.position.y);

        bullet.Mover.SetStartData(startBulletPosition, -4f);                    // Магические числа
    }
}
