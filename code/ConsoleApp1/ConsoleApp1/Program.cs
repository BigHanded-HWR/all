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
            string t = "8+8+8*3*6+7-1";
            int x = 0;
            // Console.WriteLine(t[1]);
            try
            {
                x = stm.Button2_Click(t);
                Console.WriteLine("{0}={1}", t, x);
                Console.ReadLine();

            }
            catch (StringIsEmptyException)
            {
                Console.WriteLine("error: input is empty!");
                Console.ReadLine();
            }
            catch (DivideByZeroException)
            {
                Console.WriteLine("error: Divide By Zero !");
                Console.ReadLine();
            }

            /*try
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
            catch (DivideByZeroException)
            {
                Console.WriteLine("error: Divide By Zero !");
                Console.ReadLine();
            }*/



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
    public int Button2_Click(string t)
    {
#pragma warning disable IDE0017 // Simplify object initialization
        if(t.Length>0)
        {  MSScriptControl.ScriptControl sc = new MSScriptControl.ScriptControlClass();
#pragma warning restore IDE0017 // Simplify object initialization
        sc.Language = "JavaScript";
        var x=sc.Eval(t);
        if (x == sc.Eval("5/0")) 
            throw (new DivideByZeroException("Divide by zero found")); 
        else
         {
                return (int)x;
         }
        }
        else
        {
            throw (new StringIsEmptyException("Empty input found"));
        }
    }
    /*public int ShowMath(string s)
    {
        if (s.Length > 0)
        {
            int num = 0;
            int oldnum = 0;
            int lastnum = 0;
            int eldnum = 0;
            char math = '+';
            char lmath = '+';
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
                            case '/':try
                                    {
                                       oldnum = oldnum / num;
                                       break;
                                     }
                                     catch (DivideByZeroException)
                                     {
                                      throw (new DivideByZeroException("Divide by zero found"));    
                                     }
                                      
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
    }*/
}


