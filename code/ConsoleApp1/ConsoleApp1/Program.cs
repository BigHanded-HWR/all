using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            StringToMath stm = new StringToMath();
            string t = "1";
            int x = 0;
            // Console.WriteLine(t[1]);
            try
            {
                x = stm.ShowMath(t);
                Console.WriteLine("{0}={1}",t,x);
                Console.ReadLine();
            }
            catch (StringIsEmptyException)
            {
                Console.WriteLine("error: input is empty!");
                Console.ReadLine();
            }
            
        }

    }
}

public class StringIsEmptyException : ApplicationException
{
    public StringIsEmptyException(string message) : base(message)
    {
    }
}
public class StringToMath
{
    public int ShowMath(string s)
    {
        if (s.Length > 0)
        {
            int num = 0;
            int oldnum = 0;
            char math = '+';
            int i = 0,l=s.Length;

            for (i = 0; i < s.Length; i++)
            {
                if (s[i] >= 48 && s[i] <= 57)
                {
                    num = num * 10 + (s[i]-48);
                   // Console.WriteLine(num);
                    if (i == s.Length - 1)
                    {
                        //Console.WriteLine("**********");
                        switch (math)
                        {
                            case '+': oldnum = oldnum + num; break;
                            case '-': oldnum = oldnum - num; break;
                            case '*': oldnum = oldnum * num; break;
                            case '/': oldnum = oldnum / num; break;
                        }

                    }
                }
                else
                {
                    switch (math)
                    {
                        case '+': oldnum = oldnum + num; break;
                        case '-': oldnum = oldnum - num; break;
                        case '*': oldnum = oldnum * num; break;
                        case '/': oldnum = oldnum / num; break;
                    }
                    math = s[i];
                    num = 0;
                }
            }
            return oldnum;

        }
        else
        {
            throw (new StringIsEmptyException("Empty input found"));
        }
    }
}


