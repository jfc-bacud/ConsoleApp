using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class Program
    {
        static bool[,] defaultBoard = new bool[3, 3];
        static Queue<string> playerMove = new Queue<string>();
        static Queue<string> enemyMove = new Queue<string>();
        static int[,] boardOwners = new int[3, 3];

        static void Main(string[] args)
        {
            GenerateDefaultBoard();
            int playerWin = 0;
            int compWin = 0;

            while (true)
            {
                bool turnDone = false;
                Console.Clear();
                DisplayBoard(playerWin, compWin);
                UserInput(out turnDone);

                if (!turnDone)
                {
                    continue;
                }

                if (WhoWins() == 1)
                {
                    Win(playerWin,compWin);
                    playerWin++;
                    continue;
                }

                ComputerMove();
                if (WhoWins() == 0)
                {
                    Win(playerWin, compWin);
                    compWin++;
                    continue;
                }
            }
        }
        static void GenerateDefaultBoard()
        {
            for (int x = 0; x < defaultBoard.GetLength(0); x++)
            {
                for (int y = 0; y < defaultBoard.GetLength(1); y++)
                {
                    defaultBoard[x, y] = false;
                    boardOwners[x, y] = -1;
                }
            }
        }

        // 4 METHODS

        #region User Input 
        static void UserInput(out bool turnDone)
        {
            Console.Write("Player Move <row,column> : ");
            string userInput = Console.ReadLine();
            string inputMove = "";

            if (IsInputValid(userInput, out inputMove))
            {
                PlayerMove(inputMove, out turnDone);
            }
            else
            {
                Console.WriteLine("\nAn error has occured, please input a valid value in the proper format");
                Console.WriteLine("Input a key to continue...");
                Console.ReadKey();
                turnDone = false;
            }
        }
        static bool IsInputValid(string userInput, out string inputMove)
        {
            if (userInput.Length != 0 && userInput.Length <= 3 && userInput[1] == ',')
            {
                string tempString = InputSplitter(userInput);
                if (IsChoiceValid(tempString))
                {
                    inputMove = tempString;
                    return true;
                }
            }
            inputMove = "";
            return false;
        }
        static string InputSplitter(string userInput)
        {
            string tempString = "";
            tempString += userInput[0];
            tempString += userInput[2];
            return tempString;
        }
        static bool IsChoiceValid(string tempString)
        {
            bool result = false;
            for (int i = 0; i < 2; i++)
            {
                if (tempString[i] == '0' || tempString[i] == '1' || tempString[i] == '2')
                {
                    result = true;
                }
                else
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
        #endregion

        // 3 METHODS

        #region Board Display
        static void DisplayBoard(int playerWin, int compWin)
        {
            Console.WriteLine("\n");
            Console.WriteLine($"   Player - {playerWin} Computer - {compWin}\n");
            Console.WriteLine("\t0     1     2");

            for (int x = 0; x < defaultBoard.GetLength(0); x++)
            {
                Console.WriteLine("     +-----+-----+-----+");
                Console.Write($"  {x}");

                for (int y = 0; y < defaultBoard.GetLength(1); y++)
                {
                    char tempChar = ' ';

                    if (IndexChecker(x, y, out tempChar))
                    {
                        Console.Write($"  |  {tempChar}");
                    }

                    else
                    {
                        Console.Write($"  |  {tempChar}");

                    }
                }
                Console.Write("  |");
                Console.WriteLine();
            }
            Console.WriteLine("     +-----+-----+-----+\n");
        }
        static bool IndexChecker(int x, int y, out char tempChar)
        {
            if (defaultBoard[x, y] == true)
            {
                tempChar = IndexOwner(x, y);
                return true;
            }
            else
            {
                tempChar = ' ';
                return false;
            }
        }
        static char IndexOwner(int x, int y)
        {
            if (boardOwners[x, y] == 0)
            {
                return 'X';
            }

            return 'O';
        }
        #endregion

        // 3 METHODS

        #region Player Move
        static void IndexRemover(string inputMove)
        {
            int rowIndex = int.Parse(inputMove) / 10;
            int columnIndex = int.Parse(inputMove) % 10;

            defaultBoard[rowIndex, columnIndex] = false;
            boardOwners[rowIndex, columnIndex] = -1;
        }
        static bool IndexChecker(string inputMove)
        {
            int rowIndex = int.Parse(inputMove) / 10;
            int columnIndex = int.Parse(inputMove) % 10;

            if (defaultBoard[rowIndex, columnIndex] == false && boardOwners[rowIndex, columnIndex] == -1)
            {
                defaultBoard[rowIndex, columnIndex] = true;
                boardOwners[rowIndex, columnIndex] = 1;
                return true;
            }
            else
            {
                return false;
            }
        }
        static void PlayerMove(string inputMove, out bool turnDone)
        {
            if (IndexChecker(inputMove))
            {
                if (playerMove.Count < 3)
                {
                    playerMove.Enqueue(inputMove);
                }
                else
                {
                    IndexRemover(playerMove.Peek());
                    playerMove.Dequeue();
                    playerMove.Enqueue(inputMove);
                }

                turnDone = true;
            }
            else
            {
                Console.WriteLine("\nSpecific index has been taken, please input an available index");
                Console.WriteLine("Input any key to continue....");
                Console.ReadKey();

                turnDone = false;
            }

        }
        #endregion

        // 2 METHODS

        #region Win Condition Met
        static int WhoWins()
        {
            for (int i = 0; i < 2; i++)
            {
                for (int x = 0; x < boardOwners.GetLength(0); x++)
                {
                    if (boardOwners[x, 0] == i && boardOwners[x, 1] == i && boardOwners[x, 2] == i)
                    {
                        return i;
                    }
                }

                for (int y = 0; y < boardOwners.GetLength(1); y++)
                {
                    if (boardOwners[0, y] == i && boardOwners[1, y] == i && boardOwners[2, y] == i)
                    {
                        return i;
                    }
                }

                if (boardOwners[0, 0] == i && boardOwners[1, 1] == i && boardOwners[2, 2] == i || boardOwners[0, 2] == i && boardOwners[1, 1] == i && boardOwners[0, 2] == i)
                {
                    return i;
                }
            }

            return -1;
        }
        static void Win(int playerWin, int compWin)
        {
            Console.Clear();

            if (WhoWins() == 1)
            {

                DisplayBoard(playerWin, compWin);
                Console.WriteLine("\nYou Won!");
                Console.ReadKey();
            }

            for (int x = 0; x < playerMove.Count; x++)
            {
                playerMove.Dequeue();
            }
            GenerateDefaultBoard();
        }
        #endregion

        static void ComputerMove()
        {
            

    
        }

    }
}
