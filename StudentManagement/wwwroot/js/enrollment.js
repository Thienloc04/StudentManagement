$(document).ready(function () {

    // 1. Tải ds Hs và Lớp học phần vào Dropdown khi mở trang
    loadStudentDropdown();
    loadSubjectClassDropdown();


    // 2. Sự kiện khi chọn 1 Học Sinh -> Tự động load bảng ds môn đã đăng ký
    $('#ddlStudent').change(function () {
        var studentId = $(this).val();

        if (studentId) {
            loadStudentEnrollments(studentId);
        } else {
            $('#enrollmentTableBody').html('<tr><td> colspan="6" class="text-center text-muted">Vui lòng chọn học sinh để xem danh sách môn đã đăng ký</td></tr>');
        }
    });

    // 3. Sự kiện bấm nút "Đăng ký"
    $('#btnEnroll').click(function () {
        var studentId = $('#ddlStudent').val();
        var subjectClassId = $('#ddlSubjectClass').val();

        if (!studentId) {
            alert("Vui lòng chọn học sinh!");
            return;
        }

        if (!subjectClassId) {
            alert("Vui lòng chọn môn học phần!");
            return;
        }

        var requestData = {
            studentId: parseInt(studentId),
            subjectClassId: parseInt(subjectClassId)
        };

        $.ajax({
            url: '/Enrollment/RegisterSubjectApi',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(requestData),
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    loadStudentEnrollments(studentId);
                } else {
                    alert(response.message);
                }
            }
        });
    });

    // 4. Sự kiện bấm nút "Hủy đăng ký" (xóa)
    $(document).on('click', '.btn-cancel-enroll', function () {
        var enrollmentId = $(this).data('id');
        var studentId = $('#ddlStudent').val();

        if (confirm("Bạn có chắc chắn muốn hủy đăng ký môn học này?")) {
            $.ajax({
                url: '/Enrollment/CancelEnrollmentApi?id=' + enrollmentId,
                type: 'DELETE',
                success: function (response) {
                    if (response.success) {
                        alert(response.message);
                        loadStudentEnrollments(studentId);
                    } else {
                        alert(response.message);
                    }
                }
            });
        }
    });

    // --- Các hàm helper ajax ---
    function loadStudentDropdown() {
        $.ajax({
            url: '/Student/GetStudentDataApi',
            type: 'GET',
            success: function (response) {

                var dllStu = $('#ddlStudent');
                dllStu.html('<option value="">-- Chọn học sinh --</option>');
                $.each(response, function (index, item) {
                    dllStu.append(`<option value = "${item.id}"> ${item.studentNo} - ${item.fullName}</option>`);
                });
            }
        })
    }

    function loadSubjectClassDropdown() {
        $.ajax({
            url: '/Enrollment/GetSubjectClassesDropdownApi',
            type: 'GET',
            success: function (response) {
                if (response.success) {
                    var dllSC = $('#ddlSubjectClass');
                    dllSC.html('<option value="">-- Chọn môn học phần --</option>');
                    $.each(response.data, function (i, item) {
                        dllSC.append(`<option value="${item.subjectClassId}">${item.displayText}</option>`)
                    });
                }
            }
        });
    }

    function loadStudentEnrollments(studentId) {
        $.ajax({
            url: '/Enrollment/GetStudentEnrollmentsApi?studentId=' + studentId,
            type: 'GET',
            success: function (response) {
                if (response.success) {
                    var tbody = $('#enrollmentTableBody');
                    tbody.empty();

                    if (response.data.length === 0) {
                        tbody.append('<tr><td colspan="6" class="text-center text-warning">Học sinh này chưa đăng ký môn học nào.</td></tr>');
                        return;
                    }

                    $.each(response.data, function (i, item) {
                        var regDate = item.registerDate ? item.registerDate.split('T')[0] : '';

                        var row = `<tr>
                            <td><strong>${item.subjectName}</strong></td>
                            <td>${item.volume}</td>
                            <td>${item.teacherName}</td>
                            <td>${item.className}</td>
                            <td>${regDate}</td>
                            <td>
                                <button class="btn btn-danger btn-sm btn-cancel-enroll" data-id="${item.enrollmentId}">
                                    <i class="fas fa-trash"></i>
                                </button>
                            </td>
                        </tr>`;
                        tbody.append(row);
                    });
                }

            }
        });
    }


});