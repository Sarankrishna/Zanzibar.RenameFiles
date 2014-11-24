using System;
using System.Collections.Generic;
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
                dlg.Description = "Select the folder";
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
            RenameFiles(txtDirectoryName.Text);
            System.Windows.MessageBox.Show("Renamed Successfully!");
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
                        var fileNameWithOutNumber = fileName.Substring(fileName.IndexOf(".") + 1);
                        var newFileName = fileInfo.DirectoryName + "\\" + int.Parse(fileNumber).ToString("D2") + "." + fileNameWithOutNumber;
                        File.Move(fileInfo.FullName, newFileName);
                    }
                }
            }
        }
    }
}
