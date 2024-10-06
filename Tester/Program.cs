
namespace Tester;

internal class Program {
	[System.STAThread]
	public static void Main() {
		using Tester game = new();
		game.Run();
	}
}
