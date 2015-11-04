# PredictionIO-.Net-SDK
PredictionIO-.Net-SDK

Thanks https://github.com/orbyone/Sensible.PredictionIO.NET. 

Event Client Usage: 

            var client = new EventClient(accessKey);

            var rand = new Random();

            // generate 10 users, with user ids 1 to 10
            for (var user = 1; user <= 10; user++)
            {
                Console.WriteLine("Add user " + user);
				//Sync
                var result = client.SetUser(user.ToString());
				//Async
				 var result = client.SetUserAsync(user.ToString(), null).Result;
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
			
Engine Client Usage:


				//Sync:
                var result = engineClient.Get("1", 10, new[] { "1" });
				//Async :
				var asyncResult = engineClient.GetAsync("1", 10, new[] { "1" }).Result;
                foreach (var res in result.ItemScores)
                {
                    Console.WriteLine("Sync:" + res.Item + " " + res.Score);
                }
We plan to support other templates like "Universal Recommender", "Product Ranking" etc.

You can visit all open source templates here: [here](https://templates.prediction.io/).