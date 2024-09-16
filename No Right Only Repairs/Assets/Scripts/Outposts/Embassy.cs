using UnityEngine;
using UnityEngine.UI;

public class Embassy : MonoBehaviourSingleton<Embassy>
{
    float rareMineralsReceived;
    public Button yesButton;
    public Button noButton;
    public void ShowTradeRefugees()
    {
        ResetEmbassy();
        rareMineralsReceived = CalculateTradeAmount(ResourceManager.Instance.refugees); 
        UIManager.Instance.ChanegEmbassyTradeText("Traded " + ResourceManager.Instance.refugees + " refugees for " + rareMineralsReceived + " rare minerals.");
    }

    private float CalculateTradeAmount(float refugees)
    {
        int rareMineralsPerRefugee = Random.Range(15, 30);
        return refugees * rareMineralsPerRefugee;
    }

    public void YesTrade() {
        ResourceManager.Instance.UpdateRefugee(-ResourceManager.Instance.refugees);
        ResourceManager.Instance.UpdateRareMineral(rareMineralsReceived);
        yesButton.interactable = false;
        noButton.interactable = false;
    }

    public void NoTrade() {
        yesButton.interactable = false;
        noButton.interactable = false;
    }

    public void ResetEmbassy() {
        yesButton.interactable = true;
        noButton.interactable = true;
    }
}
