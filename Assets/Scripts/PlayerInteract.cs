using System.Collections;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float interactRange = 2f;
    public LayerMask interactLayer;

    PlayerStats playerStats;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            TryInteract();
            Debug.Log("F bastý");
        }
    }

    void TryInteract()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactRange, interactLayer);
        if (hits == null || hits.Length == 0) return;

        foreach (var hit in hits)
        {
            var interactable = hit.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact(playerStats);
            }
            else
            {
                var baseItem = hit.GetComponent<baseInteractableItems>();
                if (baseItem != null)
                    baseItem.Interact(playerStats);
            }
        }        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
