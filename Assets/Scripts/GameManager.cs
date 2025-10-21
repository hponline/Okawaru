using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject player;

    [Header("Slider")]
    public Image currentStaminaImage;
    public Image currentSanityImage;
    public Image currentHungerImage;
    public Image currentDayTimeImage;

    public GameObject deathPanel;

    public Image progresBar;
    public Image progresBarBg;

    public TextMeshProUGUI messageTxt;

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

    }

    public void LoadScene(int index)
    {
        SceneManager.LoadScene(index);
        Time.timeScale = 1.0f;
    }
    public void CurrentLoadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
        Time.timeScale = 1.0f;
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
