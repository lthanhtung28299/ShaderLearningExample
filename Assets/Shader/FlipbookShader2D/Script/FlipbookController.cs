using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlipbookController : MonoBehaviour
{
    [SerializeField] private Image fbSlash, fbWind, fbRecovery;
    [SerializeField] private Toggle autoLoop;
    [SerializeField] private Slider flipBookProgress;
    private static readonly int AutoLoop = Shader.PropertyToID("_AutoLoop");
    private static readonly int Tile = Shader.PropertyToID("_Tile");
    private static readonly int TextureWidth = Shader.PropertyToID("_TextureWidth");
    private static readonly int TextureHeight = Shader.PropertyToID("_TextureHeight");
    private const string AutoLoopKeyword = "AUTOLOOP";

    private void Start()
    {
        flipBookProgress.onValueChanged.AddListener(OnProgressChange);
        autoLoop.onValueChanged.AddListener(OnAutoLoop);
        var width = fbSlash.material.GetFloat(TextureWidth);
        var height = fbSlash.material.GetFloat(TextureHeight);
        flipBookProgress.maxValue = width * height;
        autoLoop.isOn = true;
    }

    private void OnAutoLoop(bool isOn)
    {
        UpdateMaterial(fbSlash.material,AutoLoop, AutoLoopKeyword,isOn);
        UpdateMaterial(fbWind.material,AutoLoop, AutoLoopKeyword,isOn);
        UpdateMaterial(fbRecovery.material,AutoLoop, AutoLoopKeyword,isOn);
    }

    private void OnProgressChange(float value)
    {
        fbSlash.material.SetFloat(Tile,value);
        fbWind.material.SetFloat(Tile,value);
        fbRecovery.material.SetFloat(Tile,value);
    }

    private void UpdateMaterial(Material material,int propertyId, string keyword, bool isOn)
    {
        material.SetFloat(propertyId,isOn ? 1 : 0);
        if (isOn)
        {
            material.EnableKeyword(keyword);
        }
        else
        {
            material.DisableKeyword(keyword);
        }
    }
}