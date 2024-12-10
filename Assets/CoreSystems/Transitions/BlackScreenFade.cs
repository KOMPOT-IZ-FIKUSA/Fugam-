using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlackScreenFade : MonoBehaviour
{
    public bool FadeInOnEnable = false;

    private Canvas canvas;
    private Image blackRenderer;

    public Sprite Black;

    private bool fadeInInProgress = false;
    private bool fadeOutInProgress = false;
    private float timeLeft = 0;

    public float FadeTime = 1;

    public Action OnFinished = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (fadeInInProgress)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                timeLeft = 0;
                fadeInInProgress = false;
                if (OnFinished != null) OnFinished();
            }
            blackRenderer.color = new Color(1, 1, 1, timeLeft / FadeTime);
        }
        else if (fadeOutInProgress)
        {
            timeLeft -= Time.deltaTime;
            if (timeLeft <= 0)
            {
                timeLeft = 0;
                fadeOutInProgress = false;
                if (OnFinished != null) OnFinished();
            }
            blackRenderer.color = new Color(1, 1, 1, 1 - timeLeft / FadeTime);
        }
    }

    private void OnEnable()
    {
        if (FadeInOnEnable)
        {
            FadeIn();
        }
    }



    public void FadeOut()
    {
        _prepareCanvas();
        blackRenderer.color = new Color(1, 1, 1, 0);
        fadeOutInProgress = true;
        timeLeft = FadeTime;
    }

    private void _prepareCanvas()
    {
        canvas = GetComponent<Canvas>();
        if (canvas == null)
        {
            canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        }
        blackRenderer = transform.GetComponentInChildren<Image>();
        if (blackRenderer == null)
        {
            GameObject child = new GameObject("black");
            child.transform.parent = transform;
            blackRenderer = child.AddComponent<Image>();
        }
        blackRenderer.sprite = Black;
        blackRenderer.rectTransform.localScale = new Vector3(100f, 100f, 100f);
    }

    public void FadeIn()
    {
        _prepareCanvas();
        blackRenderer.color = new Color(1, 1, 1, 1);
        fadeInInProgress = true;
        timeLeft = FadeTime;
    }
}
