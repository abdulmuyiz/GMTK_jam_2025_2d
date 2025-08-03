using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    public GameManger gameManger;
    public RectTransform topIntro, botIntro, upgrade, gameOver; 
    public float topPosX, botPosX, midPos;
    public float tweenDuration;
    public bool popUp= true;
    public bool popDown = true;

    // Update is called once per frame
    void Update()
    {
        if(gameManger.initEemiesCount <= 0 && popUp)
        {
            StartCoroutine(uiPOP());
        }
        if (gameManger.playerHealth <= 0 && popDown)
        {
            gameOver.gameObject.SetActive(true);
            gameOver.DOAnchorPosY(-30, tweenDuration).SetUpdate(true);
            popDown = false;
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("Backshots");
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Menu");
    }

    void Intro()
    {
        topIntro.gameObject.SetActive(true);
        botIntro.gameObject.SetActive(true);
        topIntro.DOAnchorPosX(midPos, tweenDuration).SetUpdate(true);
        upgrade.DOAnchorPosX(775, tweenDuration).SetUpdate(true);
        botIntro.DOAnchorPosX(midPos, tweenDuration).SetUpdate(true); ;
    }

    void Outro()
    {
        //await topIntro.DOAnchorPosX(topPosX, tweenDuration).SetUpdate(true).AsyncWaitForCompletion;
        //await botIntro.DOAnchorPosX(botPosX, tweenDuration).SetUpdate(true).AsyncWaitForCompletion;
        topIntro.DOAnchorPosX(botPosX, tweenDuration).SetUpdate(true);
        upgrade.gameObject.SetActive(false);
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
