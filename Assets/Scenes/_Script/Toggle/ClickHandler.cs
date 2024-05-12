using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickHandler : MonoBehaviour
{
    [SerializeField] GameObject[] pins;
    public GameObject[] Pins { get { return pins; } set { pins = value; } }

    [SerializeField] List<ToggleCard> cards;
    public List<ToggleCard> Cards { get { return cards; } set { cards = value; } }
}
