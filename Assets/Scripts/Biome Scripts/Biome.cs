
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

    //My Data
    public BiomeTypes myBiome;
    public Faction.FactionTypes myFaction;

    //Private
    private int numberOfStartingWisps;
    
    //Constructor
    public Biome(BiomeTypes setBiome, Faction.FactionTypes setFaction, int setStartingWisps)
    {
        //Set all of our data here
        myBiome = setBiome;
        myFaction = setFaction;
        numberOfStartingWisps = setStartingWisps;

        BuildWithData();
    }

    //Functions

    /// <summary>
    /// This method will actually create the game object in the screen. 
    /// It only gets called in the constructor after we have generated all the data to build
    /// the biome
    /// </summary>
    private void BuildWithData()
    {

    }
}
