namespace io.prediction.example
{
    public class MainClass
    {
        public static void Main(string[] args)
        {
            var opType = args.Length > 0 ? args[0] : "0";
            var accessKey = args.Length > 0 ? args[1] : "Sieb00r5aX9SABGEJSK8SXFW1y9myisAl3CBfsKQviXo3sKoP9cYzdMv9nJg64Vh";
            if (opType == "1")
            {
                new EventExample().Run(accessKey);
            }
            else
            {
                new EngineExample().Run(accessKey);
            }
        }
    }
}
