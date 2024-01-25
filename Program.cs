using Bogus;
using EpumpTest;

Dictionary<int, EpumpTest.Action> ActionSet = new()
{
	{ 1, new EpumpTest.Action { Name = "Social Share", RewardValue = 10, IsPercentage = false } },
	{ 2, new EpumpTest.Action { Name = "Survery", RewardValue = 5, IsPercentage = false } },
	{ 3, new EpumpTest.Action { Name = "Purchase", RewardValue = 10, IsPercentage = true } },
	{ 4, new EpumpTest.Action { Name = "Write Review", RewardValue = 5, IsPercentage = false } }
};

static List<Product> CreateProducts(int numberOfProducts)
{
	var fake = new Faker<Product>()
		.RuleFor(p => p.Name, f => f.Commerce.ProductName())
		.RuleFor(p => p.Price, f => f.Random.Int(100, 1000));

	var products = fake.Generate(numberOfProducts);
	return products;
}

static List<User> CreateUsers(int numberOfUsers)
{
	var fake = new Faker<User>()
		.RuleFor(u => u.FirstName, f => f.Name.FirstName())
		.RuleFor(u => u.LastName, f => f.Name.LastName());

	var users = fake.Generate(numberOfUsers);
	return users;
}

void Simulate(int rounds, int numberOfUsers, int numberOfProducts, int minRedeemablePoints)
{
	var rng = new Random();
	var users = CreateUsers(numberOfUsers);
	var products = CreateProducts(numberOfProducts);

	// for 10 rounds, where each user is iterated over once, select an action and perform it
	for (int i = 0; i < rounds; i++)
	{
		Console.WriteLine($"\nRound {i + 1}");
		foreach (var user in users)
		{
			var action = ActionSet[rng.Next(1, ActionSet.Count)];

			if (action.Name == "Purchase")
			{
				var product = products[rng.Next(0, products.Count)];
				Console.WriteLine($"{user.FirstName} {user.LastName} purchased {product.Name} for {product.Price}");
				user.PerformAction(action, product.Price);
				continue;
			}

			Console.WriteLine($"{user.FirstName} {user.LastName} performed {action.Name} worth {action.RewardValue} points");
			user.PerformAction(action);

			if (user.Points >= minRedeemablePoints)
			{
				var pointsToRedeem = rng.Next(1, (int)user.Points);
				Console.WriteLine($"{user.FirstName} {user.LastName} redeemed {pointsToRedeem} points");
				user.RedeemPoints(pointsToRedeem);
			}
		}
	}

	// after all rounds, print the total rewards for each user
	Console.WriteLine("\nSimulation Results:");
	foreach (var user in users)
	{
		Console.WriteLine($"{user.FirstName} {user.LastName} has {user.Points} points");
	}
}

Simulate(10, 3, 10, 100);