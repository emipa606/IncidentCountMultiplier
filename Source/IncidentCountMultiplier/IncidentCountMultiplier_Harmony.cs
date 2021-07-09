using HarmonyLib;
using Verse;

namespace IncidentCountMultiplier
{
    [StaticConstructorOnStartup]
    public static class IncidentCountMultiplier_Harmony
    {
        //いつもの
        public static Harmony harmony;

        static IncidentCountMultiplier_Harmony()
        {
            harmony = new Harmony("IncidentCountMultiplier");
            //harmony.PatchAll();
        }
    }
}