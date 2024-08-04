
using Game;

var server = new Server();

server.Init();
server.Start();

Console.ReadKey();

server.Stop();
