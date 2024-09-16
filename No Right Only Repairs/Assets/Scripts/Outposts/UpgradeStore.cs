using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeStore : MonoBehaviourSingleton<UpgradeStore>
{
    public List<SO_PowerUp> powerUpObjects; 
    public Button[] upgradeButtons;

    private List<SO_PowerUp> selectedUpgrades = new List<SO_PowerUp>();
    private HashSet<PowerUpType> obtainedUpgrades = new HashSet<PowerUpType>();
    private bool upgradeSelected = false; 

    public void AssignRandomPerks()
    {
        ResetStore();

        // Filter out already obtained upgrades
        List<SO_PowerUp> availableForSelection = new List<SO_PowerUp>();
        foreach (SO_PowerUp powerUp in powerUpObjects)
        {
            if (!obtainedUpgrades.Contains(powerUp.powerUpType))
            {
                availableForSelection.Add(powerUp);
            }
        }

        int selectionCount = Mathf.Min(3, availableForSelection.Count);
        ShuffleList(availableForSelection);
        selectedUpgrades = availableForSelection.GetRange(0, selectionCount);

        // Assign data to UI buttons
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            if (i < selectedUpgrades.Count)
            {
                upgradeButtons[i].gameObject.SetActive(true);
                SetButtonData(upgradeButtons[i], selectedUpgrades[i]);
            }
            else
            {
                upgradeButtons[i].gameObject.SetActive(false); // Hide buttons if fewer than 3 upgrades
            }
        }
    }

    private void ShuffleList(List<SO_PowerUp> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            SO_PowerUp temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private void SetButtonData(Button button, SO_PowerUp powerUp)
    {
        button.GetComponent<Image>().sprite = powerUp.itemIcon;
        button.GetComponent<TooltipInfo>().description = powerUp.description;

        // You can also add a listener to the button to handle the purchase
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => PurchaseUpgrade(button, powerUp));
    }

    public void PurchaseUpgrade(Button selectedButton, SO_PowerUp powerUp)
    {
        if (upgradeSelected) return; // Prevent purchasing more than one upgrade

        if (selectedUpgrades.Contains(powerUp))
        {
            ApplyUpgrade(powerUp);
            Debug.Log(powerUp.itemName + " purchased!");
            upgradeSelected = true; // Mark that an upgrade has been selected

            DisableOtherButtons(selectedButton);
        }
        else
        {
            Debug.Log("Upgrade not available.");
        }
    }

    private void ApplyUpgrade(SO_PowerUp powerUp)
    {
        // Mark the upgrade as obtained
        obtainedUpgrades.Add(powerUp.powerUpType);

        // Apply the upgrade effect
        PowerUpEffect.Instance.ApplyPowerUpEffect(powerUp.powerUpType);
    }

    private void DisableOtherButtons(Button selectedButton)
    {
        foreach (Button button in upgradeButtons)
        {
            if (button != selectedButton)
            {
                button.interactable = false;
            }
        }
    }

    public void ResetStore()
    {
        foreach (Button button in upgradeButtons)
        {
            button.interactable = true;
        }
        upgradeSelected = false;
    }
}
