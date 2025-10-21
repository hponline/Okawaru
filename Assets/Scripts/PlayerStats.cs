using System;
using System.Collections;
using UnityEngine;


public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    public static Vector3 spawnPos;

    [Header("Stats")]
    public bool IsDead = false;
    public bool atHome = true;
    public bool gameMode = false;

    public float maxStamina = 100f;
    public float maxSanity = 100f;
    public float maxHunger = 100f;

    [Header("Stats azalma")]
    public float staminaDrainRate = 0.2f;
    public float sanityDrainRate = 0.5f;
    public float hungerDrainRate = 0.5f;

    [Header("Stats arttýrma")]
    public float staminaIntervalRate = 0.2f;
    public float sanityIntervalRate = 5.0f;
    public float hungerIntervalRate = 10.0f;

    [Header("CurrentStats")]
    public float currentStamina;
    public float currentSanity;
    public float currentHunger;

    private void Awake()
    {
        currentStamina = maxStamina;
        currentSanity = maxSanity;
        currentHunger = maxHunger;

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }        
    }

    private void Update()
    {
        if (!atHome)
        {
            PlayerOutSide();
        }
        if (currentSanity < 200)
        {
            GameManager.instance.ShowMessageText("Sanýrým akýl saðlýgýmý kaybediyorum");
        }
        if (currentHunger < 150)
        {
            GameManager.instance.ShowMessageText("Açlýktan ölücem bir þeyler yemeliyim");
        }

        StartCoroutine(ChangeStatOverTime(SanityChange: -sanityDrainRate, HungerChange: -hungerDrainRate, duration: 0.2f)); // Genel stat azalma
        ClampValues();

        if (gameMode) // Ölümsüz
        {
            currentStamina = 100;
            currentSanity = 100;
            currentHunger = 100;
        }
    }

    public void ClampValues() // Stat sýnýr
    {
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        currentSanity = Mathf.Clamp(currentSanity, 0, maxSanity);
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
    }

    #region USE ITEM
    public void RestStamina(float amount)
    {
        currentStamina += amount;
        GameManager.instance.ShowMessageText($"Stamina +{amount}");
        ClampValues();
    }

    public void RestSanity(float amount)
    {
        currentSanity += amount;
        GameManager.instance.ShowMessageText($"Sanity +{amount}");
        ClampValues();
    }

    public void EatFood(float amount)
    {
        currentHunger += amount;
        GameManager.instance.ShowMessageText($"Food +{amount}");
        ClampValues();
    }
    #endregion


    public IEnumerator ChangeStatOverTime(float staminaChange = 0f, float SanityChange = 0f, float HungerChange = 0f, float duration = 1f)
    {
        float elapsed = 0f;
        while (elapsed < duration && !IsDead)
        {
            currentStamina += staminaChange * Time.deltaTime;
            currentSanity += SanityChange * Time.deltaTime;
            currentHunger += HungerChange * Time.deltaTime;

            ClampValues();
            CheckDeath();

            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    public void CheckDeath()
    {
        if (currentSanity <= 0 || currentHunger <= 0)
        {
            IsDead = true;
            GameManager.instance.deathPanel.SetActive(IsDead);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // etkileþim baþlangýç
        if (other.CompareTag("Siginak"))
        {
            atHome = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Siginak") && atHome)
        {
            // karakter statlarý daha az gider            
            StartCoroutine(ChangeStatOverTime(HungerChange: -hungerDrainRate * -0.05f, SanityChange: -sanityDrainRate * -0.05f, duration: 0.2f));
            Debug.Log($"player {other.tag}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Siginak"))
        {
            if (atHome)
            {
                atHome = false;
            }
            // gece gündüz döngüsü için farklý hesaplamalar yapýlabilir
            //StartCoroutine(ChangeStatOverTime(staminaChange: -staminaDrainRate, HungerChange: -hungerDrainRate, SanityChange: - sanityDrainRate, duration: 0.2f));
        }
    }

    public void PlayerOutSide()
    {
        StartCoroutine(ChangeStatOverTime(staminaChange: -staminaDrainRate, HungerChange: -hungerDrainRate, SanityChange: -sanityDrainRate, duration: 0.2f));
        //Debug.Log($"stamina: {staminaDrainRate} açlýk: {hungerDrainRate} sanity: {sanityDrainRate}");
    }


    #region CanvasUI etkileþim gösterisi
    IEnumerator FillProgressBar(float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            GameManager.instance.progresBar.fillAmount = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }

        GameManager.instance.progresBar.fillAmount = 1f; // tam dolu
        GameManager.instance.progresBarBg.gameObject.SetActive(false);
    }

    IEnumerator ClearCanvas(float time)
    {
        yield return new WaitForSeconds(time);
        GameManager.instance.progresBar.gameObject.SetActive(false);
        GameManager.instance.progresBarBg.gameObject.SetActive(false);
    }

    public IEnumerator Cooldown(float cooldown, Action onCoolDownEnd)
    {
        yield return new WaitForSeconds(cooldown);
        onCoolDownEnd?.Invoke();
    }
    #endregion

    public void PlayerIntereact(float ProgressBarDuration = 3f, float ClearCanvasDuration = 3f)
    {
        GameManager.instance.progresBar.gameObject.SetActive(true);
        GameManager.instance.progresBarBg.gameObject.SetActive(true);
        StartCoroutine(FillProgressBar(ProgressBarDuration));
        StartCoroutine(ClearCanvas(ClearCanvasDuration));
    }
}
