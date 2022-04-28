using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enemy
{
    internal class FriendlyNPC_Dialogue
    {
        public void Dialogue_Level1_Boss()
        {
            Console.WriteLine("Hello! I am the Beadle");
            Thread.Sleep(3500);
            Console.WriteLine("I'm here to warn you of the upcoming boss fight, it'll be a much different enemy...");
            Thread.Sleep(5000);
            Console.WriteLine("It will create division problems! To defeat it, you must input both:");
            Thread.Sleep(5000);
            Console.WriteLine("The quotient and the remainder.");
            Thread.Sleep(3500);
            Console.WriteLine("Like this!");
            Thread.Sleep(3500);
            Console.WriteLine("EXAMPLE - 10 / 3 = ?");
            Thread.Sleep(2500);
            Console.WriteLine("EXAMPLE - Input Quotient: 3");
            Thread.Sleep(2500);
            Console.WriteLine("EXAMPLE - Input Remainder: 1");
            Thread.Sleep(2500);
            Console.WriteLine("You need to get both right to damage the boss, or else you'll be counted as wrong!");
            Thread.Sleep(3500);
            Console.WriteLine("I'll be going now! Good luck young mathematician!");
            Thread.Sleep(5000);
        }
        public void Dialogue_Level2_Boss()
        {
            Console.WriteLine("So you've defeated the inhabitants of the Forest of Intermediate Math...");
            Thread.Sleep(4500);
            Console.WriteLine("Nicely done!");
            Thread.Sleep(2500);
            Console.WriteLine("However, the next boss is much harder than any you've faced yet.");
            Thread.Sleep(4500);
            Console.WriteLine("It will create algebraic problems! To defeat it, you must solve for the value of x, ");
            Thread.Sleep(4500);
            Console.WriteLine("in the equations it will give out.");
            Thread.Sleep(3500);
            Console.WriteLine("These equations can use any of the 4 operations.");
            Thread.Sleep(3500);
            Console.WriteLine("I'll give an example:");
            Thread.Sleep(3500);
            Console.WriteLine("EXAMPLE - x/3 = 18");
            Thread.Sleep(3500);
            Console.WriteLine("My solution would be to isolate x, such that ");
            Thread.Sleep(3500);
            Console.WriteLine("x = 18 * 3");
            Thread.Sleep(3500);
            Console.WriteLine("which gives me the answer, x = 54");
            Thread.Sleep(3500);
            Console.WriteLine("I'll be going now! Good luck young mathematician!");
            Thread.Sleep(5000);
        }
    }
}
