namespace Movie.BuildingBlocks
{

    public enum UserRoles : byte
    {
        USER,
        ADMIN
    }

    public static class StaticDetails
    {
        public static IDictionary<UserRoles, string> userRolesDict = 
            new Dictionary<UserRoles, string>
            {
                {UserRoles.USER, "User" },
                {UserRoles.ADMIN, "Admin" }
            };

        public static string AccessToken = "JWTToken";
        public static string RefreshToken = "RefreshToken";
    }
}
