
/// <summary>
/// Base Biome Class
/// </summary>
public class Biome {

    //Public
    public enum BiomeTypes
    {
        Woodland,
        Mountians,
        Tropical,
        Arctic
    };

    public BiomeTypes myBiome;

    //Private
    
    
    //Constructor
    public Biome(BiomeTypes setBiome)
    {
        //Set all of our data here
        myBiome = setBiome;
    }

    //Functions
}
