using System.Collections.Generic;
using System.Linq;
using Verse;

namespace IncidentCountMultiplier
{
    public class IncidentCountMultiplierSettings : ModSettings
    {
        public SimpleCurve MTBEventOccurs_Multiplier = new SimpleCurve
        {
            {0f, 2f}
        };
        //private SimpleCurve MinIncidentCountMultiplier = new SimpleCurve
        //{
        //    {0f,   1f}
        //};
        //private SimpleCurve MaxIncidentCountMultiplier = new SimpleCurve
        //{
        //    {0f,   1f}
        //};
        //private SimpleCurve IncidentCycleAcceleration = new SimpleCurve
        //{
        //    {0f,   1f}
        //};

        public override void ExposeData()
        {
            base.ExposeData();

            var points1 = MTBEventOccurs_Multiplier.ToList();
            //List<CurvePoint> points2 = MinIncidentCountMultiplier.ToList();
            //List<CurvePoint> points3 = MaxIncidentCountMultiplier.ToList();
            //List<CurvePoint> points4 = IncidentCycleAcceleration.ToList();

            Scribe_Collections.Look(ref points1, "MTBEventOccurs_Multiplier");
            //Scribe_Collections.Look<CurvePoint>(ref points2, "MinIncidentCountMultiplier");
            //Scribe_Collections.Look<CurvePoint>(ref points3, "MaxIncidentCountMultiplier");
            //Scribe_Collections.Look<CurvePoint>(ref points4, "IncidentCycleAcceleration");

            if (points1 != null)
            {
                MTBEventOccurs_Multiplier = ListToSimpleCurve(points1);
            }

            //if (points2 != null)
            //{
            //    MinIncidentCountMultiplier = ListToSimpleCurve(points2);
            //}
            //if (points3 != null)
            //{
            //    MaxIncidentCountMultiplier = ListToSimpleCurve(points3);
            //}
            //if (points4 != null)
            //{
            //    IncidentCycleAcceleration = ListToSimpleCurve(points4);
            //}
        }

        private SimpleCurve ListToSimpleCurve(List<CurvePoint> list)
        {
            var curves = new SimpleCurve();
            foreach (var curvePoint in list)
            {
                curves.Add(curvePoint);
            }

            return curves;
        }
    }
}