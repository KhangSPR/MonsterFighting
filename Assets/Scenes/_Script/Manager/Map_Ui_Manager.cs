using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Ui_Manager : SaiMonoBehaviour
{
    public static Map_Ui_Manager instance;
    public static Map_Ui_Manager Instance { get { return instance; } }

    public RectTransform UI_Top_left;
    public RectTransform UI_Top_center;
    public RectTransform UI_Top_right;
    public RectTransform UI_Bottom_right;
    public RectTransform UI_Bottom_left;
    public RectTransform UI_Bottom_center;
    public RectTransform UIWin;
    public RectTransform UILose;
    public RectTransform UICenter;
    protected override void Awake()
    {
        base.Awake();

        Map_Ui_Manager.instance = this;

    }
    public void OpenRectransform(RectTransform UI)
    {
        UI.gameObject.SetActive(true);
    }
    public void CloseRectransform(RectTransform UI)
    {
        UI.gameObject.SetActive(false);
    }
}