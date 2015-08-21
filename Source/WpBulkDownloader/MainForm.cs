using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using CookComputing.XmlRpc;
using System.Text.RegularExpressions;
using System.Collections.Specialized;


namespace WpBulkDownloader {
    public partial class MainForm : Form {
        
        /// <summary>
        /// This is the default count of the posts of your blog. Due to the lack of getPosts() API in wp-xmlrpc.php file, we have
        /// to use getRecentPost API that requires how many recent posts that you want to get.  
        /// </summary>
        int defaultPostCount = 1000;

        public MainForm() {
            InitializeComponent();
        }
        /// <summary>
        /// Gets the posts from blog.
        /// </summary>
        /// <returns></returns>
        private Post[] getPosts() {
            MetaWeblog _metaWebLog = new MetaWeblog(Properties.Settings.Default.BlogURL + "/xmlrpc.php" );
            string userName = Properties.Settings.Default.UserName;
            string password = Properties.Settings.Default.Password;
            Post[] posts = _metaWebLog.getRecentPosts("MyBlog", userName, password, defaultPostCount);
            return posts;
        }
        /// <summary>
        /// Find the image url. <img></img> And <a href="http://michaelsync.net/image/jpg"></a>
        /// </summary>
        /// <param name="posts"></param>
        private List<string> findImageURLs(Post[] posts) {
            int postCount = 0;
            List<string> sCol = new List<string>();
            foreach (Post p in posts) {
                Console.WriteLine("Post Title: {0}", p.title);
                postCount++;
                string ftr = @"(?<=img\s+src\=[\x27\x22])(?<Url>[^\x27\x22]*)(?=[\x27\x22])";

                Regex regEx = new Regex(ftr, RegexOptions.Compiled | RegexOptions.IgnoreCase);

                MatchCollection matches = regEx.Matches(p.description);
            

                string ahref = string.Empty;
                string title = string.Empty;
                string value = string.Empty;
                string fileName = string.Empty;

                foreach (Match match in matches) {
                    ahref = match.Groups[0].Value;
                    if ((ahref.LastIndexOf("://") > 0))
                                sCol.Add(ahref);                    
                }
            }
            return sCol;

        }
        List<string> hrefs = new List<string>();
        
        private void downloadImages(List<string> Urls) {
            FileDownloader download = null;
            foreach (string url in Urls){
                string folderPath = createDirectory(url);
                download = new FileDownloader(url, folderPath);
                download.StartDownload();
            }
        }
        private string createDirectory(string path) {
            path = path.Replace("http://", "");
            path = path.Replace("https://", "");
            string[] folderNames = path.Split('/');
            string folderpath = AppDomain.CurrentDomain.BaseDirectory + "\\Downloaded Images\\";
            if (!System.IO.Directory.Exists(folderpath))
                    System.IO.Directory.CreateDirectory(folderpath);
            for (int i = 1; i < folderNames.Length - 1; i++) {
                folderpath += folderNames[i] + "\\";
                if (!System.IO.Directory.Exists(folderpath))
                    System.IO.Directory.CreateDirectory(folderpath);
            }
            return folderpath; 
        }

        private void MainForm_Load(object sender, EventArgs e) {

        }

        
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) {

        }

        private void downloadButton_Click(object sender, EventArgs e) {
            downloadImages(hrefs);
        }

        private void optionButton_Click(object sender, EventArgs e) {
            Options _optionForm = new Options();
            _optionForm.ShowDialog();
        }
        private void startButton_Click(object sender, EventArgs e) {
            hrefs = findImageURLs(getPosts());
            hrefs.Sort();
            foreach (string href in hrefs) {
                fileListView.Items.Add(href);
            }
            downloadButton.Enabled = true;
        }

        private void closeButton_Click(object sender, EventArgs e) {
            Application.Exit();
        }
    }
}
