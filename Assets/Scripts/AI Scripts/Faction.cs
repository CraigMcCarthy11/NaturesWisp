﻿public class Faction {

	//Public
    public enum FactionTypes
    {
        WoodlandFaction,
        MountiansFaction,
        TropicalFaction,
        ArcticFaction
    };

    public FactionTypes myFaction;

    //Private
    
    
    //Constructor
    public Faction(FactionTypes setFaction)
    {
        //Set all of our data here
        myFaction = setFaction;

    }

    //Functions
    
}
