using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public CoinManagement coinManagement;




    int coins = 0;

    public void AddCoins(int amount)
    {


        coins += amount;
        coinManagement.UpdateCoinUI(coins);

    }

}
