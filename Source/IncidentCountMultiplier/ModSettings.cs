using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace IncidentCountMultiplier
{
    public class IncidentCountMultiplier : Mod
    {
        private Vector2 scrollPosition = new Vector2(0f, 0f);
        private static float ViewRectHeight { get; set; }

        static readonly SimpleCurveDrawerStyle style = new SimpleCurveDrawerStyle
        {
            LabelX = "day",
            DrawCurveMousePoint = true,
            DrawMeasures = true,
            UseFixedScale = true,
            UseFixedSection = true,
            YIntegersOnly = true,
            XIntegersOnly = true,
            MeasureLabelsXCount = 6
        };

        public IncidentCountMultiplierSettings settings;

        public IncidentCountMultiplier(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<IncidentCountMultiplierSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Rect viewRect = new Rect(0, 0, inRect.width - 30, ViewRectHeight);
            Widgets.BeginScrollView(inRect, ref scrollPosition, viewRect, true);

            Rect viewRect2 = new Rect(0, 0, viewRect.width, 99999);

            Listing_Standard listing_Standard = new Listing_Standard
            {
                ColumnWidth = (float)(viewRect.width - 10.0),
            };
            listing_Standard.Begin(viewRect2);

            listing_Standard.Label("IncidentCountMultiplierDescription".Translate());

            listing_Standard.GapLine(24);

            //listing_Standard.Label("IncidentCountMultiplier".Translate(), -1, "IncidentCountMultiplier_ToolTip".Translate());
            listing_Standard.Label("IncidentCountMultiplier".Translate());
            SimpleCurveEditor(listing_Standard, ref settings.MTBEventOccurs_Multiplier);
            listing_Standard.GapLine();

            //listing_Standard.Label("MinIncidentCountMultiplier".Translate());
            //SimpleCurveEditor(listing_Standard, ref settings.MinIncidentCountMultiplier);
            //listing_Standard.GapLine();

            //listing_Standard.Label("MaxIncidentCountMultiplier".Translate());
            //SimpleCurveEditor(listing_Standard, ref settings.MaxIncidentCountMultiplier);
            //listing_Standard.GapLine();

            //listing_Standard.Label("IncidentCycleAcceleration".Translate());
            //SimpleCurveEditor(listing_Standard, ref settings.IncidentCycleAcceleration);
            //listing_Standard.GapLine();


            style.FixedScale = new Vector2(0, settings.MTBEventOccurs_Multiplier.View.rect.yMax);
            style.FixedSection = new FloatRange(0, settings.MTBEventOccurs_Multiplier.View.rect.xMax);
            listing_Standard.GapLine(24);


            Rect rect = listing_Standard.GetRect(250);
            SimpleCurveDrawer.DrawCurve(rect, new SimpleCurveDrawInfo
            {
                curve = settings.MTBEventOccurs_Multiplier,
                label = "Multiplier"
            }, style);

            listing_Standard.Label("Sample Preset");
            if (listing_Standard.ButtonText("200%"))
            {
                settings.MTBEventOccurs_Multiplier = new SimpleCurve { { 0, 2 } };
            }
            if (listing_Standard.ButtonText("50%"))
            {
                settings.MTBEventOccurs_Multiplier = new SimpleCurve { { 0, 0.5f } };
            }
            if (listing_Standard.ButtonText("100%(year 0) -> 300%(year 10)"))
            {
                settings.MTBEventOccurs_Multiplier = new SimpleCurve
                {
                    { 0, 1 },
                    { 600, 3 }
                };
            }
            if (listing_Standard.ButtonText("150%(year 0) -> 150%(year 1.5) -> 220%(year 4) -> 300%(year 10)"))
            {
                settings.MTBEventOccurs_Multiplier = new SimpleCurve
                {
                    { 90, 1.5f },
                    { 240, 2.2f },
                    { 600, 3 }
                };
            }

            if (listing_Standard.ButtonText("50%(year 0) -> 100%(year 1) -> 200%(year 2) -> 350%(year 3) -> 500%(year 4)"))
            {
                settings.MTBEventOccurs_Multiplier = new SimpleCurve
                {
                    { 0, 0.5f },
                    { 60, 1f },
                    { 120, 2f },
                    { 180, 3.5f },
                    { 240, 5f }
                };
            }

            listing_Standard.End();
            ViewRectHeight = listing_Standard.CurHeight;
            Widgets.EndScrollView();
        }

        public override string SettingsCategory()
        {
            return "Incident Count Multiplier";
        }

        public override void WriteSettings()
        {
            settings.MTBEventOccurs_Multiplier.SortPoints();
            //settings.MinIncidentCountMultiplier.SortPoints();
            //settings.MaxIncidentCountMultiplier.SortPoints();
            //settings.IncidentCycleAcceleration.SortPoints();
            base.WriteSettings();
        }

        private void SimpleCurveEditor(Listing_Standard listing_Standard, ref SimpleCurve curve, float min = 0f, float max = 1E+09f)
        {
            float lastx = 0;
            for (int num = 0; num < curve.PointsCount; num++)
            {
                CurvePoint point = curve.Points[num];

                Rect rect1 = listing_Standard.GetRect(Text.LineHeight);
                rect1.width = (rect1.width - 55) / 2;
                Rect rect2 = new Rect(rect1)
                {
                    x = rect1.xMax + 5
                };
                Rect rect3 = new Rect(rect1)
                {
                    width = 20,
                    x = rect2.xMax + 5
                };
                Rect rect4 = new Rect(rect3)
                {
                    x = rect3.xMax + 5
                };

                float x = point.x;
                if(x <= lastx) { x = lastx + 1f; }
                //string buffer2 = x.ToString();
                x = Widgets.HorizontalSlider(rect1, x, 0, 5000f, false, "DaysDescription".Translate() + ": " + x, null, null, 1);
                //listing_Standard.Label("DaysDescription".Translate() + ": " + x);
                //listing_Standard.Slider(x, 0, 10000f);
                //Widgets.TextFieldNumeric<float>(rect2, ref y, ref buffer2, min, max);
                

                float y = point.y;
                float ypercent = y * 100;
                //string buffer1 = y.ToString();
                ypercent = Widgets.HorizontalSlider(rect2, ypercent, 0, 10000f, false, "PercentDescription".Translate() + ": " + ypercent + "%", null, null, 1);
                //listing_Standard.Label("PercentDescription".Translate() + ": " + ypercent + "%");
                //listing_Standard.Slider(ypercent, 0, 1000f);
                y = ypercent / 100;
                //Widgets.TextFieldNumeric<float>(rect1, ref x, ref buffer1, min, max);

                if (x != point.x || y != point.y)
                {
                    curve.Points[num] = new CurvePoint(x, y);
                    curve.View.SetViewRectAround(curve);
                }

                if (Widgets.ButtonText(rect3, "+"))
                {
                    curve.Add(new CurvePoint(point), true);
                    curve.View.SetViewRectAround(curve);
                }
                if (Widgets.ButtonText(rect4, "-"))
                {
                    //curves.RemovePointNear(point);
                    curve.Points.RemoveAt(num);
                    if (curve.PointsCount == 0)
                    {
                        curve.Add(0, 1);
                    }
                    curve.View.SetViewRectAround(curve);
                }

                listing_Standard.Gap(listing_Standard.verticalSpacing);
                lastx = x;
            }
        }

    }

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

            List<CurvePoint> points1 = MTBEventOccurs_Multiplier.ToList();
            //List<CurvePoint> points2 = MinIncidentCountMultiplier.ToList();
            //List<CurvePoint> points3 = MaxIncidentCountMultiplier.ToList();
            //List<CurvePoint> points4 = IncidentCycleAcceleration.ToList();

            Scribe_Collections.Look<CurvePoint>(ref points1, "MTBEventOccurs_Multiplier");
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
            SimpleCurve curves = new SimpleCurve();
            foreach (CurvePoint curvePoint in list)
            {
                curves.Add(curvePoint);
            }
            return curves;
        }
    }

}
