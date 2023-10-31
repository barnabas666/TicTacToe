namespace ConsoleUI;

internal class Program
{
    static void Main(string[] args)
    {
        Player player1 = new Player() { Name = "Barnabas", BattleStone = "x" };
        Player player2 = new Player() { Name = "Dennis", BattleStone = "o" };
        TicTacToeBattle ticTacToeBattle = new TicTacToeBattle(5, player1, player2);  

        ticTacToeBattle.Play();
    }
}
