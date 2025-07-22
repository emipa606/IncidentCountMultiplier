using System.Collections.Generic;
using System.Linq;
using Verse;

namespace IncidentCountMultiplier;

public class IncidentCountMultiplierSettings : ModSettings
{
    public SimpleCurve MTBEventOccurs_Multiplier = new()
    {
        { 0f, 2f }
    };

    public override void ExposeData()
    {
        base.ExposeData();

        var points1 = MTBEventOccurs_Multiplier.ToList();

        Scribe_Collections.Look(ref points1, "MTBEventOccurs_Multiplier");

        if (points1 != null)
        {
            MTBEventOccurs_Multiplier = listToSimpleCurve(points1);
        }
    }

    private static SimpleCurve listToSimpleCurve(List<CurvePoint> list)
    {
        var curves = new SimpleCurve();
        foreach (var curvePoint in list)
        {
            curves.Add(curvePoint);
        }

        return curves;
    }
}