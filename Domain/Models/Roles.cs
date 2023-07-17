namespace Domain.Models
{
    public static class Roles
    {
        /// <summary>
        /// ADMIN can create and company OWN_COMAPANY
        /// </summary>
        public const string ADMIN = "admin";
        /// <summary>
        /// OWN_COMPANY
        /// </summary>
        public const string OWN_COMPANY = "own_company";
        /// <summary>
        /// MANAGER - assistant for OWN_COMPANY
        /// </summary>
        public const string MANAGER = "manager";
        /// <summary>
        /// WORKER - just a company employee
        /// </summary>
        public const string WORKER = "worker";
        /// <summary>
        /// CLIENT is a client
        /// </summary>
        public const string CLIENT = "client";
    }
}
