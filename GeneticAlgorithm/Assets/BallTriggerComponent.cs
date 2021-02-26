using System.Collections;
using UnityEngine;

public class BallTriggerComponent : MonoBehaviour
{
    private SolutionApplier applier;

    void Start()
    {
        this.applier = this.transform.parent.GetComponent<SolutionApplier>();
    }

    private void OnTriggerExit(Collider other)
    {
        applier.OnFitnessMeasurementFail();
    }
}
