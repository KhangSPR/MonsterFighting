using System.Collections;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;

namespace UIGameDataManager
{
    public class StatReader : MonoBehaviour
    {
        [SerializeField] private CardStatsTower cardStatsTower;
        public StatKeys statKey;

        public Text statText;
        public Text differenceText;

        private float currentValue;
        private int currentLevel;


        private CharacterData characterData;

        Stat stat;
        private void OnEnable()
        {
            SetClassReader();

            statText.text = stat.statValue.ToString();
        }
        private void OnDisable()
        {
            differenceText.text = "";
        }
        public void SetClassReader()
        {
            //Gan Class
            characterData = cardStatsTower.GetCharacterData();
            stat = cardStatsTower.GetStat(statKey);

            currentValue = stat.statValue;
            currentLevel = characterData.CurrentLevel;
        }
        public float GetCurrentValue()
        {
            return currentValue;
        }
        //private void Update()
        //{
        //    this.UpdateText();
        //}
        public void UpdateText()
        {
            if (currentLevel != characterData.CurrentLevel)
            {

                currentLevel = characterData.CurrentLevel;
                stat = cardStatsTower.GetStat(statKey);

                float difference = Mathf.Abs(stat.statValue - currentValue);


                //Debug.Log("stat.Key: " + stat.statKey.ToString());


                StopAllCoroutines();
                if (difference % 1 != 0)  // Kiểm tra difference là số thập phân
                {
                    differenceText.text = "-" + difference.ToString("F2");
                    //StartCoroutine
                    StartCoroutine(TickTextUpBinary(difference));

                }
                else if (difference > 0)
                {
                    differenceText.text = "+" + difference;
                    //StartCoroutine
                    StartCoroutine(TickTextUp(difference));

                }
                else if (difference < 0)
                {
                    differenceText.text = "-" + difference;
                    differenceText.color = new Color(255, 0, 0, 255);
                    //StartCoroutine
                    StartCoroutine(TickTextDown(difference));

                }
                //else
                //{
                //    differenceText.text = difference.ToString();
                //    StartCoroutine(TickTextUp(difference));

                //    Debug.Log("difference = 0");
                //}
            }
        }

        public IEnumerator TickTextUp(float difference)
        {
            yield return new WaitForSeconds(1f);
            while (difference > 0)
            {
                difference--;
                currentValue++;
                differenceText.text = "+" + difference.ToString();

                if (statKey.ToString() == "SpecialAttack")
                    statText.text = currentValue.ToString() + "/s";
                else
                    statText.text = currentValue.ToString();

                yield return new WaitForSeconds(0.1f);
            }

            differenceText.text = "";
        }
        public IEnumerator TickTextUpBinary(float difference)
        {
            yield return new WaitForSeconds(1f);

            while (difference > 0) // Sử dụng ngưỡng nhỏ hơn
            {
                difference -= 0.01f;
                currentValue -= 0.01f;
                differenceText.text = "-" + difference.ToString("F2");
                statText.text = currentValue.ToString("F2") + "/s";

                yield return new WaitForSeconds(0.1f);
            }


            differenceText.text = "";
        }




        public IEnumerator TickTextDown(float difference)
        {
            yield return new WaitForSeconds(1f);
            while (difference < 0)
            {
                difference++;
                currentValue--;
                differenceText.text = "-" + difference.ToString();
                statText.text = currentValue.ToString();

                yield return new WaitForSeconds(0.1f);
            }

            differenceText.text = "";
        }
    }
}
