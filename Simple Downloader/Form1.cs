using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Threading;

namespace Simple_Downloader
{
    public partial class Form1 : Form
    {
        // Create the file name string.
        string filename;
        // Create the webclient.
        WebClient webClient = new WebClient();
        public Form1()
        {
            InitializeComponent();
        }

        // Create the download void.
        private void DownloadFile(string url, string save)
        {
            // We need to add an using, so we can download another file after the first one completes. If we don't that it will not work.

            // Run code every time the download changes.
            webClient.DownloadProgressChanged += webClient_DownloadProgressChanged;
            // Run codes when file download has been completed.
            webClient.DownloadFileCompleted += webClient_DownloadFileCompleted;
            // Start the download.
            webClient.DownloadFileAsync(new Uri(url), save);
            
        }
        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Update the progress bar.
            prgDownload.Value = e.ProgressPercentage;
            // Update status label.
            lblStatus.Text = "Status: downloading...";
            // Update progress label.
            //lblProgress.Text = "Progress: downloaded " + (e.BytesReceived / 1024d / 1024d).ToString("0.00 MB") + " (" + e.BytesReceived + " bytes) of " + (e.TotalBytesToReceive/1024d/1024d).ToString("0.00 MB") + " (" + e.TotalBytesToReceive + " bytes) " + e.ProgressPercentage + "%";
            lblProgress.Text =(e.BytesReceived / 1024d / 1024d).ToString("0.00 MB") + " of " + (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00 MB  ")+ e.ProgressPercentage + "%";
            // Disable Download button.
            btnDownload.Enabled = false;
            // Enable Cancel button.
            btnCancel.Enabled = true;
            // Make the textbox readonly.
            txtUrl.ReadOnly = true;
        }

        private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //Run download canceled codes.
                if (e.Cancelled == true)
            {
                lblStatus.Text = "Status: download canceled.";
            }
            // Run download completed codes.
            else if (e.Cancelled == false)
            {
                lblStatus.Text = "Status: download completed!";
                MessageBox.Show("Download completed!", "Completed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // Enable Download button.
            btnDownload.Enabled = false;
            // Disabled Cancel button.
            btnCancel.Enabled = false;
            // Reset the progress bar.
            prgDownload.Value = 0;
            // Make the textbox writeable.
            txtUrl.ReadOnly = false;
            txtUrl.Text = string.Empty;
        }


     

        private void txtUrl_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Now we are going to create the function, that enter the file name automatically in the savefiledialog.
                Uri uri = new Uri(txtUrl.Text);
                // Save the file name to the string.
                filename = Path.GetFileName(uri.LocalPath);
                btnDownload.Enabled = true;
            }
            catch(Exception ex)
            {
                // Leave this blank, we don't need an exception message.
            }
        }

        private void btnDownload_Click(object sender, EventArgs e)
        {
            // Check if the user has entered an url address.
            if (txtUrl.Text == string.Empty)
            {
                // Show error message if messagebox is empty.
                MessageBox.Show("Please enter an url address, before clicking on Download.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    // Create a savefiledialog so the user can save the file.
                    SaveFileDialog dialog = new SaveFileDialog();
                    // Select the file type as all files.
                    dialog.Filter = "All Files|*.*";
                    // Write the file name for the user.
                    dialog.FileName = filename;
                    // Open the dialog with dialogresult.
                    DialogResult result = dialog.ShowDialog();
                    // Check if the user has clicked OK.
                    if (result == DialogResult.OK)
                    {
                        // Start the download.
                        DownloadFile(txtUrl.Text, dialog.FileName);
                    }

                }
                catch
                {
                    // Leave this blank, we don't need an exception message.
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Cancel the download.
            webClient.CancelAsync();
        }








        //static void Main(string[] args)
        //{
        //    var fw = new FileDownload("http://download.microsoft.com/download/E/E/2/EE2D29A1-2D5C-463C-B7F1-40E4170F5E2C/KinectSDK-v1.0-Setup.exe", @"D:\KinetSDK.exe", 5120);

        //    // Display progress...
        //    Task.Factory.StartNew(() =>
        //    {
        //        while (!fw.Done)
        //        {
        //            Console.SetCursorPosition(0, Console.CursorTop);
        //            Console.Write(string.Format("ContentLength: {0} | BytesWritten: {1}", fw.ContentLength, fw.BytesWritten));
        //        }
        //    });

        //    // Start the download...
        //    fw.Start();

        //    // Simulate pause...
        //    Thread.Sleep(500);
        //    fw.Pause();
        //    Thread.Sleep(2000);

        //    // Start the download from where we left, and when done print to console.
        //    fw.Start().ContinueWith(t => Console.WriteLine("Done"));

        //    Console.ReadKey();
        //}
        //public void Downloade()
        //{
        //    var fw = new FileDownload("http://download.microsoft.com/download/E/E/2/EE2D29A1-2D5C-463C-B7F1-40E4170F5E2C/KinectSDK-v1.0-Setup.exe", @"D:\KinetSDK.exe", 5120);

        //    // Display progress...
        //    Task.Factory.StartNew(() =>
        //    {
        //        while (!fw.Done)
        //        {
        //            Console.SetCursorPosition(0, Console.CursorTop);
        //            Console.Write(string.Format("ContentLength: {0} | BytesWritten: {1}", fw.ContentLength, fw.BytesWritten));
        //        }
        //    });

        //    // Start the download...
        //    fw.Start();

        //    fw.Pause();
           

        //    // Start the download from where we left, and when done print to console.
        //    fw.Start().ContinueWith(t => Console.WriteLine("Done"));

        //    Console.ReadKey();
        //}


    }
}
