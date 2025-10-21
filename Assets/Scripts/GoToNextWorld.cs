using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToNextWorld : MonoBehaviour
{
    public int goToScene;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + goToScene);
            PlayerStats.Instance.transform.position = new Vector3(0, 0, 0);
        }
    }
}
