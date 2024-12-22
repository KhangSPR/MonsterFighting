using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CardFX : MonoBehaviour {
    [SerializeField] GameObject avatar, bg;

    private void Start(){
        var sequence = DOTween.Sequence();

        sequence.AppendInterval(0.5f);

        sequence.Append(bg.transform.DOScaleX(
            endValue: 1,
            duration: 1)
        .From(
            fromValue: 0))
        .SetEase(Ease.OutQuart);

        sequence.Join(avatar.transform.DOScaleX(
            endValue: 1,
            duration: 1)
        .From(
            fromValue: 0))
        .SetEase(Ease.OutQuart);
    }
}
