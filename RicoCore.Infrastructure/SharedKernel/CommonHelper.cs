using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RicoCore.Infrastructure.SharedKernel
{
    public class CommonHelper
    {
        public const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string AlphabetAndNumber = "ABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random Random = new Random();

        #region Generate random code
        /// <summary>
        /// Generate random code
        /// </summary>
        /// <returns></returns>
        public static string GenerateRandomCode()
        {
            var randomAlphabet = GenerateRandomString(Alphabet, 1);

            var randomNumber = Enumerable.Range(0, 9)
                .OrderBy(x => Random.Next())
                .Take(3);
            return $"{string.Join(string.Empty, randomNumber)}{randomAlphabet}";
            //return $"{randomAlphabet}{string.Join(string.Empty, randomNumber)}";
        }

        /// <summary>
        /// Generate random code
        /// </summary>
        /// <returns></returns>
        public static string Generate6CharsRandomCode()
        {
            var randomAlphabet = GenerateRandomString(Alphabet, 3);

            var randomNumber = Enumerable.Range(0, 9)
                .OrderBy(x => Random.Next())
                .Take(6);

            return $"{randomAlphabet}{string.Join(string.Empty, randomNumber)}";
        }


        public static string GenerateRandomNumberCode()
        {
            //var randomAlphabet = GenerateRandomString(Alphabet, 1);

            var randomNumber = Enumerable.Range(0, 9)
                .OrderBy(x => Random.Next())
                .Take(4);
            return $"{string.Join(string.Empty, randomNumber)}";
            //return $"{randomAlphabet}{string.Join(string.Empty, randomNumber)}";
        }

        public static string GenerateExtensionOfUrl()
        {
            var randomAlphabet = GenerateRandomString(Alphabet, 2);

            var randomNumber = Enumerable.Range(0, 9)
                .OrderBy(x => Random.Next())
                .Take(3);
            return $"{randomAlphabet}-{string.Join(string.Empty, randomNumber)}";
        }

        /// <summary>
        /// Generate random string
        /// </summary>
        /// <param name="random"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomString(string random, int length)
        {
            return new string(Enumerable.Repeat(random, length)
                .Select(s => s[Random.Next(s.Length)]).ToArray());
        }
        #endregion

        #region[Lottery]      

        public static DateTime ConvertStringToDate(string input)
        {
            List<string> arr = input.Split('-').ToList();
            int day = Int32.Parse(arr[0]);
            int month = Int32.Parse(arr[1]);
            int year = Int32.Parse(arr[2]);
            var time = new DateTime(year, month, day);
            return time;
        }

        public static string ValidDateStringForConvertToDateTime(string input)
        {
            List<string> list = input.Split('-').ToList();
            return list[1] + "/" + list[0] + "/" + list[2];            
        }

        public static string ConvertDateToString(DateTime input)
        {
            string output = "";
            string day = (input.Day < 10) ? "0" + input.Day.ToString() : input.Day.ToString();
            string month = (input.Month < 10) ? "0" + input.Month.ToString() : input.Month.ToString();
            string year = input.Year.ToString();

            if (input != null)
            {
                output = day + "-" + month + "-" + year;
            }
            return output;
        }

        public static string ConvertToVietnameseDayOfWeekString(string str)
        {
            switch (str)
            {
                case "Thứ 2":
                    str = "Thứ hai";
                    break;

                case "Thứ 3":
                    str = "Thứ ba";
                    break;

                case "Thứ 4":
                    str = "Thứ tư";
                    break;

                case "Thứ 5":
                    str = "Thứ năm";
                    break;

                case "Thứ Sáu":
                    str = "Thứ sáu";
                    break;

                case "Thứ Bảy":
                    str = "Thứ bảy";
                    break;

                case "Chủ Nhật":
                    str = "Chủ nhật";
                    break;

                case "Thứ Hai":
                    str = "Thứ hai";
                    break;

                case "Thứ Ba":
                    str = "Thứ ba";
                    break;

                case "Thứ Tư":
                    str = "Thứ tư";
                    break;

                case "Thứ Năm":
                    str = "Thứ năm";
                    break;

                case "Thứ 6":
                    str = "Thứ sáu";
                    break;

                case "Thứ 7":
                    str = "Thứ bảy";
                    break;               

                case "Monday":
                    str = "Thứ hai";
                    break;

                case "Tuesday":
                    str = "Thứ ba";
                    break;

                case "Wednesday":
                    str = "Thứ tư";
                    break;

                case "Thursday":
                    str = "Thứ năm";
                    break;

                case "Friday":
                    str = "Thứ sáu";
                    break;

                case "Saturday":
                    str = "Thứ bảy";
                    break;

                case "Sunday":
                    str = "Chủ nhật";
                    break;

                default:
                    break;
            }
            return str;
        }
        public static string GetDayOfWeekString(string str)
        {
            Match dayOfWeekRegex = Regex.Match(str, @"((Thứ[\s](Hai|Ba|Tư|Bốn|Năm|Sáu|Bảy|hai|ba|tư|bốn|năm|sáu|bảy|2|3|4|5|6|7)){1})|(Chủ Nhật){1}|(Chủ nhật){1}");
            string dayOfWeek = dayOfWeekRegex.Groups[0].Value.ToString();
            return dayOfWeek;
        }

        public static string GetDateStringByHyphen(string str)
        {
            Match dateStr = Regex.Match(str, @"([0-9]{1,2}[\-]{1}[0-9]{1,2}[\-]{1}[0-9]{4}){1}");
            string date = dateStr.Groups[0].Value.ToString();
            return date;
        }

        public static string GetDateStringBySlash(string str)
        {
            Match dateStr = Regex.Match(str, @"([0-9]{1,2}[\/]{1}[0-9]{1,2}[\/]{1}[0-9]{4}){1}");
            string date = dateStr.Groups[0].Value.ToString();
            return date;
        }

        public static List<string> GetLotteriesInStringViaRssFile(string str)
        {
            //string[] digits = Regex.Split(description, @"\D+|([[0-9]{1}[\:]{1})");
            string[] resultsArr = Regex.Split(str, @"\D+|[0-9]{1}[\:]{1}");
            List<string> result = new List<string>();
            foreach (string value in resultsArr)
            {                
                if (int.TryParse(value, out var number))
                {
                    result.Add(value.ToString());
                }
            }
            return result;
        }

        #endregion

        #region[Sort]
        #region[quick sort]
        public void Swap(int[] ar, int a, int b)
        {
            int temp;
            temp = ar[a];
            ar[a] = ar[b];
            ar[b] = temp;
        }
        public int Partition(int[] input, int low, int high)
        {
            int pivot = input[high];
            int i = low - 1;

            for (int j = low; j < high - 1; j++)
            {
                if (input[j] <= pivot)
                {
                    i++;
                    Swap(input, i, j);
                }
            }
            Swap(input, i + 1, high);
            return i + 1;
        }
        public void Quicksort(int[] input, int low, int high)
        {
            int pivot_loc = 0;

            if (low < high)
            {
                pivot_loc = Partition(input, low, high);
                Quicksort(input, low, pivot_loc - 1);
                Quicksort(input, pivot_loc + 1, high);
            }
        }
        #endregion // end quick sort   

        #endregion

        #region[String]
        public static bool CheckSpecialFormatInputString(string input)
        {
            if (input == "*" || input == "&nbsp;")
                return false;
            else
                return true;
        }
        #endregion

        #region[NumberPhone]

        //	1234567890
        //	123-456-7890
        //	123-456-7890 x1234
        //	123-456-7890 ext1234
        //	(123)-456-7890
        //  123.456.7890
        //  123 456 7890
        public static bool IsValidNumberPhone(String str)
        {
            if (Regex.IsMatch(str, @"^[0-9]{10}$")
                || Regex.IsMatch(str, @"^[0-9]{3}[\-]{1}[0-9]{3}[\-]{1}[0-9]{4}$")
                || Regex.IsMatch(str, @"^[0-9]{3}[\-]{1}[0-9]{3}[\-]{1}[0-9]{4}[\s]{1}[a-z]{1}[0-9]{4}$")
                || Regex.IsMatch(str, @"^[0-9]{3}[\-]{1}[0-9]{3}[\-]{1}[0-9]{4}[\s]{1}[a-z]{3}[0-9]{4}$")
                || Regex.IsMatch(str, @"^[\(]{1}[0-9]{3}[\)]{1}[\-]{1}[0-9]{3}[\-]{1}[0-9]{4}$")
                || Regex.IsMatch(str, @"^[0-9]{3}[\.]{1}[0-9]{3}[\.]{1}[0-9]{4}$")
                || Regex.IsMatch(str, @"^[0-9]{3}[\s]{1}[0-9]{3}[\s]{1}[0-9]{4}$"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check input(Số điện thoại VN bắt đầu với 0 hoặc +84)
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsValidPhoneVN(string phone)
        {
            phone = phone.Trim();
            if (Regex.IsMatch(phone, @"^0[0-9]{9,10}$") || Regex.IsMatch(phone, @"^(\+84)[0-9]{9,10}$"))
            {
                return true;
            }
            return false;
        }

        #endregion

        #region[Email]
        public static bool IsValidEmail(string str)
        {
            if (Regex.IsMatch(str, @"^([a-z0-9_\.\+-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check input(định dạng email)
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public static bool IsValidEmailBySystem(string email)
        {
            email = email.Trim();
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region[Letters]
        public static bool OnlyLetters(String str)
        {
            if (Regex.IsMatch(str, @"^[a-zA-Z]+$")) return true;
            return false;
        }

        public static bool OnlyLettersAndWhiteSpace(String str)
        {
            if (Regex.IsMatch(str, @"^[a-zA-Z\s]*$")) return true;
            return false;
        }


        public static bool OnlyLettersAndNumbers(String str)
        {
            if (Regex.IsMatch(str, @"^[a-zA-Z0-9]+$")) return true;
            return false;
        }

        public static bool OnlyLettersAndNumbersAndUnderscore(String str)
        {
            if (Regex.IsMatch(str, @"^[a-zA-Z0-9_]+$")) return true;
            return false;
        }
        #endregion

        #region[Number]

        /// <summary>
        /// Check input(số)
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsValidNumberic(object value)
        {
            if (value == null || value is DateTime)
            {
                return false;
            }

            if (value is Int16 || value is Int32 || value is Int64 || value is Decimal || value is Single || value is Double || value is Boolean)
            {
                return true;
            }

            try
            {
                if (value is string)
                    Double.Parse(value as string);
                else
                    Double.Parse(value.ToString());
                return true;
            }
            catch { }
            return false;
        }
        public static bool IsNumber(String str)
        {
            if (Regex.IsMatch(str, @"^[0-9]{1,}$")) return true;
            return false;
        }
        public static bool RangeLengthString(String str)
        {
            if (Regex.IsMatch(str, @"^[a-zA-Z]{1,50}$")) return true;
            return false;
        }

        public static bool GreaterLengthString(String str)
        {
            if (Regex.IsMatch(str, @"^[a-zA-Z\s]{5,}$")) return true;
            return false;
        }
        #endregion

        #region[DataType]

        /// <summary>
        /// Check input(Long or Int)
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsValidInt16Int32Int64(object value)
        {
            if (value == null || value is DateTime)
            {
                return false;
            }

            if (value is Int16 || value is Int32 || value is Int64)
            {
                return true;
            }

            try
            {
                if (value is string)
                    long.Parse(value as string);
                else
                    long.Parse(value.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region[StringLength]
        /// <summary>
        /// Check input(Độ dài chuỗi)
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static bool IsValidStringLength(String str, int length)
        {
            str = str.Trim();
            return str.Length <= length;
        }
        #endregion

        #region[DateTime]
        /// <summary>
        /// Check input(DateTime)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsValidDateTime(String str)
        {
            str = str.Trim();            
            return DateTime.TryParse(str, out var tempOut);
        }
        #endregion

        #region[Others]

        /// <summary>
        /// Check input(Tên)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsValidCharName(String str)
        {
            str = str.Trim();
            return Regex.IsMatch(str, @"^[a-zA-Z_ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]+"
                                    + @"([\s]{1}[a-zA-Z_ÀÁÂÃÈÉÊÌÍÒÓÔÕÙÚĂĐĨŨƠàáâãèéêìíòóôõùúăđĩũơƯĂẠẢẤẦẨẪẬẮẰẲẴẶẸẺẼỀỀỂưăạảấầẩẫậắằẳẵặẹẻẽềềểỄỆỈỊỌỎỐỒỔỖỘỚỜỞỠỢỤỦỨỪễệỉịọỏốồổỗộớờởỡợụủứừỬỮỰỲỴÝỶỸửữựỳỵỷỹ]+)+$");
        }
        //public static bool IsValidCharName(String str)
        //{
        //    return Regex.IsMatch(str, @"^[a-zA-Z]+([\s]{1}[a-zA-Z]+)+$");
        //}

        public static bool IsValidId(String str)
        {
            if (Regex.IsMatch(str, @"^[0-9]{1,}$")) return true;
            return false;
        }

        public static bool IsValidName(String str)
        {
            if (Regex.IsMatch(str, @"^[a-zA-Z]+$")) return true;
            return false;
        }

        public static bool IsValidAge(String str)
        {
            if (Regex.IsMatch(str, @"^\d+$")) return true;
            return false;
        }
        #endregion

    }
}
