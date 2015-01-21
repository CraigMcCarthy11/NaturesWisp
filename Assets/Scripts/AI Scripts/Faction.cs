using System.Collections.Generic;

public class Faction {

	//Public
    public enum FactionTypes
    {
        Woodland,
        Desert,
        Tropical,
        Arctic
    };

    public FactionTypes myFaction;
    public List<FactionTypes> Enemies = new List<FactionTypes>();
    public List<FactionTypes> Allies = new List<FactionTypes>();

    //Private
    
    
    //Constructor
    public Faction(FactionTypes faction, List<Faction.FactionTypes> setEnemies, List<Faction.FactionTypes> setAllies)
    {
        Enemies = setEnemies;
        Allies = setAllies;
        myFaction = faction;
    }
}
