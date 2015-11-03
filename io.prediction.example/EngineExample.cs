using System;

namespace io.prediction.example
{
    public class EngineExample
    {
        public void Run(string accessKey)
        {
            Console.WriteLine("EngineExample Running");
            var engineClient = new EngineClient(accessKey);

            try
            {
                var result = engineClient.Get("1", 10, new[] { "1" });
                foreach (var res in result.ItemScores)
                {
                    Console.WriteLine("Sync:" + res.Item + " " + res.Score);
                }
                var asyncResult = engineClient.GetAsync("1", 10, new[] { "1" }).Result;
                foreach (var res in asyncResult.ItemScores)
                {
                    Console.WriteLine("Async:" + res.Item + " " + res.Score);
                }
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}
