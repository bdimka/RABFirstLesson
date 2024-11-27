namespace RABFirstLesson
{
    [Transaction(TransactionMode.Manual)]
    public class cmdSkills01 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Revit application and document variables
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Your Module 01 Skills code goes here
            // Delete the TaskDialog below and add your code
            TaskDialog.Show("Module 01 Skills", "Got Here!");

            Transaction t = new Transaction(doc);
            t.Start("I'm doing something in Revit");
            Level newLevel = Level.Create(doc, 10);
            newLevel.Name = "HELLO I'M A LEVEL";

            //create a filtered element collector to get view family type
            FilteredElementCollector collector1 = new FilteredElementCollector(doc);
            collector1.OfClass(typeof(ViewFamilyType));

            ViewFamilyType floorPlanVFT = null;
            foreach(ViewFamilyType curVFT in collector1)
            {
                if(curVFT.ViewFamily == ViewFamily.FloorPlan)
                {
                    floorPlanVFT = curVFT;
                }
            }
            //Create a floor plan view
            ViewPlan newFloorPlan = ViewPlan.Create(doc, floorPlanVFT.Id, newLevel.Id);
            newFloorPlan.Name = ($"{newLevel.Name} FLOOR PLAN");


            ViewFamilyType ceilingPlanVFT = null;
            foreach(ViewFamilyType curVFT in collector1)
            {
                if(curVFT.ViewFamily == ViewFamily.CeilingPlan)
                {
                    ceilingPlanVFT = curVFT;
                }
            }
            ViewPlan newCeilingPlan = ViewPlan.Create(doc, ceilingPlanVFT.Id, newLevel.Id);
            newCeilingPlan.Name = ($"{ newLevel.Name} CEILING PLAN");

            //get a title block type
            FilteredElementCollector collector2 = new FilteredElementCollector(doc);
            collector2.OfCategory(BuiltInCategory.OST_TitleBlocks);
            collector2.WhereElementIsElementType();

            // Create sheet
            ViewSheet newSheet = ViewSheet.Create(doc, collector2.FirstElementId());
            newSheet.Name = "My New Sheet";
            newSheet.SheetNumber = "A101";

            //Create Viewport
            XYZ insPoint = new XYZ(1,0.5,0);
            Viewport newViewport = Viewport.Create(doc, newSheet.Id, newFloorPlan.Id, insPoint);

            t.Commit();
            t.Dispose();
            return Result.Succeeded;
        }
    }

}
