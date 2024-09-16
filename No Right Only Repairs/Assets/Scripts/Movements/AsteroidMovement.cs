using UnityEngine;
public class AsteroidMovement : BaseMovement
{
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Movement();
        Rotation();
    }

    public override void Movement() {

        GameObject ship = GameObject.FindGameObjectWithTag("Ship");
        _rb.velocity = ship.transform.position - transform.position;
    }

    public override void Rotation() {
        gameObject.transform.Rotate(0,0,RotationSpeed);
    }
}
