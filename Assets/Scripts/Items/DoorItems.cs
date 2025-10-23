using UnityEngine;

public class DoorItems : baseInteractableItems
{
    bool open = false;
    public float openZRotation = 90f;
    AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public override void OnUse(PlayerStats player)
    {
        open = !open;
        float z = open ? openZRotation : 0f;
        transform.localRotation = Quaternion.Euler(-90, 0, z);
        audioSource.Play();
    }
}
