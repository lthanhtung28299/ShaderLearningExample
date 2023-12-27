using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapImageShaderController : MonoBehaviour
{
    [SerializeField] private Image imageSwap;
    [SerializeField] private Material imgSwapMT;
    [SerializeField] private Toggle tglDissolve, tglFlipX, tglFlipY, tglHorizontalGra, tlgVerticalGra;
    [SerializeField] private Slider sldProgress;

    private static readonly int FlipTransition = Shader.PropertyToID("_FlipTransition");
    private static readonly int Transition = Shader.PropertyToID("_Transition");
    private static readonly int Dissolve = Shader.PropertyToID("_DISSOLVE");
    private static readonly int FlipX = Shader.PropertyToID("_FLIPX");
    private static readonly int FlipY = Shader.PropertyToID("_FLIPY");
    private static readonly int HorizontalGradient = Shader.PropertyToID("_GRADIENTHOR");
    private static readonly int VerticalGradient = Shader.PropertyToID("_GRADIENTVERT");

    private const string DissolveKeyword = "DISSOLVE";
    private const string FLIPX = "FLIPX";
    private const string FLIPY = "FLIPY";
    private const string GRADIENTHOR = "GRADIENTHOR";
    private const string GRADIENTVERT = "GRADIENTVERT";


    private bool _dissolve;
    private bool _flipX;
    private bool _flipY;
    private bool _horiGra;
    private bool _vertiGra;

    private void Start()
    {
        sldProgress.onValueChanged.AddListener(OnChangeProgress);
        tglDissolve.onValueChanged.AddListener(OnChangeDissolve);
        tglFlipX.onValueChanged.AddListener(OnChangeFlipX);
        tglFlipY.onValueChanged.AddListener(OnChangeFlipY);
        tglHorizontalGra.onValueChanged.AddListener(OnChangeHorizontalGradient);
        tlgVerticalGra.onValueChanged.AddListener(OnChangeVerticalGradient);
        SetDefault();
    }

    private void OnChangeProgress(float value)
    {
        var property = _flipX || _flipY ? FlipTransition : Transition;
        imageSwap.material.SetFloat(property, value);
    }

    private void OnChangeDissolve(bool isOn)
    {
        _dissolve = isOn;
        UpdateAllValueMaterial();
    }

    private void OnChangeFlipX(bool isOn)
    {
        _flipX = isOn;
        UpdateAllValueMaterial();
    }

    private void OnChangeFlipY(bool isOn)
    {
        _flipY = isOn;
        UpdateAllValueMaterial();
    }

    private void OnChangeHorizontalGradient(bool isOn)
    {
        _horiGra = isOn;
        UpdateAllValueMaterial();
    }

    private void OnChangeVerticalGradient(bool isOn)
    {
        _vertiGra = isOn;
        UpdateAllValueMaterial();
    }

    private void UpdateAllValueMaterial()
    {
        imageSwap.material.SetFloat(Dissolve, _dissolve ? 1 : 0);
        UpdateMaterial(_dissolve, DissolveKeyword);
        imageSwap.material.SetFloat(FlipX, _flipX ? 1 : 0);
        UpdateMaterial(_flipX, FLIPX);
        imageSwap.material.SetFloat(FlipY, _flipY ? 1 : 0);
        UpdateMaterial(_flipY, FLIPY);
        imageSwap.material.SetFloat(HorizontalGradient, _horiGra ? 1 : 0);
        UpdateMaterial(_horiGra, GRADIENTHOR);
        imageSwap.material.SetFloat(VerticalGradient, _vertiGra ? 1 : 0);
        UpdateMaterial(_vertiGra, GRADIENTVERT);
        var maxValue = _flipX || _flipY ? 1 : 1.4f;
        sldProgress.maxValue = maxValue;
    }

    private void UpdateMaterial(bool isOn, string keyword)
    {
        if (isOn)
        {
            imageSwap.material.EnableKeyword(keyword);
        }
        else
        {
            imageSwap.material.DisableKeyword(keyword);
        }
    }

    private void SetDefault()
    {
        tglDissolve.isOn = true;
        UpdateAllValueMaterial();
    }
}