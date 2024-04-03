using UnityEngine;

public class ObstacleMover : MonoBehaviour
{
    private float _speed;
    //private float _minPosX;                         // Вынести в сущность что будет контролировать "время жизни" данного объекта
    private float _startPosX;
    private float _startPosY;               // Высоту получать от контролирующей сущности
    private float _currentPosX;

    private void Start()
    {
        transform.position = new Vector2(_startPosX, _startPosY);
    }

    private void Update()
    {
        Move();
    }

    public void SetStartData(float startPosX, float startPosY, float speed)
    {
        _speed = speed;
        _startPosX = startPosX;
        _currentPosX = _startPosX;
        _startPosY = startPosY;
    }

    private void Move()
    {
        _currentPosX -= _speed * Time.deltaTime;
        transform.position = new Vector2(_currentPosX, _startPosY);
    }
}
