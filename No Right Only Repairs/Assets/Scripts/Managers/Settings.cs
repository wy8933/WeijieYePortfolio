public class Settings: MonoBehaviourSingleton<Settings>
{
    public bool BelovedLeader = false;
    public float GracePeriod = 0f;
    
    public bool Energizer = false;
    public float EnergizerRange = 1f;

    public bool TradingHub = false;
    public float TradingMultiplier = 1f;

    public bool NanobotComposition = false;
    public float GoodWillMultiplier = 1f;

    public bool DroneBuddy = false;

    public bool Harder = false;
    public float DurabilityMultiplier = 1f;

    public bool Fire = false;
    public float FireCooldownMultiplier = 1f;

    public bool EfficientThrusters = false;
    public float PlayerSpeedMultiplier = 1f;

    public bool Asylum = false;
    public float RefugeeDropModifier = 1f;

    public bool Engineer = false;
    public float RepairCostModifier = 5f;

    public bool Scavenger = false;
    public float RoomDropMultiplier = 1f;

    public bool FoundFamily = false;

    public float SFXVolumn = 1;
    public float BGMVolumn = 1;

    public float GoodwillOverall;
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
