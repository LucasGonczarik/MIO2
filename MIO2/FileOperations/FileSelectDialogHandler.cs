namespace PerceptonMIO.FileOperations
{
    class FileSelectDialogHandler
    {
        public static string GetFilePath()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog
            {
                DefaultExt = ".csv",
                Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*"
            };

            bool? result = dlg.ShowDialog();

            string filePath = null;
            if (result == true)
            {
                filePath = dlg.FileName;
            }

            return filePath;
        }
    }
}