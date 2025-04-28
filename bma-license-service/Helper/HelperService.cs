using bma_license_repository.CustomModel.SysOrgranize;
using bma_license_repository.Helper;
using ClosedXML.Excel;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using static bma_license_repository.CustomModel.AppSetting.AppSettingConfig;

namespace bma_license_service.Helper
{
    public static class HelperService
    {
        public static async Task<DateTime> GetTokenExpiredDefault(JWTConfig _jwtConfig, DateTime dateTime, bool isSetDays)
        {
            if (isSetDays)
            {
                return dateTime.AddDays(int.Parse(_jwtConfig.TokenValidityInDays));
            }
            else
            {
                return dateTime.AddMinutes(int.Parse(_jwtConfig.TokenValidityInMinutes));
            }
        }

        public static string GenerateToken(string secretKey, string issuer, string audience, DateTime expireDay, Guid id, string role)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(role))
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Sid, id.ToString()));
                claims.Add(new Claim("roles", role));
            }
            else
            {
                throw new Exception("Role not set!!");
            }

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims.ToArray(),
                expires: expireDay, // Set expiration time here
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static Guid GenerateGuid()
        {
            Guid guid = Guid.NewGuid();

            return guid;
        }

        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower(); // Convert to hex string
            }
        }

        public static MemoryStream GenerateExcelFile(string dataJson, List<string> listColumn)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("Sheet1");

                worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Style.Font.FontSize = 16;
                worksheet.Style.Font.FontName = "TH SarabunPSK";

                int startrow = 1;
                int startcolumn = 1;
                int rowHeight = 20;
                #region | Set Header  |

                foreach (var item in listColumn)
                {
                    worksheet.Row(startrow).Height = rowHeight;

                    worksheet.Cell(startrow, startcolumn).Value = item;

                    worksheet.Cell(startrow, startcolumn).Style.Font.FontSize = 16;
                    worksheet.Cell(startrow, startcolumn).Style.Font.Bold = true;
                    startcolumn++;
                }
                startrow++;
                #endregion

                #region | Set Item |
                var listData = JsonConvert.DeserializeObject<List<dynamic>>(dataJson);

                foreach (var item in listData)
                {
                    startcolumn = 1;
                    var dict = item.ToObject<Dictionary<string, object>>();

                    foreach (var keyValue in dict)
                    {
                        if (keyValue.Value == null)
                        {
                            worksheet.Cell(startrow, startcolumn++).Value = "";
                        }
                        else
                        {
                            worksheet.Cell(startrow, startcolumn++).Value = keyValue.Value;
                        }
                    }

                    startrow++;
                }

                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();

                #endregion

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0; // Reset stream position to the beginning

                return stream;
            }
        }

        public static MemoryStream GenerateExcelFile(string dataJson, List<string> listColumn, int countData, string departmentName, string divisonName, string sectionName, string jobName)
        {
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.AddWorksheet("Sheet1");

                worksheet.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                worksheet.Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                worksheet.Style.Font.FontSize = 16;
                worksheet.Style.Font.FontName = "TH SarabunPSK";

                int startrow = 1;
                int startcolumn = 1;
                int rowHeight = 20;
                #region | Set Header  |

                worksheet.Row(startrow).Height = rowHeight;
                worksheet.Cell(startrow, startcolumn).Value = $"โครงการจัดซื้อลิขสิทธิ์โปรแกรมจัดการสำนักงาน จำนวน {countData} ลิขสิทธิ์";
                worksheet.Cell(startrow, startcolumn).Style.Font.FontSize = 16;
                worksheet.Cell(startrow, startcolumn).Style.Font.Bold = true;
                startrow++;

                worksheet.Row(startrow).Height = rowHeight;
                worksheet.Cell(startrow, startcolumn).Value = $"สำนัก / สำนักงานเขต : {departmentName}";
                worksheet.Cell(startrow, startcolumn).Style.Font.FontSize = 16;
                worksheet.Cell(startrow, startcolumn).Style.Font.Bold = true;
                startrow++;

                worksheet.Row(startrow).Height = rowHeight;
                worksheet.Cell(startrow, startcolumn).Value = $"สำนัก / กอง / ศูนย์ / เขต : {divisonName}";
                worksheet.Cell(startrow, startcolumn).Style.Font.FontSize = 16;
                worksheet.Cell(startrow, startcolumn).Style.Font.Bold = true;
                startrow++;

                worksheet.Row(startrow).Height = rowHeight;
                worksheet.Cell(startrow, startcolumn).Value = $"ฝ่าย / กลุ่มงาน : {sectionName}";
                worksheet.Cell(startrow, startcolumn).Style.Font.FontSize = 16;
                worksheet.Cell(startrow, startcolumn).Style.Font.Bold = true;
                startrow++;

                worksheet.Row(startrow).Height = rowHeight;
                worksheet.Cell(startrow, startcolumn).Value = $"งาน : {jobName}";
                worksheet.Cell(startrow, startcolumn).Style.Font.FontSize = 16;
                worksheet.Cell(startrow, startcolumn).Style.Font.Bold = true;
                startrow = startrow + 2;

                foreach (var item in listColumn)
                {
                    worksheet.Row(startrow).Height = rowHeight;

                    worksheet.Cell(startrow, startcolumn).Value = item;

                    worksheet.Cell(startrow, startcolumn).Style.Font.FontSize = 16;
                    worksheet.Cell(startrow, startcolumn).Style.Font.Bold = true;
                    startcolumn++;
                }
                startrow++;
                #endregion

                #region | Set Item |
                var listData = JsonConvert.DeserializeObject<List<dynamic>>(dataJson);

                foreach (var item in listData)
                {
                    startcolumn = 1;
                    var dict = item.ToObject<Dictionary<string, object>>();

                    foreach (var keyValue in dict)
                    {
                        if (keyValue.Value == null)
                        {
                            worksheet.Cell(startrow, startcolumn++).Value = "";
                        }
                        else
                        {
                            worksheet.Cell(startrow, startcolumn++).Value = keyValue.Value;
                        }
                    }

                    startrow++;
                }

                worksheet.Columns().AdjustToContents();
                worksheet.Rows().AdjustToContents();

                #endregion

                var stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0; // Reset stream position to the beginning

                return stream;
            }
        }

        public static List<GetSysOrgranize> GetOrgranize(this List<GetSysOrgranize> list, string typeName)
        {
            return list.Where(a => a.TypeName == typeName).ToList();
        }

        public static GetSysOrgranize GetOrgranize(this List<GetSysOrgranize> list, string typeName, string departmentCode, string divisionCode = "", string sectionCode = "", string jobCode = "")
        {
            if (typeName == OrgranizeType.Department)
            {
                return list.Where(a => a.TypeName == typeName && a.DepartmentCode == departmentCode).FirstOrDefault();
            }
            else if (typeName == OrgranizeType.Division)
            {
                return list.Where(a => a.TypeName == typeName && a.DepartmentCode == departmentCode && a.DivisionCode == divisionCode).FirstOrDefault();
            }
            else if (typeName == OrgranizeType.Section)
            {
                return list.Where(a => a.TypeName == typeName && a.DepartmentCode == departmentCode && a.DivisionCode == divisionCode && a.SectionCode == sectionCode).FirstOrDefault();
            }
            else if (typeName == OrgranizeType.Job)
            {
                return list.Where(a => a.TypeName == typeName && a.DepartmentCode == departmentCode && a.DivisionCode == divisionCode && a.SectionCode == sectionCode && a.JobCode == jobCode).FirstOrDefault();
            }
            return null;
        }

        public static List<T> ManageData<T>(this List<T> list, bool isAll, int pageSize, int pageNumber)
        {
            if (isAll)
            {
                return list;
            }
            else
            {
                return list.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }
        }

        public static (string departmentCode, string divisionCode, string sectionCode, string jobCode) GetCodeFromDataImport(this string sysOrgranizeFullName, bool isValidate)
        {
            string departmentCode = string.Empty, divisionCode = string.Empty, sectionCode = string.Empty, jobCode = string.Empty;
            if (!string.IsNullOrEmpty(sysOrgranizeFullName))
            {
                if (isValidate)
                {
                    Match match = Regex.Match(sysOrgranizeFullName, @"\[(.*?)\]");
                    if (match.Success)
                    {
                        string extracted = match.Groups[1].Value;
                        for (int i = 0; i < extracted.Length; i += 2)
                        {
                            string codeSegment = extracted.Substring(i, 2);
                            if (string.IsNullOrEmpty(departmentCode)) departmentCode = codeSegment;
                            else if (string.IsNullOrEmpty(divisionCode)) divisionCode = codeSegment;
                            else if (string.IsNullOrEmpty(sectionCode)) sectionCode = codeSegment;
                            else if (string.IsNullOrEmpty(jobCode)) jobCode = codeSegment;
                        }
                    }
                }
                else
                {
                    string extracted = sysOrgranizeFullName;
                    for (int i = 0; i < extracted.Length; i += 2)
                    {
                        string codeSegment = extracted.Substring(i, 2);
                        if (string.IsNullOrEmpty(departmentCode)) departmentCode = codeSegment;
                        else if (string.IsNullOrEmpty(divisionCode)) divisionCode = codeSegment;
                        else if (string.IsNullOrEmpty(sectionCode)) sectionCode = codeSegment;
                        else if (string.IsNullOrEmpty(jobCode)) jobCode = codeSegment;
                    }
                }
            }

            return (departmentCode, divisionCode, sectionCode, jobCode);
        }

        public static bool IsAllDigits(this string input)
        {
            return !string.IsNullOrEmpty(input) && input.All(char.IsDigit);
        }

        public static string GetMonthNameInThai(this int month)
        {
            if (month < 1 || month > 12)
            {
                throw new ArgumentOutOfRangeException(nameof(month), "Month must be between 1 and 12.");
            }

            string[] monthsInThai = new string[]
            {
        "มกราคม", "กุมภาพันธ์", "มีนาคม", "เมษายน", "พฤษภาคม", "มิถุนายน",
        "กรกฎาคม", "สิงหาคม", "กันยายน", "ตุลาคม", "พฤศจิกายน", "ธันวาคม"
            };

            return monthsInThai[month - 1];
        }
    }
}
