using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject gameElements; // игровые (поле)
    [SerializeField] GameObject mainMenuUIElements;
    [SerializeField] GameObject inGameUIElements;
    [SerializeField] GameObject deathScreen;
    [SerializeField] AudioSource audioSource;

    public static UnityEngine.Events.UnityAction<Vector3> OnSetDirection = null;

    private void Awake()
    {
        Snake.OnLoose += ShowDeathScreen;
    }

    private void OnDestroy()
    {
        Snake.OnLoose -= ShowDeathScreen;
    }

    private void Start()
    {
        mainMenuUIElements.SetActive(true);
        inGameUIElements.SetActive(false);
        gameElements.SetActive(false);
        deathScreen.SetActive(false);
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }

    public void OnStartButtonPressed()
    {
        gameElements.SetActive(true);
        inGameUIElements.SetActive(true);
        mainMenuUIElements.SetActive(false);
    }

    public void ShowDeathScreen()
    {
        gameElements.SetActive(false);
        inGameUIElements.SetActive(false);

        deathScreen.SetActive(true); // вот тут можно было префабом, 50/50 как по мне

        audioSource.Play();

        deathScreen.GetComponent<YouDied>().PlayAnimation(delegate ()
        {
            OnBackToMainMenu();
            // можно было просто Юнити анимацией сделать, поздно об этом вспомнил, хотя есть вероятность что это производительнее, несмотря на корутину в мейн треде, тк юнити всякие OnAnimationEnded чекает в апдейте (но это не точно) ((так сказать, аптемезиравали змеюку))
        });
    }

    private void OnBackToMainMenu()
    {
        deathScreen.SetActive(false);
        mainMenuUIElements.SetActive(true);
        inGameUIElements.SetActive(false);
    }

    public void OnRightButtonPressed() => OnSetDirection?.Invoke(Vector3.right); // ну в инспекторе вектор передать нельзя, так что ;(
    public void OnLeftButtonPressed() => OnSetDirection?.Invoke(Vector3.left);
    public void OnTopButtonPressed() => OnSetDirection?.Invoke(Vector3.up);
    public void OnDownButtonPressed() => OnSetDirection?.Invoke(Vector3.down);
}
