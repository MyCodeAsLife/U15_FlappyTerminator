using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    //private const int RequiredNumberOfPlatforms = 5;
    private Obstacle _prefabGround;
    private ObjectPool<Obstacle> _platforms;            // Добавить vector2 с начальными координатами, реализовать определение платформ
    private ObjectPool<Obstacle> _walls;                // Добавить vector2 с начальными координатами, реализовать определение стен

    private int _countOfPlatforms;

    //private float _minPosX = 6f;                         // Вынести в сущность что будет контролировать "время жизни" данного объекта, когда спавнить следующий блок ground

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
