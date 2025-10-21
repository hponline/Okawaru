using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public int sahne;
    public void SahneYukle(int sahne)
    {
        SceneManager.LoadScene(sahne);
    }
    public void exit()
    {
        Application.Quit();
    }
}
