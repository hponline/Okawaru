using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Slider")]
    public Image currentStaminaImage;
    public Image currentSanityImage;
    public Image currentHungerImage;
    public Image currentDayTimeImage;

    [Header("Panel")]
    public GameObject deathPanel;
    public Image progresBar;
    public Image progresBarBg;
    public TextMeshProUGUI messageTxt;
    public TextMeshProUGUI currentDayTxt;

    [Header("Music")]
    public AudioSource bgMusic;

    DayNightManager dayNightManager;
    PlayerStats playerStats;


    private void Awake()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        dayNightManager = GameObject.FindWithTag("DayNightManager").GetComponent<DayNightManager>();

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        // ambiyans müzik
        // kapý, yemek müzik
    }

    private void Start()
    {
        bgMusic = GetComponent<AudioSource>();
        bgMusic.Play();
    }

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Menu();
        }
    }

    public void Menu()
    {
        if (!deathPanel.activeInHierarchy)
        {
            deathPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else if(deathPanel.activeInHierarchy)
        {
            deathPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void LoadScene(int index)
    {
        deathPanel.SetActive(false);
        playerStats.transform.position = new Vector3(-10, 2, -4);
        playerStats.currentStamina = 500;
        playerStats.currentSanity = 500;
        playerStats.currentHunger = 500;
        playerStats.IsDead = false;

        SceneManager.LoadScene(index);
        Time.timeScale = 1.0f;
    }
    public void Restart()
    {
        deathPanel.SetActive(false);
        playerStats.transform.position = new Vector3 (-10, 2, -4);
        playerStats.currentStamina = 500;
        playerStats.currentSanity = 500;
        playerStats.currentHunger = 500;
        playerStats.IsDead = false;
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }
    public void exit()
    {
        Application.Quit();
    }

    public void PlayerIntereact()
    {
        progresBarBg.gameObject.SetActive(true);
        progresBar.fillAmount = 5f * Time.deltaTime;

    }

    private void Update()
    {
        ShowUIStats();

        if (playerStats.IsDead)
        {
            Time.timeScale = 0;
            Debug.Log($"GameOver beyin saðlýgýn öldü\n Akýl saðlýgý: {playerStats.currentSanity} Açlýk: {playerStats.currentHunger}\n");
        }
        else
            Time.timeScale = 1;
    }

    public void ShowUIStats()
    {
        currentStaminaImage.fillAmount = playerStats.currentStamina / playerStats.maxStamina;
        currentHungerImage.fillAmount = playerStats.currentHunger / playerStats.maxHunger;
        currentSanityImage.fillAmount = playerStats.currentSanity / playerStats.maxSanity;
        currentDayTimeImage.fillAmount = dayNightManager.dayTimeNormalized;
        currentDayTxt.text = "Day: " +dayNightManager.dayCount.ToString();
    }

    public void ShowMessageText(string message, float duration = 2f)
    {
        messageTxt.gameObject.SetActive(true);
        messageTxt.text = message;
        StopAllCoroutines();
        StartCoroutine(ClearMessage(duration));
    }

    IEnumerator ClearMessage(float time)
    {
        yield return new WaitForSeconds(time);
        messageTxt.text = "";
        messageTxt.gameObject.SetActive(false);
    }

}
