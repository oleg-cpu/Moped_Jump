using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.5f;
    public float gameSpeed { get; private set; }
    public GameObject mainMenuUi;
    public GameObject gameOverMenuUi;
    public GameObject gameUi;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiscoreText;
    public Button retryButton;
    public AudioSource bacgroundMusic;
    public Image muteButtonImage;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    private bool isMuted = false;


    private Player player;
    private AnimatedSprite playerAnimation;
    private Spawner spawner;
    private float score;


    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            DestroyImmediate(gameObject);
        }
    }

    private void OnDestroy()
    {

        if (Instance == this)
        {
            Instance = null;

        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        spawner = FindObjectOfType<Spawner>();
        playerAnimation = player.GetComponent<AnimatedSprite>();

        int mutedState = PlayerPrefs.GetInt("IsMuted", 0);
        isMuted = (mutedState == 1);
        AudioListener.volume = isMuted ? 0f : 1f;
        UpdateMuteButtonIcon();
        ShowMainMenu();
        UpdateHiscore();

    }

    public void ShowMainMenu()
    {
        mainMenuUi.SetActive(true);
        gameOverMenuUi.SetActive(false);
        gameUi.SetActive(false);
        Time.timeScale = 0f;
        spawner.gameObject.SetActive(false);
        if (player != null)
        {
            player.enabled = false;
            playerAnimation.StopAnimation();
        }
        
        PlayBackgroundMusic();

    }

    public void StartGame()
    {
        mainMenuUi.SetActive(false);
        gameOverMenuUi.SetActive(false);
        gameUi.SetActive(true);

        gameSpeed = initialGameSpeed;
        enabled = true;
        Time.timeScale = 1f;
        spawner.gameObject.SetActive(true);
        if (player != null)
        {
            player.enabled = true;
            if (playerAnimation != null)
            {
                playerAnimation.StartAnimation();
            }
        }

       
        PlayBackgroundMusic();

        ResetGame();
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void ResetGame()
    {
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();

        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }
        score = 0f;
        scoreText.text = Mathf.FloorToInt(score).ToString();

        UpdateHiscore();
    }

    public void GameOver()
    {
        gameSpeed = 0f;
        enabled = false;
        UpdateHiscore();
        showGameOverUi();
    }

    public void showGameOverUi()
    {
        mainMenuUi.SetActive(false);
        gameOverMenuUi.SetActive(true);
        gameUi.SetActive(true);
        spawner.gameObject.SetActive(false);
        Time.timeScale = 0f;

        if (player != null)
        {
            player.enabled = false;
        }

        if (playerAnimation != null)
        {
            playerAnimation.StopAnimation();
        }

        if (bacgroundMusic != null)
        {
            bacgroundMusic.Stop();
        }
    }

    public void GoToMainMenu()
    {
        ResetGame();
        ShowMainMenu();
    }

    private void Update()
    {

        gameSpeed += gameSpeedIncrease * Time.deltaTime;

    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = Mathf.FloorToInt(score).ToString();
    }

    private void UpdateHiscore()
    {

        float hiscore = PlayerPrefs.GetFloat("hiscore", 0);

        if (score > hiscore)
        {

            hiscore = score;
            PlayerPrefs.SetFloat("hiscore", hiscore);
        }

        hiscoreText.text = Mathf.FloorToInt(hiscore).ToString();
    }

    private void UpdateMuteButtonIcon()
    {
        if (isMuted)
        {
            muteButtonImage.sprite = soundOffSprite;
        }
        else
        {
            muteButtonImage.sprite = soundOnSprite;
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        if (isMuted)
        {
            AudioListener.volume = 0f;
        }
        else
        {
            AudioListener.volume = 1f;
        }
        UpdateMuteButtonIcon();
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }
    private void PlayBackgroundMusic()
    {
        if (bacgroundMusic != null && !bacgroundMusic.isPlaying)
        {
            bacgroundMusic.Play();
        }
    }

}
