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
    public Faction(FactionTypes myFaction, List<Faction.FactionTypes> Enemies, List<Faction.FactionTypes> Allies)
    {
        this.Enemies = Enemies;
        this.Allies = Allies;
        this.myFaction = myFaction;
    }
}
