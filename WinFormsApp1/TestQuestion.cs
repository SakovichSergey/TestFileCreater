using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsApp1
{
    class TestQuestion
    {
        public String question;
        public (String, bool)[] answers;
        public TestQuestion(String questStr, (String, bool)[] ansStr)
        {
            question = questStr;
            answers = ansStr;
        }
        public String retStrTask ()
        {
            String retStr = question + "\n";
                for (int i = 0; i < answers.Length; i++)
                retStr += retStrAnswer(i);
            return retStr;
        }
        public String retStrTaskWhithoutAnswers()
        {
            String retStr = question + "\n";
            for (int i = 0; i < answers.Length; i++)
                retStr += retStrWhithoutAnswer(i);
            return retStr;
        }
        public String retStrAnswer (int numberAnswer)
        {
            if (numberAnswer < answers.Length)
            {
                int val = numberAnswer+1;
                return val.ToString() +":"+answers[numberAnswer].Item1 + "\u00FF" + answers[numberAnswer].Item2.ToString() + "\n";
            }
            else
            {
                return "Нет варианта ответа!";
            }
        }
        public String retStrWhithoutAnswer(int numberAnswer)
        {
            if (numberAnswer < answers.Length)
            {
                int val = numberAnswer + 1;
                return val.ToString() + ":" + answers[numberAnswer].Item1 + "\n";
            }
            else
            {
                return "Нет варианта ответа!";
            }
        }
        public void  editAnswersInTask(int numberAnswer, String answerVariant, bool Correctness)
        {
            if(numberAnswer < answers.Length)
            {
                answers[numberAnswer] = (answerVariant, Correctness);
            }
            else
            {
                (String, bool)[] ansStr = new (String, bool)[answers.Length + 1];
                for (int i = 0; i < answers.Length; i++)
                    ansStr[i] = answers[i];
                answers = ansStr;
                editAnswersInTask(numberAnswer, answerVariant, Correctness); 
            }
        }
    }
}
