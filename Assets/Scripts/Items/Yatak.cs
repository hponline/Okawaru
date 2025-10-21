public class Yatak : baseInteractableItems
{
    public int cooldown = 30;
    public bool canUse = true;
    public override void OnUse(PlayerStats player)
    {
        if (!canUse)
        {
            GameManager.instance.ShowMessageText("Tekrar kullanmadan bekle");
            return;
        }

        canUse = false;
        player.RestStamina(100);
        player.RestSanity(100);
        player.EatFood(100);
        GameManager.instance.ShowMessageText($"stamina: {100}, sanity: {100}, hunger: {100}");
        StartCoroutine(player.Cooldown(cooldown, () => canUse = true));
    }
}
