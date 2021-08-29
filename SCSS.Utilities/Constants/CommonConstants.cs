using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Constants
{
    public class BooleanConstants
    {
        public const bool TRUE = true;
        public const bool FALSE = false;
    }

    public class SMSMesage
    {
        public static string SMSForRestorePasswod(string otp) => $"Mã OTP để khôi phục mật khẩu của bạn là {otp}";

        public static string SMSForOTPRegister(string otp) => $"Mã OTP để đăng kí tài khoản của bạn là {otp}";

        public static string SMSForLogin(string otp) => $"Mã OTP để đăng nhập là {otp}";
    }


    public class CommonsConstants
    {
        public const string MessageCode = "msgCode";
    }

    public class AccountStatus
    {
        public const int NOT_APPROVED = 1;
        public const int ACTIVE = 2;
        public const int BANNING = 3;

        public static readonly List<int> StatusCollection = new List<int>()
        {
            NOT_APPROVED,
            ACTIVE,
            BANNING,
        };
    }


    public class ClientIdConstant
    {
        public const string SellerMobileApp = "SCSS-Seller-Mobile";
        public const string CollectorMobileApp = "SCSS-Collector-Mobile";
        public const string DealerMobileApp = "SCSS-Dealer-Mobile";
        public const string WebAdmin = "SCSS-WebAdmin-FrontEnd";
    }


    public class IdentityServer
    {
        public static string ClientId { get; set; }

        public const string GrantType = "phone_number_token";
        public const string VerifyOTP = "verify_otp_number";
        public const string RestorePassword = "resend_token_restore_password";
    }


    public class AccountRoleConstants
    {
        public const string ADMIN = "Admin";
        public const string SELLER = "Seller";
        public const string COLLECTOR = "Collector";
        public const string DEALER = "Dealer";
    }

    public class RegularExpression
    {
        public const string PhoneRegex = @"(84|0[3|5|7|8|9])+([0-9]{8})\b";
        public const string EmailRegex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$";
    }
}
