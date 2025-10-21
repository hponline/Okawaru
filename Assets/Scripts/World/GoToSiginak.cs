using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToSiginak : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(0);
            PlayerStats.Instance.transform.position = new Vector3 (0,0,0);
        }
    }
}
