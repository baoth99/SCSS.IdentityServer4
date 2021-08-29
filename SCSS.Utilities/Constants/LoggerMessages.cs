namespace SCSS.Utilities.Constants
{
    public class LoggerMessages
    {
        public static string SendSMSSuccess(string content, string phoneTo) => $"SMS {content} sended to {phoneTo} successfully";

        public static string SendSMSFail(string content, string phoneTo) => $"SMS {content} sended to {phoneTo} fail";

        public static string RegisterTokenError(string phone) => $"RegisterToken is wrong or expired with {phone}";

        public static string PhoneNotMatch(string phone, string func) => $"{func},Phone {phone} is wrong format";

        public static string PhoneIsExisted(string phone, string func) => $"{func},Phone {phone} is existed in system";

        public static string RegisterUserSucess(string phone, string name, string role) => $"User with name: {name}, phone: {phone} has created with {role} successfully";

        public static string RegisterUserFail(string phone, string name, string role) => $"User with name: {name}, phone: {phone} has created with {role} fail";

        public static string UserNotFound(string id) => $"User {id} is not found";

        public static string UpdateUserSuccess(string id) => $"User {id} has updated successfully";

        public static string OTPInvalid(string type) => $"{type} OTP is invalid";

        public static string OTPConfirm(string type) => $"{type} OTP was confirm successfully !";

        public static string ClientIdInvalid => $"Account is invalid with client";

        public static string AccountStatus(string status) => $"Account was {status}";

        public static string AccStatusInvalid => $"Account status is invalid";

        public static string ChangeStatus(string id, string statusTo) => $"Change Account {id}'s status to {statusTo} successfully";

        public static string RestorePasswordSuccess(string id) => $"Account {id} has restored password sucessfully";
    }
}
