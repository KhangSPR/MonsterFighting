using DG.Tweening;
using System.Collections.Generic;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

public class Swipe : MonoBehaviour
{
    public Color[] colors;
    public GameObject scrollbar, imageContent;
    private float scrollPos = 0;
    private float[] pos;
    private bool runIt = false;
    private float time;
    private Button takeTheBtn;
    private int btnNumber;

    public float smoothTime = 0.2f;
    public float scaleLerpSpeed = 0.1f;
    public float minScale = 0.8f;
    public float maxScale = 1f;
    public float imageMaxScale = 1.2f;
    public float colorSmoothTime = 0.3f;

    private Tweener scaleTween;
    private Tweener[] colorTweens;
    public List<GuildChoosing> guildChoosings; // List to store buttons

    private void Start()
    {
        InitializePositions();
        InitializeTweens();
        DOTween.SetTweensCapacity(2000, 50);
    }
    public void InitializeTweens()
    {
        colorTweens = new Tweener[pos.Length];
    }
    private void Update()
    {
        UpdatePositions();

        if (runIt)
        {
            SmoothTransition();
            time += Time.deltaTime;
            if (time > 1f)
            {
                time = 0;
                runIt = false;
            }
        }

        if (Input.GetMouseButton(0))
        {
            scrollPos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            SnapToClosest();
        }

        ApplyScaling();
        UpdateButtonNumber();
    }

    public void InitializePositions()
    {
        pos = new float[transform.childCount];
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
        }
    }

    private void UpdatePositions()
    {
        if (pos.Length != transform.childCount)
        {
            InitializePositions();
        }
    }

    private void SmoothTransition()
    {
        for (int i = 0; i < pos.Length; i++)
        {
            if (scrollPos < pos[i] + (1f / (2 * pos.Length)) && scrollPos > pos[i] - (1f / (2 * pos.Length)))
            {
                scrollbar.GetComponent<Scrollbar>().value = Mathf.SmoothStep(scrollbar.GetComponent<Scrollbar>().value, pos[btnNumber], smoothTime);
            }
        }
    }

    private void SnapToClosest()
    {
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
            {
                scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], smoothTime);
            }
        }
    }

    private void ApplyScaling()
    {
        float distance = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            if (scrollPos < pos[i] + (distance / 2) && scrollPos > pos[i] - (distance / 2))
            {
                if (scaleTween != null)
                    scaleTween.Kill();
                if (colorTweens[i] != null)
                    colorTweens[i].Kill();

                scaleTween = transform.GetChild(i).DOScale(new Vector2(maxScale, maxScale), scaleLerpSpeed).SetEase(Ease.OutCubic);
                colorTweens[i] = imageContent.transform.GetChild(i).GetComponent<Image>().DOColor(colors[1], colorSmoothTime).SetEase(Ease.OutCubic);

                for (int j = 0; j < pos.Length; j++)
                {
                    if (j != i)
                    {
                        transform.GetChild(j).DOScale(new Vector2(minScale, minScale), scaleLerpSpeed).SetEase(Ease.OutCubic);
                        colorTweens[j] = imageContent.transform.GetChild(j).GetComponent<Image>().DOColor(colors[0], colorSmoothTime).SetEase(Ease.OutCubic);
                    }
                }
            }
        }
    }

    private void UpdateButtonNumber()
    {
        float closestDistance = Mathf.Infinity;
        int closestIndex = btnNumber;

        for (int i = 0; i < pos.Length; i++)
        {
            float distance = Mathf.Abs(scrollPos - pos[i]);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }

        if (btnNumber != closestIndex)
        {
            btnNumber = closestIndex;
            Debug.Log("btnNumber updated to: " + btnNumber);
            UpdateButtonInteractivity();
        }
        //else
        //{
        //    UpdateButtonInteractivity();

        //    Debug.Log("btnNumber != closestIndex");
        //}
    }

    public void UpdateButtonInteractivity()
    {
        if (guildChoosings == null) return;

        for (int i = 0; i < guildChoosings.Count; i++)
        {
            if (guildChoosings[i].GuildSO.Cost <= GameDataManager.Instance.GameData.badGe)
            {
                guildChoosings[i].ButtonPurchase.interactable = true;
            }
            else
            {
                guildChoosings[i].ButtonPurchase.interactable = false;
            }

            // Set the button interactable state based on both conditions
            guildChoosings[i].ButtonPurchase.interactable = guildChoosings[i].ButtonPurchase.interactable && (i == btnNumber);

            if (i != btnNumber)
            {
                guildChoosings[i].TextBlur.BlurTexts();
                guildChoosings[i].ICON.color = new Color(1, 1, 1, 90 / 255f);
            }
            else
            {
                guildChoosings[i].TextBlur.ClearBlur();
                guildChoosings[i].ICON.color = new Color(1, 1, 1, 1);

            }

        }
    }


    public void WhichBtnClicked(Button btn)
    {
        int clickedIndex = -1;
        for (int i = 0; i < btn.transform.parent.childCount; i++)
        {
            if (btn.transform.parent.GetChild(i).gameObject == btn.gameObject)
            {
                clickedIndex = i;

                Debug.Log("clickedIndex " + clickedIndex);
                break;
            }
        }

        if (clickedIndex == -1)
        {
            Debug.LogError("Button not found in its parent!");
            return;
        }

        if (clickedIndex == btnNumber)
        {
            Debug.Log("Return");

            return;
        }
        btnNumber = clickedIndex;
        takeTheBtn = btn;
        time = 0;
        scrollPos = pos[btnNumber];
        runIt = true;

        UpdateButtonInteractivity();
    }
}