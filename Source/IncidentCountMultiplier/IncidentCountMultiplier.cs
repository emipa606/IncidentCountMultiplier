using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
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

    [StaticConstructorOnStartup]
    public static class Patches
    {
        //ログ用
        private static StringBuilder stringBuilder;

        private static IncidentCountMultiplierSettings settings = LoadedModManager.GetMod(typeof(IncidentCountMultiplier)).GetSettings<IncidentCountMultiplierSettings>();

        static Patches()
        {
            Harmony harmony = IncidentCountMultiplier_Harmony.harmony;

            stringBuilder = new StringBuilder();
            stringBuilder.Append("Harmony Patches by IncidentCountMultiplier\n\n");

            IEnumerable<Type> storytellerComps = GenTypes.AllSubclassesNonAbstract(typeof(StorytellerComp));

            foreach (Type comp in storytellerComps)
            {
                //MakeIntervalIncidentsメソッドのyield return用に作られた内部クラス
                Type innerclass = comp.GetNestedTypes(AccessTools.all).FirstOrDefault(x => x.Name.Contains("MakeIntervalIncidents"));
                if (innerclass != null)
                {
                    stringBuilder.Append(innerclass.ToString());
                    MethodInfo original = AccessTools.Method(innerclass, "MoveNext");
                    HarmonyMethod transpiler = new HarmonyMethod(typeof(Patches), nameof(Patches.Transpiler));
                    harmony.Patch(original, null, null, transpiler);
                }
                else
                {
                    stringBuilder.Append(string.Concat(new object[]
                    {
                        comp,
                        " is not patched. Target method is not found."
                    }));
                }
                stringBuilder.AppendLine();
            }
            if (true)
            {
                Log.Message(stringBuilder.ToString());
            }
        }

        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            if (instructions.Any(x => x.opcode == OpCodes.Call && x.operand is MethodInfo method && method.Name == "MTBEventOccurs"))
            {
                //大部分の、呼ばれるたびに確率でインシデントを起こすタイプ
                stringBuilder.Append(" is patched.\nMethod 'MTBEventOccurs' is replaced.");
                return instructions.MethodReplacer_MTBEventOccurs();
            }
            else if (instructions.Any(x => x.opcode == OpCodes.Call && x.operand is MethodInfo method && method.Name == "IncidentCountThisInterval"))
            {
                //OnOffCycleなど、一定期間のうちに規定数のインシデントを起こすタイプ
                stringBuilder.Append(" is patched.\nMethod 'IncidentCountThisInterval' is replaced.");
                return instructions.MethodReplacer_IncidentCountThisInterval();
            }
            else
            {
                //JournyOfferなど、ランダム性のないやつ
                stringBuilder.Append(" is not patched.\n");
                return instructions;
            }
        }

        private static float Multiplier { get => settings.MTBEventOccurs_Multiplier.EvaluateOnCurrentDay(); }

        private static bool MTBEventOccurs_Patch(float mtb, float mtbUnit, float checkDuration)
        {
            // mtb/= と実質一緒

            //checkDuration *= settings.MTBEventOccurs_Multiplier.EvaluateOnCurrentDay();
            if(Multiplier == 0)
            {
                return false;
            }
            checkDuration *= Multiplier;
            return Rand.MTBEventOccurs(mtb, mtbUnit, checkDuration);
        }

        private static int IncidentCountThisInterval_Patch(IIncidentTarget target, int randSeedSalt, float minDaysPassed, float onDays, float offDays, float minSpacingDays, float minIncidents, float maxIncidents, float acceptFraction = 1f)
        {
            //minIncidents *= settings.MinIncidentCountMultiplier.EvaluateOnCurrentDay();
            //maxIncidents *= settings.MaxIncidentCountMultiplier.EvaluateOnCurrentDay();
            //float IncidentCycleAcceleration = settings.IncidentCycleAcceleration.EvaluateOnCurrentDay();
            //onDays *= IncidentCycleAcceleration;
            //offDays *= IncidentCycleAcceleration;
            //minSpacingDays /= settings.MaxIncidentCountMultiplier.EvaluateOnCurrentDay();
            minIncidents *= Multiplier;
            maxIncidents *= Multiplier;
            if (Multiplier <= 0.5)
            {
                minSpacingDays /= 0.5f;
            }
            else
            {
                minSpacingDays /= Multiplier;
            }
            return IncidentCycleUtility.IncidentCountThisInterval(target, randSeedSalt, minDaysPassed, onDays, offDays, minSpacingDays, minIncidents, maxIncidents, acceptFraction);
        }

        private static IEnumerable<CodeInstruction> MethodReplacer_MTBEventOccurs(this IEnumerable<CodeInstruction> instructions)
        {
            return instructions.MethodReplacer(
                AccessTools.Method(typeof(Rand), nameof(Rand.MTBEventOccurs)),
                AccessTools.Method(typeof(Patches), nameof(Patches.MTBEventOccurs_Patch))
                );
        }

        private static IEnumerable<CodeInstruction> MethodReplacer_IncidentCountThisInterval(this IEnumerable<CodeInstruction> instructions)
        {
            return instructions.MethodReplacer(
                AccessTools.Method(typeof(IncidentCycleUtility), nameof(IncidentCycleUtility.IncidentCountThisInterval)),
                AccessTools.Method(typeof(Patches), nameof(Patches.IncidentCountThisInterval_Patch))
                );
        }

        private static float EvaluateOnCurrentDay(this SimpleCurve simpleCurve)
        {
            return simpleCurve.Evaluate(Find.TickManager.TicksGame / 60000);
        }
    }


}
