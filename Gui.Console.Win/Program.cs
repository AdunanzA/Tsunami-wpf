using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerArgs;
using System.IO;
using Tsunami;

namespace System
{
    [ArgExceptionBehavior(ArgExceptionPolicy.StandardExceptionHandling)]
    public class TsunamiConsoleWin
    {
        [HelpHook, ArgShortcut("-?"), ArgDescription("Shows this help")]
        public bool Help { get; set; }

        [ArgActionMethod, ArgDescription("Adds the two operands")]
        public void List(TwoOperandArgs args)
        {
            Console.WriteLine(args.Value1 + args.Value2);
        }

        [ArgActionMethod, ArgDescription("Subtracts the two operands")]
        public void Down(TwoOperandArgs args)
        {
            Console.WriteLine(args.Value1 - args.Value2);
        }

        [ArgActionMethod, ArgDescription("Multiplies the two operands")]
        public void Add(TwoOperandArgs args)
        {
            Console.WriteLine(args.Value1 * args.Value2);
        }

        [ArgActionMethod, ArgDescription("Divides the two operands")]
        public void Remove(TwoOperandArgs args)
        {
            Console.WriteLine(args.Value1 / args.Value2);
        }
    }

    public class TwoOperandArgs
    {
        [ArgRequired, ArgDescription("The first operand to process"), ArgPosition(1)]
        public double Value1 { get; set; }
        [ArgRequired, ArgDescription("The second operand to process"), ArgPosition(2)]
        public double Value2 { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine(@"
████████╗███████╗██╗   ██╗███╗   ██╗ █████╗ ███╗   ███╗██╗ 
╚══██╔══╝██╔════╝██║   ██║████╗  ██║██╔══██╗████╗ ████║██║
   ██║   ███████╗██║   ██║██╔██╗ ██║███████║██╔████╔██║██║
   ██║   ╚════██║██║   ██║██║╚██╗██║██╔══██║██║╚██╔╝██║██║ 
   ██║   ███████║╚██████╔╝██║ ╚████║██║  ██║██║ ╚═╝ ██║██║     
   ╚═╝   ╚══════╝ ╚═════╝ ╚═╝  ╚═══╝╚═╝  ╚═╝╚═╝     ╚═╝╚═╝
                 ██████╗ ██████╗ ██████╗               
                 ██╔══██╗╚════██╗██╔══██╗
                 ██████╔╝ █████╔╝██████╔╝
                 ██╔═══╝ ██╔═══╝ ██╔═══╝
                 ██║     ███████╗██║
                 ╚═╝     ╚══════╝╚═╝                                    
");
            if (Console.BackgroundColor == ConsoleColor.Black)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine("################################################################");
                Console.ForegroundColor = ConsoleColor.Yellow;

                Console.WriteLine("*** Starting Webserver on " + Tsunami.Settings.User.WebAddress
                + " port: " +  Tsunami.Settings.User.WebPort + " ***");
                if (args != null)
                {
                }
                else Args.InvokeAction<TsunamiConsoleWin>(args);



                // Stating webserver

                Tsunami.SessionManager.Initialize();
                // Leave the console window open
                Console.ReadKey();
            }
        }
    }
}
