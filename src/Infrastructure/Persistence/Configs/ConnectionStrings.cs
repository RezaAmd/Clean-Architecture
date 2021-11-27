namespace Infrastructure.Persistence.Configs
{
    class ConnectionStrings
    {
        private static string ipAddress = @"IP_ADDRESS"; // Local

        #region Identity Connection String
        private static string defaultCatalog = "CATALOG";
        private static string defaultUser = "USER";
        private static string defaultPassword = "PASSWORD";
        public static string Identity = $"Data Source={ipAddress};Initial Catalog={defaultCatalog};User ID={defaultUser};Password={defaultPassword};Persist Security Info=True;";
        #endregion        
    }
}