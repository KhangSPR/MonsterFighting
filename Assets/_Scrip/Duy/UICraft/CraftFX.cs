using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CraftFX : MonoBehaviour {
    [SerializeField] Transform item;
    [SerializeField] Transform combo;

    [SerializeField] ParticleSystem craftParticle;
    
    public void BonusEffect(){
        combo.DOScale(
            endValue: 1f,
            duration: 0.5f
        ).From(
            fromValue: 1.3f
        )
        .SetEase(Ease.InCubic);
    }

    public void CraftEffect(){
        var sequence = DOTween.Sequence();
        sequence.Append(
            item.DORotate(
                endValue: Vector3.zero,
                duration: 0f,
                mode: RotateMode.Fast
            )
        );
        sequence.Append(
            item.DOShakeRotation(
                duration: 0.5f,
                strength: 35f,
                vibrato: 8,
                randomness: 50f,
                randomnessMode: ShakeRandomnessMode.Harmonic
            )
            .SetEase(Ease.InQuint)
        );

        craftParticle.Play();
    }
}
