public class StaminaItems : baseInteractableItems
{
    public override void OnUse(PlayerStats player)
    {
        if (player == null) return;

        if (ItemData.itemValue != 0)
        {
            player.RestStamina(ItemData.itemValue);
        }
    }
}
