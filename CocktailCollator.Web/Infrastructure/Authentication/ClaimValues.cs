namespace CocktailCollator.Web.Infrastructure.Authentication;

public static class ClaimValues
{
#pragma warning disable IDE1006
    public static class Permissions
    {
        public static class Users
        {
            public const string ChangePassword = "Permissions.Users.ChangePassword";
            public const string Manage = "Permissions.Users.Manage";
            public const string View = "Permissions.Users.View";
        }

        public static class Recipes
        {
            public const string Manage = "Permissions.Recipes.Manage";
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
#pragma warning restore IDE1006

    public static List<string> GetAllClaims()
    {
        var _Claims = new List<string>();
        var _PermissionClasses = typeof(Permissions).GetNestedTypes();
        foreach (var _PermissionClass in _PermissionClasses)
        {
            var _Fields = _PermissionClass.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.FlattenHierarchy);
            foreach (var _Field in _Fields)
            {
                if (_Field.IsLiteral && !_Field.IsInitOnly && _Field.FieldType == typeof(string))
                    _Claims.Add((string)_Field.GetRawConstantValue()!);
            }
        }
        return _Claims;
    }
}
