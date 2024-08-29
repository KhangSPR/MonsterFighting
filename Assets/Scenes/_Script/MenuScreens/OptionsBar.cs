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

        [SerializeField] TMP_Text m_BadgeLabel;
        [SerializeField] TMP_Text m_EnemyStoneLabel;
        [SerializeField] TMP_Text m_EnemyStoneUpStarLabel;
        [SerializeField] TMP_Text m_RubyLabel;

        //
        [SerializeField] TMP_Text m_BadgeGuild;


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


        public void SetBadge(uint gold)
        {
            uint startValue = (uint)Int32.Parse(m_BadgeLabel.text);
            StartCoroutine(LerpRoutine(m_BadgeLabel, startValue, gold, k_LerpTime));
        }
        public void SetRuby(uint gold)
        {
            uint startValue = (uint)Int32.Parse(m_RubyLabel.text);
            StartCoroutine(LerpRoutine(m_RubyLabel, startValue, gold, k_LerpTime));
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
            SetStoneBoss(gameData.StoneBoss);
            SetStoneEnemy(gameData.StoneEnemy);
        }
        void OnFundsUpdated(GameData gameData)
        {
            SetBadge(gameData.badGe);
            SetStoneBoss(gameData.StoneBoss);
            SetStoneEnemy(gameData.StoneEnemy);
            SetRuby(gameData.ruby);
            SetBadgeGuild(gameData.badGe);

        }
        public void SetBadgeGuild(uint badguild)
        {
            uint startValue = (uint)Int32.Parse(m_BadgeGuild.text);
            StartCoroutine(LerpRoutine(m_BadgeGuild, startValue, badguild, k_LerpTime));
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