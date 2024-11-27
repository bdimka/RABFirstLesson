namespace RABFirstLesson
{
    [Transaction(TransactionMode.Manual)]
    public class DeleteBackups : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // Set variables
            int counter = 0;
            string logPath = "";

            // Create a list for log file
            List<string> deletedFiles = new List<string>();
            deletedFiles.Add("The following backup files have been deleted: ");

            // Create our folder browser
            FolderBrowserDialog selectFolder = new FolderBrowserDialog();
            selectFolder.ShowNewFolderButton = false;

            //Open folder dialog, only run code if folder is selected
            if(selectFolder.ShowDialog() == DialogResult.OK)
            {
                //Create variable to get selected folder path
                string directory = selectFolder.SelectedPath;

                //Get all files from selected folder. Open and closed brackets indicated an array
                string[] files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);

                //Loop through files
                foreach(string file in files)
                {
                    //Check if file is RVT file
                    if (Path.GetExtension(file) == ".rvt" || Path.GetExtension(file) == ".rfa")
                    {
                        string checkString = file.Substring(file.Length - 9, 9);

                        if(checkString.Contains(".00"))
                        {
                            //add file name to the list for dialog
                            deletedFiles.Add(file);

                            //delete the file
                            File.Delete(file);

                            //incerement counter
                            counter++;

                        }
                        
                    }

                }

                //Output log file
                if(deletedFiles.Count > 0)
                {
                    logPath = WriteListToText(deletedFiles, directory);
                }

            }

            //Alert the user
            TaskDialog td = new TaskDialog("Complete");
            td.MainInstruction = "Deleted " + counter.ToString() + " backup files.";
            td.AddCommandLink(TaskDialogCommandLinkId.CommandLink1, "Clocl to view log file");
            td.CommonButtons = TaskDialogCommonButtons.Ok;

            TaskDialogResult result = td.Show();

            if (result == TaskDialogResult.CommandLink1)
            {
                Process.Start(logPath);
            }

            return Result.Succeeded;
        }
        internal string WriteListToText(List<string> stringList, string filePath)
        {
            string fileName = "_Deleted Backup Files.txt";
            string fullPath = filePath + @"\" + fileName;
            File.WriteAllLines(fullPath, stringList);

            return fullPath;
        }
    }

}