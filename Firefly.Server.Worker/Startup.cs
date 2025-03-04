namespace Firefly.Server.Worker
{
    public static class Startup
    {

        public static async void Run() {
            // Spawn one MasterWorker. Originally planned to spawn off a seperate thread, but as this thread will just sit idle, better to utilise the main thread for Master Worker.
            var masterWorker = new MasterWorker();
            await masterWorker.Start();
        }

    }
}
