using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class TMProDropdownManager : MonoBehaviour
{
    public TMPro.TMP_Dropdown classDropdown;

    private void Start()
    {
        // Thêm các lựa chọn vào Dropdown
        AddOptionsToDropdown();

        // Thiết lập giá trị mặc định là "Level"
        SetDefaultDropdownValue("Level");

        // Đặt sự kiện lắng nghe cho Dropdown
        classDropdown.onValueChanged.AddListener(new UnityEngine.Events.UnityAction<int>(OnDropdownValueChanged));
    }

    private void AddOptionsToDropdown()
    {
        // Tạo danh sách các lựa chọn
        string[] classOptions = { "ALL", "Melee", "Witch", "Archer" };

        // Xóa các lựa chọn cũ (nếu có)
        classDropdown.ClearOptions();

        // Thêm các lựa chọn mới
        classDropdown.AddOptions(new System.Collections.Generic.List<string>(classOptions));
    }

    private void SetDefaultDropdownValue(string defaultValue)
    {
        // Thiết lập giá trị mặc định cho Dropdown
        classDropdown.value = System.Array.IndexOf(classDropdown.options.Select(option => option.text).ToArray(), defaultValue);
    }

    private void OnDropdownValueChanged(int index)
    {
        // Xử lý khi giá trị của Dropdown thay đổi
        string selectedClass = classDropdown.options[index].text;

        // In ra console giá trị được chọn
        Debug.Log("Selected Class: " + selectedClass);

        // Gọi hàm để xử lý dựa trên giá trị được chọn (ví dụ: Hiển thị các card Tower thuộc class đã chọn)
        HandleDropdownSelection(selectedClass);
    }

    private void HandleDropdownSelection(string selectedClass)
    {
        // Đặt logic xử lý dựa trên giá trị được chọn
        // Ví dụ: Nếu selectedClass là "ALL", hiển thị tất cả các card, ngược lại hiển thị
    }
}
