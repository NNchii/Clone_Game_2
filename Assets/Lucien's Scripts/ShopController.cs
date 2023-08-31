using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopController : MonoBehaviour
{
    public PlayerController playerController; // Reference to the PlayerController script
    public GameObject shopPanel; // Reference to the shop UI panel
    public TMP_Text coinText; // Reference to the Text element displaying the coin count

    public Button speedBoostButton;
    public Button shieldButton;
    public Button rocketBoostButton;

    public TMP_Text speedBoostCostText;
    public TMP_Text shieldCostText;
    public TMP_Text rocketBoostCostText;

    void Start()
    {
        // Open the shop when the game starts
        OpenShop();
    }

    void Update()
    {
        int speedBoostNextCost = (playerController.GetSpeedBoostLevel() + 1) * playerController.upgradeIncrement;
        int shieldNextCost = (playerController.GetShieldLevel() + 1) * playerController.upgradeIncrement;
        int rocketBoostNextCost = (playerController.GetRocketBoostLevel() + 1) * playerController.upgradeIncrement;

        speedBoostButton.interactable = playerController.CanAffordUpgrade(playerController.GetSpeedBoostLevel());
        shieldButton.interactable = playerController.CanAffordUpgrade(playerController.GetShieldLevel());
        rocketBoostButton.interactable = playerController.CanAffordUpgrade(playerController.GetRocketBoostLevel());

        speedBoostCostText.text = "Speed Boost Upgrade Cost: " + speedBoostNextCost;
        shieldCostText.text = "Shield Upgrade Cost: " + shieldNextCost;
        rocketBoostCostText.text = "Rocket Boost Upgrade Cost: " + rocketBoostNextCost;
    }

    public void OpenShop()
    {
        // Pause the game
        playerController.ToggleGameplay(false);

        // Update the coin count display
        coinText.text = "Coins: " + playerController.GetCoinsCollected();

        // Show the shop panel
        shopPanel.SetActive(true);
    }

    public void CloseShop()
    {
        Debug.Log("CloseShop called");

        // Reset the player
        playerController.ResetPlayer();

        // Resume the game
        playerController.ToggleGameplay(true);

        // Hide the shop panel
        shopPanel.SetActive(false);
    }

    public void UpgradePowerUp()
    {
        // Implement your upgrade logic here
        int coins = playerController.GetCoinsCollected();
        if (coins >= 10) // Assuming an upgrade costs 10 coins
        {
            // Deduct the coins and apply the upgrade
            playerController.SetCoinsCollected(coins - 10);

            // Upgrade the power-up in the PlayerController script
            playerController.jetpackForce += 1.0f; // Example upgrade
        }
    }
}