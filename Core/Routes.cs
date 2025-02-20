namespace Core;

public static class Routes
{
    private const string BaseUrl = "/";

    public static class Customer
    {
        private const string CustomersBaseUrl = BaseUrl + "customers";
        public const string Add = CustomersBaseUrl + "/add";
        public const string List = CustomersBaseUrl;
        public const string Details = CustomersBaseUrl + "/{id:int}";
    }

    public static class Product
    {
        private const string ProductsBaseUrl = BaseUrl + "products";
        public const string Add = ProductsBaseUrl + "/add";
        public const string List = ProductsBaseUrl;
        public const string Details = ProductsBaseUrl + "/{id:int}";
    }
}