using System;
using System.Collections.Generic;

namespace Memory_card_game
{
     public class UI
     {
          private List<char> m_CardsData;
          private GameLogic.Pc m_PcPlayerLogic;
          private eGameDifficulty m_GameDifficulty;

          public UI(int size = 0)
          {
               m_GameDifficulty = eGameDifficulty.Easy;
               m_CardsData = new List<char>(size);
               m_PcPlayerLogic = null;
          }

          private class ReadInput
          {
               internal static eGameDifficulty LevelOfDifficulty()
               {
                    string inputString;
                    int userPick;
                    string outPutMsg1 = string.Format(
@"Please select game difficulty:
Press 1 for easy game
Press 2 for medium game
Press 3 for hard game");
                    Console.WriteLine(outPutMsg1);
                    do
                    {
                         Console.Write("Game difficulty - ");
                         Console.ForegroundColor = ConsoleColor.Red;
                         inputString = Console.ReadLine();
                         Console.ResetColor();
                    }
                    while(InputValidation.LevelOfDifficulty(inputString, out userPick) == false);

                    return (eGameDifficulty)userPick;
               }

               internal static string PlayerName()
               {
                    string inputString;

                    do
                    {
                         Console.WriteLine("Please enter player name (english letters only, max 20)");
                         Console.Write("Player Name - ");
                         Console.ForegroundColor = ConsoleColor.Red;
                         inputString = Console.ReadLine();
                         Console.ResetColor();
                    }
                    while(InputValidation.PlayerName(inputString) == false);

                    return inputString;
               }

               internal static GameLogic.eGameMode GameMode(string i_Player1Name)
               {
                    string inputString; 
                    int userPick;
                    string outPutMsg1 = string.Format(
@"Hey {0}, Please select game Mode:
Press 1 to play VS PC (one player)
Press 2 to play VS friend (Two players)",
        i_Player1Name);

                    Console.WriteLine(outPutMsg1);
                    do
                    {
                         Console.Write("Game mode - ");
                         Console.ForegroundColor = ConsoleColor.Red;
                         inputString = Console.ReadLine();
                         Console.ResetColor();
                    }
                    while(InputValidation.GameMode(inputString, out userPick) == false);

                    return (GameLogic.eGameMode)userPick;
               }

               internal static eNewGameRequest NewGameRequest()
               {
                    string inputString;
                    int userPick;
                    string outPutMsg1 = string.Format(
@"=============================
Thanks for playing.
Do you want start a new game?
Press 1 for YES
Press 2 to No");

                    Console.WriteLine(outPutMsg1);
                    do
                    {
                         Console.Write("New game? - ");
                         Console.ForegroundColor = ConsoleColor.Red;
                         inputString = Console.ReadLine();
                         Console.ResetColor();
                    }
                    while(InputValidation.NewGameRequest(inputString, out userPick) == false);

                    return (eNewGameRequest)userPick;
               }

               internal static void BoardDimensions(out int o_BoardNumOfRows, out int o_BoardNumOfCols)
               {
                    string inputString;
                    bool isTotalCellsNumberOdd = false;
                    do
                    {
                         Console.WriteLine("Please enter board dimensions HEIGHT and WIDTH (4-6 each) total cells number must be even");
                         Console.WriteLine("System recommendation is 6X6");
                         do
                         {
                              Console.Write("HEIGHT - ");
                              Console.ForegroundColor = ConsoleColor.Red;
                              inputString = Console.ReadLine();
                              Console.ResetColor();
                         }
                         while(InputValidation.Dimension(inputString, out o_BoardNumOfRows) == false);

                         do
                         {
                              Console.Write("WIDTH - ");
                              Console.ForegroundColor = ConsoleColor.Red;
                              inputString = Console.ReadLine();
                              Console.ResetColor();
                         }
                         while(InputValidation.Dimension(inputString, out o_BoardNumOfCols) == false);

                         if((o_BoardNumOfRows * o_BoardNumOfCols) % 2 == 0)
                         {
                              isTotalCellsNumberOdd = true;
                         }
                         else
                         {
                              Console.WriteLine("input ERROR - total cells number must be even. please follow the instructions");
                         }
                    }
                    while(isTotalCellsNumberOdd == false);
               }

               internal static void CardSelection(GameLogic.Node[,] i_GameBoard, out string o_InputString)
               {
                    int boardNumOfRows = i_GameBoard.GetLength(0);
                    char maxChar = transformDimensionColForPrint(i_GameBoard.GetLength(1));
                    string outPutMsg1 = string.Format("Please select card spot (col[A-{0}] and row[1-{1}] - for example B4)", maxChar, boardNumOfRows);
                    do
                    {
                         Console.WriteLine(outPutMsg1);
                         Console.ForegroundColor = ConsoleColor.Red;
                         o_InputString = Console.ReadLine();
                         Console.ResetColor();
                    }
                    while(InputValidation.CardSelection(i_GameBoard, o_InputString) == false);
               }
          }

          private class InputValidation
          {
               internal static bool LevelOfDifficulty(string i_InputString, out int o_UserPick)
               {
                    bool inputIsValid = false;

                    if(int.TryParse(i_InputString, out o_UserPick) == true)
                    {
                         if(o_UserPick == 1 || o_UserPick == 2 || o_UserPick == 3)
                         {
                              inputIsValid = true;
                         }
                    }

                    if(inputIsValid == false)
                    {
                         Console.WriteLine("input ERROR - game difficulty must be 1 , 2 or 3. please follow the instructions");
                    }

                    return inputIsValid;
               }

               internal static bool PlayerName(string i_InputString)
               {
                    bool inputIsValid = false;

                    if(i_InputString.Length > 0)
                    {
                         if(i_InputString.Length <= 20)
                         {
                              if(englishString(i_InputString) == true)
                              {
                                   inputIsValid = true;
                              }
                              else
                              {
                                   Console.WriteLine("input ERROR - only english letters. please follow the instructions");
                              }
                         }
                         else
                         {
                              Console.WriteLine("input ERROR - max 20 letters. please follow the instructions");
                         }
                    }
                    else
                    {
                         Console.WriteLine("input ERROR - name length is min 1 letter. please follow the instructions");
                    }

                    return inputIsValid;
               }

               internal static bool GameMode(string i_InputString, out int o_UserPick)
               {
                    bool inputIsValid = false;

                    if(int.TryParse(i_InputString, out o_UserPick) == true)
                    {
                         if(o_UserPick == 1 || o_UserPick == 2)
                         {
                              inputIsValid = true;
                         }
                    }

                    if(inputIsValid == false)
                    {
                         Console.WriteLine("input ERROR - game mode must be 1 or 2. please follow the instructions");
                    }

                    return inputIsValid;
               }

               internal static bool Dimension(string i_InputString, out int o_UserPick)
               {
                    bool inputIsValid = false;

                    if(int.TryParse(i_InputString, out o_UserPick) == true)
                    {
                         if(o_UserPick >= 4 && o_UserPick <= 6)
                         {
                              inputIsValid = true;
                         }
                    }

                    if(inputIsValid == false)
                    {
                         Console.WriteLine("input ERROR - board dimensions must be 4-6. please follow the instructions");
                    }

                    return inputIsValid;
               }

               internal static bool CardSelection(GameLogic.Node[,] i_GameBoard, string i_InputString)
               {
                    bool isValidCardSpotSelection = false;
                    int boardNumOfRows = i_GameBoard.GetLength(0) - 1, boardNumOfCols = i_GameBoard.GetLength(1) - 1, inputColsAsInt, inputRowAsInt;

                    exitString(i_InputString);
                    if(i_InputString.Length == 2)
                    {
                         if(char.IsUpper(i_InputString[0]) == true)
                         {
                              inputColsAsInt = i_InputString[0] - 'A';
                              if(inputColsAsInt <= boardNumOfCols)
                              {
                                   if(char.IsNumber(i_InputString[1]))
                                   {
                                        inputRowAsInt = i_InputString[1] - '1';
                                        if(inputRowAsInt >= 0 && inputRowAsInt <= boardNumOfRows)
                                        {
                                             if(i_GameBoard[inputRowAsInt, inputColsAsInt].IsFaceUp == false)
                                             {
                                                  isValidCardSpotSelection = true;
                                             }
                                             else
                                             {
                                                  Console.WriteLine("input ERROR - this card is already face up. please follow the instructions");
                                             }
                                        }
                                        else
                                        {
                                             Console.WriteLine("input ERROR - row selction is out of bounds. please follow the instructions");
                                        }
                                   }
                                   else
                                   {
                                        Console.WriteLine("input ERROR - row selction must be number. please follow the instructions");
                                   }
                              }
                              else
                              {
                                   Console.WriteLine("input ERROR - col selction is out of bounds. please follow the instructions");
                              }
                         }
                         else
                         {
                              Console.WriteLine("input ERROR - col selction must be uppercase letter. please follow the instructions");
                         }
                    }
                    else
                    {
                         Console.WriteLine("input ERROR - input length must be 2 (row and col). please follow the instructions");
                    }

                    return isValidCardSpotSelection;
               }

               internal static bool NewGameRequest(string i_InputString, out int o_UserPick)
               {
                    bool inputIsValid = false;

                    exitString(i_InputString);
                    if(int.TryParse(i_InputString, out o_UserPick) == true)
                    {
                         if(o_UserPick == 1 || o_UserPick == 2)
                         {
                              inputIsValid = true;
                         }
                    }

                    if(inputIsValid == false)
                    {
                         Console.WriteLine("input ERROR - request must be 1 or 2. please follow the instructions");
                    }

                    return inputIsValid;
               }

               private static bool englishString(string i_InputString)
               {
                    bool isValid = true;
                    foreach(char currChar in i_InputString)
                    {
                         if(!(currChar >= 'A' && currChar <= 'Z') && !(currChar >= 'a' && currChar <= 'z') && !(currChar == ' '))
                         {
                              isValid = false;
                         }
                    }

                    return isValid;
               }

               private static void exitString(string i_StringToCheck)
               {
                    if(i_StringToCheck == "Q")
                    {
                         Environment.Exit(0);
                    }
               }
          }

          private enum eGameDifficulty
          {
               Easy = 1, Medium = 2, Hard = 3
          }

          private enum eNewGameRequest
          {
               Yes = 1, No = 2
          }

          public void Run()
          {
               GameLogic game = this.gamePreparations();
               this.playGame(game);
          }

          private GameLogic gamePreparations()
          {
               string player1Name = ReadInput.PlayerName(), player2Name = null;
               GameLogic.eGameMode gameMode = ReadInput.GameMode(player1Name);

               if(gameMode == GameLogic.eGameMode.TwoPlayers)
               {
                    player2Name = ReadInput.PlayerName();
               }
               else
               {
                    this.m_GameDifficulty = ReadInput.LevelOfDifficulty();
               }

               ReadInput.BoardDimensions(out int boardNumOfRows, out int boardNumOfCols);
               this.m_CardsData.Capacity = boardNumOfRows * boardNumOfCols / 2;
               this.cardsDataInitialization();

               if(gameMode == GameLogic.eGameMode.OnePlayer)
               {
                    m_PcPlayerLogic = new GameLogic.Pc(boardNumOfRows, boardNumOfCols);
               }

               return new GameLogic(boardNumOfRows, boardNumOfCols, gameMode, player1Name, player2Name);
          }

          private void playGame(GameLogic i_Game)
          {
               eNewGameRequest requestForNewGame;
               do
               {
                    printIntroAndRules(i_Game);
                    if(i_Game.GameMode == GameLogic.eGameMode.TwoPlayers)
                    {
                         do
                         {
                              Ex02.ConsoleUtils.Screen.Clear();
                              turn(i_Game);
                         }
                         while(i_Game.IsGameFinished() == false);
                    }
                    else
                    {
                         playVsPc(i_Game);
                    }

                    Ex02.ConsoleUtils.Screen.Clear();
                    printScoreBoard(i_Game);
                    printCurrentBoardGame(i_Game.Data);
                    printWinnerOrTie(i_Game);
                    requestForNewGame = ReadInput.NewGameRequest();
                    if(requestForNewGame == eNewGameRequest.Yes)
                    {
                         ReadInput.BoardDimensions(out int boardNumOfRows, out int boardNumOfCols);
                         this.m_CardsData.Capacity = boardNumOfRows * boardNumOfCols / 2;
                         this.cardsDataInitialization();
                         if(i_Game.GameMode == GameLogic.eGameMode.OnePlayer)
                         {
                              this.m_GameDifficulty = ReadInput.LevelOfDifficulty();
                         }

                         i_Game.ResetGame(boardNumOfRows, boardNumOfCols);
                         m_PcPlayerLogic = new GameLogic.Pc(boardNumOfRows, boardNumOfCols);
                    }
               }
               while(requestForNewGame == eNewGameRequest.Yes);
          }

          private void playVsPc(GameLogic i_Game)
          {
               do
               {
                    Ex02.ConsoleUtils.Screen.Clear();

                    if(i_Game.Turn == GameLogic.eTurn.Player1)
                    {
                         turn(i_Game);
                    }
                    else
                    {
                         pcTurn(i_Game);
                    }
               }
               while(i_Game.IsGameFinished() == false);
          }

          private void pcPick(out string o_Cell1, out string o_Cell2, GameLogic i_Game)
          {
               o_Cell1 = null;
               o_Cell2 = null;
               int chanceToRemember = new Random().Next(5);

               if(this.m_GameDifficulty == eGameDifficulty.Easy && chanceToRemember != 0)
               {
                    o_Cell1 = m_PcPlayerLogic.ReturnRandomCell();
                    do
                    {
                         o_Cell2 = m_PcPlayerLogic.ReturnRandomCell();
                    }
                    while(o_Cell1 == o_Cell2);
               }
               else
               {
                    pcHardMeduimPick(out o_Cell1, out o_Cell2, i_Game);
               }
          }

          private void pcHardMeduimPick(out string o_Cell1, out string o_Cell2, GameLogic i_Game)
          {
               if(m_PcPlayerLogic.SawPairOfCards != null)
               {
                    o_Cell1 = m_PcPlayerLogic.SawPairOfCards.FirstLocation;
                    o_Cell2 = m_PcPlayerLogic.SawPairOfCards.SecondLocation;
                    m_PcPlayerLogic.DeleteSpot();
               }
               else
               {
                    o_Cell1 = m_PcPlayerLogic.ReturnRandomCell();
                    o_Cell2 = null;
                    int chanceToRemember = new Random().Next(3);

                    if(m_GameDifficulty == eGameDifficulty.Hard || chanceToRemember == 0)
                    {
                         int col = transformDimensionColForUse(o_Cell1[0]);
                         int row = transformDimensionRowForUse(o_Cell1[1]);
                         o_Cell2 = m_PcPlayerLogic.IsPairFound(this.m_CardsData[i_Game.Data[row, col].CardIndex], o_Cell1);
                         while(o_Cell1 == o_Cell2 || o_Cell2 == null)
                         {
                              o_Cell2 = m_PcPlayerLogic.ReturnRandomCell();
                         }
                    }
                    else
                    {
                         do
                         {
                              o_Cell2 = m_PcPlayerLogic.ReturnRandomCell();
                         }
                         while(o_Cell1 == o_Cell2);
                    }
               }
          }

          private void pcTurn(GameLogic i_Game)
          {
               bool pointScored = false;
               string currPlayerTurn = i_Game.Player2.Name;
               printScoreBoard(i_Game);
               printCurrentBoardGame(i_Game.Data);
               pcPick(out string cell1, out string cell2, i_Game);
               int firstCardCol = transformDimensionColForUse(cell1[0]);
               int firstCardRow = transformDimensionRowForUse(cell1[1]);
               m_PcPlayerLogic.AddCardPcSaw(cell1, m_CardsData[i_Game.Data[firstCardRow, firstCardCol].CardIndex]);
               i_Game.Data[firstCardRow, firstCardCol].IsFaceUp = true;
               i_Game.Data[firstCardRow, firstCardCol].IsCurrPick = true;
               Ex02.ConsoleUtils.Screen.Clear();
               printScoreBoard(i_Game);
               printCurrentBoardGame(i_Game.Data);
               string pcSelection = string.Format(
 @"    PC TURN!
    First pick - {0}",
 cell1);
               Console.WriteLine(pcSelection);
               System.Threading.Thread.Sleep(3000);
               int secondCardCol = transformDimensionColForUse(cell2[0]);
               int secondCardRow = transformDimensionRowForUse(cell2[1]);
               m_PcPlayerLogic.AddCardPcSaw(cell2, m_CardsData[i_Game.Data[secondCardRow, secondCardCol].CardIndex]);
               i_Game.Data[secondCardRow, secondCardCol].IsFaceUp = true;
               i_Game.Data[secondCardRow, secondCardCol].IsCurrPick = true;
               Ex02.ConsoleUtils.Screen.Clear();
               printScoreBoard(i_Game);
               printCurrentBoardGame(i_Game.Data);
               pointScored = i_Game.PlayTurn(firstCardRow, firstCardCol, secondCardRow, secondCardCol);
               pcSelection = string.Format(
 @"    PC TURN!
    Second pick - {0}",
 cell2);
               Console.WriteLine(pcSelection);
               System.Threading.Thread.Sleep(3000);
               i_Game.Data[firstCardRow, firstCardCol].IsCurrPick = false;
               i_Game.Data[secondCardRow, secondCardCol].IsCurrPick = false;
               if(pointScored == true)
               {
                    m_PcPlayerLogic.RemoveOpenedCards(firstCardRow, firstCardCol, secondCardRow, secondCardCol, i_Game.Data.GetLength(1));
                    Console.WriteLine("it's a pair! PC earn 1 POINT!");
               }
               else
               {
                    i_Game.Data[firstCardRow, firstCardCol].IsFaceUp = false;
                    i_Game.Data[secondCardRow, secondCardCol].IsFaceUp = false;
                    i_Game.Data[firstCardRow, firstCardCol].IsCurrPick = false;
                    i_Game.Data[secondCardRow, secondCardCol].IsCurrPick = false;
                    Console.WriteLine("Thats not a pair. 2 seconds try to remember the cards");
               }

               System.Threading.Thread.Sleep(2000);
               Ex02.ConsoleUtils.Screen.Clear();
          }

          private void turn(GameLogic i_Game)
          {
               bool pointScored = false;
               string currPlayerTurn;
               if(i_Game.Turn == GameLogic.eTurn.Player1)
               {
                    currPlayerTurn = i_Game.Player1.Name;
               }
               else
               {
                    currPlayerTurn = i_Game.Player2.Name;
               }

               string firstPickMsg1 = string.Format(
@"    {0} TURN!
    First Pick",
   currPlayerTurn);
               string secondPickMsg2 = string.Format(
@"    {0} TURN!
    Second Pick",
   currPlayerTurn);
               string outPutMsg1;

               printScoreBoard(i_Game);
               printCurrentBoardGame(i_Game.Data);
               Console.WriteLine(firstPickMsg1);
               ReadInput.CardSelection(i_Game.Data, out string firstCardSelection);
               int firstCardCol = transformDimensionColForUse(firstCardSelection[0]);
               int firstCardRow = transformDimensionRowForUse(firstCardSelection[1]);
               i_Game.Data[firstCardRow, firstCardCol].IsFaceUp = true;
               i_Game.Data[firstCardRow, firstCardCol].IsCurrPick = true;
               Ex02.ConsoleUtils.Screen.Clear();
               printScoreBoard(i_Game);
               printCurrentBoardGame(i_Game.Data);
               Console.WriteLine(secondPickMsg2);
               ReadInput.CardSelection(i_Game.Data, out string seconedCardSelection);
               int secondCardCol = transformDimensionColForUse(seconedCardSelection[0]);
               int secondCardRow = transformDimensionRowForUse(seconedCardSelection[1]);
               i_Game.Data[secondCardRow, secondCardCol].IsFaceUp = true;
               i_Game.Data[secondCardRow, secondCardCol].IsCurrPick = true;
               Ex02.ConsoleUtils.Screen.Clear();
               printScoreBoard(i_Game);
               printCurrentBoardGame(i_Game.Data);
               i_Game.Data[firstCardRow, firstCardCol].IsCurrPick = false;
               i_Game.Data[secondCardRow, secondCardCol].IsCurrPick = false;

               if(i_Game.GameMode == GameLogic.eGameMode.OnePlayer)
               {
                    m_PcPlayerLogic.AddCardPcSaw(firstCardSelection, m_CardsData[i_Game.Data[firstCardRow, firstCardCol].CardIndex]);
                    m_PcPlayerLogic.AddCardPcSaw(seconedCardSelection, m_CardsData[i_Game.Data[secondCardRow, secondCardCol].CardIndex]);
               }

               pointScored = i_Game.PlayTurn(firstCardRow, firstCardCol, secondCardRow, secondCardCol);
               if(pointScored == true)
               {
                    if(i_Game.GameMode == GameLogic.eGameMode.OnePlayer)
                    {
                         m_PcPlayerLogic.RemoveOpenedCards(firstCardRow, firstCardCol, secondCardRow, secondCardCol, i_Game.Data.GetLength(1));
                    }

                    outPutMsg1 = string.Format(
@"Good job {0}!
it's a pair! {0} earn 1 POINT!",
    currPlayerTurn);
                    Console.WriteLine(outPutMsg1);
               }
               else
               {
                    i_Game.Data[firstCardRow, firstCardCol].IsFaceUp = false;
                    i_Game.Data[secondCardRow, secondCardCol].IsFaceUp = false;
                    Console.WriteLine("Thats not a pair. 2 seconds try to remember the cards");
               }

               System.Threading.Thread.Sleep(2000);
               Ex02.ConsoleUtils.Screen.Clear();
          }

          private void cardsDataInitialization()
          {
               this.m_CardsData.Clear();
               for(int i = 0; i < m_CardsData.Capacity; i++)
               {
                    this.m_CardsData.Add(Convert.ToChar(i + 'A'));
               }
          }

          private static char transformDimensionColForPrint(int i_BoardNumOfCol)
          {
               return Convert.ToChar(i_BoardNumOfCol + 'A' - 1);
          }

          private static int transformDimensionColForUse(char i_BoardNumOfCol)
          {
               return i_BoardNumOfCol - 'A';
          }

          private static int transformDimensionRowForUse(char i_BoardNumOfRow)
          {
               return i_BoardNumOfRow - '1';
          }

          private void printCurrentBoardGame(GameLogic.Node[,] i_GameBoard)
          {
               string outPutMsg1, outPutMsg2, outPutMsg3;
               int boardNumOfRows = i_GameBoard.GetLength(0), boardNumOfCols = i_GameBoard.GetLength(1);
               Console.ForegroundColor = ConsoleColor.Yellow;

               Console.Write(" ");
               for(int i = 1; i <= boardNumOfCols; i++)
               {
                    outPutMsg2 = string.Format("   {0}", transformDimensionColForPrint(i));
                    Console.Write(outPutMsg2);
               }

               Console.WriteLine();
               for(int i = 0; i < boardNumOfRows; i++)
               {
                    printBoardWrapper(i_GameBoard);
                    outPutMsg1 = string.Format("{0} |", i + 1);
                    Console.Write(outPutMsg1);
                    for(int j = 0; j < boardNumOfCols; j++)
                    {
                         if(i_GameBoard[i, j].IsFaceUp == true)
                         {
                              if(i_GameBoard[i, j].IsCurrPick == true)
                              {
                                   Console.ForegroundColor = ConsoleColor.Red;
                              }

                              outPutMsg3 = string.Format(" {0} ", this.m_CardsData[i_GameBoard[i, j].CardIndex]);
                              Console.Write(outPutMsg3);
                              Console.ForegroundColor = ConsoleColor.Yellow;
                         }
                         else
                         {
                              Console.Write("   ");
                         }

                         Console.Write("|");
                    }

                    Console.WriteLine();
               }

               printBoardWrapper(i_GameBoard);
               Console.WriteLine();
               Console.ResetColor();
          }

          private void printBoardWrapper(GameLogic.Node[,] i_GameBoard)
          {
               if(i_GameBoard.GetLength(1) == 4)
               {
                    Console.WriteLine("  =================");
               }
               else if(i_GameBoard.GetLength(1) == 5)
               {
                    Console.WriteLine("  =====================");
               }
               else
               {
                    Console.WriteLine("  =========================");
               }
          }

          private void printScoreBoard(GameLogic i_Game)
          {
               string outPutMsg1 = string.Format(
@"===========SCORE BOARD===========
{0}:{1}
{2}:{3}
===========SCORE BOARD===========",
   i_Game.Player1.Name,
   i_Game.Player1.Points,
   i_Game.Player2.Name,
   i_Game.Player2.Points);
               Console.ForegroundColor = ConsoleColor.Yellow;
               Console.WriteLine(outPutMsg1);
               Console.ResetColor();
               Console.WriteLine();
          }

          private void printWinnerOrTie(GameLogic i_Game)
          {
               string winnerOrTie = determineWinnerOrTie(i_Game);

               Console.Write("GAME RESULT - ");
               if(winnerOrTie == null)
               {
                    Console.WriteLine("ITS A TIE!!");
               }
               else
               {
                    string outPutMsg1 = string.Format("{0} WINS!", winnerOrTie);
                    Console.WriteLine(outPutMsg1);
               }

               Console.WriteLine();
          }

          private string determineWinnerOrTie(GameLogic i_Game)
          {
               string winnerOfTheGame = null;
               if(i_Game.Player1.Points > i_Game.Player2.Points)
               {
                    winnerOfTheGame = i_Game.Player1.Name;
               }
               else if(i_Game.Player1.Points < i_Game.Player2.Points)
               {
                    winnerOfTheGame = i_Game.Player2.Name;
               }

               return winnerOfTheGame;
          }

          private void printIntroAndRules(GameLogic i_Game)
          {
               string outPutMsg1 = string.Format(
@"
Memory Card game rules:
At the beginning of the game, all the cards are mixed up and laid in rows, face down.
Player 1 starts and turns over two cards:
If the two cards match, it’s a pair! cards stay face up. player get 1 point and has the right to play again.
However, If the cards don’t match (not a pair), cards flip back over. NO points and it’s then the turn of player 2.
and so on ...

Try to remember what was on each card and where it was, also during the other player's turn.
The game is over when all the cards have been matched.
The winner is the player with the most points. if both players points is equal its a TIE.

Key board Options(use as input):
Q - QUIT

Game info:
Game mode - {0}
PLAYER 1 - {1}
PLAYER 2 - {2}

GOOD LUCK & HAVE FUN!
",
       i_Game.GameMode,
       i_Game.Player1.Name,
       i_Game.Player2.Name);
               Console.Write(outPutMsg1);

               Console.WriteLine("PLAY IN:");
               for(int i = 5; i >= 1; i--)
               {
                    Console.WriteLine(i + "....");
                    System.Threading.Thread.Sleep(1000);
               }
          }
     }
}