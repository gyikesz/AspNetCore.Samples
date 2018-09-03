namespace AspNetCore.SimpleApi
{
    public static class Api
    {
        public static class Routes
        {
            public const string VERSIONED_API_ROUTE = "api/v{version:apiVersion}/[controller]";
        }

        public static class Versions
        {
            public const string V1 = "1.0";
            public const string V1_NAME = "v1.0";
        }
    }
}
