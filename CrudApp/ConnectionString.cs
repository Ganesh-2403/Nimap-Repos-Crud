namespace CrudApp
{
    public static class ConnectionString
    {
        private static string cs = "Server=MSI\\SQLEXPRESS; Database=CRUD; Trusted_Connection=True";
        public static string Dbcs { get => cs; }
    }
}
