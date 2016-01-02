using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Zanzibar.RenameFiles
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void cmdSelectFolder_Click(object sender, RoutedEventArgs e)
        {
            BrowseFolder();
        }


        private void BrowseFolder()
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Select the folder for renaming the file(s)..........:";
                dlg.ShowNewFolderButton = true;
                DialogResult result = dlg.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    txtDirectoryName.Text = dlg.SelectedPath;

                }
            }
        }

        private void cmdClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cmdRename_Click(object sender, RoutedEventArgs e)
        {
            RenameFilesv1(txtDirectoryName.Text);
            System.Windows.MessageBox.Show("Renamed Successfully!");
        }

        private bool IsNumber(string strNumber)
        {
            int n;
            bool isNumeric = int.TryParse(strNumber, out n);
            return isNumeric;
        }
        private void RenameFiles(string directoryName)
        {

            DirectoryInfo d = new DirectoryInfo(directoryName);
            FileSystemInfo[] infos = d.GetFileSystemInfos();


            foreach (FileSystemInfo f in infos)
            {
                if (!(f.Name.ToUpper().Equals("THUMBS.DB")))
                {
                    FileAttributes attr = File.GetAttributes(f.FullName);
                    if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                        RenameFiles(f.FullName);
                    // Files in directory
                    else
                    {
                        FileInfo fileInfo = new FileInfo(f.FullName);
                        var fileName = fileInfo.Name;
                        var fileNumber = fileName.Substring(0, fileName.IndexOf("."));
                        if (IsNumber(fileNumber))
                        {
                            var fileNameWithOutNumber = fileName.Substring(fileName.IndexOf(".") + 1);
                            var newFileName = fileInfo.DirectoryName + "\\" + int.Parse(fileNumber).ToString("D2") + "." + fileNameWithOutNumber;
                            File.Move(fileInfo.FullName, newFileName);
                        }
                    }
                }
            }
        }

        private void RenameFilesv1(string directoryName)
        {

            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            DirectoryInfo d = new DirectoryInfo(directoryName);
            FileSystemInfo[] infos = d.GetFileSystemInfos();


            foreach (FileSystemInfo f in infos)
            {
                if (!(f.Name.ToUpper().Equals("THUMBS.DB")))
                {
                    FileAttributes attr = File.GetAttributes(f.FullName);
                    if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                        RenameFilesv1(f.FullName);
                    // Files in directory
                    else
                    {
                        //FileInfo fileInfo = new FileInfo(f.FullName);
                        //var fileName = fileInfo.Name;

                        TagLib.File ft = TagLib.File.Create(f.FullName);
                        ft.Tag.Title = System.IO.Path.GetFileNameWithoutExtension(f.FullName);
                        ft.Tag.AlbumArtists = new string[] { "K. J. Yesudas" };
                        ft.Tag.Performers= new string[] { "K. J. Yesudas" };
                        ft.Tag.Album = "K. J. Yesudas Song";
                        ft.Tag.Track = 0;
                        ft.Save();

                        //var fileNumber = fileName.Substring(0,4);
                        //if (IsNumber(fileNumber))
                        //{
                        //    var namewithOutExtension = System.IO.Path.GetFileNameWithoutExtension(fileInfo.FullName);
                        //    var ext = System.IO.Path.GetExtension(fileInfo.FullName);
                        //    var fileNameWithOutNumber = namewithOutExtension.Substring(4);

                        //    var newFileName = fileInfo.DirectoryName + "\\" + myTI.ToTitleCase(fileNameWithOutNumber.ToLower())+".mp3";
                        //    File.Move(fileInfo.FullName, newFileName);
                        //}
                        //else
                        //{
                        //    var namewithOutExtension = System.IO.Path.GetFileNameWithoutExtension(fileInfo.FullName);
                        //    var ext = System.IO.Path.GetExtension(fileInfo.FullName);

                        //    var newFileName = fileInfo.DirectoryName + "\\" + "001 " + myTI.ToTitleCase(namewithOutExtension.ToLower().Replace("_"," ")) + ".mp3";
                        //    File.Move(fileInfo.FullName, newFileName);
                        //}
                    }
                }
            }
        }

    }
}
