using UnityEngine;

public class GeneratorRoom : BaseRoom {

    public int CurrentPowerOutput { get; private set; }

    protected override void Awake() {
        base.Awake();

        RoomType = RoomTypes.Generator;
    }

    // Override Activate to include generator-specific logic
    public override void Activate() {
        base.Activate();
        // Additional generator activation logic
        GameManager.Instance.ActivatedGeneratorRoom(this);
        Debug.Log($"{gameObject.name} is now generating power.");
    }

    // Override Deactivate to handle generator-specific logic
    public override void Deactivate() {
        base.Deactivate();
        // Additional generator deactivation logic
        GameManager.Instance.DeactivatedGeneratorRoom(this);
        Debug.Log($"{gameObject.name} stopped generating power.");
    }
}
