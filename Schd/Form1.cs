using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Schd
{
    public partial class Scheduling : Form
    {
        int count = 0;
        string[] readText;
        private bool readFile = false;
        List<Process> pList, pView;
        List<Result> resultList;


        public Scheduling()
        {
            InitializeComponent();
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            pView.Clear();
            pList.Clear();

            //파일 오픈
            string path =  SelectFilePath();
            if (path == null) return;
    
            readText = File.ReadAllLines(path);
            
            //토큰 분리
            for (int i = 0; i < readText.Length; i++)
            {
                string[] token = readText[i].Split(' ');
                Process p = new Process(int.Parse(token[1]), int.Parse(token[2]), int.Parse(token[3]), int.Parse(token[4]));
                pList.Add(p);
            }

            //Grid에 process 출력
            dataGridView1.Rows.Clear();
            string[] row = { "", "", "", "" };
            foreach (Process p in pList)
            {
                row[0] = p.processID.ToString();
                row[1] = p.arriveTime.ToString();
                row[2] = p.burstTime.ToString();
                row[3] = p.priority.ToString();
                count++;
                dataGridView1.Rows.Add(row);
            }

            //arriveTime으로 정렬
            pList.Sort(delegate(Process x, Process y)
            {
                if (x.arriveTime > y.arriveTime) return 1;
                else if (x.arriveTime < y.arriveTime) return -1;
                else
                {
                    return x.processID.CompareTo(y.processID);
                }
                //return x.arriveTime.CompareTo(y.arriveTime);
            });

            readFile = true;
        }

        private string SelectFilePath()
        {
            openFileDialog1.Filter = "텍스트파일|*.txt";
            return (openFileDialog1.ShowDialog() == DialogResult.OK) ? openFileDialog1.FileName : null;
        }

        private void Run_Click(object sender, EventArgs e)
        {
            if (!readFile) return;

            //스케쥴러 실행
            

            if (comboBox1.Text == "Priority")
                resultList = Priority.Run(pList, resultList);
            else if (comboBox1.Text == "SJF")
                resultList = SJF.Run(pList, resultList);
            else if (comboBox1.Text == "FCFS")
                resultList = FCFS.Run(pList, resultList);
            else if (comboBox1.Text == "Round Robin")
            {
                int i;
                i = int.Parse(textBox1.Text);
                resultList = RoundRobin.Run(pList, resultList, i);
            }
                   
             else
                MessageBox.Show("스케줄링 방식을 선택하세요.");


            //결과출력
            dataGridView2.Rows.Clear();
            string[] row = { "", "", "", "" };

            double watingTime = 0.0;
            foreach (Result r in resultList)
            {
                row[0] = r.processID.ToString();
                row[1] = r.burstTime.ToString();
                row[2] = r.waitingTime.ToString();
                watingTime += r.waitingTime;

                dataGridView2.Rows.Add(row);
            }

            TRTime.Text = "전체 실행시간: " + (resultList[resultList.Count - 1].startP + resultList[resultList.Count - 1].burstTime).ToString();
            avgRT.Text = "평균 대기시간: " + (watingTime / count).ToString();
            panel1.Invalidate();
        }


        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            int startPosition = 10;
            double waitingTime = 0.0;

            int resultListPosition = 0;
            foreach (Result r in resultList)
            {
                e.Graphics.DrawString("p" + r.processID.ToString(), Font, Brushes.Black, startPosition + (r.startP * 10), resultListPosition);
                e.Graphics.DrawRectangle(Pens.Red, startPosition + (r.startP * 10), resultListPosition + 20, r.burstTime * 10, 30);
                e.Graphics.DrawString(r.burstTime.ToString(), Font, Brushes.Black, startPosition + (r.startP * 10), resultListPosition + 60);
                e.Graphics.DrawString(r.waitingTime.ToString(), Font, Brushes.Black, startPosition + (r.startP * 10), resultListPosition + 80);
                waitingTime += (double)r.waitingTime;
            }
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void TRTime_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            pList = new List<Process>();
            pView = new List<Process>();
            resultList = new List<Result>();
           

            //입력창
            DataGridViewTextBoxColumn processColumn = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn arriveTimeColumn = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn burstTimeColumn = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn priorityColumn = new DataGridViewTextBoxColumn();

            processColumn.HeaderText = "프로세스";
            processColumn.Name = "process";
            arriveTimeColumn.HeaderText = "도착시간";
            arriveTimeColumn.Name = "arriveTime";
            burstTimeColumn.HeaderText = "실행시간";
            burstTimeColumn.Name = "burstTime";
            priorityColumn.HeaderText = "우선순위";
            priorityColumn.Name = "priority";

            dataGridView1.Columns.Add(processColumn);
            dataGridView1.Columns.Add(arriveTimeColumn);
            dataGridView1.Columns.Add(burstTimeColumn);
            dataGridView1.Columns.Add(priorityColumn);



            //결과창
            DataGridViewTextBoxColumn resultProcessColumn = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn resultBurstTimeColumn = new DataGridViewTextBoxColumn();
            DataGridViewTextBoxColumn resultWaitingTimeColumn = new DataGridViewTextBoxColumn();


            resultProcessColumn.HeaderText = "프로세스";
            resultProcessColumn.Name = "process";
            resultBurstTimeColumn.HeaderText = "실행시간";
            resultBurstTimeColumn.Name = "resultBurstTimeColumn";
            resultWaitingTimeColumn.HeaderText = "대기시간";
            resultWaitingTimeColumn.Name = "waitingTime";

            dataGridView2.Columns.Add(resultProcessColumn);
            dataGridView2.Columns.Add(resultBurstTimeColumn);
            dataGridView2.Columns.Add(resultWaitingTimeColumn);
        }
    }
}
