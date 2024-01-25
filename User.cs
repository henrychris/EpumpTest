namespace EpumpTest
{
    public class User
    {
        public User()
        {

        }

        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public double Points { get; private set; }

        public void PerformAction(Action action)
        {
            Console.Write($"Previous points: {Points}.");
            Points += action.RewardValue;
            Console.Write($" New points: {Points}.\n\n");
        }

        public void PerformAction(Action action, double price)
        {
            Console.Write($"Previous points: {Points}.");

            if (action.IsPercentage)
            {
                var v = Math.Floor(price * action.RewardValue / (double)100);
                Points += v;
            }
            Console.Write($" New points: {Points}.\n\n");
        }

        public void RedeemPoints(int pointsToRedeem)
        {
            Console.Write($"Previous points: {Points}.");
            Points -= pointsToRedeem;
            Console.Write($" New points: {Points}.\n\n");
        }
    }
}