using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enemy
{
    internal class Problem_Generator
    {
        // This is where we put the ASCII art of the enemies, and the UI of the program.
        int a;
        int b;
        int dividend;
        int divisor;
        int correct;
        int remainder;
        // Addition Enemy Problem
        public void AddProblem()
        {
            Random rand = new Random();
            a = rand.Next(1, 21);
            b = rand.Next(1, 21);
            correct = a + b;
            Console.WriteLine("Problem: " + a + " + " + b + " = ?");
        }

        // Subtraction Enemy Problem
        public void SubtractProblem()
        {
            Random rand = new Random();
            a = rand.Next(11, 21);
            b = rand.Next(1, 11);
            correct = a - b;
            Console.WriteLine("Problem: " + a + " - " + b + " = ?");
        }
        // Multiplication Enemy Problem
        public void MultiplyProblem()
        {
            Random rand = new Random();
            a = rand.Next(1, 21);
            b = rand.Next(1, 21);
            correct = a * b;
            Console.WriteLine("Problem: " + a + " * " + b + " = ?");
        }

        // Divider Boss:
        public void DivideProblemLevel1()
        {
            Random rand = new Random();
            dividend = rand.Next(10, 21);
            divisor = rand.Next(1, 11);
            correct = dividend / divisor;
            remainder = dividend % divisor;
            Console.WriteLine("Problem: " + dividend + " / " + divisor + " = ?");
        }
        // return var ENEMY
        public int correct_answer()
        {
            return correct;
        }
        // return var BOSS
        public int remainder_answer()
        {
            return remainder;
        }
    }
}
