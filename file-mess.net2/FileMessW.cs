using share;
using System;
using System.IO;
using System.Threading;
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
            {
                this.Invoke(new Logger((String msg) =>
                {
                    listBox1.Items.Add(msg);
                    listBox1.SelectedIndex = listBox1.Items.Count - 1;
                }), new Object[] { log });
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button1.Enabled = false;
            MessOption option = radioButton1.Checked ? MessOption.MESS : MessOption.UNMESS;
            bool isDel = checkBox2.Checked;
            if (checkBox1.Checked)
            {
                String path = textBox1.Text;
                if (!String.IsNullOrEmpty(path) && Directory.Exists(path))
                {
                    try
                    {
                        new Thread(() =>
                        {
                            FileMess.MessDir(path, option, isDel, Log);
                        }).Start();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("无法处理的异常：" + ex.Message);
                    }
                }
            }
            else
            {
                if (openFileDialog1.FileNames != null && openFileDialog1.FileNames.Length > 0)
                {
                    try
                    {
                        String[] names = openFileDialog1.FileNames;
                        new Thread(() =>
                        {
                            foreach (var name in names)
                            {
                                Log("<---" + name);
                                String newName = FileMess.Mess(name, option, isDel);
                                Log("--->" + newName);
                            }
                        }).Start();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("无法处理的异常：" + ex.Message);
                    }

                }
            }
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
