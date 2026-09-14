$(document).ready(function () {

    // 1. Tải danh sách môn học khi mở trang
    loadSubjectData();

    // 2. Sự kiện bấm nút "Thêm môn học mới"
    $('#btnCreateSubject').click(function () {
        clearForm();
        $('#subjectModalLabel').html('<i class="fas fa-plus-circle"></i> Thêm Môn Học Mới');
        $('#divStatusGroup').hide(); // Thêm mới thì ẩn dropdown trạng thái
        var modal = new bootstrap.Modal(document.getElementById('subjectModal'));
        modal.show();
    });

    // 3. Sự kiện bấm nút "Sửa" trên từng dòng bảng
    $(document).on('click', '.btn-edit-subject', function () {
        var id = $(this).data('id');

        $.ajax({
            url: '/Subject/GetSubjectByIdApi?id=' + id,
            type: 'GET',
            success: function (response) {
                if (response.success && response.data) {
                    var data = response.data;
                    $('#txtSubjectId').val(data.id);
                    $('#txtSubjectName').val(data.name);
                    $('#txtSubjectVolume').val(data.volume);
                    $('#ddlSubjectStatus').val(data.status);

                    $('#subjectModalLabel').html('<i class="fas fa-edit"></i> Cập Nhật Môn Học');
                    $('#divStatusGroup').show(); // Hiện dropdown trạng thái khi sửa

                    var modal = new bootstrap.Modal(document.getElementById('subjectModal'));
                    modal.show();
                } else {
                    alert(response.message || "Không tìm thấy thông tin môn học!");
                }
            }
        });
    });

    // 4. Sự kiện bấm nút "Lưu dữ liệu" trong Modal
    $('#btnSaveSubject').click(function () {
        var id = parseInt($('#txtSubjectId').val());
        var name = $('#txtSubjectName').val().trim();
        var volume = parseInt($('#txtSubjectVolume').val());

        if (!name) {
            alert("Vui lòng nhập tên môn học!");
            return;
        }

        if (isNaN(volume) || volume <= 0) {
            alert("Số tín chỉ phải lớn hơn 0!");
            return;
        }

        var isEdit = id > 0;

        var requestData = {
            id: id,
            name: name,
            volume: volume,
            status: isEdit ? parseInt($('#ddlSubjectStatus').val()) : 1
        };

        var apiUrl = isEdit ? '/Subject/UpdateSubjectApi' : '/Subject/AddSubjectApi';
        var apiType = isEdit ? 'PUT' : 'POST';

        $.ajax({
            url: apiUrl,
            type: apiType,
            contentType: 'application/json',
            data: JSON.stringify(requestData),
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    bootstrap.Modal.getInstance(document.getElementById('subjectModal')).hide();
                    loadSubjectData();
                } else {
                    alert("Lỗi: " + response.message);
                }
            }
        });
    });

    // 5. Sự kiện bấm nút "Xóa"
    $(document).on('click', '.btn-delete-subject', function () {
        var id = $(this).data('id');
        var name = $(this).data('name');

        if (confirm(`Bạn có chắc chắn muốn xóa môn học "${name}"?`)) {
            $.ajax({
                url: '/Subject/DeleteSubjectApi?id=' + id,
                type: 'DELETE',
                success: function (response) {
                    if (response.success) {
                        alert(response.message);
                        loadSubjectData();
                    } else {
                        alert("Lỗi: " + response.message);
                    }
                }
            });
        }
    });

    // --- Hàm Helper ---
    function loadSubjectData() {
        $.ajax({
            url: '/Subject/GetSubjectDataApi',
            type: 'GET',
            success: function (response) {
                if (response.success) {
                    var tbody = $('#subjectTableBody');
                    tbody.empty();

                    if (response.data.length === 0) {
                        tbody.append('<tr><td colspan="5" class="text-center text-warning">Chưa có môn học nào trong hệ thống.</td></tr>');
                        return;
                    }

                    $.each(response.data, function (index, item) {
                        var statusBadge = item.status === 1
                            ? '<span class="badge bg-success">Đang giảng dạy</span>'
                            : '<span class="badge bg-secondary">Tạm ngưng</span>';

                        var row = `<tr>
                            <td class="text-center">${index + 1}</td>
                            <td><strong>${item.name}</strong></td>
                            <td class="text-center">${item.volume}</td>
                            <td class="text-center">${statusBadge}</td>
                            <td class="text-center">
                                <button class="btn btn-warning btn-sm btn-edit-subject" data-id="${item.id}">
                                    <i class="fas fa-edit"></i> Sửa
                                </button>
                                <button class="btn btn-info btn-sm btn-manage-subject-classes me-1" data-id="${item.id}" data-name="${item.name}">
                                    <i class="fas fa-layer-group"></i> Lớp HP
                                </button>

                                <button class="btn btn-danger btn-sm btn-delete-subject" data-id="${item.id}" data-name="${item.name}">
                                    <i class="fas fa-trash"></i> Xóa
                                </button>
                            </td>
                        </tr>`;
                        tbody.append(row);
                    });
                }
            }
        });
    }

    function clearForm() {
        $('#txtSubjectId').val(0);
        $('#txtSubjectName').val('');
        $('#txtSubjectVolume').val('');
        $('#ddlSubjectStatus').val(1);
    }

    // --- XỬ LÝ QUẢN LÝ LỚP HỌC PHẦN ---
    var isDropdownLoaded = false;

    // 1. Click nút "Lớp HP" trên bảng môn học
    $(document).on('click', '.btn-manage-subject-classes', function () {
        var subjectId = $(this).data('id');
        var subjectName = $(this).data('name');

        $('#txtCurrentSubjectId').val(subjectId);
        $('#subjectClassModalLabel').html(`<i class="fas fa-layer-group"></i> LỚP HỌC PHẦN - MÔN: ${subjectName}`);

        if (!isDropdownLoaded) {
            loadDropdownData();
        }

        loadSubjectClasses(subjectId);

        var modal = new bootstrap.Modal(document.getElementById('subjectClassModal'));
        modal.show();
    });

    // 2. Load danh sách Lớp HP của môn học
    function loadSubjectClasses(subjectId) {
        $.ajax({
            url: '/Subject/GetSubjectClassesApi?subjectId=' + subjectId,
            type: 'GET',
            success: function (response) {
                if (response.success) {
                    var tbody = $('#subjectClassTableBody');
                    tbody.empty();

                    if (response.data.length === 0) {
                        tbody.append('<tr><td colspan="4" class="text-center text-warning">Môn học này chưa có lớp học phần nào được mở.</td></tr>');
                        return;
                    }

                    $.each(response.data, function (i, item) {
                        var row = `<tr>
                        <td class="text-center">${i + 1}</td>
                        <td><strong class="text-primary">${item.className}</strong></td>
                        <td>${item.teacherName}</td>
                        <td class="text-center">
                            <button class="btn btn-danger btn-sm btn-delete-subject-class" data-id="${item.subjectClassId}">
                                <i class="fas fa-trash"></i> Hủy
                            </button>
                        </td>
                    </tr>`;
                        tbody.append(row);
                    });
                }
            }
        });
    }

    // 3. Load dữ liệu Dropdown Lớp & Giảng viên
    function loadDropdownData() {
        $.ajax({
            url: '/Subject/GetDropdownDataApi',
            type: 'GET',
            success: function (response) {
                if (response.success) {
                    var ddlClass = $('#ddlClassSelect');
                    var ddlTeacher = $('#ddlTeacherSelect');

                    ddlClass.html('<option value="">-- Chọn lớp --</option>');
                    $.each(response.classes, function (i, c) {
                        ddlClass.append(`<option value="${c.id}">${c.classNo}</option>`);
                    });

                    ddlTeacher.html('<option value="">-- Chọn giảng viên --</option>');
                    $.each(response.teachers, function (i, t) {
                        ddlTeacher.append(`<option value="${t.id}">${t.fullName}</option>`);
                    });

                    isDropdownLoaded = true;
                }
            }
        });
    }

    // 4. Click nút "Mở Lớp"
    $('#btnOpenSubjectClass').click(function () {
        var subjectId = parseInt($('#txtCurrentSubjectId').val());
        var classId = parseInt($('#ddlClassSelect').val());
        var teacherId = parseInt($('#ddlTeacherSelect').val());

        if (!classId) {
            alert("Vui lòng chọn lớp sinh hoạt!");
            return;
        }
        if (!teacherId) {
            alert("Vui lòng chọn giảng viên!");
            return;
        }

        var requestData = {
            subjectId: subjectId,
            classId: classId,
            teacherId: teacherId
        };

        $.ajax({
            url: '/Subject/AddSubjectClassApi',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(requestData),
            success: function (response) {
                if (response.success) {
                    alert(response.message);
                    loadSubjectClasses(subjectId);
                } else {
                    alert("Lỗi: " + response.message);
                }
            }
        });
    });

    // 5. Click nút "Xóa/Hủy Lớp HP"
    $(document).on('click', '.btn-delete-subject-class', function () {
        var id = $(this).data('id');
        var subjectId = $('#txtCurrentSubjectId').val();

        if (confirm("Bạn có chắc chắn muốn hủy lớp học phần này?")) {
            $.ajax({
                url: '/Subject/DeleteSubjectClassApi?id=' + id,
                type: 'DELETE',
                success: function (response) {
                    if (response.success) {
                        alert(response.message);
                        loadSubjectClasses(subjectId);
                    } else {
                        alert("Lỗi: " + response.message);
                    }
                }
            });
        }
    });

});
