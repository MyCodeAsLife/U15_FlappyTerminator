using UnityEngine;

public class Mover : MonoBehaviour
{
    private float _speed;
    private Vector2 _position;

    //private void Start()
    //{
    //    transform.position = _position;
    //}

    private void Update()
    {
        Move();
    }

    public void SetStartData(Vector2 position, Quaternion rotation, float speed)
    {
        _speed = speed;
        _position = position;
        transform.position = _position;
        transform.rotation = rotation;
    }

    private void Move()
    {
        transform.Translate(Vector2.left * _speed * Time.deltaTime);

        //_position.x -= _speed * Time.deltaTime;
        //transform.position = _position;
    }
}
