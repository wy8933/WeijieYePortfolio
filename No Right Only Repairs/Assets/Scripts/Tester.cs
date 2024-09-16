using TMPro;
using UnityEngine;
public class Tester : MonoBehaviourSingleton<Tester>
{
    public TMP_Text fuelText;

    public float timer;
    public float goodwillNum;
    public GameObject asteroid;
    private void Update()
    {
        goodwillNum += Time.deltaTime;
        timer += Time.deltaTime;


        HUD.Instance.UpdateGoodwill(100, goodwillNum);
        HUD.Instance.UpdateRareMineral();
        HUD.Instance.UpdateRefugee();
        //refresh shop
        if (Input.GetKeyDown("o")) {
            RandomEventsManager.Instance.ShopOutpostEvent();
        }
        if (Input.GetKeyDown("p")) {
            RandomEventsManager.Instance.EmbassyOutpostEvent();
        }
        if (Input.GetKeyDown("q")) {
            RandomEventPopup.Instance.ShowEventPopUp("This is a random event");
        }
        if (Input.GetKeyDown("m")) {
            Time.timeScale = 0;
        }
        if (Input.GetKeyDown("n"))
        {
            Time.timeScale = 1;
        }

    }

}
