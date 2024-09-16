using TMPro;
using UnityEngine;
public class GameoverScore : MonoBehaviour
{
    
    void Start()
    {
        gameObject.GetComponent<TMP_Text>().text = "Your Score Is: " + Settings.Instance.GoodwillOverall;
    }

}
