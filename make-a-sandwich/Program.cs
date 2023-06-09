Dictionary<string, int> ingredients = new Dictionary<string, int>()
    {
        { "Bread", 66 },
        { "Ham", 72 },
        { "Bologna", 57 },
        { "Chicken", 17 },
        { "Corned Beef", 53 },
        { "Salami", 40 },
        { "Cheese, American", 104 },
        { "Cheese, Cheddar", 113 },
        { "Cheese, Havarti", 105 },
        { "Mayonnaise", 94 },
        { "Mustard", 10 },
        { "Butter", 102 },
        { "Garlic Aioli", 100 },
        { "Sriracha", 15 },
        { "Dressing, Ranch", 73 },
        { "Dressing, 1000 Island", 59 },
        { "Lettuce", 5 },
        { "Tomato", 4 },
        { "Cucumber", 4 },
        { "Banana Pepper", 10 },
        { "Green Pepper", 3 },
        { "Red Onion", 6 },
        { "Spinach", 7 },
        { "Avocado", 64 }
    };
ingredients = ingredients.ToDictionary(k => k.Key.ToUpper(), k => k.Value);
bool minInput = false;
int minCalories = 0;
bool maxInput = false;
int maxCalories = 0;
while (!minInput)
{
    Console.WriteLine("Enter the minimum number of calories you would like in your sandwich:");
    string minCaloriesInput = Console.ReadLine();
    minInput = Int32.TryParse(minCaloriesInput, out minCalories);
    int min = ingredients.MinBy(k => k.Value).Value;
    if (minCalories < ingredients["BREAD"] * 2 + min)
    {
        Console.WriteLine("The minimum calories is to low to make a sandwich");
        minInput = false;
    }
}
while (!maxInput)
{
    Console.WriteLine("Enter the maximum number of calories you would like in your sandwich:");
    string maxCaloriesInput = Console.ReadLine();
    maxInput = Int32.TryParse(maxCaloriesInput, out maxCalories);
    if (minCalories > maxCalories)
    {
        Console.WriteLine("The maximum number must be greater than minmum.");
        maxInput = false;
    }
}
List<string> excludedIngredients = new List<string>();
string allIngredient = "";
do
{
    Console.WriteLine("Do you want to exclude any ingredients? (Press enter to skip or end)");
    allIngredient = Console.ReadLine();
    if (!string.IsNullOrEmpty(allIngredient))
    {
        string[] ingredient = allIngredient.Split(',');
        for (int i = 0; i < ingredient.Length; i++)
        {
            if (ingredient[i].ToUpper().Trim().Contains("BREAD"))
            {
                Console.WriteLine("Sandwiches must include bread.");
                break;
            }
            bool validIngredient = ingredients.ContainsKey(ingredient[i].ToUpper().Trim());
            if (validIngredient)
            {
                excludedIngredients.Add(ingredient[i].ToUpper().Trim());
            }
        }
    }
} while (!string.IsNullOrEmpty(allIngredient));
Console.WriteLine("\nExcluding ingredients:");
if (excludedIngredients.Count == 0)
{
    Console.Write("None");
}
else
{
    foreach (string str in excludedIngredients)
    {
        Console.Write(str + ", ");
    }
}
Console.WriteLine("\nMaking your sandwich\n");
List<string> sandwich = new List<string>();
int breadCalories = ingredients["BREAD"];
int currentCalories = breadCalories * 2;
excludedIngredients.Add("BREAD");
// https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.where?view=net-6.0
ingredients = ingredients.Where(k => !excludedIngredients.Contains(k.Key)).ToDictionary(k => k.Key, k => k.Value);
// https://learn.microsoft.com/en-us/dotnet/api/system.random?view=net-6.0
Random random = new Random();
bool cannotBeAdd = false;
string lastIngredient = "";
while (currentCalories <= maxCalories && ingredients.Count > 0 && !cannotBeAdd)
{
    // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.elementat?view=net-6.0
    string randomIngredient = ingredients.Keys.ElementAt(random.Next(ingredients.Count));
    int ingredientCalories = ingredients[randomIngredient];
    if (currentCalories + ingredientCalories <= maxCalories && randomIngredient != lastIngredient)
    {
        sandwich.Add(randomIngredient);
        lastIngredient = randomIngredient;
        currentCalories += ingredientCalories;
    }
    cannotBeAdd = ingredients.Values.All(num => num > maxCalories - currentCalories);
}
while (currentCalories < minCalories && sandwich.Count > 0)
{
    string removedIngredient = sandwich.Last();
    int removedIngredientCalories = ingredients[removedIngredient];
    sandwich.Remove(removedIngredient);
    currentCalories -= removedIngredientCalories;
}
if (sandwich.Count == 0)
{
    Console.WriteLine("It's not possible to create a sandwich within the given calorie range.");
}
else
{
    int totalCalories = sandwich.Sum(ingredient => ingredients[ingredient]) + breadCalories * 2;
    Console.WriteLine($"Adding bread (66 calories)");
    foreach (string str in sandwich)
    {
        Console.WriteLine($"Adding {str.ToLower()} ({ingredients[str]} calories)");
    }
    Console.WriteLine($"Adding bread (66 calories)");
    Console.WriteLine($"Your sandwich, with {totalCalories} calories, is ready. Enjoy!");
}