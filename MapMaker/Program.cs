
namespace MapMaker {
	internal class Program {
		[System.STAThread]
		static void Main(string[] args) {
			using var maker = new global::MapMaker.MapMaker();
			maker.Run();
		}
	}
}
