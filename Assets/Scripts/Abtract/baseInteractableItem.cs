using UnityEngine;

public abstract class baseInteractableItems : MonoBehaviour, IInteractable
{
    public ItemData ItemData;

    public void Interact(PlayerStats playerStats)
    {
        OnUse(playerStats);
    }

    public abstract void OnUse(PlayerStats player);
}
