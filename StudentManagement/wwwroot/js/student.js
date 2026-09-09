// Chờ cho HTML load xong mới chạy code
$(document).ready(function () {

    // Gọi hàm load dữ liệu ngay khi vào trang
    //loadStudentData();
    searchStudents();
    loadClassDropdown();

    function loadStudentData() {
        $.ajax({
            url: '/Student/GetStudentDataApi', // Trỏ về đúng tên Controller và Action
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                // response lúc này là 1 mảng các object (Array)
                var tbody = $('#studentTableBody');
                tbody.empty(); // Xóa dòng "Đang tải dữ liệu..."

                if (response.length === 0) {
                    tbody.append('<tr><td colspan="5" class="text-center">Không có dữ liệu</td></tr>');
                    return;
                }

                // Duyệt qua từng phần tử trong mảng
                $.each(response, function (index, item) {
                    // Tên biến phải là camelCase
                    var row = `<tr>
                        <td>${item.studentNo}</td>
                        <td>${item.fullName}</td>
                        <td>${item.gender}</td>
                        <td>${item.currentClassName}</td>
                        <td>${item.statusName}</td>
                        <td>
                            <button class="btn btn-warning btn-sm btn-edit" data-id="${item.id}">Sửa</button>
                            <button class="btn btn-danger btn-sm btn-delete" data-id="${item.id}">Xóa</button>
                        </td>
                    </tr>`;

                    tbody.append(row); // Nhét từng dòng HTML vừa tạo vào bảng
                });
            },

            error: function (xhr, status, error) {
                console.error("Lỗi: ", error);
                alert("Có lỗi xảy ra khi tải dữ liệu từ máy chủ.");
            }
        });
    }


    $('#btnSaveStudent').click(function () {

        // 1. Thu thập dữ liệu từ các thẻ input
        // Tên key phía trước : phải khớp chính xác với property trong Student
        var requestData = {
            studentNo: $('#txtStudentNo').val(),
            fullName: $('#txtFullName').val(),
            sex: $('#ddlSex').val() === 'true', // So sánh với true để trả về kết quả bool
            dateOfBirth: $('#txtDob').val(),
            phone: $('#telPhone').val(),
            email: $('#txtEmail').val(),
            address: $('#txtAddress').val()
        };

        // 2. Gọi AJAX
        $.ajax({
            url: '/Student/AddStudentApi',
            type: 'POST',
            contentType: 'application/json', // Báo cho server biết tôi gửi JSON
            data: JSON.stringify(requestData), // Ép object của JS thành chuỗi JSON

            success: function (response) {
                if (response.success) {
                    alert(response.message);

                    // 3. Đóng Form Modal lại
                    $('#studentModal').modal('hide');

                    // Xóa dữ liệu cũ trên form để lần sau mở lên trắng tinh
                    $('#txtStudentNo').val('');
                    $('#txtFullName').val('');
                    $('#ddlSex').val('');
                    $('#txtDob').val('');
                    $('#txtEmail').val('');
                    $('#telPhone').val('');
                    $('#txtAddress').val('');

                    // 4. Gọi lại hàm hoad bảng để thấy dòng dữ liệu mới vừa thêm
                    loadStudentData();
                } else {
                    alert("Đã xảy ra lỗi kết nối với máy chủ.");
                }
            }
        });
    });

    // XỬ LÝ XÓA DELELECT 
    // Nếu thẻ HTML có sẵn từ đầu, dùng .click(). Nếu thẻ HTML do AJAX vẽ ra sau, bắt buộc dùng $(document).on('click', '.tên-class', function(){....})
    $(document).on('click', '.btn-delete', function () {
        // Lấy ID từ thuộc tính data-id của chính cái nút vừa click
        var studentId = $(this).data('id');

        // Hộp thoại xác nhận của trình duyệt
        if (confirm("Bạn có chắc chắn muốn xóa học sinh này?")) {
            $.ajax({
                url: '/Student/DeleteStudentApi?id=' + studentId,   // Truyền ID qua URL
                type: 'DELETE',
                success: function (response) {
                    if (response.success) {
                        alert(response.message)
                        loadStudentData(); // Load lại bảng
                    } else {
                        alert(response.message);
                    }
                }
            });
        }
    });



    // Xử lý sửa - Gồm 2 giai đoạn
    // Giai đoạn 1: Click nút Sửa -> Lấy data đổ lên modal
    $(document).on('click', '.btn-edit', function () {
        var studentId = $(this).data('id');

        $.ajax({
            url: '/Student/GetStudentByIdApi?id=' + studentId,
            type: 'GET',
            success: function (response) {
                if (response.success) {
                    var data = response.data;
                    // Đổ dữ liệu vào các thẻ input trong Modal Edit
                    $('#txtEditId').val(data.id);
                    $('#txtEditStudentNo').val(data.studentNo);
                    $('#txtEditFullName').val(data.fullName);

                    // Xử lý select box (true/false)
                    $('#ddlEditSex').val(data.sex ? 'true' : 'false');

                    // Ép kiểu ngày về định đạng yyyy-MM-đ
                    var formattedDate = data.dateOfBirth ? data.dateOfBirth.split('T')[0] : '';
                    $('#txtEditDob').val(formattedDate);

                    $('#txtEditEmail').val(data.email);
                    $('#telEditPhone').val(data.phone);
                    $('#txtEditAddress').val(data.address);

                    // Hiển thị Modal
                    $('#studentEditModal').modal('show');
                }
            }
        });
    });


    // Giai đoạn 2: Bấm nút "Lưu thay đổi" trên Modal -> Gọi API PUT
    $('#btnUpdateStudent').click(function () {
        var requestData = {
            id: $('#txtEditId').val(),
            studentNo: $('#txtEditStudentNo').val(),
            fullName: $('#txtEditFullName').val(),
            sex: $('#ddlEditSex').val() === 'true',
            dateOfBirth: $('#txtEditDob').val(),
            phone: $('#telEditPhone').val(),
            email: $('#txtEditEmail').val(),
            address: $('#txtEditAddress').val()
        };

        $.ajax({
            url: '/Student/UpdateStudentApi',
            type: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(requestData),
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    $('#studentEditModal').modal('hide');
                    loadStudentData();  // Refresh bảng
                }
            }
        });
    });

    // Hàm tìm kiếm dữ liệu
    function searchStudents() {
        var filter = {
            keyword: $('#searchName').val().trim(),
            classId: $('#searchClassId').val(),
            academicPerformance: $('#searchRank').val()
        }

        $.ajax({
            url: '/Student/SearchStudentsApi',
            type: 'GET',
            data: filter,
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    var tbody = $('#studentTableBody');
                    tbody.empty();

                    if (response.data.length === 0) {
                        tbody.append('<tr><td colspan="7" class="text-center">Không tìm thấy học sinh phù hợp</td></tr>');
                        return;
                    }

                    // Duyệt từng học sinh trả về từ Stored Procedure
                    $.each(response.data, function (index, item) {
                        var rankBadge = '';
                        if (item.academicPerformance === 'Giỏi') rankBadge = '<span class="badge bg-success">Giỏi</span>';
                        else if (item.academicPerformance === 'Khá') rankBadge = '<span class="badge bg-info">Khá</span>';
                        else if (item.academicPerformance === 'Trung bình') rankBadge = '<span class="badge bg-warning">Trung bình</span>';
                        else rankBadge = '<span class="badge bg-danger">Yếu</span>';

                        // Xử lý hiển thị giới tính
                        var genderText = item.sex ? "Nam" : "Nữ";

                        var row = `<tr>
                            <td>${item.studentNo}</td>
                            <td>${item.fullName}</td>
                            <td>${genderText}</td>
                            <td>${item.className || 'Chưa xếp lớp'}</td>
                            <td><strong class="text-primary">${item.averageScore.toFixed(2)}</strong></td>
                            <td>${rankBadge}</td>
                            <td>
                                <button class="btn btn-warning btn-sm btn-edit" data-id="${item.studentId}">Sửa</button>
                                <button class="btn btn-danger btn-sm btn-delete" data-id="${item.studentId}">Xóa</button>

                            </td>
                        </tr>`;

                        tbody.append(row);

                    });
                } else {
                    alert("Lỗi: " + response.message);
                }
            },
            error: function (xhr, status, error) {
                console.error("Lỗi search: " + error);
            }
        });
    }

    // Hàm load ds class lên dropdown
    function loadClassDropdown() {
        $.ajax({
            url: '/Student/GetClassesApi',
            type: 'GET',
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    var ddlClass = $('#searchClassId');

                    // Reset lại dropdown chỉ giữ dòng mặc định
                    ddlClass.html('<option value="">-- Tất cả các lớp --</option>');

                    // Duyệt qua danh sách Lớp từ API trả về và nhét từng option vào
                    $.each(response.data, function (index, item) {
                        ddlClass.append(`<option value="${item.id}">${item.classNo}</option>`)
                    });
                }
            },
            error: function (xhr, status, error) {
                console.error("Lỗi tài danh sách lớp: ", error);
            }
        });
    }

    // Bắt sự kiện click nút Tìm kiếm
    $('#btnSearch').click(function () {
        searchStudents();
    });

    // Bắt sự kiện Reset
    $('#btnResetSearch').click(function () {
        $('#searchName').val('');
        $('#searchClassId').val('');
        $('#searchRank').val('');
        searchStudents(); // Tự động load lại mặc định (Top 10 GPA cao nhất)
    })

});

// Javascript:
// Sử dụng let cho biến có thể thay đổi và const cho hằng số. Dùng cơ chế Event và Callback để chạy bất đồng bộ
// 2. DOM và jQuery
// - DOM: Browser parse html thành 1 cái cây cấu trúc(DOM). Nó cung cấp các hàm để JS có thể tìm, thêm, sửa, xóa các thẻ html
// - jQuery($): Viết code DOM thuần bằng JS rất dài nên ngta tạo ra thư viện jQuery. Hàm $ của jQuery giống với LINQ
//      + Tìm theo ID: $('#txtKeyword') thay vì document.getElementById('txtKeyword')
//      + Thì theo Class: $('.btn')
//      + Lấy giá trị: $('#txtKeyword').val()
// 3. AJAX
// - AJAX cho phép JS âm thầm gửi request lên controller để lấy json về, và dùng jQuery để nhét dữ liệu vào UI
// - Cấu trúc của 1 lệnh AJAX:
//  + url: nơi gửi đến(đường dẫn tới controller/action)
//  + type: HTTP method
//  + success: Hàm(Callback) sư tự động chạy nếu Server trả về HTTP 200 Ok
//  + error: Hàm sẽ chạy nếu Server trả về lỗi(400, 500....)