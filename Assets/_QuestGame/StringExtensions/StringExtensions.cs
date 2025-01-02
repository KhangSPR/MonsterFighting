using System.Text.RegularExpressions;

public static class StringExtensions
{
    public static string ToFormattedString(this string input)
    {
        // Bước 1: Thêm khoảng cách giữa các từ (các chữ in hoa, trừ chữ đầu tiên)
        string formattedString = Regex.Replace(input, "(?<!^)([A-Z])", " $1");

        // Bước 2: Loại bỏ các chữ số khỏi chuỗi
        return Regex.Replace(formattedString, @"\d", "");
    }
}
