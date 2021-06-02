using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using ResearchOfFiles.Properties;

namespace ResearchOfFiles
{
    public partial class Form1 : Form
    {
        private string selectedPath = Settings.Default["SelectedPath"].ToString();
        private string regEx = Settings.Default["RegEx"].ToString();
        private bool isCheck = false;
        private int sec = 0;
        private int min = 0;
        private int hour = 0;
        private int findingFiles = 0;
        private int totalFiles = 0;

        public Form1()
        {
            InitializeComponent();
            textBox1.Text = regEx;
            choiceOfPath();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            sec = 0;
            min = 0;
            hour = 0;
            regEx = textBox1.Text;
            timer1.Start();
            printTime();
            isCheck = true;
            createTree();
            lFinding.Text = findingFiles.ToString();
            lTotal.Text = totalFiles.ToString();
            isCheck = false;
        }

        private void findCountFile(string curPath)
        {
            
        }
        

        private void printTime()
        {
            lTime.Text = formatDec(hour) + ":" + formatDec(min) + ":" + formatDec(sec);
        }

        private string formatDec(int dec)
        {
            string s = dec.ToString();
            return s.PadLeft(2, '0');
        }

        private void choiceOfPath()
        {
            label1.Text = selectedPath;
            DirectoryInfo dirInfo = new DirectoryInfo(selectedPath);

            createTree();
        }

        private void createTree()
        {
            treeView1.Nodes.Clear();
            findingFiles = 0;
            totalFiles = 0;
            createNodes(false, selectedPath, treeView1.Nodes, 0);
            createNodes(true, selectedPath, treeView1.Nodes, 0);

        }

        private void createNodes(bool isFile, string curPath, TreeNodeCollection tnc, int level)
        {
            string[] obj = { };

            try
            {
                obj = isFile ? Directory.GetFiles(curPath) : Directory.GetDirectories(curPath);
            }
            catch(Exception e)
            {
                
            }

            foreach (string s in obj)
            {

                DirectoryInfo objInfo = new DirectoryInfo(s);
                bool isFinding = Regex.IsMatch(objInfo.Name, regEx);

                if (!isCheck || isFinding || !isFile)
                {
                    TreeNode curNode = new TreeNode(objInfo.Name);

                    if(isFinding && isFile && level == 0)
                    {
                        findingFiles++;
                    }

                    if(isFile && level == 0)
                    {
                        totalFiles++;
                    }

                    if (!isFile && level < 1)
                    {
                        createNodes(false, s, curNode.Nodes, level + 1);
                        createNodes(true, s, curNode.Nodes, level + 1);
                    }

                    tnc.Add(curNode);

                }
            }
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.Nodes.Clear();
            string curPath = selectedPath + @"\" + e.Node.FullPath;
            try
            {
                createNodes(false, curPath, e.Node.Nodes, 0);
                createNodes(true, curPath, e.Node.Nodes, 0);

            }
            catch (Exception ex) { }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default["SelectedPath"] = selectedPath;
            Settings.Default["RegEx"] = regEx;
            Settings.Default.Save();
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            selectedPath += @"\" + e.Node.FullPath;
            choiceOfPath();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(selectedPath);
            selectedPath = dirInfo.Parent != null ? dirInfo.Parent.FullName : selectedPath;
            choiceOfPath();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            printTime();
            sec++;
            if(sec > 59)
            {
                sec = 0;
                min++;
            }

            if(min > 59)
            {
                min = 0;
                hour++;
            }
        }
    }
}
