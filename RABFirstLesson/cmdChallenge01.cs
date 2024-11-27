namespace RABFirstLesson
{
    [Transaction(TransactionMode.Manual)]
    public class cmdChallenge01 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Revit application and document variables
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            Transaction t = new Transaction(doc);
            t.Start("Let's do this");
            // Your Module 01 Challenge code goes here

            int numOne = 250;
            double startingElevation = 0;
            double floorHeight = 15;

            for (int i = 1; i <= numOne; i++)
            {
                Level newLevel = Level.Create(doc, i*floorHeight);

                if (i % 3 == 0 && i % 5 == 0)
                {
                    newLevel.Name = ($"FizzBuzz_{i}");
                    FilteredElementCollector collectorOne = new FilteredElementCollector(doc);
                    collectorOne.OfClass(typeof(ViewFamilyType));

                    ViewFamilyType floorPlanVFT = null;
                    foreach (ViewFamilyType curVFT in collectorOne)
                    {
                        if (curVFT.ViewFamily == ViewFamily.FloorPlan)
                        {
                            floorPlanVFT = curVFT;
                        }
                    }
                    ViewPlan floorPlan = ViewPlan.Create(doc, floorPlanVFT.Id, newLevel.Id);
                    floorPlan.Name = ($"{newLevel.Name} FLOOR PLAN");

                    FilteredElementCollector collectorTB = new FilteredElementCollector(doc);
                    collectorTB.OfCategory(BuiltInCategory.OST_TitleBlocks);
                    collectorTB.WhereElementIsElementType();

                    ViewSheet newSheet = ViewSheet.Create(doc, collectorTB.FirstElementId());
                    newSheet.Name = floorPlan.Name;
                    newSheet.SheetNumber = ($"A{i}");

                    XYZ insPoint = new XYZ(1.5, 1, 0); //create the point
                    Viewport newViewport = Viewport.Create(doc, newSheet.Id, floorPlan.Id, insPoint);

                }



                else if (i % 3 == 0)
                {
                    newLevel.Name = ($"Fizz_{i}");
                    FilteredElementCollector collectorOne = new FilteredElementCollector(doc);
                    collectorOne.OfClass(typeof(ViewFamilyType));

                    ViewFamilyType floorPlanVFT = null;
                    foreach (ViewFamilyType curVFT in collectorOne)
                    {
                        if (curVFT.ViewFamily == ViewFamily.FloorPlan)
                        {
                            floorPlanVFT = curVFT;
                        }
                    }
                    ViewPlan floorPlan = ViewPlan.Create(doc, floorPlanVFT.Id, newLevel.Id);
                    floorPlan.Name = ($"{newLevel.Name} FLOOR PLAN");
                }

                else if (i % 5 == 0)
                {
                    newLevel.Name = ($"Buzz_{i}");
                    FilteredElementCollector collectorOne = new FilteredElementCollector(doc);
                    collectorOne.OfClass(typeof(ViewFamilyType));

                    ViewFamilyType floorPlanVFT = null;
                    foreach (ViewFamilyType curVFT in collectorOne)
                    {
                        if (curVFT.ViewFamily == ViewFamily.FloorPlan)
                        {
                            floorPlanVFT = curVFT;
                        }
                    }
                    ViewPlan floorPlan = ViewPlan.Create(doc, floorPlanVFT.Id, newLevel.Id);
                    floorPlan.Name = ($"{newLevel.Name} FLOOR PLAN");
                }

                else
                {
                    newLevel.Name = ($"Level {i}");
                    FilteredElementCollector collectorOne = new FilteredElementCollector(doc);
                    collectorOne.OfClass(typeof(ViewFamilyType));

                    ViewFamilyType floorPlanVFT = null;
                    foreach (ViewFamilyType curVFT in collectorOne)
                    {
                        if (curVFT.ViewFamily == ViewFamily.FloorPlan)
                        {
                            floorPlanVFT = curVFT;
                        }
                    }
                    ViewPlan floorPlan = ViewPlan.Create(doc, floorPlanVFT.Id, newLevel.Id);
                    floorPlan.Name = ($"{newLevel.Name} FLOOR PLAN");
                }

            }
            t.Commit();
            t.Dispose();
            return Result.Succeeded;
        }
        internal static PushButtonData GetButtonData()
        {
            // use this method to define the properties for this command in the Revit ribbon
            string buttonInternalName = "btnChallenge01";
            string buttonTitle = "Module\r01";

            Common.ButtonDataClass myButtonData = new Common.ButtonDataClass(
                buttonInternalName,
                buttonTitle,
                MethodBase.GetCurrentMethod().DeclaringType?.FullName,
                Properties.Resources.Module01,
                Properties.Resources.Module01,
                "Module 01 Challenge");

            return myButtonData.Data;
        }
    }

}
