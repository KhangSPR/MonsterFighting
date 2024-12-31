using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RewardFX : MonoBehaviour {
    [SerializeField] Transform holder;
    [SerializeField] GameObject itemPrf;
    [SerializeField] int itemCount;

    private void Start(){
        var sequence = DOTween.Sequence();
        for (int i = 0; i < itemCount; i++){
            sequence.AppendCallback(() => {
                var item = Instantiate(itemPrf, holder.transform).transform;
                // item.localScale = Vector3.one * 1.3f;
                item.DOScale(1, 1).From(1.3f)
                .SetEase(Ease.OutBack);
            });
            sequence.AppendInterval(1);
        }
    }
}
