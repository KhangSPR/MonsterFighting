using System.Collections;
using System.Collections.Generic;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Button _acceptLabel;
    [SerializeField] private Button _deAcceptLabel;
    [Header("Managers")]
    [SerializeField] DialogUI dialogManager;

    public CardPlayer cardPlayer;
    private void Start()
    {
        // Thêm sự kiện lắng nghe cho nút Accept
        _acceptLabel.onClick.AddListener(() => OnClick(true));

        // Thêm sự kiện lắng nghe cho nút DeAccept
        _deAcceptLabel.onClick.AddListener(() => OnClick(false));
    }

    private void OnClick(bool isAccept)
    {
        if (isAccept)
        {
            transform.parent.gameObject.SetActive(false);

            dialogManager.ChooseNext(cardPlayer);
        }
        else
        {
            gameObject.SetActive(false);    
        }
    }
}
