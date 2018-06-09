using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Schd
{
    class ReadyQueueElement
    {
        public int processID;
        public int burstTime;
        public int waitingTime;
        public int priority;

        public ReadyQueueElement(int processID, int burstTime, int waitingTime, int priority)
        {
            this.processID = processID;
            this.burstTime = burstTime;
            this.waitingTime = waitingTime;
            this.priority = priority;
        }
    }

    class Priority
    {
        public static List<Result> Run(List<Process> jobList, List<Result> resultList)
        {
            int currentProcess = 0;//현재 실행되고 있는 프로세스
            int cpuTime = 0;
            int cpuDone = 0;

            int runTime = 0;
            List<ReadyQueueElement> readyQueue = new List<ReadyQueueElement>();
            do
            {
                while (jobList.Count != 0)//잡 리스트  갯수가 0이 아닐 때
                {
                    Process frontJob = jobList.ElementAt(0); //프런트잡에 잡리스트의 0번째 요소 넣기
               
                    if (frontJob.arriveTime == runTime)//프런트잡의 도착시간이랑 실행시간이랑 같으면
                    {
                        readyQueue.Add(new ReadyQueueElement(frontJob.processID, frontJob.burstTime, 0, frontJob.priority));//레디큐에 PID랑 Burst Time 갖다박기
                        jobList.RemoveAt(0);//잡리스트 0번째 삭제
                    }
                    else//프런트잡의 도착시간이랑 실행시간이랑 다르면
                    {
                        break;
                    }
                }
                int min;
                ReadyQueueElement temp;
                if (currentProcess == 0)//현재 실행되고 있는 프로세스가 없다면
                {
                    if (readyQueue.Count != 0)//이때 레디큐가 비어있지 않으면
                    {
                            for (int i = 0; i < readyQueue.Count-1; i++)//우선순위가 높은 순서대로 레디큐의 프로세스들을 소팅
                            {
                                min = i;
                                for(int j = i+1; j < readyQueue.Count; j++)
                                {
                                    if (readyQueue.ElementAt(j).priority < readyQueue.ElementAt(min).priority)
                                        min = j;
                                }
                                temp = readyQueue[i];
                                readyQueue[i] = readyQueue[min];
                                readyQueue[min] = temp;
                                
                            }
                        ReadyQueueElement rq = readyQueue.ElementAt(0);//rq에 레디큐 0번째 요소 넣기
                        resultList.Add(new Result(rq.processID, runTime, rq.burstTime, rq.waitingTime));//결과리스트에 rq의 PID, Run Time, Burst Time, Wating Time 넣기
                        cpuDone = rq.burstTime;//cpuDone에 rq의 Burst Time 넣기
                        cpuTime = 0;//CPU 시간 = 0
                        currentProcess = rq.processID;//현재 실행되고 있는 프로세스 = rq의 PID
                        readyQueue.RemoveAt(0);//레디큐의 0번째 삭제
                    }
                }
                else//현재 실행되고 있는 프로세스가 있다면
                {
                    if (cpuTime == cpuDone)//cpu시간과 == cpuDone 같으면
                    {
                        currentProcess = 0;//현재 실행되고 있는 프로세스 없도록 만들어줌
                        continue;
                    }
                }

                cpuTime++;//cpu시간 1증가
                runTime++;//런타임 1증가

                for (int i = 0; i < readyQueue.Count; i++)//레디큐 남은 갯수만큼
                {
                    readyQueue.ElementAt(i).waitingTime++;//레디큐에 있는 모든 프로세스들 waiting Time 1씩 증가
                }

            } while (jobList.Count != 0 || readyQueue.Count != 0 || currentProcess != 0);//잡리스트나 레디큐나 current프로세스의 갯수가 0이 되면 탈출

            return resultList;
        }
    }
    class SJF
    {
        public static List<Result> Run(List<Process> jobList, List<Result> resultList)
        {
            int currentProcess = 0;//현재 실행되고 있는 프로세스
            int cpuTime = 0;
            int cpuDone = 0;

            int runTime = 0;
            List<ReadyQueueElement> readyQueue = new List<ReadyQueueElement>();
            do
            {
                while (jobList.Count != 0)//잡 리스트  갯수가 0이 아닐 때
                {
                    Process frontJob = jobList.ElementAt(0); //프런트잡에 잡리스트의 0번째 요소 넣기

                    if (frontJob.arriveTime == runTime)//프런트잡의 도착시간이랑 실행시간이랑 같으면
                    {
                        readyQueue.Add(new ReadyQueueElement(frontJob.processID, frontJob.burstTime, 0, frontJob.priority));//레디큐에 PID랑 Burst Time 갖다박기
                        jobList.RemoveAt(0);//잡리스트 0번째 삭제
                    }
                    else//프런트잡의 도착시간이랑 실행시간이랑 다르면
                    {
                        break;
                    }
                }
                int min;
                ReadyQueueElement temp;
                if (currentProcess == 0)//현재 실행되고 있는 프로세스가 없다면
                {
                    if (readyQueue.Count != 0)//이때 레디큐가 비어있지 않으면
                    {
                            for (int i = 0; i < readyQueue.Count - 1; i++)//burstTime이 작은 순서대로 레디큐에 들어와있는 값들 소팅
                            {
                                min = i;
                                for (int j = i + 1; j < readyQueue.Count; j++)
                                {
                                    if (readyQueue.ElementAt(j).burstTime < readyQueue.ElementAt(min).burstTime)
                                        min = j;
                                }
                                temp = readyQueue[i];
                                readyQueue[i] = readyQueue[min];
                                readyQueue[min] = temp;

                            }
                        ReadyQueueElement rq = readyQueue.ElementAt(0);//rq에 레디큐 0번째 요소 넣기
                        resultList.Add(new Result(rq.processID, runTime, rq.burstTime, rq.waitingTime));//결과리스트에 rq의 PID, Run Time, Burst Time, Wating Time 넣기
                        cpuDone = rq.burstTime;//cpuDone에 rq의 Burst Time 넣기
                        cpuTime = 0;//CPU 시간 = 0
                        currentProcess = rq.processID;//현재 실행되고 있는 프로세스 = rq의 PID
                        readyQueue.RemoveAt(0);//레디큐의 0번째 삭제
                    }
                }
                else//현재 실행되고 있는 프로세스가 있다면
                {
                    if (cpuTime == cpuDone)//cpu시간과 == cpuDone 같으면
                    {
                        currentProcess = 0;//현재 실행되고 있는 프로세스 없도록 만들어줌
                        continue;
                    }
                }

                cpuTime++;//cpu시간 1증가
                runTime++;//런타임 1증가

                for (int i = 0; i < readyQueue.Count; i++)//레디큐 남은 갯수만큼
                {
                    readyQueue.ElementAt(i).waitingTime++;//레디큐에 있는 모든 프로세스들 waiting Time 1씩 증가
                }

            } while (jobList.Count != 0 || readyQueue.Count != 0 || currentProcess != 0);//잡리스트나 레디큐나 current프로세스의 갯수가 0이 되면 탈출

            return resultList;
        }
    }
    class FCFS
    {
        public static List<Result> Run(List<Process> jobList, List<Result> resultList)
        {
            int currentProcess = 0;
            int cpuTime = 0;
            int cpuDone = 0;

            int runTime = 0;

            List<ReadyQueueElement> readyQueue = new List<ReadyQueueElement>();
            do
            {
                while (jobList.Count != 0)
                {
                    Process frontJob = jobList.ElementAt(0);
                    if (frontJob.arriveTime == runTime)
                    {
                        readyQueue.Add(new ReadyQueueElement(frontJob.processID, frontJob.burstTime, 0, frontJob.priority));
                        jobList.RemoveAt(0);
                    }
                    else
                    {
                        break;
                    }
                }

                if (currentProcess == 0)
                {
                    if (readyQueue.Count != 0)
                    {
                        ReadyQueueElement rq = readyQueue.ElementAt(0);
                        resultList.Add(new Result(rq.processID, runTime, rq.burstTime, rq.waitingTime));
                        cpuDone = rq.burstTime;
                        cpuTime = 0;
                        currentProcess = rq.processID;
                        readyQueue.RemoveAt(0);

                    }
                }
                else
                {
                    if (cpuTime == cpuDone)
                    {
                        currentProcess = 0;
                        continue;
                    }
                }

                cpuTime++;
                runTime++;

                for (int i = 0; i < readyQueue.Count; i++)
                {
                    readyQueue.ElementAt(i).waitingTime++;
                }

            } while (jobList.Count != 0 || readyQueue.Count != 0 || currentProcess != 0);

            return resultList;
        }
    }
    class RoundRobin
    {
        public static List<Result> Run(List<Process> jobList, List<Result> resultList, int timequantum)
        {
            int currentProcess = 0;
            int cpuTime = 0;
            int cpuDone = 0;

            int runTime = 0;

            int bigger_than_quantum = 0;//burstTime이 timeQuantum보다 큰지 확인하기 위한 카운팅 변수
            ReadyQueueElement temp = null;//ReadyQueueElement 형을 임시로 저장할 곳 선언

            List<ReadyQueueElement> readyQueue = new List<ReadyQueueElement>();
            do
            {
                while (jobList.Count != 0)
                {
                    Process frontJob = jobList.ElementAt(0);
                    if (frontJob.arriveTime == runTime)
                    {
                        readyQueue.Add(new ReadyQueueElement(frontJob.processID, frontJob.burstTime, 0, frontJob.priority));
                        jobList.RemoveAt(0);
                    }
                    else
                    {
                        break;
                    }
                }
                
                if (currentProcess == 0)
                {
                    if (readyQueue.Count != 0)
                    {
                        ReadyQueueElement rq = readyQueue.ElementAt(0);
                        cpuTime = 0;
                        currentProcess = rq.processID;
                        if (rq.burstTime > timequantum)//burstTime보다 timeQuantum보다 클 때
                        {
                            bigger_than_quantum++;//burstTime이 timeQuantum보다 클 때 카운트
                            resultList.Add(new Result(rq.processID, runTime, timequantum, rq.waitingTime));//리스트에 넣을때 burstTime을 timeQuantum으로 추가
                            cpuDone = timequantum;//끝나는 시간을 timequantum으로 설정
                            rq.burstTime = rq.burstTime - timequantum;//burstTime에서 timequantum만큼 빼줌
                            temp = rq;//프로세스가 끝날 때 남은 부분을 레디큐에 넣어주기위해 임시로 저장
                        }
                        else//burstTime보다 timeQuantum보다 작거나 같을 때
                        {
                            resultList.Add(new Result(rq.processID, runTime, rq.burstTime, rq.waitingTime));
                            cpuDone = rq.burstTime;//끝나는시간은 burstTime
                        }
                        readyQueue.RemoveAt(0);
                    }
                }
                else
                {
                    if (cpuTime == cpuDone)
                    {
                        currentProcess = 0;
                        if(bigger_than_quantum > 0)//burstTime이 timeQuantum보다 크다고 카운트 되었을 때
                        {
                            readyQueue.Add(new ReadyQueueElement(temp.processID, temp.burstTime, 0, temp.priority));//임시로 저장해 둔 남은 프로세스부분을 레디큐에 넣어주기
                            bigger_than_quantum = 0;//카운팅 변수 초기화
                        }
                        continue;
                    }
                }

                cpuTime++;
                runTime++;

                for (int i = 0; i < readyQueue.Count; i++)
                {
                    readyQueue.ElementAt(i).waitingTime++;
                }

            } while (jobList.Count != 0 || readyQueue.Count != 0 || currentProcess != 0);

            return resultList;
        }
    }
}