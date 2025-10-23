using UnityEngine;

public class DiscoButton : baseInteractableItems
{
    public GameObject discoBtnAnim;
    public GameObject discoBallAnim;
    public GameObject canvasAnim;
    public GameObject discoPlane;
    public GameObject particle;
    public bool canUse = true;
    Animator _discoBtn;

    private void Start()
    {
        _discoBtn = discoBtnAnim.GetComponent<Animator>();
    }

    public override void OnUse(PlayerStats player)
    {
        if (!canUse)
        {
            GameManager.instance.ShowMessageText("Tekrar kullanmadan bekle");
            return;
        }
        canUse = false;
        Play();        
        GameManager.instance.bgMusic.Stop();
        // 57 saniye sonra tekrar aktif hale getir ve objeleri kapat
        StartCoroutine(PlayerStats.Instance.Cooldown(57f, () =>
        {            
            canUse = true;
            discoBallAnim.SetActive(false);
            canvasAnim.SetActive(false);
            discoPlane.SetActive(false);
            _discoBtn.SetBool("IsDisco", false);
            GameManager.instance.bgMusic.Play();
        }));
    }
    public void Play()
    {
        _discoBtn.SetBool("IsDisco", true);
        discoPlane.SetActive(true);
        discoBallAnim.SetActive(true);
        canvasAnim.SetActive(true);
        particle.SetActive(true);
        PlayerStats.Instance.currentStamina += 500;
        PlayerStats.Instance.currentSanity += 500;
        PlayerStats.Instance.currentHunger += 500;
    }
}
