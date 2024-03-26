using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using HarmonyLib;
using RimWorld;
using Verse;

namespace IncidentCountMultiplier;

[StaticConstructorOnStartup]
public static class Patches
{
    //ログ用
    private static readonly StringBuilder stringBuilder;

    private static readonly IncidentCountMultiplierSettings settings = LoadedModManager
        .GetMod(typeof(IncidentCountMultiplier)).GetSettings<IncidentCountMultiplierSettings>();

    static Patches()
    {
        var harmony = IncidentCountMultiplier_Harmony.harmony;

        stringBuilder = new StringBuilder();
        stringBuilder.Append("Harmony Patches by IncidentCountMultiplier\n\n");

        IEnumerable<Type> storytellerComps = typeof(StorytellerComp).AllSubclassesNonAbstract();

        foreach (var comp in storytellerComps)
        {
            //MakeIntervalIncidentsメソッドのyield return用に作られた内部クラス
            var innerclass = comp.GetNestedTypes(AccessTools.all)
                .FirstOrDefault(x => x.Name.Contains("MakeIntervalIncidents"));
            if (innerclass != null)
            {
                stringBuilder.Append(innerclass);
                var original = AccessTools.Method(innerclass, "MoveNext");
                var transpiler = new HarmonyMethod(typeof(Patches), nameof(Transpiler));
                harmony.Patch(original, null, null, transpiler);
            }
            else
            {
                stringBuilder.Append(string.Concat([
                    comp,
                    " is not patched. Target method is not found."
                ]));
            }

            stringBuilder.AppendLine();
        }

        if (true)
        {
            Log.Message(stringBuilder.ToString());
        }
    }

    private static float Multiplier => settings.MTBEventOccurs_Multiplier.EvaluateOnCurrentDay();

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        if (instructions.Any(x =>
                x.opcode == OpCodes.Call && x.operand is MethodInfo { Name: "MTBEventOccurs" }))
        {
            //大部分の、呼ばれるたびに確率でインシデントを起こすタイプ
            stringBuilder.Append(" is patched.\nMethod 'MTBEventOccurs' is replaced.");
            return instructions.MethodReplacer_MTBEventOccurs();
        }

        if (instructions.Any(x =>
                x.opcode == OpCodes.Call && x.operand is MethodInfo { Name: "IncidentCountThisInterval" }))
        {
            //OnOffCycleなど、一定期間のうちに規定数のインシデントを起こすタイプ
            stringBuilder.Append(" is patched.\nMethod 'IncidentCountThisInterval' is replaced.");
            return instructions.MethodReplacer_IncidentCountThisInterval();
        }

        //JournyOfferなど、ランダム性のないやつ
        stringBuilder.Append(" is not patched.\n");
        return instructions;
    }

    private static bool MTBEventOccurs_Patch(float mtb, float mtbUnit, float checkDuration)
    {
        // mtb/= と実質一緒

        //checkDuration *= settings.MTBEventOccurs_Multiplier.EvaluateOnCurrentDay();
        if (Multiplier == 0)
        {
            return false;
        }

        checkDuration *= Multiplier;
        return Rand.MTBEventOccurs(mtb, mtbUnit, checkDuration);
    }

    private static int IncidentCountThisInterval_Patch(IIncidentTarget target, int randSeedSalt,
        float minDaysPassed, float onDays, float offDays, float minSpacingDays, float minIncidents,
        float maxIncidents, float acceptFraction = 1f)
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

        return IncidentCycleUtility.IncidentCountThisInterval(target, randSeedSalt, minDaysPassed, onDays, offDays,
            minSpacingDays, minIncidents, maxIncidents, acceptFraction);
    }

    private static IEnumerable<CodeInstruction> MethodReplacer_MTBEventOccurs(
        this IEnumerable<CodeInstruction> instructions)
    {
        return instructions.MethodReplacer(
            AccessTools.Method(typeof(Rand), nameof(Rand.MTBEventOccurs)),
            AccessTools.Method(typeof(Patches), nameof(MTBEventOccurs_Patch))
        );
    }

    private static IEnumerable<CodeInstruction> MethodReplacer_IncidentCountThisInterval(
        this IEnumerable<CodeInstruction> instructions)
    {
        return instructions.MethodReplacer(
            AccessTools.Method(typeof(IncidentCycleUtility),
                nameof(IncidentCycleUtility.IncidentCountThisInterval)),
            AccessTools.Method(typeof(Patches), nameof(IncidentCountThisInterval_Patch))
        );
    }

    private static float EvaluateOnCurrentDay(this SimpleCurve simpleCurve)
    {
        return simpleCurve.Evaluate(Find.TickManager.TicksGame / (float)60000);
    }
}