using System;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace Enemy
{
    class MainClass
    {
        // Very unfamiliar with what any of these do, (copy pasted from: https://www.meziantou.net/cancelling-console-read.htm )
        // BUT it's required to cancel the input once 20 seconds is up (player ran out of time)
        const int STD_INPUT_HANDLE = -10;

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CancelIoEx(IntPtr handle, IntPtr lpOverlapped);

        public static void Main(string[] args)
        {
            // Extra Coding Features, stopwatch comes from System.Diagnostics
            Random rand = new Random(); // initiate RNG
            Stopwatch stopwatch = new Stopwatch(); // initiate TIME

            var Game = true;
            while (Game == true)
            {
                // Variables (for all levels)
                int Player_HP = 5;
                int Player_Max_HP = 5;
                int Player_Damage = 3;
                int Damage_Inflicted = 0;
                int Enemy_HP = 999; // just to make it not null
                int Enemy_Max_HP = 999;
                string Enemy_Name = "";

                // Level select:
                Console.Clear();
                Console.WriteLine("Select a level: [1] or [2] (input the number without the [] brackets)");
                string LevelSelect = Console.ReadLine();

                // Level 1 <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                if (LevelSelect == "1")
                {
                    // Process
                    for (int i = 1; i < 6; i++) // Enemies 1 to 5
                    {
                        // Show current enemy
                        Console.Clear();
                        Console.WriteLine("Enemy " + i + " out of 5");
                        Thread.Sleep(1000);

                        // Select enemy
                        int Enemy_select = rand.Next(1, 4);

                        if (Enemy_select == 1) // Addition enemy
                        {
                            Enemy_HP = 20;
                            Enemy_Max_HP = 20;
                            Enemy_Name = "Addition";
                        }
                        else if (Enemy_select == 2) // Subtraction enemy
                        {
                            Enemy_HP = 15;
                            Enemy_Max_HP = 15;
                            Enemy_Name = "Subtraction";
                        }
                        else if (Enemy_select == 3) // Multiplication enemy
                        {
                            Enemy_HP = 12;
                            Enemy_Max_HP = 12;
                            Enemy_Name = "Multiplication";
                        }

                        // Combat
                        while (Player_HP > 0 && Enemy_HP > 0) // while both player and enemy are alive
                        {
                            // Clear
                            Console.Clear();

                            // Game Over initiated 1
                            if (Player_HP == 0)
                            {
                                break; // escape while loop
                            }

                            // UI
                            Console.WriteLine("Your HP: " + Player_HP + "/" + Player_Max_HP);
                            Console.WriteLine(Enemy_Name + " Enemy HP: " + Enemy_HP + "/" + Enemy_Max_HP);

                            // Create Problem (call the Problem Generator.cs)
                            Problem_Generator a = new Problem_Generator();
                            if (Enemy_select == 1) // Addition enemy
                            {
                                a.AddProblem();
                            }
                            else if (Enemy_select == 2) // Subtraction enemy
                            {
                                a.SubtractProblem();
                            }
                            else if (Enemy_select == 3) // Multiplication enemy
                            {
                                a.MultiplyProblem();
                            }

                            // INPUT PROCESS SECTION START --------------------------------------------------------------
                            // Start the timeout
                            var read = false;

                            // modify    V this value to change delay (in milliseconds)
                            Task.Delay(20000).ContinueWith(_ =>
                            {
                                if (!read)
                                {
                                    // Timeout => cancel the console read
                                    var handle = GetStdHandle(STD_INPUT_HANDLE);
                                    CancelIoEx(handle, IntPtr.Zero);
                                }
                            });

                            try
                            {
                                // Start reading from the console
                                Console.Write("\nInput Answer: ");
                                stopwatch.Start(); // Time start

                                var answer = Console.ReadLine(); // input answer
                                read = true; // checks if an input was given on time

                                stopwatch.Stop(); // Time end
                                                  // NOTE: variable for time is "stopwatch.ElapsedMilliseconds"
                                int time = (int)stopwatch.ElapsedMilliseconds; // extra info: it's also a "long" data type, 
                                                                               // it can be turned into "int" using (int)
                                stopwatch.Reset(); // time reset to 0

                                // checks if input is a number (copy pasted from https://stackoverflow.com/questions/46246472/a-local-or-parameter-named-e-cannot-be-declared-in-this-scope)
                                int value;
                                if (int.TryParse(answer, out value))
                                {
                                    // is number correct?
                                    if (int.Parse(answer) == a.correct_answer())
                                    {
                                        if (time <= 5000) // <5 sec, critical hit!
                                        {
                                            Enemy_HP -= 2;
                                            Enemy_HP -= Player_Damage;
                                            Damage_Inflicted = Player_Damage + 2;
                                            Console.WriteLine("Correct! Enemy lost " + Damage_Inflicted + " HP");
                                            Console.WriteLine("Answered in " + time / 1000 + " seconds");
                                            Console.WriteLine("Quick answer!");
                                        }
                                        else if (time <= 10000) // <10 sec, average hit.
                                        {
                                            Enemy_HP -= 1;
                                            Enemy_HP -= Player_Damage;
                                            Damage_Inflicted = Player_Damage + 1;
                                            Console.WriteLine("Correct! Enemy lost " + Damage_Inflicted + " HP");
                                            Console.WriteLine("Answered in " + time / 1000 + " seconds");
                                            Console.WriteLine("Solved nicely.");
                                        }
                                        else if (time > 10000) // >10 sec, lowered hit.
                                        {
                                            Enemy_HP -= Player_Damage;
                                            Damage_Inflicted = Player_Damage;
                                            Console.WriteLine("Correct! Enemy lost " + Damage_Inflicted + " HP");
                                            Console.WriteLine("Answered in " + time / 1000 + " seconds");
                                            Console.WriteLine("Took a little too long...");
                                        }
                                    }
                                    else if (int.Parse(answer) != a.correct_answer()) // wrong answer
                                    {
                                        // Player lose HP
                                        Player_HP -= 1;
                                        Console.WriteLine("Wrong answer! You lost 1 HP");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Input is not a number! You lost 1 HP");
                                    Player_HP -= 1;
                                }
                            }
                            // Handle the exception when the operation is canceled
                            // Player ran out of time, lose 1 HP and make new problem
                            catch (InvalidOperationException)
                            {
                                Player_HP -= 1;
                                Console.WriteLine("\nYou've run out of time! You lost 1 HP");
                                stopwatch.Stop();
                                stopwatch.Reset();

                            }
                            catch (OperationCanceledException)
                            {
                                Player_HP -= 1;
                                Console.WriteLine("\nYou've run out of time! You lost 1 HP");
                                stopwatch.Stop();
                                stopwatch.Reset();
                            }
                            // INPUT PROCESS SECTION END ------------------------------------------------------

                            Thread.Sleep(1500); // wait 1.5 seconds
                        }
                        // ^^^ turn into a function

                        // Game Over initiated 2
                        if (Player_HP == 0)
                        {
                            break; // escape for loop
                        }
                    }

                    // Game Over screen
                    if (Player_HP == 0)
                    {
                        Console.WriteLine("GAME OVER");
                        Console.WriteLine(Player_HP + "/5");
                        Thread.Sleep(1500);
                    }
                    else
                    {
                        // Preparing for Boss Fight <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                        Console.Clear();
                        Console.WriteLine("5 enemies defeated, prepare for boss battle!");
                        Thread.Sleep(2250);
                        Console.Clear();

                        Console.WriteLine("WAIT!!!");
                        // Friendly NPC comes in:
                        FriendlyNPC_Dialogue level1 = new FriendlyNPC_Dialogue();
                        level1.Dialogue_Level1_Boss();

                        // Player HP heal before boss
                        if (Player_HP <= 2)
                        {
                            Player_HP += 3;
                        }
                        else if (Player_HP >= 3)
                        {
                            Player_HP = 5;
                        }
                        Console.WriteLine("Before he left, he gave you a Health Potion...");
                        Thread.Sleep(2250);
                        Console.WriteLine("Player HP healed by 3 HP!");
                        Thread.Sleep(2250);
                        Console.Clear();
                        Console.WriteLine("THE DIVIDER HAS ARRIVED");
                        Thread.Sleep(2250);

                        // Boss stats:
                        Enemy_HP = 30;
                        Enemy_Max_HP = 30;
                        Enemy_Name = "The Divider";

                        // COMBAT
                        while (Player_HP > 0 && Enemy_HP > 0) // while both player and BOSS are alive
                        {
                            // Clear
                            Console.Clear();

                            // Game Over initiated 1
                            if (Player_HP == 0)
                            {
                                break; // escape while loop
                            }

                            // UI
                            Console.WriteLine("Your HP: " + Player_HP + "/" + Player_Max_HP);
                            Console.WriteLine(Enemy_Name + " | BOSS HP: " + Enemy_HP + "/" + Enemy_Max_HP);

                            // Create Problem (call the Problem Generator.cs)
                            Problem_Generator a = new Problem_Generator();
                            a.DivideProblemLevel1();

                            // INPUT PROCESS SECTION START --------------------------------------------------------------
                            // Start the timeout
                            var read = false;

                            // modify    V this value to change delay (in milliseconds)
                            Task.Delay(20000).ContinueWith(_ =>
                            {
                                if (!read)
                                {
                                    // Timeout => cancel the console read
                                    var handle = GetStdHandle(STD_INPUT_HANDLE);
                                    CancelIoEx(handle, IntPtr.Zero);
                                }
                            });

                            // this part has 2 "try" to accout for the two ReadLines for Quotient and Remainder
                            try
                            {
                                // QUOTIENT PART
                                Console.Write("\nInput Quotient: ");
                                stopwatch.Start(); // Time start

                                var answer = Console.ReadLine(); // input answer
                                read = true; // checks if an input was given on time

                                try
                                {
                                    // REMAINDER PART
                                    Console.Write("\nInput Remainder: ");

                                    var remainder = Console.ReadLine(); // input answer
                                    read = true; // checks if an input was given on time

                                    stopwatch.Stop(); // Time end
                                    int time = (int)stopwatch.ElapsedMilliseconds;
                                    stopwatch.Reset(); // time reset to 0

                                    int value;
                                    if (int.TryParse(remainder, out value))
                                    {
                                        // is number correct?
                                        if (int.Parse(answer) == a.correct_answer() && int.Parse(remainder) == a.remainder_answer())
                                        {
                                            if (time <= 5000) // <5 sec, critical hit!
                                            {
                                                Enemy_HP -= 2;
                                                Enemy_HP -= Player_Damage;
                                                Damage_Inflicted = Player_Damage + 2;
                                                Console.WriteLine("Correct! Enemy lost " + Damage_Inflicted + " HP");
                                                Console.WriteLine("Answered in " + time / 1000 + " seconds");
                                                Console.WriteLine("Quick answer!");
                                            }
                                            else if (time <= 10000) // <10 sec, average hit.
                                            {
                                                Enemy_HP -= 1;
                                                Enemy_HP -= Player_Damage;
                                                Damage_Inflicted = Player_Damage + 1;
                                                Console.WriteLine("Correct! Enemy lost " + Damage_Inflicted + " HP");
                                                Console.WriteLine("Answered in " + time / 1000 + " seconds");
                                                Console.WriteLine("Solved nicely.");
                                            }
                                            else if (time > 10000) // >10 sec, lowered hit.
                                            {
                                                Enemy_HP -= Player_Damage;
                                                Damage_Inflicted = Player_Damage;
                                                Console.WriteLine("Correct! Enemy lost " + Damage_Inflicted + " HP");
                                                Console.WriteLine("Answered in " + time / 1000 + " seconds");
                                                Console.WriteLine("Took a little too long...");
                                            }
                                        }
                                        else if (int.Parse(answer) != a.correct_answer() || int.Parse(remainder) != a.remainder_answer()) // wrong answer
                                        {
                                            // Player lose HP
                                            Player_HP -= 1;
                                            Console.WriteLine("Wrong answer! You lost 1 HP");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Input is not a number! You lost 1 HP");
                                        Player_HP -= 1;
                                    }
                                }
                                // Handle the exception when the operation is canceled
                                // Player ran out of time, lose 1 HP and make new problem
                                catch (InvalidOperationException)
                                {
                                    Player_HP -= 1;
                                    Console.WriteLine("\nYou've run out of time! You lost 1 HP");
                                    stopwatch.Stop();
                                    stopwatch.Reset();

                                }
                                catch (OperationCanceledException)
                                {
                                    Player_HP -= 1;
                                    Console.WriteLine("\nYou've run out of time! You lost 1 HP");
                                    stopwatch.Stop();
                                    stopwatch.Reset();
                                }
                            }
                            // Handle the exception when the operation is canceled
                            // Player ran out of time, lose 1 HP and make new problem
                            catch (InvalidOperationException)
                            {
                                Player_HP -= 1;
                                Console.WriteLine("\nYou've run out of time! You lost 1 HP");
                                stopwatch.Stop();
                                stopwatch.Reset();

                            }
                            catch (OperationCanceledException)
                            {
                                Player_HP -= 1;
                                Console.WriteLine("\nYou've run out of time! You lost 1 HP");
                                stopwatch.Stop();
                                stopwatch.Reset();
                            }
                            // INPUT PROCESS SECTION END ------------------------------------------------------

                            Thread.Sleep(1500); // wait 1.5 seconds
                        }
                    }

                    // Game Over screen
                    if (Player_HP == 0)
                    {
                        Console.WriteLine("GAME OVER");
                        Console.WriteLine(Player_HP + "/5");
                        Thread.Sleep(1500);
                    }
                    else
                    {
                        Console.WriteLine("\nTHE DIVIDER HAS BEEN DEFEATED!");
                        Thread.Sleep(1500);

                        // Return to Level Selection 
                        Console.Clear();
                        Console.WriteLine("Return to Level selection? YES/NO (any other input counts as NO)");
                        string choice = Console.ReadLine().ToUpper();
                        if (choice == "YES")
                        {
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("Exiting game.");
                            Thread.Sleep(2500);
                            break;
                        }
                    }
                }

                // Level 2 <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                else if (LevelSelect == "2")
                {
                    // Upgrade:
                    Console.WriteLine("Choose your upgrade!");
                    Thread.Sleep(1000);
                    Console.WriteLine("\n[1] = Player Max HP increase by 3");
                    Console.WriteLine("[2] = Player base damage increase by 1");
                    Console.Write("\nSelect Upgrade: ");
                    string upgrade = Console.ReadLine();
                    if (upgrade == "1")
                    {
                        Player_HP += 3;
                        Player_Max_HP += 3;
                        Console.WriteLine("Player HP increased.");

                    }
                    else if (upgrade == "2")
                    {
                        Player_Damage += 1;
                        Console.WriteLine("Player damage increased.");
                    }
                    Thread.Sleep(1500);

                    // Process
                    for (int i = 1; i < 7; i++) // Enemies 1 to 6
                    {
                        // Show current enemy
                        Console.Clear();
                        Console.WriteLine("Enemy " + i);
                        Thread.Sleep(1000);

                        // Select enemy
                        int Enemy_select = rand.Next(1, 5);

                        if (Enemy_select == 1) // Orc (add and subtract) enemy
                        {
                            Enemy_HP = 25;
                            Enemy_Max_HP = 25;
                            Enemy_Name = "Orc";
                        }
                        else if (Enemy_select == 2) // Slime (Multiply and Add) enemy
                        {
                            Enemy_HP = 21;
                            Enemy_Max_HP = 21;
                            Enemy_Name = "Slime";
                        }
                        else if (Enemy_select == 3) // Ghost (Multiply and Subtract) enemy
                        {
                            Enemy_HP = 18;
                            Enemy_Max_HP = 18;
                            Enemy_Name = "Ghost";
                        }
                        else if (Enemy_select == 4) // Goblin (Division) enemy
                        {
                            Enemy_HP = 15;
                            Enemy_Max_HP = 15;
                            Enemy_Name = "Goblin";
                        }

                        // Combat enemy
                        while (Player_HP > 0 && Enemy_HP > 0) // while both player and enemy are alive
                        {
                            // Clear
                            Console.Clear();

                            // Game Over initiated 1
                            if (Player_HP == 0)
                            {
                                break; // escape while loop
                            }

                            // UI
                            Console.WriteLine("Your HP: " + Player_HP + "/" + Player_Max_HP);
                            Console.WriteLine(Enemy_Name + " Enemy HP: " + Enemy_HP + "/" + Enemy_Max_HP);

                            // Create Problem (call the Problem Generator.cs)
                            Problem_Generator_Level2 a = new Problem_Generator_Level2();
                            if (Enemy_select == 1) // Orc enemy
                            {
                                a.OrcProblem();
                            }
                            else if (Enemy_select == 2) // Slime enemy
                            {
                                a.SlimeProblem();
                            }
                            else if (Enemy_select == 3) // Ghost enemy
                            {
                                a.GhostProblem();
                            }
                            else if (Enemy_select == 4) // Goblin enemy
                            {
                                a.GoblinProblem();
                            }

                            // INPUT PROCESS SECTION START --------------------------------------------------------------
                            // Start the timeout
                            var read = false;

                            // modify    V this value to change delay (in milliseconds)
                            Task.Delay(30000).ContinueWith(_ =>
                            {
                                if (!read)
                                {
                                    // Timeout => cancel the console read
                                    var handle = GetStdHandle(STD_INPUT_HANDLE);
                                    CancelIoEx(handle, IntPtr.Zero);
                                }
                            });

                            try
                            {
                                // Start reading from the console
                                Console.Write("\nInput Answer: ");
                                stopwatch.Start(); // Time start

                                var answer = Console.ReadLine(); // input answer
                                read = true; // checks if an input was given on time

                                stopwatch.Stop(); // Time end
                                int time = (int)stopwatch.ElapsedMilliseconds;
                                stopwatch.Reset(); // time reset to 0

                                // checks if input is a number (copy pasted from https://stackoverflow.com/questions/46246472/a-local-or-parameter-named-e-cannot-be-declared-in-this-scope)
                                double value;
                                if (double.TryParse(answer, out value))
                                {
                                    // is number correct?
                                    if (double.Parse(answer) == a.correct_answer())
                                    {
                                        if (time <= 10000) // <10 sec, critical hit!
                                        {
                                            Enemy_HP -= 2;
                                            Enemy_HP -= Player_Damage;
                                            Damage_Inflicted = Player_Damage + 2;
                                            Console.WriteLine("Correct! Enemy lost " + Damage_Inflicted + " HP");
                                            Console.WriteLine("Answered in " + time / 1000 + " seconds");
                                            Console.WriteLine("Quick answer!");
                                        }
                                        else if (time <= 20000) // <20 sec, average hit.
                                        {
                                            Enemy_HP -= 1;
                                            Enemy_HP -= Player_Damage;
                                            Damage_Inflicted = Player_Damage + 1;
                                            Console.WriteLine("Correct! Enemy lost " + Damage_Inflicted + " HP");
                                            Console.WriteLine("Answered in " + time / 1000 + " seconds");
                                            Console.WriteLine("Solved nicely.");
                                        }
                                        else if (time > 20000) // >20 sec, lowered hit.
                                        {
                                            Enemy_HP -= Player_Damage;
                                            Damage_Inflicted = Player_Damage;
                                            Console.WriteLine("Correct! Enemy lost " + Damage_Inflicted + " HP");
                                            Console.WriteLine("Answered in " + time / 1000 + " seconds");
                                            Console.WriteLine("Took a little too long...");
                                        }
                                    }
                                    else if (int.Parse(answer) != a.correct_answer()) // wrong answer
                                    {
                                        // Player lose HP
                                        Player_HP -= 1;
                                        Console.WriteLine("Wrong answer! You lost 1 HP");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Input is not a number! You lost 1 HP");
                                    Player_HP -= 1;
                                }
                            }
                            // Handle the exception when the operation is canceled
                            // Player ran out of time, lose 1 HP and make new problem
                            catch (InvalidOperationException)
                            {
                                Player_HP -= 1;
                                Console.WriteLine("\nYou've run out of time! You lost 1 HP");
                                stopwatch.Stop();
                                stopwatch.Reset();

                            }
                            catch (OperationCanceledException)
                            {
                                Player_HP -= 1;
                                Console.WriteLine("\nYou've run out of time! You lost 1 HP");
                                stopwatch.Stop();
                                stopwatch.Reset();
                            }
                            // INPUT PROCESS SECTION END ------------------------------------------------------

                            Thread.Sleep(1500); // wait 1.5 seconds
                        }

                        // Game Over initiated 2
                        if (Player_HP == 0)
                        {
                            break; // escape for loop
                        }
                    }

                    // Game Over screen
                    if (Player_HP == 0)
                    {
                        Console.WriteLine("GAME OVER");
                        Console.WriteLine(Player_HP + "/5");
                        Thread.Sleep(1500);
                    }
                    else
                    {
                        // Preparing for Boss Fight
                        // Preparing for Boss Fight <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                        Console.Clear();
                        Console.WriteLine("6 enemies defeated, prepare for boss battle!");
                        Thread.Sleep(2250);
                        Console.Clear();

                        Console.WriteLine("Hello again!");
                        Thread.Sleep(2250);
                        // Friendly NPC comes in:
                        FriendlyNPC_Dialogue level2 = new FriendlyNPC_Dialogue();
                        level2.Dialogue_Level2_Boss();

                        // Player HP heal before boss
                        if (Player_HP <= Player_Max_HP - 3)
                        {
                            Player_HP += 3;
                        }
                        else if (Player_HP >= Player_Max_HP - 3)
                        {
                            Player_HP = Player_Max_HP;
                        }
                        Console.WriteLine("Before he left, he gave you a Health Potion...");
                        Thread.Sleep(2250);
                        Console.WriteLine("Player HP healed by 3 HP!");
                        Thread.Sleep(2250);
                        Console.Clear();
                        Console.WriteLine("KNIGHT X STANDS IN YOUR WAY");
                        Thread.Sleep(2250);

                        // Boss stats:
                        Enemy_HP = 50;
                        Enemy_Max_HP = 50;
                        Enemy_Name = "Knight X";

                        // COMBAT
                        while (Player_HP > 0 && Enemy_HP > 0) // while both player and BOSS are alive
                        {
                            // Clear
                            Console.Clear();

                            // Game Over initiated 1
                            if (Player_HP == 0)
                            {
                                break; // escape while loop
                            }

                            // UI
                            Console.WriteLine("Your HP: " + Player_HP + "/" + Player_Max_HP);
                            Console.WriteLine(Enemy_Name + " | BOSS HP: " + Enemy_HP + "/" + Enemy_Max_HP);

                            // Create Problem
                            int Enemy_select = rand.Next(1, 5);
                            Problem_Generator_Level2 a = new Problem_Generator_Level2();
                            if (Enemy_select == 1) // 2x = 10 + 10 format
                            {
                                a.Equation1();
                            }
                            else if (Enemy_select == 2) // x/3 = 18 format
                            {
                                a.Equation2();
                            }
                            else if (Enemy_select == 3) // x + 10 - 10 = 20 format
                            {
                                a.Equation3();
                            }
                            else if (Enemy_select == 4) // x = 100 - 50 format
                            {
                                a.Equation4();
                            }

                            // INPUT PROCESS SECTION START --------------------------------------------------------------
                            // Start the timeout
                            var read = false;

                            // modify    V this value to change delay (in milliseconds)
                            Task.Delay(40000).ContinueWith(_ =>
                            {
                                if (!read)
                                {
                                    // Timeout => cancel the console read
                                    var handle = GetStdHandle(STD_INPUT_HANDLE);
                                    CancelIoEx(handle, IntPtr.Zero);
                                }
                            });

                            try
                            {
                                // Start reading from the console
                                Console.Write("\nInput Answer: ");
                                stopwatch.Start(); // Time start

                                var answer = Console.ReadLine(); // input answer
                                read = true; // checks if an input was given on time

                                stopwatch.Stop(); // Time end
                                int time = (int)stopwatch.ElapsedMilliseconds;
                                stopwatch.Reset(); // time reset to 0

                                // checks if input is a number (copy pasted from https://stackoverflow.com/questions/46246472/a-local-or-parameter-named-e-cannot-be-declared-in-this-scope)
                                double value;
                                if (double.TryParse(answer, out value))
                                {
                                    // is number correct?
                                    if (double.Parse(answer) == a.correct_answer())
                                    {
                                        if (time <= 15000) // <15 sec, critical hit!
                                        {
                                            Enemy_HP -= 2;
                                            Enemy_HP -= Player_Damage;
                                            Damage_Inflicted = Player_Damage + 2;
                                            Console.WriteLine("Correct! Enemy lost " + Damage_Inflicted + " HP");
                                            Console.WriteLine("Answered in " + time / 1000 + " seconds");
                                            Console.WriteLine("Quick answer!");
                                        }
                                        else if (time <= 25000) // <25 sec, average hit.
                                        {
                                            Enemy_HP -= 1;
                                            Enemy_HP -= Player_Damage;
                                            Damage_Inflicted = Player_Damage + 1;
                                            Console.WriteLine("Correct! Enemy lost " + Damage_Inflicted + " HP");
                                            Console.WriteLine("Answered in " + time / 1000 + " seconds");
                                            Console.WriteLine("Solved nicely.");
                                        }
                                        else if (time > 25000) // >25 sec, lowered hit.
                                        {
                                            Enemy_HP -= Player_Damage;
                                            Damage_Inflicted = Player_Damage;
                                            Console.WriteLine("Correct! Enemy lost " + Damage_Inflicted + " HP");
                                            Console.WriteLine("Answered in " + time / 1000 + " seconds");
                                            Console.WriteLine("Took a little too long...");
                                        }
                                    }
                                    else if (int.Parse(answer) != a.correct_answer()) // wrong answer
                                    {
                                        // Player lose HP
                                        Player_HP -= 1;
                                        Console.WriteLine("Wrong answer! You lost 1 HP");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Input is not a number! You lost 1 HP");
                                    Player_HP -= 1;
                                }
                            }
                            // Handle the exception when the operation is canceled
                            // Player ran out of time, lose 1 HP and make new problem
                            catch (InvalidOperationException)
                            {
                                Player_HP -= 1;
                                Console.WriteLine("\nYou've run out of time! You lost 1 HP");
                                stopwatch.Stop();
                                stopwatch.Reset();

                            }
                            catch (OperationCanceledException)
                            {
                                Player_HP -= 1;
                                Console.WriteLine("\nYou've run out of time! You lost 1 HP");
                                stopwatch.Stop();
                                stopwatch.Reset();
                            }
                            // INPUT PROCESS SECTION END ------------------------------------------------------

                            Thread.Sleep(1500); // wait 1.5 seconds
                        }

                        // Game Over initiated 2
                        if (Player_HP == 0)
                        {
                            break; // escape for loop
                        }
                    }

                    // Game Over screen
                    if (Player_HP == 0)
                    {
                        Console.WriteLine("GAME OVER");
                        Console.WriteLine(Player_HP + "/5");
                        Thread.Sleep(1500);
                    }
                    else
                    {
                        Console.WriteLine("\nKNIGHT X HAS BEEN DEFEATED!");
                        Thread.Sleep(1500);

                        // Return to Level Selection 
                        Console.Clear();
                        Console.WriteLine("Return to Level selection? YES/NO (any other input counts as NO)");
                        string choice = Console.ReadLine().ToUpper();
                        if (choice == "YES")
                        {
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("Exiting game.");
                            Thread.Sleep(2500);
                            break;
                        }
                    }
                }

                else //invalid level input
                {
                    Console.WriteLine("Invalid Input.");
                    Thread.Sleep(2500);
                    Console.Clear();
                }                
            }
        }
    }
}

// KNOWN BUGS
// 1. if you let the time run out, then put an invalid input (basically anything thats not a number), it will not process the invalid input
// --- so you can just do another input and it will process it correctly
// --- idk how to fix this, it probably has something to do with the unfamiliar codes in the INPUT PROCESS SECTION, but for now it doesnt seem like a big issue.


// Changes compared to Flowchart
// 1. Lvl 1 now only gives 5 enemies before boss
// 2. Lvl 2 now only gives 6 enemies before boss
// 3. Lvl 2 damage breakpoint durations increased to <10, <20, and >20
// 4. Lvl 2 boss damage breakpoint durations increased to <15, <25, and >25, with total duration increased to 40s
// 5. Player always heals 3 HP before boss
// 6. Upgrade menu now appears at the start of Lvl 2
// 7. Lvl 2 goblin enemy only chooses #s 1-30