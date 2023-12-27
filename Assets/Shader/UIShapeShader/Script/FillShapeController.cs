using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class FillShapeController : MonoBehaviour
{
    [SerializeField] private Image starImg, heartImg;
    [SerializeField] private Slider sliderFill;
    private static readonly int FillAmount = Shader.PropertyToID("_FillAmount");

    private void Start()
    {
        sliderFill.onValueChanged.AddListener(ControlFill);
        sliderFill.value = 0.5f;
    }

    private void ControlFill(float value)
    {
        starImg.materialForRendering.SetFloat(FillAmount, 1 - value);
        heartImg.materialForRendering.SetFloat(FillAmount, 1 - value);
    }
}