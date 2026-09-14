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
                                <button class="btn btn-primary btn-sm btn-manage-grades" data-id="${item.enrollmentId}"
                                data-subject="${item.subjectName}"
                                    <i class="fas fa-edit"></i> Nhập điểm
                                </button>
                            </td>
                            <td>
                                <button class="btn btn-danger btn-sm btn-cancel-enroll" data-id="${item.enrollmentId}">
                                    <i class="fas fa-trash"></i>  Hủy đăng ký
                                </button>
                            </td>
                        </tr>`;
                        tbody.append(row);
                    });
                }

            }
        });
    }

    $(document).on('click', '.btn-manage-grades', function () {
        var enrollmentId = $(this).data('id');
        var subjectName = $(this).data('subject');

        $('#modalSubjectTitle').text('Môn học: ' + subjectName);
        loadGradesForEnrollment(enrollmentId);

        var modal = new bootstrap.Modal(document.getElementById('gradeModal'));
        modal.show();
    });

    function loadGradesForEnrollment(enrollmentId) {
        $.ajax({
            url: '/Grade/GetGradeApi?enrollmentid=' + enrollmentId,
            type: 'GET',
            success: function (response) {
                if (response.success) {
                    var tbody = $('#gradeTableBody');
                    tbody.empty();

                    var totalWeightScore = 0;
                    var totalWeight = 0;

                    $.each(response.data, function (i, item) {
                        var scoreVal = item.score > 0 ? item.score : '';

                        totalWeightScore += item.score * item.weight;
                        totalWeight += item.weight;

                        var row = `<tr>
                            <td class="fw-bold">${item.gradeTypeName}</td>
                            <td><span class="badge bg-secondary">${item.weight * 100}%</span></td>
                            <td>
                                <input type="number" step="0.1" min="0" max="10"
                                    class="form-control input-score"
                                    id="score_input_${item.gradeTypeId}"
                                    value="${scoreVal}" placeholder="Chưa nhập" />
                            </td>
                            <td class="text-center">
                                <button class="btn btn-success btn-sm btn-save-single-grade"
                                    data-enrollmentid="${item.enrollmentId}"
                                    data-gradetypeid="${item.gradeTypeId}">
                                    <i class="fas fa-save"></i> Lưu
                                </button>
                            </td>
                        </tr>`;
                        tbody.append(row);
                    });

                    $('#lblCalculatedGpa').text(totalWeightScore.toFixed(2));
                } else {
                    alert(response.message);
                }
            }
        });
    }

    // Bắt sự kiện bấm nút "Lưu" cho từng cột điểm
    $(document).on('click', '.btn-save-single-grade', function () {
        var enrollmentId = $(this).data('enrollmentid');
        var gradeTypeId = $(this).data('gradetypeid');
        var scoreInput = $('#score_input_' + gradeTypeId).val();
        if (scoreInput === '' || isNaN(scoreInput)) {
            alert("Vui lòng nhập điểm số hợp lệ từ 0 đến 10!");
            return;
        }
        var score = parseFloat(scoreInput);
        if (score < 0 || score > 10) {
            alert("Điểm số phải nằm trong khoảng từ 0.0 đến 10.0!");
            return;
        }
        var requestData = {
            enrollmentId: enrollmentId,
            gradeTypeId: gradeTypeId,
            score: score
        };
        $.ajax({
            url: '/Grade/SaveGrade',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(requestData),
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    // Reload lại bảng điểm trong modal để cập nhật ĐTB mới
                    loadGradesForEnrollment(enrollmentId);
                } else {
                    alert("Lỗi: " + response.message);
                }
            }
        });
    });
});
