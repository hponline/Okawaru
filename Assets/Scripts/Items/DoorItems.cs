using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorItems : baseInteractableItems
{
    bool open = false;
    public float openZRotation = 90f;
    public override void OnUse(PlayerStats player)
    {
        open = !open;
        float z = open ? openZRotation : 0f;
        transform.localRotation = Quaternion.Euler(-90, 0, z);
    }
}
