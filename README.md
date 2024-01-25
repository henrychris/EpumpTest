# FuelMetrics Test

## Description

The aim was to create a _mini_ loyalty program. The user can set the reward parameters and the minimum number of points that can be redeemed. In this program, the user can add a set of actions and the rewards to be issued to the user.  
After setup, a simulation runs where randomly generated customers perform random actions and accrue reward points.

## Installation

```bash
git clone https://github.com/henrychris/EpumpTest.git
```

```bash
cd EpumpTest
```

```bash
dotnet run
```

## Usage

The user will receive a set of prompts when the program executes:

```bash
✔ Please enter the number of actions you would like to simulate: 1
✔ Please enter the name of action 1: Survey
✔ How many points would Survey reward a user? (integer): 10

NOTE: A Purchase action was added. This action allows a user to purchase a product and receive points based on the price of the product.

✔ How many rounds should be simulated? : 3
✔ How many users should be generated for this simulation? : 3
✔ How many products should be created for this simulation? : 2
✔ How many points should a user have before they can redeem them? : 10
```

After input, the simulation runs, logging the results for each round. At the end, the final points tally is listed for each generated user.

## Note

As mentioned in the **Usage** section, a _Purchase_ action is automatically added to the actions set. This action randomly purchases an item, and the user will accrue a percentage of the price in reward points.
