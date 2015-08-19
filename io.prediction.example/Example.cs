using System;
using System.Collections.Generic;

namespace io.prediction.example
{
    public class Example
    {
        public static void Main(String[] args)
        {
            var accessKey = args.Length > 0 ? args[0] : "AEEASHv5XNkTHL4AzyoE1Hu9gTrDP2AFa7qXEQfWA5y33KdO5Hk5sNExJkW5udWX";
            Console.WriteLine("Running");
            var client = new EventClient(accessKey);
            var rand = new Random();
            var emptyProperty = new Dictionary<string, object>();

            // generate 10 users, with user ids 1 to 10
            for (int user = 1; user <= 10; user++)
            {
                Console.WriteLine("Add user " + user);
                client.DeleteUser("i"+user);
            }

            // generate 50 items, with item ids 1 to 50
            for (int item = 1; item <= 50; item++)
            {
                Console.WriteLine("Add item " + item);
                client.DeleteItem("" + item);
            }

            // each user randomly views 10 items
            for (int user = 1; user <= 10; user++)
            {
                for (int i = 1; i <= 10; i++)
                {
                    int item = rand.Next(50) + 1;
                    Console.WriteLine("User " + user + " views item " + item);
                    client.UserActionItem("view", "" + user, "" + item, emptyProperty);
                }
            }

            //client.Dispose();
            var engineClient = new EngineClient();
            var dic = new Dictionary<string, object>();
            dic.Add("user", 1);
            dic.Add("num", 4);
            try
            {
                var result = engineClient.SendQuery<ItemScoresModel>(dic);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
