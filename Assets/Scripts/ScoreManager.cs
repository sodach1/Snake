using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private const string BEST_SCORE_PREFS_KEY = "BEST_SCORE";

    [SerializeField] TMPro.TextMeshProUGUI bestScoreText;
    [SerializeField] TMPro.TextMeshProUGUI inGameScoreText;

    public int currentScore { get; private set; } = 0;

    private void Awake()
    {
        Snake.OnFoodGrabbed += UpdateInGameScore;
        Snake.OnLoose += CheckIfBestScore;
        Snake.OnSnakeSpawned += ResetInGameScore;
    }

    private void OnDestroy()
    {
        Snake.OnFoodGrabbed -= UpdateInGameScore;
        Snake.OnLoose -= CheckIfBestScore;
        Snake.OnSnakeSpawned -= ResetInGameScore; // можно было и в OnLoose сделать, ну может кому то еще потом будет интересто когда она спавнится
    }

    private void Start()
    {
        SetBestScoreText(PlayerPrefs.GetInt(BEST_SCORE_PREFS_KEY, 0));
    }

    private void UpdateInGameScore()
    {
        currentScore++;
        SetInGameScoreText(currentScore);
    }

    private void ResetInGameScore()
    {
        currentScore = 0;
        SetInGameScoreText(currentScore);
    }

    private void SetInGameScoreText(int score) => inGameScoreText.text = string.Format("Score: {0}", score);

    private void CheckIfBestScore()
    {
        if (currentScore > PlayerPrefs.GetInt(BEST_SCORE_PREFS_KEY, 0))
        {
            PlayerPrefs.SetInt(BEST_SCORE_PREFS_KEY, currentScore);
            SetBestScoreText(currentScore);
        }
    }

    private void SetBestScoreText(int bestScore) => bestScoreText.text = string.Format("Best score: {0}", bestScore);
}
