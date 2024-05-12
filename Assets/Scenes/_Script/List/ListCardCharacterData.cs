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

        foreach (CardCharacter cardCharacterData in CardManager.Instance.CardManagerData.cardCharacter)
        {
            GameObject newObj = CardSpawner.Instance.Spawn("CardSpawner", transform.position, Quaternion.identity).gameObject;

            GameObject newCardObject = Instantiate(newObj, CardContent);

            newCardObject.SetActive(true);
            //Settings
            newCardObject.transform.Find("Frame").GetComponent<Image>().sprite = cardCharacterData.frame;
            newCardObject.transform.Find("Background").GetComponent<Image>().sprite = cardCharacterData.background;
            newCardObject.transform.Find("Name").GetComponent<TMP_Text>().text = cardCharacterData.nameCard;

            // Set Prefab Instance
            newCardObject.GetComponent<CardBtn>().CardPrefabSet = PlayerSpawner.Instance.GetGameobjectPrefabByName(cardCharacterData.nameCard);
            newCardObject.GetComponent<CardBtn>().CardCharacter = cardCharacterData;
            newCardObject.GetComponent<CardBtn>().Sprite = cardCharacterData.avatar;
            newCardObject.GetComponent<CardBtn>().Price = cardCharacterData.price;
            newCardObject.GetComponent<CardBtn>().CardRefresh.cooldownDuration = cardCharacterData.cardRefresh;
        }
        Debug.Log("Click");
    }
}
