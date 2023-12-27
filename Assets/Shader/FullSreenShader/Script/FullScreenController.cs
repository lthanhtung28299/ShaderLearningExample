using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FullScreenController : MonoBehaviour
{
    [SerializeField] private Button btnPre, btnNext, btnPlay;
    [SerializeField] private TMP_Text txtTransitionPattern;
    [SerializeField] private Image fullScreenImg;
    [SerializeField] private List<Sprite> patternSprites;

    private int _patternIndex;
    private Sequence _transitionSeq;
    private static readonly int Cutoff = Shader.PropertyToID("_Cutoff");

    private void Start()
    {
        btnPre.onClick.AddListener(OnSelectPreviousPattern);
        btnNext.onClick.AddListener(OnSelectNextPattern);
        btnPlay.onClick.AddListener(OnPlayTransition);
        txtTransitionPattern.text = patternSprites[_patternIndex].name;
        fullScreenImg.sprite = patternSprites[_patternIndex];
    }

    private void OnSelectPreviousPattern()
    {
        _patternIndex--;
        _patternIndex = _patternIndex < 0 ? patternSprites.Count - 1 : _patternIndex;
        _patternIndex %= patternSprites.Count;
        txtTransitionPattern.text = patternSprites[_patternIndex].name;
    }

    private void OnSelectNextPattern() 
    {
        _patternIndex++;
        _patternIndex %= patternSprites.Count;
        txtTransitionPattern.text = patternSprites[_patternIndex].name;
    }

    private void OnPlayTransition()
    {
        _transitionSeq = DOTween.Sequence()
            .OnStart(() =>
            {
                fullScreenImg.sprite = patternSprites[_patternIndex];
                btnPlay.interactable = false;
            })
            .AppendCallback(DOVirtual.Float(2, -1, 1, s =>
            {
                fullScreenImg.material.SetFloat(Cutoff, s);
            }).onPlay)
            .AppendCallback(DOVirtual.Float(-1, 2, 1, s =>
            {
                fullScreenImg.material.SetFloat(Cutoff, s);
            }).onPlay)
            .OnComplete(() => btnPlay.interactable = true);
    }
}
