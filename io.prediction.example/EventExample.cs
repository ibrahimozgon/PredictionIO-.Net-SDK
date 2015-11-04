using System;

namespace predictionIO.example
{
    public class EventExample
    {
        public void Run(string accessKey)
        {
            Console.WriteLine("EventExample Running");
            var client = new EventClient(accessKey);

            var rand = new Random();

            // generate 10 users, with user ids 1 to 10
            for (var user = 1; user <= 10; user++)
            {
                Console.WriteLine("Add user " + user);
                var result = client.SetUser(user.ToString());
            }

            // generate 50 items, with item ids 1 to 50
            for (var item = 50; item <= 100; item++)
            {
                Console.WriteLine("Add item " + item);
                client.SetItemWithCategory(item.ToString(), new[] { "1" });
            }

            // each user randomly views 10 items
            for (var user = 1; user <= 10; user++)
            {
                for (var i = 1; i <= 10; i++)
                {
                    var item = rand.Next(100) + 51;
                    Console.WriteLine("User " + user + " views item " + item);
                    client.UserViewedItem(user.ToString(), item.ToString());
                }
                for (var i = 1; i <= 10; i++)
                {
                    var item = rand.Next(100) + 51;
                    Console.WriteLine("User " + user + " views item " + item);
                    client.UserBoughtItem(user.ToString(), item.ToString());
                }
            }
            Console.ReadLine();
        }
    }
}
