using System;

namespace Memory_card_game
{
     public class Program
     {
          public static void Main()
          {
               UI memoryGame = new UI();
               memoryGame.Run();
               Console.Read();
          }
     }
}
