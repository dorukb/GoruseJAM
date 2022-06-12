using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject creditsButton;
    [SerializeField] private GameObject settingsButton;
    [SerializeField] private Image Title;
    [SerializeField] private GameObject Credits;
    [SerializeField] private RectTransform texts;
    [SerializeField] private GameObject Settings;
    [SerializeField] private Slider slider;
    [SerializeField] private GameObject camera;

    private Movement mov;
    void Start()
    {
        StartCoroutine(ButtonMoves());
        mov = FindObjectOfType<Movement>();
        mov.DisablePlayerMovement();
    }

    private IEnumerator ButtonMoves()
    {
        yield return new WaitForSeconds(.2f);
        startButton.GetComponent<RectTransform>().DOLocalMoveX(0, 1f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(.33f);
        creditsButton.GetComponent<RectTransform>().DOLocalMoveX(0, 1f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(.33f);
        settingsButton.GetComponent<RectTransform>().DOLocalMoveX(0, 1f).SetEase(Ease.OutBack);
    }


    public void StartGame()
    {
        StartCoroutine(EndButtonMove());
        
    }

    private IEnumerator EndButtonMove()
    {
        startButton.GetComponent<RectTransform>().DOLocalMoveX(1300, 1f).SetEase(Ease.InBack);
        yield return new WaitForSeconds(.33f);
        creditsButton.GetComponent<RectTransform>().DOLocalMoveX(1300, 1f).SetEase(Ease.InBack);
        yield return new WaitForSeconds(.33f);
        settingsButton.GetComponent<RectTransform>().DOLocalMoveX(1300, 1f).SetEase(Ease.InBack);

        yield return new WaitForSeconds(1.1f);
        GetComponent<Image>().DOFade(0, .5f);
        camera.SetActive(true);

        yield return new WaitForSeconds(3f);
        Title.DOFade(0, 1f);
        mov.EnablePlayerMovement();
    }

    public void OpenCredits()
    {
        StartCoroutine(CreditsCor());
    }

    private IEnumerator CreditsCor()
    {
        Credits.SetActive(true);
        texts.DOMoveY(2500, 20f);
        yield break;
    }

    public void OpenSettings()
    {
        Settings.SetActive(true);
    }
    public void Back()
    {
        Credits.SetActive(false);
        Settings.SetActive(false);
    }

    public void SetSlider()
    {
        GameSettings.Instance.ChangeSensitivity(slider.value);
    }

    private void Update()
    {
        if (Settings.activeSelf)
        {
            SetSlider();
        }
    }
}
