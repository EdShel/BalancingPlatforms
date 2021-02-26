using UnityEngine;
using Random = System.Random;

public sealed class PlatformIndividual : Individual<int>
{
    private const int POSITION_RESOLUTION = 5;

    private const int GENES_COUNT = POSITION_RESOLUTION;

    public PlatformIndividual() : base(GENES_COUNT)
    {
    }

    protected override Individual<int> CreateIndividual()
    {
        return new PlatformIndividual();
    }

    protected override int GenerateRandomGeneValue(Random rng)
    {
        return rng.Next(-90, 90);
    }

    public float GetTiltDirection(float distNormalized)
    {
        int positionIndex = Mathf.RoundToInt(distNormalized * POSITION_RESOLUTION) % POSITION_RESOLUTION;
        return this.GetGene(positionIndex);
    }
}
