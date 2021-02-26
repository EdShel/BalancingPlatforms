using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public float RotationSpeed;

    public GameObject Ball;

    private Vector3 ballInitialPosition;

    void Start()
    {
        this.ballInitialPosition = Ball.transform.position;
    }

    public void TiltTo(float dirX)
    {
        this.transform.rotation = Quaternion.RotateTowards(
            this.transform.rotation,
            Quaternion.Euler(dirX, 0f, 0f),
            Time.deltaTime * RotationSpeed);

        if (Ball != null)
        {
            Ball.GetComponent<Rigidbody>().WakeUp();
        }
    }

    public void ResetState()
    {
        this.transform.rotation = Quaternion.identity;

        this.Ball.transform.position = this.ballInitialPosition;
        var ballRb = this.Ball.GetComponent<Rigidbody>();
        ballRb.velocity = Vector3.zero;
        ballRb.angularVelocity = Vector3.zero;
    }
}
