using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        XmlDocument testXMLDocument = new XmlDocument();
        List<TestQuestion> test;
        string filePath;// = "test-doc.xml";
        string testName;
        int answerInputCount;// = 0;
        public Form1()
        {
            InitializeComponent();
            test = new();
            panel1.Visible = false;
            button6.Visible = false;
            answerInputCount = 0;
        }
        private void readTestFromXMLDocument()
        {
           testXMLDocument.Load(filePath);
            var rootNode = testXMLDocument.DocumentElement;
            testName = rootNode.GetAttribute("name");
            if (testName != null)
            {
                label6.Text += testName;
            }
          var countTask=  rootNode.ChildNodes.Count;
           foreach(XmlNode taskNode in rootNode.ChildNodes)
            {
               string question = taskNode.ChildNodes[0].InnerText;
                var count = 0;
               (string, bool)[] answers= new (string, bool)[taskNode.ChildNodes[1].ChildNodes.Count];
                foreach(XmlNode varNode in taskNode.ChildNodes[1].ChildNodes)
                {
                    answers[count] = (varNode.InnerText, Convert.ToBoolean(varNode.Attributes["accuracy"].Value));
                    count++;
                }
                 TestQuestion que = new TestQuestion(question, answers);
                 test.Add(que);
                 listBox1.Items.Add("Вопрос №" + test.Count.ToString()); 
            }
        }
        private void createTestXMLDocumentFROMList()
        {
            var countTask = test.Count();
            XmlNode testNameNode = testXMLDocument.CreateElement("testName");
            XmlAttribute attributeTestName = testXMLDocument.CreateAttribute("name");
            attributeTestName.Value = testName;
            testNameNode.Attributes.Append(attributeTestName);
            testXMLDocument.AppendChild(testNameNode);
            foreach (TestQuestion tq in test)
            {
                XmlNode taskNode = testXMLDocument.CreateElement("task");
                testNameNode.AppendChild(taskNode);
                XmlNode questionNode = testXMLDocument.CreateElement("question");
                taskNode.AppendChild(questionNode);
                questionNode.InnerText = tq.question;
                XmlNode answersNode = testXMLDocument.CreateElement("answers");
                taskNode.AppendChild(answersNode);
                var lengthAnswer = tq.answers.Length;
                for (int i = 0; i < lengthAnswer; i++)
                {
                    XmlNode variantNode = testXMLDocument.CreateElement("variant" + i.ToString());
                    answersNode.AppendChild(variantNode);
                    XmlAttribute attribute = testXMLDocument.CreateAttribute("accuracy");
                    attribute.Value = tq.answers[i].Item2.ToString();
                    variantNode.Attributes.Append(attribute);
                    variantNode.InnerText = tq.answers[i].Item1;
                }
                testNameNode.AppendChild(taskNode);
            }
            testXMLDocument.Save(filePath);
        }
        private void новыйТестToolStripMenuItem_Click(object sender, EventArgs e)
        {
            testName = Microsoft.VisualBasic.Interaction.InputBox("Введите название теста:");
            label6.Text = label6.Text + testName;
            filePath = testName + ".xml";
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void открытьТестToolStripMenuItem_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            test.Clear();
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;
                
                }
            }
            if (filePath != null)
                readTestFromXMLDocument();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
                label5.Text = textBox1.Text+"\n";
        }
       
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
                for(int i = record;i<listBox1.Items.Count;i++)
                {
                    listBox1.Items[i] = "Вопрос№"+(i+1).ToString();
                }
            }
        }

        private void сохранитьТестToolStripMenuItem_Click(object sender, EventArgs e)
        {
            createTestXMLDocumentFROMList();
        }

       private void экспортВPDFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var pdfTest = new iTextSharp.text.Document();
            var pdfWriter = PdfWriter.GetInstance(pdfTest, new FileStream(filePath+".pdf", FileMode.Create));
             pdfTest.Open(); 
          
            BaseFont baseFont = BaseFont.CreateFont("c:/Windows/Fonts/arial.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            var font = new iTextSharp.text.Font(baseFont, iTextSharp.text.Font.DEFAULTSIZE, iTextSharp.text.Font.NORMAL);
            pdfTest.Add(new iTextSharp.text.Phrase("Тест:" + testName + "\n",font));
            pdfTest.Add(new iTextSharp.text.Phrase("Фамилия И.О.___________________\t Группа№____________________________\n",font));
            for (int idx = 0; idx < test.Count;idx++ )
            {

                pdfTest.Add(new iTextSharp.text.Phrase("Задание " + (idx + 1).ToString() + ":" + test[idx].retStrTaskWhithoutAnswers(), font));
            }
            pdfTest.Close();
            pdfWriter.Close();
        }
    }
}
