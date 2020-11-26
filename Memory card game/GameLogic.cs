using System;
using System.Collections.Generic;

namespace Memory_card_game
{
     public class GameLogic
     {
          private Node[,] m_Data;
          private Player m_Player1;
          private Player m_Player2;
          private eGameMode m_GameMode;
          private eTurn m_Turn;

          public GameLogic(int i_Rows, int i_Cols, eGameMode i_NewGameMode, string i_Name1, string i_Name2)
          {
               m_Player1 = new Player(i_Name1);
               if(i_Name2 != null)
               {
                    m_Player2 = new Player(i_Name2);
               }
               else
               {
                    m_Player2 = new Player("PC");
               }

               m_Data = new Node[i_Rows, i_Cols];

               for(int i = 0; i < i_Rows; i++)
               {
                    for(int j = 0; j < i_Cols; j++)
                    {
                         m_Data[i, j] = new Node();
                    }
               }

               resetArray(i_Rows, i_Cols);
               m_Turn = 0;
               m_GameMode = i_NewGameMode;
          }

          public class Player
          {
               private string m_Name;
               private int m_Points;

               public Player(string i_Name)
               {
                    m_Name = i_Name;
                    m_Points = 0;
               }

               public string Name
               {
                    get
                    {
                         return m_Name;
                    }

                    set
                    {
                         m_Name = value;
                    }
               }

               public int Points
               {
                    get
                    {
                         return m_Points;
                    }

                    set
                    {
                         m_Points = value;
                    }
               }
          }

          public class Node
          {
               private int m_Pair;
               private bool m_IsFaceUp;
               private bool m_IsCurrPick;
               private int m_CardIndex;

               public Node(int i_Pair = 0, int i_Index = 0)
               {
                    m_Pair = i_Pair;
                    m_IsFaceUp = false;
                    m_IsCurrPick = false;
                    m_CardIndex = i_Index;
               }

               public int Pair
               {
                    get
                    {
                         return m_Pair;
                    }

                    set
                    {
                         m_Pair = value;
                    }
               }

               public int CardIndex
               {
                    get
                    {
                         return m_CardIndex;
                    }

                    set
                    {
                         m_CardIndex = value;
                    }
               }

               public bool IsFaceUp
               {
                    get
                    {
                         return m_IsFaceUp;
                    }

                    set
                    {
                         m_IsFaceUp = value;
                    }
               }

            public bool IsCurrPick
            {
                get
                {
                    return m_IsCurrPick;
                }

                set
                {
                    m_IsCurrPick = value;
                }
            }
        }

          public class Pc
          {
               private Dictionary<int, string> m_PickOptions;
               private Dictionary<char, Node> m_OpenCards;
               private Queue<Node> m_Saw2Spots;

               public Pc(int i_Row, int i_Col)
               {
                    m_OpenCards = new Dictionary<char, Node>();
                    m_PickOptions = new Dictionary<int, string>();
                    m_Saw2Spots = new Queue<Node>();

                    for(int i = 0, k = 0; i < i_Row; i++)
                    {
                         for(int j = 0; j < i_Col; j++, k++)
                         {
                              m_PickOptions.Add(k, makeCellFromInt(i, j));
                         }
                    }
               }

               public string IsPairFound(char i_FirstCellOpened, string i_LocationFound)
               {
                    string cellToReturn = null;
                    m_OpenCards.TryGetValue(i_FirstCellOpened, out Node o_CheckIfFoundPair);

                    if(o_CheckIfFoundPair != null)
                    {
                         if(o_CheckIfFoundPair.FirstLocation != i_LocationFound)
                         {
                              cellToReturn = o_CheckIfFoundPair.FirstLocation;
                         }
                    }

                    return cellToReturn;
               }

               public Node SawPairOfCards
               {
                    get
                    {
                         if(m_Saw2Spots.Count == 0)
                         {
                              return null;
                         }
                         else
                         {
                              return m_Saw2Spots.Peek();
                         }
                    }
               }

               public void DeleteSpot()
               {
                    m_Saw2Spots.Dequeue();
               }

               public string ReturnRandomCell()
               {
                    Random randomKey = new Random();
                    Dictionary<int, string>.KeyCollection keys = m_PickOptions.Keys;
                    string keyToReturn = null;
                    int randomKeyToReturn = randomKey.Next(keys.Count);
                    int i = 0;
                    foreach(int value in keys)
                    {
                         if(i == randomKeyToReturn)
                         {
                              m_PickOptions.TryGetValue(value, out keyToReturn);
                              break;
                         }

                         i++;
                    }

                    return keyToReturn;
               }

               private string makeCellFromInt(int i_Row, int i_Col)
               {
                    int convert = i_Row + 1;
                    string strToReturn = string.Format("{0}{1}", Convert.ToChar(i_Col + 'A'), convert);
                    return strToReturn;
               }

               public void AddCardPcSaw(string i_Value, char i_Key)
               {
                    if(m_OpenCards.ContainsKey(i_Key))
                    {
                         if(m_OpenCards[i_Key].SecondLocation == null && m_OpenCards[i_Key].FirstLocation != i_Value)
                         {
                              m_OpenCards[i_Key].SecondLocation = i_Value;
                              m_Saw2Spots.Enqueue(m_OpenCards[i_Key]);
                         }
                    }
                    else
                    {
                         m_OpenCards.Add(i_Key, new Node(i_Value));
                    }
               }

               public void RemoveOpenedCards(int i_CardRow1, int i_CardCol1, int i_CardRow2, int i_CardCol2, int i_MaxCols)
               {
                    int key1 = (i_CardRow1 * i_MaxCols) + i_CardCol1;
                    int key2 = (i_CardRow2 * i_MaxCols) + i_CardCol2;
                    m_PickOptions.Remove(key1);
                    m_PickOptions.Remove(key2);
                    removeFromQueue(i_CardRow1, i_CardCol1, i_MaxCols);
               }

               private void removeFromQueue(int i_Row, int i_Col, int i_NumOfCols)
               {
                    Node nodeToCheck = null;
                    string cell = makeCellFromInt(i_Row, i_Col);
                    Queue<Node> check = new Queue<Node>();
                    int queueSize = m_Saw2Spots.Count;
                    for(int i = 0; i < queueSize; i++)
                    {
                         nodeToCheck = m_Saw2Spots.Dequeue();
                         if(nodeToCheck.FirstLocation != cell && nodeToCheck.SecondLocation != cell)
                         {
                              check.Enqueue(nodeToCheck);
                         }
                    }

                    m_Saw2Spots = check;
               }

               public class Node
               {
                    private string m_FirstLocation;
                    private string m_SecondLocation;

                    public Node(string i_Value)
                    {
                         m_FirstLocation = i_Value;
                         m_SecondLocation = null;
                    }

                    public Node(string i_FirstValue, string i_SecondValue)
                    {
                         m_FirstLocation = i_FirstValue;
                         m_SecondLocation = i_SecondValue;
                    }

                    public string FirstLocation
                    {
                         get
                         {
                              return m_FirstLocation;
                         }

                         set
                         {
                              m_FirstLocation = value;
                         }
                    }

                    public string SecondLocation
                    {
                         get
                         {
                              return m_SecondLocation;
                         }

                         set
                         {
                              m_SecondLocation = value;
                         }
                    }
               }
          }

          public enum eGameMode
          {
               OnePlayer = 1, TwoPlayers = 2
          }

          public enum eTurn
          {
               Player1 = 0, Player2 = 1
          }

          public Node[,] Data
          {
               get
               {
                    return m_Data;
               }

               set
               {
                    m_Data = value;
               }
          }

          public Player Player1
          {
               get
               {
                    return m_Player1;
               }

               set
               {
                    m_Player1 = value;
               }
          }

          public Player Player2
          {
               get
               {
                    return m_Player2;
               }

               set
               {
                    m_Player2 = value;
               }
          }

          public void ResetGame(int i_Rows, int i_Cols)
          {
               m_Player1.Points = 0;
               m_Player2.Points = 0;
               Turn = eTurn.Player1;
               m_Data = new Node[i_Rows, i_Cols];

               for(int i = 0; i < i_Rows; i++)
               {
                    for(int j = 0; j < i_Cols; j++)
                    {
                         if(m_Data[i, j] == null)
                         {
                              m_Data[i, j] = new Node();
                         }
                         else
                         {
                              m_Data[i, j].CardIndex = 0;
                              m_Data[i, j].IsFaceUp = false;
                              m_Data[i, j].IsCurrPick = false;
                        m_Data[i, j].Pair = 0;
                         }
                    }
               }

               resetArray(i_Rows, i_Cols);
          }

          private void resetArray(int i_Rows, int i_Cols)
          {
               int total = i_Rows * i_Cols, firstItem = 0, secondItem = 0;
               int[] indexes = new int[total / 2];
               int[] numbers = new int[total];
               initializeArray(indexes);
               initializeArray(numbers);
               randomArray(numbers);

            for(int i = 0, j = 0; i < total; i++, j++)
            {
                firstItem = numbers[i++];
                secondItem = numbers[i];
                m_Data[firstItem / i_Cols, firstItem % i_Cols].Pair = secondItem;
                m_Data[secondItem / i_Cols, secondItem % i_Cols].Pair = firstItem;
                m_Data[firstItem / i_Cols, firstItem % i_Cols].IsFaceUp = false;
                m_Data[secondItem / i_Cols, secondItem % i_Cols].IsFaceUp = false;
                m_Data[firstItem / i_Cols, firstItem % i_Cols].IsCurrPick = false;
                m_Data[secondItem / i_Cols, secondItem % i_Cols].IsCurrPick = false;
                m_Data[firstItem / i_Cols, firstItem % i_Cols].CardIndex = indexes[j];
                m_Data[secondItem / i_Cols, secondItem % i_Cols].CardIndex = indexes[j];
            }
          }

          public bool IsGameFinished()
          {
               bool isFinish = false;

               int totalPlayersPoints = m_Player1.Points + m_Player2.Points;
               int maxPoints = (m_Data.GetLength(0) * m_Data.GetLength(1)) / 2;

               if(totalPlayersPoints == maxPoints)
               {
                    isFinish = true;
               }

               return isFinish;
          }

          private void initializeArray(int[] i_Array)
          {
               for(int i = 0; i < i_Array.Length; i++)
               {
                    i_Array[i] = i;
               }
          }

          private void randomArray(int[] i_Array)
          {
               Random rnd = new Random();
               int randomNumber = 0, temp = 0;
               for(int i = 0; i < i_Array.Length; i++)
               {
                    randomNumber = rnd.Next(i_Array.Length - 1);
                    temp = i_Array[i];
                    i_Array[i] = i_Array[randomNumber];
                    i_Array[randomNumber] = temp;
               }
          }

          public eGameMode GameMode
          {
               get
               {
                    return m_GameMode;
               }

               set
               {
                    m_GameMode = value;
               }
          }

          public eTurn Turn
          {
               get
               {
                    return m_Turn;
               }

               set
               {
                    m_Turn = value;
               }
          }

          public bool PlayTurn(int i_CellI1, int i_CellJ1, int i_CellI2, int i_CellJ2)
          {
               bool isPair = false;

               if(m_Data[i_CellI1, i_CellJ1].CardIndex == m_Data[i_CellI2, i_CellJ2].CardIndex)
               {
                    isPair = true;
                    addPoint();
               }
               else
               {
                    switchTurn();
               }

               return isPair;
          }

          private void switchTurn()
          {
               if(Turn == eTurn.Player1)
               {
                    Turn = eTurn.Player2;
               }
               else
               {
                    Turn = eTurn.Player1;
               }
          }

          private void addPoint()
          {
               if(Turn == eTurn.Player1)
               {
                    m_Player1.Points++;
               }
               else
               {
                    m_Player2.Points++;
               }
          }
     }
}