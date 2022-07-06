using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        XmlDocument testXMLDocument = new XmlDocument();
        List<TestQuestion> test;
        public Form1()
        {
            InitializeComponent();
            test = new();
            panel1.Visible = false;
            button6.Visible = false;

        }
        private void readTestFromXMLDocument()
        {
           /* testXMLDocument.Load(filePath);
            foreach(XmlNode questionNode in testXMLDocument)
            {
                string question = questionNode["Question"].InnerText;
                int ansCounter = int.Parse(questionNode["Answers"].InnerText);
                string[] answers = new string[ansCounter];
                for (int i = 0; i < ansCounter; i++)
                    answers[i] = questionNode[("Answer" + i.ToString())].InnerText;
                TestQuestion que = new TestQuestion(question, answers);
                test.Add(que);
                listBox1.Items.Add(que); 
            }*/
        }
        private void createTestXMLDocumentFROMList()
        {
            XmlNode testNameNode = testXMLDocument.CreateElement("TestName");
            testXMLDocument.AppendChild(testNameNode);
            XmlNode taskNode = testXMLDocument.CreateElement("Task");
            testNameNode.AppendChild(taskNode);
            XmlNode questionNode = testXMLDocument.CreateElement("Question");
            taskNode.AppendChild(questionNode);
            questionNode.InnerText = test[0].question;
            //-----------------------------------------------------
            XmlNode answersNode = testXMLDocument.CreateElement("Answers");
            taskNode.AppendChild(answersNode);
            // XmlAttribute question = testXMLDocument.CreateAttribute("Question");
            // question.Value = test[0].question;
            // taskNode.Attributes.Append(question);
            XmlAttribute attribute = testXMLDocument.CreateAttribute("answer");
            attribute.Value = test[0].answers[0].Item2.ToString();
            answersNode.Attributes.Append(attribute);
            answersNode.InnerText = test[0].answers[0].Item1;
            //--------------------------------------------------------
            testNameNode.AppendChild(taskNode);
            testXMLDocument.Save("test-doc.xml");
        }
        private void новыйТестToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void открытьТестToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
                label5.Text = textBox1.Text+"\n";
        }
        int answerInputCount = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            answerInputCount++;
            bool correct = false;
            if (checkBox1.Checked)
                correct = true;
            if (label5.Text.Length != 0)
                label5.Text += answerInputCount.ToString() + ": " + textBox2.Text + " \u00FF "+ correct.ToString() +"\n";
            else
                MessageBox.Show("Введите вопрос!");
            if (checkBox1.Checked)
                checkBox1.Checked = false;
            textBox2.Text = "";
        }
        private void button3_Click(object sender, EventArgs e)
        {
           
            String strInput = label5.Text;
            String[] testTask = strInput.Split("\n");
            (String, bool)[] answers = new (String, bool)[testTask.Length-2];
            String question = testTask[0];
            for(int i = 1; i < testTask.Length-1; i++)
            {
                String[] str = testTask[i].Split("\u00FF");
                String[] strNum = str[0].Split(":");
                answers[i - 1].Item1 = strNum[1];
                answers[i - 1].Item2 = Convert.ToBoolean(str[1]);
            }
            TestQuestion task = new TestQuestion(question, answers);
            test.Add(task);
            listBox1.Items.Add("Вопрос №"+test.Count.ToString());
            label5.Text = "";
            textBox1.Text = "";
            answerInputCount = 0;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                TestQuestion task = test[listBox1.SelectedIndex];
                label5.Text = task.retStrTask();
                textBox1.Text = task.question;
                panel1.Visible = true;
                button6.Visible = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            TestQuestion task = test[listBox1.SelectedIndex];
            if (textBox1.Text != task.question)
            { task.question = textBox1.Text;
                label5.Text = task.retStrTask();
            }
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox2.Text != "")
            {
                if (textBox2.Text.Contains(":"))
                {
                    String[] str = textBox2.Text.Split(":");
                    TestQuestion task = test[listBox1.SelectedIndex];
                    int answerNumber = Convert.ToInt32(str[0]);
                    bool correct = false;
                    if (checkBox1.Checked)
                        correct = true;
                    task.editAnswersInTask(answerNumber - 1, str[1], correct);
                    test[listBox1.SelectedIndex] = task;
                    label5.Text = test[listBox1.SelectedIndex].retStrTask();
                }
                else
                MessageBox.Show("Введите редактируемый вариант ответа в формате:\n {Номер варианта}:{Вариант ответа}!");
            }
            else
                MessageBox.Show("Введите вариант ответа!");
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            listBox1.ClearSelected();
            textBox1.Text = "";
            textBox2.Text = "";
            label5.Text = "";
            panel1.Visible = false;
            button6.Visible = false;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                int record = listBox1.SelectedIndex;
                test.Remove(test[record]);
                listBox1.Items.RemoveAt(record);
                listBox1_MouseDoubleClick(null,null);
            }
        }

        private void сохранитьТестToolStripMenuItem_Click(object sender, EventArgs e)
        {
            createTestXMLDocumentFROMList();
        }
    }
}
