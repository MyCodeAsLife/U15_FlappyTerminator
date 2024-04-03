using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    //private const int RequiredNumberOfPlatforms = 5;
    private Obstacle _prefabGround;
    private ObjectPool<Obstacle> _platforms;            // �������� vector2 � ���������� ������������, ����������� ����������� ��������
    private ObjectPool<Obstacle> _walls;                // �������� vector2 � ���������� ������������, ����������� ����������� ����

    private int _countOfPlatforms;

    //private float _minPosX = 6f;                         // ������� � �������� ��� ����� �������������� "����� �����" ������� �������, ����� �������� ��������� ���� ground

    private void Awake()
    {
        _prefabGround = Resources.Load()
        _platforms = new ObjectPool<Obstacle>(CreateObstacle, EnableObstacle, DisableObstacle, 3);
        //_countOfPlatforms = RequiredNumberOfPlatforms;
    }

    private Obstacle CreateObstacle() => this.AddComponent<Obstacle>();

    private void EnableObstacle(Obstacle obstacle)
    {
        obstacle.gameObject.SetActive(true);
        obstacle.Outdated += OnOutdated;
    }

    private void DisableObstacle(Obstacle obstacle)
    {
        obstacle.Outdated -= OnOutdated;
        obstacle.gameObject.SetActive(false);
    }

    private void OnOutdated(Obstacle obstacle)
    {
        //_countOfPlatforms--;
        _platforms.Return(obstacle);
        _platforms.Get();
    }
}
