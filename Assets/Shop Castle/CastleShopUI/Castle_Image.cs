using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Castle_Image : MonoBehaviour
{
    [Header("Image")]
    [SerializeField] Image icon;
    [SerializeField] Image backGround;
    [SerializeField] Image avatar;
    [SerializeField] Image group_StatCastle;
    [SerializeField] Image iconPurchase;

    [Header("TMP")]
    [SerializeField] TMP_Text nameCastle;
    [SerializeField] TMP_Text cost;
    [SerializeField] TMP_Text historyDes;
    [SerializeField] TMP_Text descriptionStat;

    [Header("Button")]
    [SerializeField] Button btn_Use_Castle_Button;
    public Button Btn_Use_Castle_Button => btn_Use_Castle_Button;
    [SerializeField] Button btn_Purchase;

    [Header("Component")]
    [SerializeField] UseCastleButtonUI useCastleButtonUI;
    [SerializeField] PurchasePopupUI purchasePopupUI;
    //[SerializeField] ResizeUI resizeUI;
    [SerializeField] BlurManager blurManager;
    public BlurManager BlurManager => blurManager;
    public void SetUI(CastleSO castleSO)
    {
        backGround.sprite = castleSO.backGround;
        group_StatCastle.sprite = castleSO.frameGround;
        icon.sprite = castleSO.icon;
        avatar.sprite = castleSO.avatar;
        historyDes.text = castleSO.castle_history;
        //Text Area
        SetTextDes_Area(castleSO.ability);

        nameCastle.text = castleSO.castle_name + " / KingDoms";
    }
    void SetTextDes_Area(Ability ability)
    {


        More_Ability more_Ability = ability as More_Ability;

        if (more_Ability == null)
        {
            descriptionStat.text = "";
            return;
        }

        string hpText = "";
        string goldText = "";
        string goldGrowthText = "";
        string slotText = "";
        if (more_Ability.moreHp_Plus > 0)
        { 
            hpText = "Máu thành tăng <color=red> " + more_Ability.moreHp_Plus + "HP</color>";
        }
        if(more_Ability.moreMoney_Plus > 0)
        {
            goldText = "Vàng tăng<color=yellow> " + more_Ability.moreMoney_Plus + "$</color>";
        }
        if(more_Ability.moreMoneyGrowth_Plus > 0)
        {
            goldGrowthText = "Tốc độ sản xuất tăng<color=yellow> +" + more_Ability.moreMoneyGrowth_Plus + "$</color>";
        }
        if(more_Ability.moreSlot_Plus > 0)
        {
            slotText = "Hàng chờ thẻ<color=green> +" + more_Ability.moreSlot_Plus + "</color>";
        }
        //Set Text Descript Stat Castle
        string[] texts = { hpText, goldText, goldGrowthText, slotText };
        descriptionStat.text = ConcatenateTextsWithSeparator(texts, "; ", ".");

    }
    string ConcatenateTextsWithSeparator(string[] texts, string separator, string lastSeparator)
    {
        string finalResult = "";

        // Kiểm tra xem có phải là lần cuối cùng không
        bool isLastValidText = false;

        // Kiểm tra từng biến text
        for (int i = 0; i < texts.Length; i++)
        {
            // Nếu text không rỗng
            if (!string.IsNullOrEmpty(texts[i]))
            {
                // Thêm vào kết quả cuối cùng
                finalResult += texts[i];

                // Kiểm tra xem text hiện tại có phải là text cuối cùng có giá trị hay không
                isLastValidText = true;
                for (int j = i + 1; j < texts.Length; j++)
                {
                    if (!string.IsNullOrEmpty(texts[j]))
                    {
                        isLastValidText = false;
                        break;
                    }
                }

                // Nếu text hiện tại không phải là text cuối cùng có giá trị, thêm separator
                if (!isLastValidText)
                {
                    finalResult += separator;
                }
                else
                {
                    // Nếu text hiện tại là text cuối cùng có giá trị, thêm lastSeparator
                    finalResult += lastSeparator;
                }
            }
        }
        //if (texts is "hpText")
        //    return finalResult + "HP";
        //else if (texts is "goldText")
        //    return finalResult + "$";
        //else

        return finalResult;
    }

    //Máu thành tăng<color=red>3HP</color>, Vàng tăng<color=yellow>5$</color>, Tốc độ sản xuất tăng<color=yellow>+1$</color>, Hàng chờ thẻ<color=green>+1</color>
    public void SetCost(int Cost)
    {
        int digitCount_Cost = Cost.ToString().Length;

        Debug.Log(digitCount_Cost);

        DigitCount_Cost(digitCount_Cost);

        string formattedCost = AddCommas(Cost);


        //Set Text
        cost.text = formattedCost;
    }
    public void LoadBlur()
    {
        blurManager.GetblurMaterial();
    }
    public void CheckIs_Owned(bool isOwned, bool Choosen_Item)
    {
        if (isOwned)
        {
            useCastleButtonUI.gameObject.SetActive(true);


            CompareChoosen_Item(Choosen_Item);

            purchasePopupUI.gameObject.SetActive(false);
        }
        else
        {
            useCastleButtonUI.gameObject.SetActive(false);
            purchasePopupUI.gameObject.SetActive(true);
        }
    }
    void CompareChoosen_Item(bool Choosen_Item)
    {
        if (Choosen_Item)
        {
            useCastleButtonUI.GetComponent<Button>().interactable = false;
        }
        else
        {
            useCastleButtonUI.GetComponent<Button>().interactable = true;
        }
    }
    void DigitCount_Cost(int digitCount)
    {
        // Tính toán khoảng cách di chuyển dựa trên số lượng chữ số
        float moveDistance = -13f * digitCount;

        MoveObject_Cost(moveDistance);
    }

    void MoveObject_Cost(float movement)
    {
        // Lấy RectTransform của m_CostIconGroup
        iconPurchase.GetComponent<RectTransform>().localPosition = new Vector2(movement, iconPurchase.GetComponent<RectTransform>().localPosition.y);
    }
    string AddCommas(int number)
    {
        string numberString = number.ToString();
        int length = numberString.Length;

        // Nếu chiều dài của chuỗi nhỏ hơn hoặc bằng 3, không cần thêm dấu phẩy
        if (length <= 3)
        {
            return numberString;
        }

        // Chèn dấu phẩy sau mỗi 3 chữ số từ cuối lên
        string result = "";
        int commaCount = 0;
        for (int i = length - 1; i >= 0; i--)
        {
            result = numberString[i] + result;
            commaCount++;
            if (commaCount == 3 && i > 0)
            {
                result = "," + result;
                commaCount = 0;
            }
        }

        return result;
    }
}
