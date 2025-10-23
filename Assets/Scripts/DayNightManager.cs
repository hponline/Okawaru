using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayNightManager : MonoBehaviour
{
    public static DayNightManager instance;

    [Header("Sun")]
    public float temperatureSpeed = 200f;
    public bool temperatureIncreasing = true;
    public float sunTurnSpeed = 2f;
    public int sunMinX = 0;
    public int sunMaX = 150;
    public float sunAngleX = 0;
    public bool isReversing = false;
    public float daySpeed = 25f;

    [Header("Time")]
    public float dayLengthSeconds = 600f;
    public float dayTimeNormalized = 0f;

    [Header("Day")]
    public int dayCount = 1;

    [Header("Light / Visuals")]
    public Light directionalLight;

    [Header("Temperature")]
    public AnimationCurve temperatureOverday;
    public float minTemperature = 1500f;
    public float maxTemperature = 20000f;

    [Header("Intensity Opsiyonal")]
    public float minIntensity = 0f; // Iþýk yoðunluk
    public float maxIntensity = 1.2f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        if (directionalLight == null)
            directionalLight = RenderSettings.sun;
    }

    private void Update()
    {
        dayTimeNormalized += daySpeed * Time.deltaTime / dayLengthSeconds;

        if (dayTimeNormalized >= 1f)
        {
            dayTimeNormalized = 0f;
            dayCount++;
            Debug.Log("NewDay: " + dayCount);
        }
        Isýk();
    }

    public void Isýk()
    {
        if (temperatureIncreasing)
        {
            directionalLight.colorTemperature = Mathf.MoveTowards(
                directionalLight.colorTemperature,
                maxTemperature,
                temperatureSpeed * Time.deltaTime
            );

            if (directionalLight.colorTemperature >= maxTemperature)
                temperatureIncreasing = false;
        }
        else
        {
            directionalLight.colorTemperature = Mathf.MoveTowards(
                directionalLight.colorTemperature,
                minTemperature,
                temperatureSpeed * Time.deltaTime
            );

            if (directionalLight.colorTemperature <= minTemperature)
                temperatureIncreasing = true;
        }

        var scene = SceneManager.GetSceneByBuildIndex(2);
        if (scene.buildIndex == 2)
        {
            directionalLight.gameObject.SetActive(true);
        }
        else
            directionalLight.gameObject.SetActive(false);
        RotateSun();
    }
    public void RotateSun()
    {
        float turnSpeed = sunTurnSpeed * Time.deltaTime;

        // yön belirleme
        if (!isReversing)
            sunAngleX += turnSpeed;
        else
            sunAngleX -= turnSpeed;

        if (sunAngleX >= sunMaX)
        {
            sunAngleX = sunMaX;
            isReversing = true;
        }
        else if (sunAngleX <= sunMinX)
        {
            sunAngleX = sunMinX;
            isReversing = false;
        }
        directionalLight.transform.rotation = Quaternion.Euler(sunAngleX, 0f, 0f);
    }


}
