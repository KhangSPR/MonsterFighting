using UIGameDataMap;
using UnityEngine;

public class EnemyDropCoin : EnemyAbstract
{
    [SerializeField] private Rarity rarityEnemy;
    [SerializeField] private TypeEnemy typeEnemy;

    public void SetTypeEnemy(TypeEnemy _typeEnemy)
    {
        typeEnemy = _typeEnemy;
    }

    public void SetCoinRarity(Rarity _rarityEnemy)
    {
        rarityEnemy = _rarityEnemy;
    }

    public void DropCoin()
    {
        int coinCount = CalculateCoinCount();
        SpawnCoins(coinCount);
    }

    private int CalculateCoinCount() //Tinh toan so luong coin
    {
        int minCoins = 0;
        int maxCoins = 0;

        // Xử lý dựa trên loại enemy và độ hiếm
        switch (typeEnemy)
        {
            case TypeEnemy.Default:
                switch (rarityEnemy)
                {
                    case Rarity.Common:
                        minCoins = 0;
                        maxCoins = 1;
                        break;
                    case Rarity.Rare:
                        minCoins = 0;
                        maxCoins = 2;
                        break;
                    case Rarity.Epic:
                        minCoins = 1;
                        maxCoins = 3;
                        break;
                    case Rarity.Legendary:
                        minCoins = 2;
                        maxCoins = 5;
                        break;
                }
                break;

            case TypeEnemy.Boss:
                switch (rarityEnemy)
                {
                    case Rarity.Common:
                        minCoins = 1;
                        maxCoins = 3;
                        break;
                    case Rarity.Rare:
                        minCoins = 3;
                        maxCoins = 7;
                        break;
                    case Rarity.Epic:
                        minCoins = 7;
                        maxCoins = 12;
                        break;
                    case Rarity.Legendary:
                        minCoins = 12;
                        maxCoins = 15;
                        break;
                }
                break;
        }

        return Random.Range(minCoins, maxCoins + 1); // Số coin rơi ra
    }

    private void SpawnCoins(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Transform newCoin = FXSpawner.Instance.Spawn(FXSpawner.Coin, transform.position, Quaternion.identity);

            newCoin.gameObject.SetActive(true);
        }
    }
}
