namespace CocktailCollator.Web.Infrastructure.Authentication;

public static class Permissions
{
    public static class Users
    {
        public const string Manage = "Permissions.Users.Manage";
    }

    public static class Ingredients
    {
        public const string View = "Permissions.Ingredients.View";
        public const string Manage = "Permissions.Ingredients.Manage";
    }

    public static class Measurements
    {
        public const string View = "Permissions.Measurements.View";
        public const string Manage = "Permissions.Measurements.Manage";
    }
}
