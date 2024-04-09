using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private EnvironmentController _environmentController;
    [SerializeField] private ProjectileController _projectileController;
    [SerializeField] private CharacterSpawner _characterSpawner;

    private Canvas _startMenu;

    private void Awake()
    {
        _player.gameObject.SetActive(false);
        _startMenu = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        _player.Hited += OnGameEnd;
    }

    private void OnDisable()
    {
        _player.Hited -= OnGameEnd;
    }

    public void OnGameEnd()
    {
        _startMenu.enabled = true;
        _player.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        _startMenu.enabled = false;
        _player.gameObject.SetActive(true);

        _environmentController.enabled = false;
        _projectileController.enabled = false;
        _characterSpawner.enabled = false;

        _environmentController.enabled = true;
        _projectileController.enabled = true;
        _characterSpawner.enabled = true;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
