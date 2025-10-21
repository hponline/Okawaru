using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentObjects : MonoBehaviour
{
    public GameObject player;
    public GameObject gameManager;
    public GameObject dayNightManager;
    public GameObject canvas;
    public GameObject setup;


    void Awake()
    {
        // Eðer zaten baþka bir PersistentObjects varsa, kendini sil
        PersistentObjects[] existing = FindObjectsByType<PersistentObjects>(FindObjectsSortMode.None);
        if (existing.Length > 1)
        {
            Destroy(gameObject);
            return;
        }

        // Bu obje ve alt objeleri kalýcý yap
        DontDestroyOnLoad(gameObject);

        if (player != null) DontDestroyOnLoad(player);
        if (gameManager != null) DontDestroyOnLoad(gameManager);
        if (dayNightManager != null) DontDestroyOnLoad(dayNightManager);
        if (canvas != null) DontDestroyOnLoad(canvas);
        if (setup != null) DontDestroyOnLoad(setup);

        // Sahne yüklendiðinde tekrar kontrol et
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Fazla instance'larý kontrol et
        DestroyDuplicateWithTag("Player");
        DestroyDuplicateWithTag("GameManager");
        DestroyDuplicateWithTag("DayNightManager");
        DestroyDuplicateWithTag("Canvas");
        DestroyDuplicateWithTag("Setup");

    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void DestroyDuplicateWithTag(string tag)
    {
        var objs = GameObject.FindGameObjectsWithTag(tag);
        bool keptOne = false;
        foreach (var obj in objs)
        {
            if (!keptOne)
            {
                keptOne = true;
                continue;
            }
            Destroy(obj);
        }
    }
}
