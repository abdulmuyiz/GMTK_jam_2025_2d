using UnityEngine;
using DG.Tweening;
using System.Collections;

public class UIScript : MonoBehaviour
{
    public GameManger gameManger;
    public RectTransform topIntro, botIntro; 
    public float topPosX, botPosX, midPos;
    public float tweenDuration;
    public bool popUp= true;

    // Update is called once per frame
    void Update()
    {
        if(gameManger.initEemiesCount <= 0 && popUp)
        {
            StartCoroutine(uiPOP());
        }
    }

    void Intro()
    {
        topIntro.gameObject.SetActive(true);
        botIntro.gameObject.SetActive(true);
        topIntro.DOAnchorPosX(midPos, tweenDuration).SetUpdate(true);
        botIntro.DOAnchorPosX(midPos, tweenDuration).SetUpdate(true); ;
    }

    void Outro()
    {
        //await topIntro.DOAnchorPosX(topPosX, tweenDuration).SetUpdate(true).AsyncWaitForCompletion;
        //await botIntro.DOAnchorPosX(botPosX, tweenDuration).SetUpdate(true).AsyncWaitForCompletion;
        topIntro.DOAnchorPosX(botPosX, tweenDuration).SetUpdate(true);
        botIntro.DOAnchorPosX(topPosX, tweenDuration).SetUpdate(true); ;
    }

    private IEnumerator uiPOP()
    {
        popUp = false;
        Intro();
        yield return new WaitForSeconds(2f);
        Outro();
    }
}
