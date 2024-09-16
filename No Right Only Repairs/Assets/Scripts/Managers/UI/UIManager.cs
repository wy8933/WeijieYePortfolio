using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviourSingleton<UIManager>
{
    public int electionEventNum;
    public float electionHappenValue;
    private float _nextElectionHappenValue;

    public float gameProgressSpeed = 1/1200f;
    public GameObject electionPrefab;
    public GameObject electionGroup;
    public GameObject pausePanel;
    public GameObject settingPanel;
    public Slider gameProgress;
    public Slider SFXSlider;
    public Slider BGMSlider;
    public TMP_Text goodwillValue;
    [Header("Outpost")]
    public GameObject outpostPanel;
    public GameObject shop;
    public GameObject embassy;
    public TMP_Text embassyText;

    private void Start()
    {
        for(int i = 0; i < electionEventNum; i++)
            Instantiate(electionPrefab, electionGroup.transform);
        electionHappenValue = 1.0f / (electionEventNum+1);
        _nextElectionHappenValue = electionHappenValue;
        RandomEventsManager.Instance.InitRandomEvent();
        SFXSlider.value = Settings.Instance.SFXVolumn;
        BGMSlider.value = Settings.Instance.BGMVolumn;
    }

    private void Update()
    {
        gameProgress.value += gameProgressSpeed * Time.deltaTime;

        if (gameProgress.value >= _nextElectionHappenValue)
        {
            ElectionManager.Instance.TriggerElection();
            _nextElectionHappenValue += electionHappenValue;
        }
    }

    public void ShowShopOutpost() {
        outpostPanel.SetActive(true);
        shop.SetActive(true);
    }
    public void ShowEmbassyOutpost()
    {
        outpostPanel.SetActive(true);
        embassy.SetActive(true);
    }

    public void ChanegEmbassyTradeText(string tradeText) {

        embassyText.text = tradeText;
    }

    public void CloseOutpost() {
        GameManager.Instance.StartGameTimeScale();
        if (GameObject.FindWithTag("Enemy") != null)
        {
            SoundManager.Instance.PlayBattleBGM();
        }
        else {
            SoundManager.Instance.PlayNormalBGM();
        }
        outpostPanel.SetActive(false);
        embassy.SetActive(false);
        shop.SetActive(false);
    }
}
