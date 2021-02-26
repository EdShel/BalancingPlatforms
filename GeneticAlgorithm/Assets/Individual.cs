using Random = System.Random;

public abstract class Individual<T>
{
    private readonly T[] genes;

    protected Individual(int genesCount)
    {
        this.genes = new T[genesCount];
    }

    public T Fitness { get; set; }

    public int GenesCount => genes.Length;

    protected abstract Individual<T> CreateIndividual();

    protected abstract T GenerateRandomGeneValue(Random rng);

    public void RandomizeGenes(Random rng)
    {
        for (int i = 0; i < this.genes.Length; i++)
        {
            this.genes[i] = GenerateRandomGeneValue(rng);
        }
    }

    public Individual<T> Breed(Individual<T> other, Random rng)
    {
        var child = CreateIndividual();
        Individual<T>[] parents = new[] { this, other };
        for (int i = 0; i < child.GenesCount; i++)
        {
            int parentToInheritThisGene = rng.Next(2);
            child.genes[i] = parents[parentToInheritThisGene].genes[i];
        }

        return child;
    }

    public void Mutate(float chance, Random rng)
    {
        double chancesToMutate = rng.NextDouble();
        if (chancesToMutate < chance)
        {
            this.genes[rng.Next(this.GenesCount)] = GenerateRandomGeneValue(rng);
        }
    }

    protected T GetGene(int index)
    {
        return this.genes[index];
    }

    public override string ToString()
    {
        return string.Join(":", genes);
    }
}
