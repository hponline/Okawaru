
public class SanityItems : baseInteractableItems
{
    public override void OnUse(PlayerStats player)
    {
        if (player == null) return;

        if (ItemData.itemValue != 0)
        {
            player.RestSanity(ItemData.itemValue);
            Destroy(gameObject);
        }
    }
}
