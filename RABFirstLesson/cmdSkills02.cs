using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;

namespace RABFirstLesson
{
    [Transaction(TransactionMode.Manual)]
    public class cmdSkills02 : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Revit application and document variables
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Document doc = uidoc.Document;

            // Your Module 02 Skills code goes here
            //prompt user to select elements in the model

            //1a. pick single element
            Reference pickRef = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element, "Select Element"); //the string at the end shows up in revit on the lower left corner
            Element pickElement = doc.GetElement(pickRef);

            //1b. pick multiple elements
            List<Element> pickList = uidoc.Selection.PickElementsByRectangle("Select Elements").ToList(); //tolist converts the i-list into a list

            TaskDialog.Show("Test", $"I selected {pickList.Count} elements");

            //2. Filter for elements you want (Lines, in this case)
            List<CurveElement> lineList = new List<CurveElement>();
            foreach (Element elem in pickList)
            {
                if(elem is CurveElement)
                {
                    lineList.Add(elem as CurveElement); //adds element to list, and casts it as a curve instead of an element
                }
            }

            //2b. Filter selected elements for model curves
            List<CurveElement> modelCurves = new List<CurveElement>();
            foreach(Element elem2 in pickList)
            {
                if(elem2 is CurveElement)
                {
                    CurveElement curveElem = elem2 as CurveElement;

                    if(curveElem.CurveElementType == CurveElementType.ModelCurve)
                    {
                        modelCurves.Add(curveElem);
                    }
                }
            }

            //3. curve data
            foreach(CurveElement currentCurve in modelCurves)
            {
                Curve rvtCurve = currentCurve.GeometryCurve;
                XYZ endPoint = rvtCurve.GetEndPoint(1);
                XYZ startPoint = rvtCurve.GetEndPoint(0);

                GraphicsStyle curStyle = currentCurve.LineStyle as GraphicsStyle;

                Debug.Print(curStyle.Name);
            }

            //5. Create transaction with a using statement
            using (Transaction t = new Transaction(doc))
            {
                t.Start("Create a wall");

                //4. Create Walls
               
                Level newLevel = Level.Create(doc, 20);
                Curve curCurve = modelCurves[0].GeometryCurve; //reference the geometry curve property of the model curve

                Wall newWall = Wall.Create(doc, curCurve, newLevel.Id, false);
               

                //4b. Create wall with wall type
                FilteredElementCollector wallTypes = new FilteredElementCollector(doc);
                //wallTypes.OfClass(typeof(WallType))
                //or
                wallTypes.OfCategory(BuiltInCategory.OST_Walls);
                wallTypes.WhereElementIsElementType();

                Wall newWall2 = Wall.Create(doc, curCurve, wallTypes.FirstElementId(), newLevel.Id, 20, 0, false, false);

                //6. get system types
                FilteredElementCollector systemCollector = new FilteredElementCollector(doc);
                systemCollector.OfClass(typeof(MEPSystemType));

                //7. get duct system type
                MEPSystemType ductSystem = getSystemTypeByName(doc, "Supply Air");
                foreach (MEPSystemType systemType in systemCollector)


                //8. get duct type
                Curve curve3 = modelCurves[2].GeometryCurve;
                FilteredElementCollector ductCollector = new FilteredElementCollector(doc);
                ductCollector.OfClass(typeof(DuctType));
                Curve curve4 = modelCurves[3].GeometryCurve;   

                //9. create duct
                Duct newDuct = Duct.Create(doc, ductSystem.Id, ductCollector.FirstElementId(), newLevel.Id, curve3.GetEndPoint(0), curve3.GetEndPoint(1));

                //10. create pipe
                MEPSystemType pipeSystem = getSystemTypeByName(doc, "Domestic Hot Water");
                foreach(MEPSystemType systemType in systemCollector)


                FilteredElementCollector pipeCollector = new FilteredElementCollector(doc);
                pipeCollector.OfClass(typeof(PipeType));

                Pipe newPipe = Pipe.Create(doc, pipeSystem.Id, pipeCollector.FirstElementId(), newLevel.Id, curve4.GetEndPoint(0), curve4.GetEndPoint(1));

                //13 Switch statement. Good when we have multiple things we want to check
                int numVal = 5;
                string numAsString = "";

                switch(numVal)
                {
                    case 0:
                        numAsString = "Zero";
                        break;
                    case 5:
                        numAsString = "Five";
                        break;
                    case 10:
                        numAsString = "Ten";
                        break;
                    default:
                        numAsString = "one hundred";
                        break;
                }

                t.Commit();
            }


            return Result.Succeeded;
        }

        internal string myFirstMethod()
        {
            return "This is my first method.";
        }

        internal void mySecondMethod()
        {
            Debug.Print("This is my second method.");
        }

        internal string myThirdMethod(string input)
        {
            string returnString = $"This is my third method: {input}";
            return returnString;
        }

        //Get system type method
        internal MEPSystemType getSystemTypeByName(Document doc, string name)
        {
            FilteredElementCollector systemCollector = new FilteredElementCollector(doc);
            systemCollector.OfClass(typeof(MEPSystemType));

            foreach (MEPSystemType systemType in systemCollector)
            {
                if(systemType.Name == name)
                {
                    return systemType;
                }
            }
            return null;
        }

    }

}
