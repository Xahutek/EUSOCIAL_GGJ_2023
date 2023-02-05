using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class BlockedPop : MonoBehaviour
{
    Tween
        PopTween,
        ColorTween;
    Color col;
    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
        col=image.color;
    }
    private void Update()
    {
        if (!GameManager.playerAgency&&(Input.GetMouseButtonDown(0)|| Input.GetMouseButtonDown(1)|| Input.GetMouseButtonDown(2)))
        {
            DOTween.Kill(PopTween);
            DOTween.Kill(ColorTween);

            PopTween = transform.DOShakePosition(0.5f,1);
            ColorTween = image.DOColor(Color.red, 0.2f).OnComplete(() => { ColorTween = image.DOColor(col, 0.5f); });
        }
    }
}
