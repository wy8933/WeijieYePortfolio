using UnityEngine;

public abstract class BaseMovement : MonoBehaviour
{
    public float MovementSpeed;
    public float RotationSpeed;

    protected Rigidbody2D _rb;

    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    public abstract void Movement();

    public abstract void Rotation();
}
