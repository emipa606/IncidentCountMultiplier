using Mlie;
using UnityEngine;
using Verse;

namespace IncidentCountMultiplier;

public class IncidentCountMultiplier : Mod
{
    private static readonly SimpleCurveDrawerStyle style = new SimpleCurveDrawerStyle
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

    private static string currentVersion;

    public readonly IncidentCountMultiplierSettings settings;

    private Vector2 scrollPosition = new Vector2(0f, 0f);

    public IncidentCountMultiplier(ModContentPack content) : base(content)
    {
        settings = GetSettings<IncidentCountMultiplierSettings>();
        currentVersion =
            VersionFromManifest.GetVersionFromModMetaData(
                ModLister.GetActiveModWithIdentifier("Mlie.IncidentCountMultiplier"));
    }

    private static float ViewRectHeight { get; set; }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        var viewRect = new Rect(0, 0, inRect.width - 30, ViewRectHeight);
        Widgets.BeginScrollView(inRect, ref scrollPosition, viewRect);

        var viewRect2 = new Rect(0, 0, viewRect.width, 99999);

        var listing_Standard = new Listing_Standard
        {
            ColumnWidth = (float)(viewRect.width - 10.0)
        };
        listing_Standard.Begin(viewRect2);

        var labelPosition = listing_Standard.Label("IncidentCountMultiplierDescription".Translate());
        if (currentVersion != null)
        {
            GUI.contentColor = Color.gray;
            Widgets.Label(
                new Rect(labelPosition.position + new Vector2(labelPosition.width / 3 * 2, labelPosition.y),
                    new Vector2(labelPosition.width / 3, labelPosition.height)),
                "IncidentCountMultiplierModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

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


        var rect = listing_Standard.GetRect(250);
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

        if (listing_Standard.ButtonText(
                "50%(year 0) -> 100%(year 1) -> 200%(year 2) -> 350%(year 3) -> 500%(year 4)"))
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

    private void SimpleCurveEditor(Listing_Standard listing_Standard, ref SimpleCurve curve)
    {
        float lastx = 0;
        for (var num = 0; num < curve.PointsCount; num++)
        {
            var point = curve.Points[num];

            var rect1 = listing_Standard.GetRect(Text.LineHeight);
            rect1.width = (rect1.width - 55) / 2;
            var rect2 = new Rect(rect1)
            {
                x = rect1.xMax + 5
            };
            var rect3 = new Rect(rect1)
            {
                width = 20,
                x = rect2.xMax + 5
            };
            var rect4 = new Rect(rect3)
            {
                x = rect3.xMax + 5
            };

            var x = point.x;
            if (x <= lastx)
            {
                x = lastx + 1f;
            }

            //string buffer2 = x.ToString();
            x = Widgets.HorizontalSlider(rect1, x, 0, 5000f, false, "DaysDescription".Translate() + ": " + x, null,
                null, 1);
            //listing_Standard.Label("DaysDescription".Translate() + ": " + x);
            //listing_Standard.Slider(x, 0, 10000f);
            //Widgets.TextFieldNumeric<float>(rect2, ref y, ref buffer2, min, max);


            var y = point.y;
            var ypercent = y * 100;
            //string buffer1 = y.ToString();
            ypercent = Widgets.HorizontalSlider(rect2, ypercent, 0, 10000f, false,
                "PercentDescription".Translate() + ": " + ypercent + "%", null, null, 1);
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
                curve.Add(new CurvePoint(point));
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