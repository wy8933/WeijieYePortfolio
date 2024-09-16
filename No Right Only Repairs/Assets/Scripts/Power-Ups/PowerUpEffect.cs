using UnityEngine;

// Enum of types of power-ups
public enum PowerUpType
{
    BelovedLeader,
    Energizer,
    TradingHub,
    NanobotComposition,
    DroneBuddy,
    Harder,
    Fire,
    EfficientThrusters,
    Asylum,
    Engineer,
    Scavenger,
    FoundFamily
}

public class PowerUpEffect : MonoBehaviourSingleton<PowerUpEffect>
{
    public void ApplyPowerUpEffect(PowerUpType powerUp)
    {
        switch (powerUp)
        {
            case PowerUpType.BelovedLeader:
                ApplyBelovedLeader();
                break;
            case PowerUpType.Energizer:
                ApplyEnergizer();
                break;
            case PowerUpType.TradingHub:
                ApplyTradingHub();
                break;
            case PowerUpType.NanobotComposition:
                ApplyNanobotComposition();
                break;
            case PowerUpType.DroneBuddy:
                ApplyDroneBuddy();
                break;
            case PowerUpType.Harder:
                ApplyHarder();
                break;
            case PowerUpType.Fire:
                ApplyFire();
                break;
            case PowerUpType.EfficientThrusters:
                ApplyEfficientThrusters();
                break;
            case PowerUpType.Asylum:
                ApplyAsylum();
                break;
            case PowerUpType.Engineer:
                ApplyEngineer();
                break;
            case PowerUpType.Scavenger:
                ApplyScavenger();
                break;
            case PowerUpType.FoundFamily:
                ApplyFoundFamily();
                break;
            default:
                Debug.Log("Unknown power-up: " + powerUp);
                break;
        }
    }

    public void ApplyBelovedLeader() {
        Settings.Instance.BelovedLeader = true;
        Settings.Instance.GracePeriod += 5.0f;
    }
    public void ApplyEnergizer() {
        Settings.Instance.Energizer = true;
        Settings.Instance.EnergizerRange += 1;
    }
    public void ApplyTradingHub() {
        Settings.Instance.TradingHub = true;
        Settings.Instance.TradingMultiplier *= 0.8f;
    }
    public void ApplyNanobotComposition() {
        Settings.Instance.NanobotComposition = true;
        Settings.Instance.GoodWillMultiplier *= 0.8f;
    }
    public void ApplyDroneBuddy() {
        Settings.Instance.DroneBuddy = true;
    }
    public void ApplyHarder() {
        Settings.Instance.Harder = true;
        Settings.Instance.DurabilityMultiplier *= 2f;
        GameObject.FindAnyObjectByType<PlayerHealth>().GetComponent<PlayerHealth>().PlayerMaxHealthUpdate(100);
    }
    public void ApplyFire() {
        Settings.Instance.Fire = true;
        Settings.Instance.FireCooldownMultiplier *= 0.8f;
    }
    public void ApplyEfficientThrusters() {
        Settings.Instance.EfficientThrusters = true;
        Settings.Instance.PlayerSpeedMultiplier *= 1.2f;
    }
    public void ApplyAsylum() {
        Settings.Instance.Asylum = true;
        Settings.Instance.RefugeeDropModifier += 1;
    }
    public void ApplyEngineer() {
        Settings.Instance.Engineer = true;
        Settings.Instance.RepairCostModifier -= 1;
    }
    public void ApplyScavenger() {
        Settings.Instance.Scavenger = true;
        Settings.Instance.RoomDropMultiplier *= 1.2f; 
    }
    public void ApplyFoundFamily() {
        Settings.Instance.FoundFamily = true;
    }
}

