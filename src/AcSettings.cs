namespace Ancinia;

public class AcSaveSettings
{
    // Mechanics
    //public bool SFGrenadeAnciniaHornetCompanion = false;

    // Bosses
    //public bool SFGrenadeAnciniaEncounteredWeaverPrincess = false;
    //public bool SFGrenadeAnciniaDefeatedWeaverPrincess = false;

    // Areas
    public bool SFGrenadeAnciniaVisitedAncinia = false;
    //public bool SFGrenadeAnciniaTotOpened = false;
    //public bool SFGrenadeAnciniaVisitedTestOfTeamwork = false;
    //public bool SFGrenadeAnciniaTotOpenedShortcut = false;
    //public bool SFGrenadeAnciniaTotOpenedTotem = false;

#if DEBUG_CHARMS
        // Better charms
        public bool[] gotCustomCharms = new bool[] { true, true, true, true };
        public bool[] newCustomCharms = new bool[] { false, false, false, false };
        public bool[] equippedCustomCharms = new bool[] { false, false, false, false };
        public int[] customCharmCosts = new int[] { 1, 1, 1, 1 };
#endif
}

public class AcGlobalSettings
{
}