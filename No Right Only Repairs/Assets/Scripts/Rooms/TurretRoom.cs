using UnityEngine;

public class TurretRoom : BaseRoom
{
    public Turret Turret { get; set; }

    protected override void Awake() {
        base.Awake();

        RoomType = RoomTypes.Turret;
    }

    public override void Activate() {
        base.Activate();
        Turret.IsActive = true;
        // Additional generator activation logic
        Debug.Log($"{gameObject.name} is now scanning for enemies.");
    }

    // Override Deactivate to handle generator-specific logic
    public override void Deactivate() {
        base.Deactivate();
        Turret.IsActive = false;
        // Additional generator deactivation logic
        Debug.Log($"{gameObject.name} lost the ability to shoot.");
    }
}
