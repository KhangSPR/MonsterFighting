using System;
using System.Collections;
using TMPro;
using UIGameDataManager;
using UnityEngine;


namespace UIGameDataManager
{
    public class OptionsBar : MonoBehaviour
    {

        const float k_LerpTime = 0.6f;

        [SerializeField] TMP_Text m_GoldLabel;
        [SerializeField] TMP_Text m_EnemyStoneUpStarLabel;
        [SerializeField] TMP_Text m_EnemyStoneLabel;

        private void OnEnable()
        {

            GameDataManager.FundsUpdated += OnFundsUpdated;
            GameDataManager.StonesUpdated += OnStoneUpdated;
        }

        private void OnDisable()
        {
            GameDataManager.FundsUpdated -= OnFundsUpdated;
            GameDataManager.StonesUpdated -= OnStoneUpdated;

        }


        public void SetGold(uint gold)
        {
            uint startValue = (uint)Int32.Parse(m_GoldLabel.text);
            StartCoroutine(LerpRoutine(m_GoldLabel, startValue, gold, k_LerpTime));
        }

        public void SetStoneBoss(uint gems)
        {
            uint startValue = (uint)Int32.Parse(m_EnemyStoneUpStarLabel.text);
            StartCoroutine(LerpRoutine(m_EnemyStoneUpStarLabel, startValue, gems, k_LerpTime));
        }
        public void SetStoneEnemy(uint gems)
        {
            uint startValue = (uint)Int32.Parse(m_EnemyStoneLabel.text);
            StartCoroutine(LerpRoutine(m_EnemyStoneLabel, startValue, gems, k_LerpTime));
        }
        void OnStoneUpdated(GameData gameData)
        {
            //SetGold(gameData.gold);
            SetStoneBoss(gameData.enemyBoss);
            SetStoneEnemy(gameData.enemyStone);
        }
        void OnFundsUpdated(GameData gameData)
        {
            SetGold(gameData.gold);
            SetStoneBoss(gameData.enemyBoss);
            SetStoneEnemy(gameData.enemyStone);
        }

        // animated Label counter
        IEnumerator LerpRoutine(TMP_Text label, uint startValue, uint endValue, float duration)
        {
            float lerpValue = (float)startValue;
            float t = 0f;
            label.text = string.Empty;

            while (Mathf.Abs((float)endValue - lerpValue) > 0.05f)
            {
                t += Time.deltaTime / k_LerpTime;

                lerpValue = Mathf.Lerp(startValue, endValue, t);
                label.text = lerpValue.ToString("0");
                yield return null;
            }
            label.text = endValue.ToString();
        }
    }
}