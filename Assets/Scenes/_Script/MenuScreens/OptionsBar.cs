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

        [SerializeField] TMP_Text m_EnemyStoneLabel;
        [SerializeField] TMP_Text m_EnemyStoneUpStarLabel;
        [SerializeField] TMP_Text m_BadgeLabel;
        [SerializeField] TMP_Text m_RubyLabel;

        //
        [SerializeField] TMP_Text m_BadgeGuild;

        //----------------------------------------------
        [SerializeField] TMP_Text m_EnergyLabel;
        [SerializeField] TMP_Text m_EnemyStoneMapLabel;
        [SerializeField] TMP_Text m_EnemyStoneBossMapLabel;
        //private void Start()
        //{
        //    GameDataManager.Instance.OnEnergyChanged += () => Debug.Log("Manual Test: Energy Changed");
        //}
        private void Start()
        {
            //if (GameDataManager.Instance == null)
            //{
            //    //Debug.LogError("GameDataManager.Instance is null in OnEnable!");
            //    return;
            //}

            Debug.Log("Registering events...");
            GameDataManager.ResourcesMapUpdated += OnResourceMapUpdated;

            GameDataManager.FundsUpdated += OnFundsUpdated;
            GameDataManager.StonesUpdated += OnStoneUpdated;
            GameDataManager.Instance.OnEnergyChanged += UpdateEnergyTextCurrent;     
        }


        private void OnDisable()
        {
            GameDataManager.ResourcesMapUpdated -= OnResourceMapUpdated;


            GameDataManager.FundsUpdated -= OnFundsUpdated;
            GameDataManager.StonesUpdated -= OnStoneUpdated;
            GameDataManager.Instance.OnEnergyChanged -= UpdateEnergyTextCurrent;

        }
        private void UpdateEnergyTextCurrent()
        {
            if (m_EnergyLabel == null)
            {
                Debug.LogError("m_EnergyLabel is null!");
                return;
            }

            m_EnergyLabel.text = "" + GameDataManager.Instance.CurrentEnergyAmount + "/5";
            //Debug.Log("Call UpdateEnergyTextCurrent");
        }
        public void SetResourceUI(uint stoneEnemy, uint stoneBoss)
        {
            m_EnemyStoneMapLabel.text = stoneEnemy.ToString();
            m_EnemyStoneBossMapLabel.text = stoneBoss.ToString();

            Debug.Log("SetResourceUI: "+ stoneEnemy +" -- " + stoneBoss);
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
            ////////////////////////////

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


            Debug.Log("OnStoneUpdated");

        }
        void OnFundsUpdated(GameData gameData)
        {
            SetBadge(gameData.badGe);
            SetStoneBoss(gameData.StoneBoss);
            SetStoneEnemy(gameData.StoneEnemy);
            SetRuby(gameData.ruby);
            SetBadgeGuild(gameData.badGe);
            Debug.Log("OnFundsUpdated");

        }
        void OnResourceMapUpdated(GameData gameData)
        {
            SetResourceUI(gameData.StoneEnemy, gameData.StoneBoss);

            Debug.Log("AAAA");
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