using UnityEngine;

public class GoodWillManager : MonoBehaviourSingleton<GoodWillManager>
{
    public float timer;
    public float Goodwill;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 1.0f) {
            Goodwill += Settings.Instance.GoodWillMultiplier;
            timer -= 1;
        }

        UIManager.Instance.goodwillValue.text = Goodwill.ToString();
    }
}
