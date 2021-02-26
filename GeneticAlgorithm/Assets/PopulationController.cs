using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopulationController : MonoBehaviour
{
    public int IndividualsCount;

    public float DistanceBetweenPlatforms;

    public float MutationChance;

    public GameObject PopulationUnit;

    private SolutionApplier[] platforms;

    public int Failed = 0;

    private System.Random rng = new System.Random(0);

    private int populationsCount = 0;

    private string labelText;

    void Start()
    {
        CreatePlatformsForIndividuals();
        PlatformIndividual[] individuals = GeneratePopulation();
        StartPopulation(individuals);

        MakeLabel(0, 0f);
    }

    private PlatformIndividual[] GeneratePopulation()
    {
        int platformsCount = this.platforms.Length;
        PlatformIndividual[] individuals = Enumerable.Range(0, platformsCount)
            .Select(i => new PlatformIndividual())
            .ToArray();
        foreach (var individual in individuals)
        {
            individual.RandomizeGenes(rng);
        }

        return individuals;
    }

    private void CreatePlatformsForIndividuals()
    {
        this.platforms = new SolutionApplier[IndividualsCount];
        int platformsInRow = Mathf.CeilToInt(Mathf.Sqrt(IndividualsCount));
        for (int i = 0; i < platforms.Length; i++)
        {
            var populationUnit = Instantiate(PopulationUnit);
            populationUnit.transform.position = new Vector3(
                i / platformsInRow * DistanceBetweenPlatforms, 0,
                i % platformsInRow * DistanceBetweenPlatforms);
            this.platforms[i] = populationUnit.GetComponentInChildren<SolutionApplier>();
        }

        foreach (var platform in this.platforms)
        {
            platform.Population = this;
        }
    }

    public void StartPopulation(PlatformIndividual[] individuals)
    {
        for (int i = 0; i < platforms.Length; i++)
        {
            SolutionApplier platform = platforms[i];

            platform.Individual = individuals[i];
            platform.StartFitnessMeasurement();
        }

        populationsCount++;
    }

    public void OnIndividualFail()
    {
        Failed++;
        if (Failed == platforms.Length)
        {
            Failed = 0;

            float fitnessSum = 0f;
            var previousPopulation = platforms
                .Select(p => new { Individual = p.Individual, Chance = (fitnessSum += p.Fitness) })
                .ToArray();

            MakeLabel(populationsCount, fitnessSum / previousPopulation.Length);

            var newPopulation = new PlatformIndividual[previousPopulation.Length];

            for (int i = 0; i < newPopulation.Length; i++)
            {
                float chanceForFirstParent = (float)rng.NextDouble() * fitnessSum;
                var firstParent = previousPopulation.SkipWhile(p => p.Chance < chanceForFirstParent).First();

                float chanceFor2ndParent = (float)rng.NextDouble() * fitnessSum;
                var secondParent = previousPopulation.SkipWhile(p => p.Chance < chanceFor2ndParent).First();

                newPopulation[i] = (PlatformIndividual)firstParent.Individual.Breed(secondParent.Individual, rng);
                newPopulation[i].Mutate(MutationChance, rng);
            }

            StartPopulation(newPopulation);
        }
    }

    private void MakeLabel(int generation, float fitness)
    {
        this.labelText = $"Generation #{generation}, AVG fitness {fitness:#.##}";
        Debug.Log(this.labelText);

    }

    private void OnGUI()
    {
        var style = GUIStyle.none;
        style.fontSize = 48;
        GUI.color = Color.black;
        GUI.Label(new Rect(0, 0, 400f, 100f), this.labelText, style);
    }
}
