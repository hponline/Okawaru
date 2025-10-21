using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class DiscoButton : baseInteractableItems
{
    public GameObject discoBtnAnim;
    public GameObject discoBallAnim;
    public GameObject canvasAnim;
    public GameObject discoPlane;
    public GameObject particle;
    public GameObject video;

    Animator _discoBtn;
    Animator _discoBall;
    Animator _canvasAnim;
    Animator _discoPlane;

    public bool canUse = true;

    private void Start()
    {
        _discoBtn = discoBtnAnim.GetComponent<Animator>();
        _discoBall = discoBallAnim.GetComponent<Animator>();
        _canvasAnim = canvasAnim.GetComponent<Animator>();
        _discoPlane = discoPlane.GetComponent<Animator>();

        Debug.Log("Video animasyonuna bak");
        Debug.Log("Animasyonlarý baþlangýç pozisyonlarýna döndür");
    }

    public override void OnUse(PlayerStats player)
    {
        StartDisco();
        //if (!canUse)
        //{
        //    GameManager.instance.ShowMessageText("Tekrar kullanmadan bekle");
        //    return;
        //}
        //canUse = false;
        //Play();
    }

    public void StartDisco()
    {
        PlayBallAnimation();
        PlayBtnAnimation();
        PlayCanvasAnimation();
        PlayPlaneAnimation();
        particle.SetActive(true);
    }
    //public void Play()
    //{
    //    PlayBallAnimation();
    //    PlayerStats.Instance.Cooldown(0.5f, () => canUse = true);
    //    PlayBtnAnimation();
    //    PlayerStats.Instance.Cooldown(0.5f, () => canUse = true);
    //    PlayCanvasAnimation();
    //    PlayerStats.Instance.Cooldown(0.5f, () => canUse = true);
    //    PlayPlaneAnimation();
    //    PlayerStats.Instance.Cooldown(0.5f, () => canUse = true);
    //    particle.SetActive(true);
    //    PlayerStats.Instance.Cooldown(10, () => canUse = true);
    //}


    public void PlayBtnAnimation()
    {
        _discoBtn.SetTrigger("IsDisco");
    }

    public void PlayBallAnimation()
    {
        _discoBall.SetTrigger("DiscoBall");
    }

    public void PlayCanvasAnimation()
    {
        video.SetActive(true);
        _canvasAnim.SetTrigger("DiscoVideo");
    }

    public void PlayPlaneAnimation()
    {
        _discoPlane.SetTrigger("DiscoPlane");
    }
}
