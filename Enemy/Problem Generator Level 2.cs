using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enemy
{
    internal class Problem_Generator_Level2
    {
        // This is where we put the ASCII art of the enemies, and the UI of the program.
        int a;
        int b;
        int c;
        double correct;
        double quotient;

        // Orc (add and subtract) Enemy Problem
        public void OrcProblem()
        {
            Random rand = new Random();
            a = rand.Next(10, 51);
            b = rand.Next(1, 51);
            c = rand.Next(1, 51);
            correct = a + b - c;
            Console.WriteLine("Problem: " + a + " + " + b + " - " + c + " = ?");
        }

        // Slime (multiply and add) Enemy Problem
        public void SlimeProblem()
        {
            Random rand = new Random();
            a = rand.Next(10, 51);
            b = rand.Next(1, 11); // only 1 to 10 to make it easier 
            c = rand.Next(1, 51);
            correct = a * b + c;
            Console.WriteLine("Problem: " + a + " * " + b + " + " + c + " = ?");
        }
        // Ghost (multiply and subtract) Enemy Problem
        public void GhostProblem()
        {
            Random rand = new Random();
            a = rand.Next(1, 51);
            b = rand.Next(10, 21);
            c = rand.Next(1, 11); // only 1 to 10 to make it easier 
            correct = a - b * c;
            Console.WriteLine("Problem: " + a + " - " + b + " * " + c + " = ?");
        }

        // Goblin (divide only) Enemy Problem
        public void GoblinProblem()
        {
            Random rand = new Random();
            a = rand.Next(10, 31);
            b = rand.Next(1, 21);
            quotient = (double) a / b;
            correct = Math.Round(quotient, 2);
            Console.WriteLine("Problem: " + a + " / " + b + " = ?");
        }

        // Knight X (Algebra) Boss:
        public void Equation1()
        {
            Random rand = new Random();
            a = rand.Next(1, 6);
            b = rand.Next(1, 21);
            c = rand.Next(1, 21);
            quotient = (double) (b + c) / a;
            correct = Math.Round(quotient, 2);
            Console.WriteLine("Problem: " + a + "x = " + b + " + " + c);
        }
        public void Equation2()
        {
            Random rand = new Random();
            a = rand.Next(2, 4);
            b = rand.Next(1, 21);
            correct = b * a;
            Console.WriteLine("Problem: x/" + a + " = " + b);
        }
        public void Equation3()
        {
            Random rand = new Random();
            a = rand.Next(1, 11);
            b = rand.Next(1, 11);
            c = rand.Next(1, 21);
            correct = c - a + b;
            Console.WriteLine("Problem: x + " + a + " - " + b + " = " + c);
        }
        public void Equation4()
        {
            Random rand = new Random();
            a = rand.Next(50, 101);
            b = rand.Next(7, 51);
            correct = a - b;
            Console.WriteLine("Problem: x = " + a + " - " + b);
        }
        // return var ENEMY
        public double correct_answer()
        {
            return correct;
        }

    }
}
