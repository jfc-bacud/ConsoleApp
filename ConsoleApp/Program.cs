using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
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

            while (true)
            {
                Console.Clear();
                DisplayBoard();
                UserInput();
            }

        }


        #region User Input

        /// <summary>
        /// User Input Process
        /// </summary>
        static void UserInput()
        {
            Console.Write("Player Move <row,column> : ");
            string userInput = Console.ReadLine();
            string inputMove = "";

            if (IsInputValid(userInput, out inputMove))
            {
                PlayerMove(inputMove);
            }
            else
            {
                Console.WriteLine("\nAn error has occured, please input a valid value in the proper format");
                Console.WriteLine("Input a key to continue...");
                Console.ReadKey();
            }

        }

        /// <summary>
        /// Validates if user input meets the format presented
        /// </summary>
        /// <param name="userInput"></param>
        /// <param name="inputMove"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Splits the string to not include the ',' character
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        static string InputSplitter(string userInput)
        {
            string tempString = "";
            tempString += userInput[0];
            tempString += userInput[2];

            return tempString;
        }

        /// <summary>
        /// Checks if the numbers present within the string exists in the current context of the board
        /// </summary>
        /// <param name="tempString"></param>
        /// <returns></returns>
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

            if (result)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        #endregion

        #region Board Display

        /// <summary>
        /// Generates default values for the board (everything should be false)
        /// </summary>
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

        /// <summary>
        /// Generates visual for the board
        /// </summary>
        static void DisplayBoard()
        {
            Console.WriteLine("\n");
            Console.WriteLine("\t0     1     2");

            for (int x = 0; x < defaultBoard.GetLength(0); x++)
            {
                Console.WriteLine("     +-----+-----+-----+");
                Console.Write($"  {x}");

                for (int y = 0; y < defaultBoard.GetLength(1); y++)
                {
                    char tempChar = ' ';

                    if (IndexChecker(x,y,out tempChar))
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

        #endregion

        /// <summary>
        /// Index checker for display
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="tempChar"></param>
        /// <returns></returns>
        static bool IndexChecker(int x, int y, out char tempChar)
        {
            if (defaultBoard[x, y] == true)
            {
                tempChar = IndexOwner(x, y);
                return true;
            }
            else
            {
                // goes to IndexOwner later...
                tempChar = ' ';
                return false;
            }         
        }

        /// <summary>
        /// Index checker for moves
        /// </summary>
        /// <param name="inputMove"></param>
        /// <returns></returns>
        /// 
        static bool IndexChecker(string inputMove)
        {
            int rowIndex = int.Parse(inputMove)/10;
            int columnIndex = int.Parse(inputMove)%10;

            if (defaultBoard[rowIndex, columnIndex] == false && boardOwners[rowIndex,columnIndex] == -1)
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

        static void IndexRemover(string inputMove)
        {
            int rowIndex = int.Parse(inputMove) / 10;
            int columnIndex = int.Parse(inputMove) % 10;

            defaultBoard[rowIndex, columnIndex] = false;
            boardOwners[rowIndex, columnIndex] = -1;
        }

        static void PlayerMove(string inputMove)
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
            }
            else
            {
                Console.WriteLine("\nSpecific index has been taken, please input an available index");
                Console.WriteLine("Input any key to continue....");
                Console.ReadKey();
            }

        }

        static char IndexOwner(int x, int y)
        {
            if (boardOwners[x,y] == 0)
            {
                return 'X';
            }

            return 'O';
        }
    }
}
