using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    //[SerializeField] private CharacterController _characterController;
    [SerializeField] private EnvironmentController _environmentController;
    [SerializeField] private ProjectileController _projectileController;
    [SerializeField] private CharacterSpawner _characterSpawner;

    private Canvas _startMenu;

    private void Awake()
    {
        _player.gameObject.SetActive(false);
        _startMenu = GetComponent<Canvas>();
    }

    public void StartGame()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        _startMenu.enabled = false;
        _player.gameObject.SetActive(true);

        //_environmentController.gameObject.SetActive(false);
        //_projectileController.gameObject.SetActive(false);
        //_characterSpawner.gameObject.SetActive(false);

        //_environmentController.gameObject.SetActive(true);
        //_projectileController.gameObject.SetActive(true);
        //_characterSpawner.gameObject.SetActive(true);

        _environmentController.enabled = false;
        _projectileController.enabled = false;
        _characterSpawner.enabled = false;

        _environmentController.enabled = true;
        _projectileController.enabled = true;
        _characterSpawner.enabled = true;

        //_environmentController.Restart();
        //_projectileController.Restart();
        //_characterSpawner.Restart();

    }

    public void RestartGame()
    {

    }

    public void ExitGame()
    {

    }
}
