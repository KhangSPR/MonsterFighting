using TMPro;
using UIGameDataManager;
using UnityEngine;
using UnityEngine.UI;

public class ListCardCharacterData : SaiMonoBehaviour
{
    [SerializeField] Transform CardContent;

    public void InstantiateObjectsFromData()
    {
        foreach (Transform child in CardContent)
        {
            Destroy(child.gameObject);
        }

        foreach (CardCharacter cardCharacterData in CardManager.Instance.CardManagerData.CardCharacters)
        {
            GameObject newObj = CardSpawner.Instance.Spawn("CardSpawner", transform.position, Quaternion.identity).gameObject;

            GameObject newCardObject = Instantiate(newObj, CardContent);

            CardButton cardButton = newCardObject.GetComponent<CardButton>();

            cardButton.SetUICard(cardCharacterData);

            newCardObject.SetActive(true);
        }
        Debug.Log("Click");
    }
}
