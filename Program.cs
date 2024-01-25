using Bogus;
using EpumpTest;
using Sharprompt;

static List<Product> CreateProducts(int numberOfProducts)
{
	var fake = new Faker<Product>()
		.RuleFor(p => p.Name, f => f.Commerce.ProductName())
		.RuleFor(p => p.Price, f => f.Random.Int(10, 100));

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

void Simulate(int rounds, int numberOfUsers, int numberOfProducts, int minRedeemablePoints, Dictionary<int, EpumpTest.Action> actions)
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
			var action = actions[rng.Next(1, actions.Count + 1)];
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

Dictionary<int, EpumpTest.Action> PromptUserForActions()
{
	var actions = new Dictionary<int, EpumpTest.Action>();
	var actionCount = 1;

	var numberOfActions = Prompt.Input<int>("Please enter the number of actions you would like to simulate");

	while (actionCount <= numberOfActions)
	{
		var actionName = Prompt.Input<string>($"Please enter the name of action {actionCount}");
		var actionRewardValue = Prompt.Input<int>($"How many points would {actionName} reward a user? (integer)");

		actions.Add(actionCount, new EpumpTest.Action { Name = actionName, RewardValue = actionRewardValue, IsPercentage = false });
		actionCount++;
	}

	return actions;
}

var actionSet = PromptUserForActions();
actionSet.Add(actionSet.Count + 1, new EpumpTest.Action { Name = "Purchase", RewardValue = 10, IsPercentage = true });
Console.WriteLine("NOTE: A Purchase action was added. This action allows a user to purchase a product and receive points based on the price of the product.");

// list the actions that will be simulated
Console.WriteLine("\nActions to be simulated:");
foreach (var action in actionSet)
{
	Console.WriteLine($"{action.Key}. {action.Value.Name} - {action.Value.RewardValue}{(action.Value.IsPercentage ? " percent of the product's price" : " points")}");
}

var rounds = Prompt.Input<int>("How many rounds should be simulated? ");
var numberOfUsers = Prompt.Input<int>("How many users should be generated for this simulation? ");
var numberOfProducts = Prompt.Input<int>("How many products should be created for this simulation? ");
var minRedeemablePoints = Prompt.Input<int>("How many points should a user have before they can redeem them? ");

Simulate(rounds,
		 numberOfUsers,
		 numberOfProducts,
		 minRedeemablePoints,
		 actionSet);