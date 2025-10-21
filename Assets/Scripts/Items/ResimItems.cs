public class ResimItems : baseInteractableItems
{
    public override void OnUse(PlayerStats player)
    {
        if (player == null) return;

        player.RestSanity(250);
        player.PlayerIntereact();
    }
}
