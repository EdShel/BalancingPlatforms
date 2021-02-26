using UnityEngine;

public class SolutionApplier : MonoBehaviour
{
    public PopulationController Population { get; set; }

    public float Fitness;

    public PlatformIndividual Individual;

    public PlatformController controller;

    private bool isMeasuring;

    private float beginTime;

    void Start()
    {
        this.controller = this.GetComponent<PlatformController>();
    }

    void Update()
    {
        if (isMeasuring)
        {
            Transform ball = controller.Ball.transform;
            Transform platform = controller.transform;
            Vector3 dimensionalDiff = ball.position - platform.position;
            float dif = dimensionalDiff.z;

            float platformSize = platform.lossyScale.x;
            float ballPositionNormalized = Mathf.Clamp01((dif + platformSize * 0.5f) / platformSize);

            float dir = Individual.GetTiltDirection(ballPositionNormalized);
            controller.TiltTo(dir);
        }
    }

    public void StartFitnessMeasurement()
    {
        this.beginTime = Time.time;
        this.isMeasuring = true;
        this.controller?.ResetState();
    }

    public void OnFitnessMeasurementFail()
    {
        this.isMeasuring = false;
        this.Fitness = Time.time - this.beginTime;
        this.Population.OnIndividualFail();
    }
}

[UnityEditor.CustomEditor(typeof(SolutionApplier))]
public class SolutionApplierEditor : UnityEditor.Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Print genes"))
        {
            Debug.Log((target as SolutionApplier).Individual);
        }
    }
}