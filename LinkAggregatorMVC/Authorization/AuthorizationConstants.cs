﻿namespace LinkAggregatorMVC.Authorization
{
    public class AuthorizationConstants
    {
        public static readonly string CreateOperationName = "Create";
        public static readonly string ReadOperationName = "Read";
        public static readonly string UpdateOperationName = "Update";
        public static readonly string DeleteOperationName = "Delete";
        public static readonly string ApproveOperationName = "Approve";
        public static readonly string RejectOperationName = "Reject";

        public static readonly string AdministratorRole ="Administrator";
        public static readonly string PowerUserRole = "PowerUser";
        public static readonly string UserRole = "User";

    }
}
