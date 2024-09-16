using System.Collections;
using TMPro;
using UnityEngine;

public class RandomEventPopup : MonoBehaviourSingleton<RandomEventPopup>
{
    [SerializeField]
    private GameObject _popUpWindow; 
    [SerializeField]
    private GameObject _popUpTextBackground;
    [SerializeField]
    private TMP_Text _popUpInfo;

    public void ShowEventPopUp(string text)
    {
        SoundManager.Instance.PlaySound(SoundName.Warning);
        StartCoroutine(FlashAndShowPopup(text));
    }

    private IEnumerator FlashAndShowPopup(string text)
    {
        _popUpWindow.SetActive(true);
        _popUpInfo.text = ""; 
        _popUpTextBackground.SetActive(false);

        // Flash the popup 3 times
        for (int i = 0; i < 3; i++)
        {
            _popUpWindow.SetActive(false);
            yield return new WaitForSeconds(0.5f);
            _popUpWindow.SetActive(true);
            yield return new WaitForSeconds(0.5f);
        }

        // Display the text after flashing
        _popUpWindow.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        _popUpInfo.text = text;
        _popUpTextBackground.SetActive(true);
        // Keep the popup visible for a few seconds
        yield return new WaitForSeconds(4f);

        // Hide the popup
        _popUpWindow.SetActive(false);
        _popUpTextBackground.SetActive(false);
    }

}