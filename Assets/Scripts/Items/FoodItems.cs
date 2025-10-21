public class FoodItems : baseInteractableItems
{
    public override void OnUse(PlayerStats player)
    {
        if (player == null) return;

        if (ItemData.itemValue != 0 )
        {
            player.EatFood(ItemData.itemValue);
            Destroy(gameObject);
        }
    }
}
