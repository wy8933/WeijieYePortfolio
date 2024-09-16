public class ResourceManager : MonoBehaviourSingleton<ResourceManager>
{
    public float rareMineral = 250;
    public float refugees = 3;

    public void UpdateRareMineral(float amount) {
        rareMineral += amount;
    }

    public void UpdateRefugee(float amount)
    {
        refugees += amount;
        HUD.Instance.UpdateRefugee();
    }

}
