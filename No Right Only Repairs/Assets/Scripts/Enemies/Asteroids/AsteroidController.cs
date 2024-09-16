public class AsteroidController : BaseEnemyController
{

    private void OnDestroy()
    {
        SoundManager.Instance.PlaySound(SoundName.AsteroidBreaks);
    }
}
