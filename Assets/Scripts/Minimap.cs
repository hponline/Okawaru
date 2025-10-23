using UnityEngine;

public class Minimap : MonoBehaviour
{
    public GameObject player;
    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, 40, player.transform.position.z);
    }

}
