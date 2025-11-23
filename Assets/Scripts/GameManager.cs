using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public float initialGameSpeed = 5f;
    public float gameSpeedIncrease = 0.5f;
    public float gameSpeed { get; private set; }

    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI hiscoreText;
    public Button retryButton;
    public AudioSource bacgroundMusic;
    public Image muteButtonImage;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;
    private bool isMuted = false;


    private Player player;
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

        int mutedState = PlayerPrefs.GetInt("IsMuted", 0);
        isMuted = (mutedState == 1);
        AudioListener.volume = isMuted ? 0f : 1f;
        UpdateMuteButtonIcon();


        NewGame();
    }

    public void NewGame()
    {
        Obstacle[] obstacles = FindObjectsOfType<Obstacle>();

        foreach (var obstacle in obstacles)
        {
            Destroy(obstacle.gameObject);
        }

        gameSpeed = initialGameSpeed;
        enabled = true;

        player.gameObject.SetActive(true);
        spawner.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(false);
        retryButton.gameObject.SetActive(false);

        if(bacgroundMusic != null)
        {
            bacgroundMusic.Play();
        }

        score = 0f;
        scoreText.text = Mathf.FloorToInt(score).ToString();

        UpdateHiscore();
    }

    public void GameOver()
    {

        gameSpeed = 0f;
        enabled = false;

        player.gameObject.SetActive(false);
        spawner.gameObject.SetActive(false);

        gameOverText.gameObject.SetActive(true);
        retryButton.gameObject.SetActive(true);

        UpdateHiscore();
        if(bacgroundMusic != null)
        {
            bacgroundMusic.Stop();
        }

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
        if(isMuted)
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
        if(isMuted)
        {
            AudioListener.volume = 0f;
        } 
        else
        {
            AudioListener.volume = 1f;
        }
        UpdateMuteButtonIcon();
        PlayerPrefs.SetInt("IsMuted", isMuted ? 1: 0);
        PlayerPrefs.Save();
    }

}
