﻿using System;
using System.Collections.Generic;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UIElements;


namespace UIToolkitDemo
{
    [Serializable]
    public struct MagnetData
    {
        // gold, gems, health potion, power potion
        public ShopItemType ItemType;

        // ParticleSystem pool
        public ObjectPoolBehaviour FXPool;

        // forcefield target
        public ParticleSystemForceField ForceField;
    }

    // FX generator when buying an item from the shop
    public class CoinMagnet : MonoBehaviour
    {
        [SerializeField] List<MagnetData> m_MagnetData;

        [Header("Camera")]
        [Tooltip("Use Camera and Depth to calculate world space positions.")]
        [SerializeField] Camera m_Camera;
        [SerializeField] float m_ZDepth = 10f;
        [Tooltip("3D offset to the particle emission")]
        [SerializeField] Vector3 m_SourceOffset = new Vector3(0f, 0.1f, 0f);

        // start and end coordinates for effect
        void OnEnable()
        {
            GameDataManager.TransactionProcessed += OnTransactionProcessed;
        }

        void OnDisable()
        {
            GameDataManager.TransactionProcessed -= OnTransactionProcessed;
        }

        ObjectPoolBehaviour GetFXPool(ShopItemType itemType)
        {
            MagnetData magnetData = m_MagnetData.Find(x => x.ItemType == itemType);
            return magnetData.FXPool;
        }

        ParticleSystemForceField GetForcefield(ShopItemType itemType)
        {
            MagnetData magnetData =  m_MagnetData.Find(x => x.ItemType == itemType);
            return magnetData.ForceField;
        }
        
        void PlayPooledFX(Vector2 screenPos, ShopItemType contentType)
        {
            Vector3 worldPos = screenPos.ScreenPosToWorldPos(m_Camera, m_ZDepth) + m_SourceOffset;

            ObjectPoolBehaviour fxPool = GetFXPool(contentType);

            // initialize ParticleSystem
            ParticleSystem ps = fxPool.GetPooledObject().GetComponent<ParticleSystem>();

            if (ps == null)
                return;

            ps.gameObject.SetActive(true);
            ps.gameObject.transform.position = worldPos;
            ParticleSystem.ExternalForcesModule externalForces = ps.externalForces;
            externalForces.enabled = true;

            // add the Forcefield for destination
            ParticleSystemForceField forceField = GetForcefield(contentType);
            forceField.gameObject.SetActive(true);
            externalForces.AddInfluence(forceField);

            ps.Play();

        }
        // buying an item from the ShopScreen
        void OnTransactionProcessed(ShopItemSO shopItem, Vector2 screenPos)
        {
            if(shopItem.contentType == ShopItemType.Watch)
            {
                PlayPooledFX(screenPos, shopItem.contentType);
            }
        }
    }
}
