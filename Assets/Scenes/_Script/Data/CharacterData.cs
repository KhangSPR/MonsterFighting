using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.TextCore.Text;
using Unity.VisualScripting;

namespace UIGameDataManager
{
    // stores data for character instance + static data from a ScriptableObject

    public class CharacterData : MonoBehaviour
    {
        // baseline data and common shared stats
        [SerializeField] CardCharacter m_CharacterBaseData;


        [SerializeField] GameObject m_PreviewInstance;


        //public GameObject PreviewInstance { get { return m_PreviewInstance; } set { m_PreviewInstance = value; } }
        public CardCharacter CharacterBaseData => m_CharacterBaseData;
        public void SetCharacterBaseData(CardCharacter cardTower)
        {
            m_CharacterBaseData = cardTower;

        }
    }
}