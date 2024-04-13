namespace HotelFuen31.APIs.Uitilities
{
    public class EmailTemplate
    {
        public static string General(string title, string content)
        {
            return $@"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
<meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
<title>Demystifying Email Design</title>
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0""/>
</head>
<body>
<table align=""center""  ellpadding=""0"" cellspacing=""0"" width=""600"">
    <tr>
        <td align=""center"" bgcolor=""#9b7c64"" style=""color: white; padding: 40px 0 30px 0;  font-family: Arial, sans-serif; font-size: 24px;"">
        <h1>
        <b>Kalsari Hotel</b>
        </h1>
        </td>
    </tr>
    <tr>
        <td bgcolor=""#ffffff"" style=""padding: 40px 30px 40px 30px;"">
        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
            <tr>
                <td>
                <h2>
                    {title}
                </h2>
                </td>
            </tr>
            <tr>
                <td style=""padding: 20px 0 30px 0;  font-family: Arial, sans-serif; font-size: 16px;"">
                {content}
                </td>
            </tr>
        </table>
        </td>
    </tr>
    <tr>
        <td bgcolor=""#00000"" style=""color: white; padding: 30px 30px 30px 30px;  font-family: Arial, sans-serif; font-size: 12px;"">
        &reg; Kalsalri Hotel 2024
        </td>
    </tr>
    </table>
</body>
</html>
";
        }
        public static string Validation(string url)
        {
            return $@"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
<meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
<title>Demystifying Email Design</title>
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0""/>
</head>
<body>
<table align=""center""  ellpadding=""0"" cellspacing=""0"" width=""600"">
    <tr>
        <td align=""center"" bgcolor=""#9b7c64"" style=""color: white; padding: 40px 0 30px 0;  font-family: Arial, sans-serif; font-size: 24px;"">
        <h1>
        <b>Kalsari Hotel</b>
        </h1>
        </td>
    </tr>
    <tr>
        <td bgcolor=""#ffffff"" style=""padding: 40px 30px 40px 30px;"">
        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
            <tr>
                <td>
                <h2>
                    會員認證信
                </h2>
                </td>
            </tr>
            <tr>
                <td style=""padding: 20px 0 30px 0;  font-family: Arial, sans-serif; font-size: 16px;"">
                請點擊以下連結以完成會員認證
                </td>
            </tr>
            <tr>
                <td>
                <a href=""{url}"" style=""background-color: #9b7c64; color:#ffffff;border-radius:4px;display:inline-block;font-size:16px;font-weight:bold;letter-spacing:1px; padding: 7px 60px;text-decoration: none ;"">會員認證</a>
                </td>
            </tr>
        </table>
        </td>
    </tr>
    <tr>
        <td bgcolor=""#00000"" style=""color: white; padding: 30px 30px 30px 30px;  font-family: Arial, sans-serif; font-size: 12px;"">
        &reg; Kalsalri Hotel 2024
        </td>
    </tr>
    </table>
</body>
</html>
";
        }
        public static string ResetPwd(string url)
        {
            return $@"
<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
<meta http-equiv=""Content-Type"" content=""text/html; charset=UTF-8"" />
<title>Demystifying Email Design</title>
<meta name=""viewport"" content=""width=device-width, initial-scale=1.0""/>
</head>
<body>
<table align=""center""  ellpadding=""0"" cellspacing=""0"" width=""600"">
    <tr>
        <td align=""center"" bgcolor=""#9b7c64"" style=""color: white; padding: 40px 0 30px 0;  font-family: Arial, sans-serif; font-size: 24px;"">
        <h1>
        <b>Kalsari Hotel</b>
        </h1>
        </td>
    </tr>
    <tr>
        <td bgcolor=""#ffffff"" style=""padding: 40px 30px 40px 30px;"">
        <table cellpadding=""0"" cellspacing=""0"" width=""100%"">
            <tr>
                <td>
                <h2>
                    密碼重置
                </h2>
                </td>
            </tr>
            <tr>
                <td style=""padding: 20px 0 30px 0;  font-family: Arial, sans-serif; font-size: 16px;"">
                請點擊以下連結以進行重置密碼。
                </td>
            </tr>
            <tr>
                <td>
                <a href=""{url}"" style=""background-color: #9b7c64; color:#ffffff;border-radius:4px;display:inline-block;font-size:16px;font-weight:bold;letter-spacing:1px; padding: 7px 60px;text-decoration: none ;"">重置密碼</a>
                </td>
            </tr>
            <tr>
                <td style=""padding: 20px 0 30px 0;  font-family: Arial, sans-serif; font-size: 16px;"">
                如未進行該項動作，請無視此信件，並請向本飯店回報。
                </td>
            </tr>
        </table>
        </td>
    </tr>
    <tr>
        <td bgcolor=""#00000"" style=""color: white; padding: 30px 30px 30px 30px;  font-family: Arial, sans-serif; font-size: 12px;"">
        &reg; Kalsalri Hotel 2024
        </td>
    </tr>
    </table>
</body>
</html>
";
        }
    }
}
