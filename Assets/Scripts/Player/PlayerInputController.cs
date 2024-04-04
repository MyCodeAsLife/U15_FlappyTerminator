using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private float _force;

    private PlayerInputActions _inputActions;
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _inputActions = new PlayerInputActions();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
        _inputActions.Move.FlyUp.performed += AddForce;
    }

    private void OnDisable()
    {
        _inputActions.Move.FlyUp.performed -= AddForce;
        _inputActions.Disable();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
    }

    private void AddForce(InputAction.CallbackContext obj)
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.velocity = new Vector2(0, _force);
    }

    private void Shoot()                                // Стрельба
    {

    }
}
