using UnityEngine;

public class CrewQuarters : BaseRoom {
    protected override void Awake() {
        base.Awake();

        RoomType = RoomTypes.Crew;
    }

    public override void Activate() {
        base.Activate();
        // Additional generator activation logic
        GameManager.Instance.CrewQuartersActivated();
        Debug.Log($"{gameObject.name} can now accomodate a larger population.");
    }

    // Override Deactivate to handle generator-specific logic
    public override void Deactivate() {
        base.Deactivate();
        GameManager.Instance.CrewQuartersDeactivated();
        // Additional generator deactivation logic
        Debug.Log($"{gameObject.name} cannot sustain a population in its current condition.");
    }
}
