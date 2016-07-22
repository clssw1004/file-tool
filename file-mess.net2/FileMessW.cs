using mess_lib;
using System;
using System.IO;
using System.Windows.Forms;

namespace file_mess.net2
{
    public partial class FileMessW : Form
    {
        public FileMessW()
        {
            InitializeComponent();
            textBox1.ReadOnly = true;
            button2.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            if (checkBox1.Checked)
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    String path = folderBrowserDialog1.SelectedPath;
                    if (!String.IsNullOrEmpty(path))
                    {
                        textBox1.Text = path;
                        Log(@"已选择文件夹--->" + path);
                        button2.Enabled = true;
                    }
                }
            }
            else
            {
                openFileDialog1.ShowDialog();
            }
        }
        private void Log(String log)
        {
            if (!String.IsNullOrEmpty(log))
                listBox1.Items.Add(log);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button1.Enabled = false;
            Log(@"开始执行操作");
            MessOption option = radioButton1.Checked ? MessOption.MESS : MessOption.UNMESS;
            if (checkBox1.Checked)
            {
                String path = textBox1.Text;
                if (!String.IsNullOrEmpty(path) && Directory.Exists(path))
                {
                    FileMess.MessDir(path, option, checkBox2.Checked, Log);
                }
            }
            else
            {
                if (openFileDialog1.FileNames != null && openFileDialog1.FileNames.Length > 0)
                {
                    foreach (var name in openFileDialog1.FileNames)
                    {
                        Log("文件：" + name);
                        String newName = FileMess.Mess(name, option, checkBox2.Checked);
                        Log("--->" + newName);
                    }
                }
            }
            Log(@"任务完成");
            button1.Enabled = true;
            textBox1.Text = "";
        }
        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
            if (openFileDialog1.FileNames != null && openFileDialog1.FileNames.Length > 0)
            {
                foreach (var name in openFileDialog1.SafeFileNames)
                {
                    textBox1.Text += name + ",";
                    button2.Enabled = true;
                }
            }
        }
    }
}
