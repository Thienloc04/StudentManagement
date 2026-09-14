using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;
using StudentManagement.Models.Entities;

namespace StudentManagement.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Tự động tạo DB nếu chưa có
            await context.Database.EnsureCreatedAsync();

            // 1. Seed Lớp sinh hoạt (Classes)
            if (!context.Classes.Any())
            {
                var classes = new List<Class>
                {
                    new Class { ClassNo = "20DTH01", Status = 1, CreatedDate = DateTime.Now },
                    new Class { ClassNo = "20DTH02", Status = 1, CreatedDate = DateTime.Now },
                    new Class { ClassNo = "21DTH01", Status = 1, CreatedDate = DateTime.Now },
                    new Class { ClassNo = "21DTH02", Status = 1, CreatedDate = DateTime.Now },
                    new Class { ClassNo = "22DTH01", Status = 1, CreatedDate = DateTime.Now }
                };
                await context.Classes.AddRangeAsync(classes);
                await context.SaveChangesAsync();
            }

            // 2. Seed Giảng viên (Teachers)
            if (!context.Teachers.Any())
            {
                var teachers = new List<Teacher>
                {
                    new Teacher { TeacherNo = "GV001", FullName = "Nguyễn Văn Thắng", Sex = true, Phone = "0901111111", Email = "thangnv@school.edu.vn", Address = "TP.HCM", Status = 1 },
                    new Teacher { TeacherNo = "GV002", FullName = "Trần Thị Hương", Sex = false, Phone = "0902222222", Email = "huongtt@school.edu.vn", Address = "Hà Nội", Status = 1 },
                    new Teacher { TeacherNo = "GV003", FullName = "Lê Hoàng Nam", Sex = true, Phone = "0903333333", Email = "namlh@school.edu.vn", Address = "Đà Nẵng", Status = 1 },
                    new Teacher { TeacherNo = "GV004", FullName = "Phạm Minh Tuấn", Sex = true, Phone = "0904444444", Email = "tuanpm@school.edu.vn", Address = "Cần Thơ", Status = 1 },
                    new Teacher { TeacherNo = "GV005", FullName = "Vũ Thị Mai", Sex = false, Phone = "0905555555", Email = "maivt@school.edu.vn", Address = "TP.HCM", Status = 1 }
                };
                await context.Teachers.AddRangeAsync(teachers);
                await context.SaveChangesAsync();
            }

            // 3. Seed Môn học (Subjects)
            if (!context.Subjects.Any())
            {
                var subjects = new List<Subject>
                {
                    new Subject { Name = "Lập trình C# MVC", Volume = 3, Status = 1 },
                    new Subject { Name = "Cơ sở dữ liệu SQL", Volume = 4, Status = 1 },
                    new Subject { Name = "Phân tích thiết kế hệ thống", Volume = 3, Status = 1 },
                    new Subject { Name = "Tiếng Anh chuyên ngành", Volume = 2, Status = 1 },
                    new Subject { Name = "Cấu trúc dữ liệu & Giải thuật", Volume = 4, Status = 1 }
                };
                await context.Subjects.AddRangeAsync(subjects);
                await context.SaveChangesAsync();
            }

            // 4. Seed Loại điểm (GradeTypes)
            if (!context.GradeTypes.Any())
            {
                var gradeTypes = new List<GradeType>
                {
                    new GradeType { Type = TypeEnum.Attendance, Weight = 0.1m }, // Điểm chuyên cần (10%)
                    new GradeType { Type = TypeEnum.Midterm, Weight = 0.3m },    // Điểm giữa kỳ (30%)
                    new GradeType { Type = TypeEnum.Final, Weight = 0.6m }       // Điểm cuối kỳ (60%)
                };
                await context.GradeTypes.AddRangeAsync(gradeTypes);
                await context.SaveChangesAsync();
            }

            // 5. Seed Sinh viên (Students) & Phân vào Lớp sinh hoạt
            if (!context.Students.Any())
            {
                var students = new List<Student>
                {
                    new Student { StudentNo = "SV001", FullName = "Nguyễn Văn A", Sex = true, DateOfBirth = new DateTime(2002, 5, 10), Phone = "0911111111", Email = "nva@gmail.com", Address = "TP.HCM", Status = 1 },
                    new Student { StudentNo = "SV002", FullName = "Trần Thị B", Sex = false, DateOfBirth = new DateTime(2002, 8, 20), Phone = "0922222222", Email = "ttb@gmail.com", Address = "Bình Dương", Status = 1 },
                    new Student { StudentNo = "SV003", FullName = "Lê Văn C", Sex = true, DateOfBirth = new DateTime(2003, 1, 15), Phone = "0933333333", Email = "lvc@gmail.com", Address = "Đồng Nai", Status = 1 },
                    new Student { StudentNo = "SV004", FullName = "Phạm Thị D", Sex = false, DateOfBirth = new DateTime(2003, 11, 30), Phone = "0944444444", Email = "ptd@gmail.com", Address = "Long An", Status = 1 },
                    new Student { StudentNo = "SV005", FullName = "Hoàng Văn E", Sex = true, DateOfBirth = new DateTime(2004, 3, 25), Phone = "0955555555", Email = "hve@gmail.com", Address = "TP.HCM", Status = 1 },
                    new Student { StudentNo = "SV006", FullName = "Đỗ Thị F", Sex = false, DateOfBirth = new DateTime(2004, 7, 12), Phone = "0966666666", Email = "dtf@gmail.com", Address = "Tiền Giang", Status = 1 }
                };
                await context.Students.AddRangeAsync(students);
                await context.SaveChangesAsync();

                // Gán học sinh vào Lớp sinh hoạt (Student_Classes)
                var classList = context.Classes.ToList();
                var studentClasses = new List<Student_Class>
                {
                    new Student_Class { StudentId = students[0].Id, ClassId = classList[0].Id, AcademicYear = 2020, StartDate = DateTime.Now },
                    new Student_Class { StudentId = students[1].Id, ClassId = classList[0].Id, AcademicYear = 2020, StartDate = DateTime.Now },
                    new Student_Class { StudentId = students[2].Id, ClassId = classList[1].Id, AcademicYear = 2021, StartDate = DateTime.Now },
                    new Student_Class { StudentId = students[3].Id, ClassId = classList[1].Id, AcademicYear = 2021, StartDate = DateTime.Now },
                    new Student_Class { StudentId = students[4].Id, ClassId = classList[2].Id, AcademicYear = 2022, StartDate = DateTime.Now },
                    new Student_Class { StudentId = students[5].Id, ClassId = classList[2].Id, AcademicYear = 2022, StartDate = DateTime.Now }
                };
                await context.StudentClasses.AddRangeAsync(studentClasses);
                await context.SaveChangesAsync();
            }

            // 6. Seed Lớp học phần (Subject_Classes)
            if (!context.SubjectClasses.Any())
            {
                var subjects = context.Subjects.ToList();
                var classes = context.Classes.ToList();
                var teachers = context.Teachers.ToList();

                var subjectClasses = new List<Subject_Class>
                {
                    new Subject_Class { SubjectId = subjects[0].Id, ClassId = classes[0].Id, TeacherId = teachers[0].Id, CreatedDate = DateTime.Now },
                    new Subject_Class { SubjectId = subjects[1].Id, ClassId = classes[0].Id, TeacherId = teachers[1].Id, CreatedDate = DateTime.Now },
                    new Subject_Class { SubjectId = subjects[0].Id, ClassId = classes[1].Id, TeacherId = teachers[2].Id, CreatedDate = DateTime.Now },
                    new Subject_Class { SubjectId = subjects[2].Id, ClassId = classes[1].Id, TeacherId = teachers[3].Id, CreatedDate = DateTime.Now },
                    new Subject_Class { SubjectId = subjects[3].Id, ClassId = classes[2].Id, TeacherId = teachers[4].Id, CreatedDate = DateTime.Now }
                };
                await context.SubjectClasses.AddRangeAsync(subjectClasses);
                await context.SaveChangesAsync();
            }

            // 7. Seed Học sinh đăng ký học phần (Enrollments) & Nhập điểm (Grades)
            if (!context.Enrollments.Any())
            {
                var students = context.Students.ToList();
                var subjectClasses = context.SubjectClasses.ToList();

                var enrollments = new List<Enrollment>
                {
                    new Enrollment { StudentId = students[0].Id, SubjectClassId = subjectClasses[0].Id, RegisterDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(4) },
                    new Enrollment { StudentId = students[1].Id, SubjectClassId = subjectClasses[0].Id, RegisterDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(4) },
                    new Enrollment { StudentId = students[2].Id, SubjectClassId = subjectClasses[1].Id, RegisterDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(4) },
                    new Enrollment { StudentId = students[3].Id, SubjectClassId = subjectClasses[2].Id, RegisterDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(4) },
                    new Enrollment { StudentId = students[4].Id, SubjectClassId = subjectClasses[3].Id, RegisterDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(4) }
                };
                await context.Enrollments.AddRangeAsync(enrollments);
                await context.SaveChangesAsync();

                // Seed Điểm tương ứng cho Enrollments
                var gradeTypes = context.GradeTypes.ToList();
                var grades = new List<Grade>
                {
                    // Học sinh 1 - Môn C#
                    new Grade { EnrollmentId = enrollments[0].Id, GradeTypeId = gradeTypes[0].Id, Score = 9.0m },
                    new Grade { EnrollmentId = enrollments[0].Id, GradeTypeId = gradeTypes[1].Id, Score = 8.5m },
                    new Grade { EnrollmentId = enrollments[0].Id, GradeTypeId = gradeTypes[2].Id, Score = 9.5m },

                    // Học sinh 2 - Môn C#
                    new Grade { EnrollmentId = enrollments[1].Id, GradeTypeId = gradeTypes[0].Id, Score = 7.0m },
                    new Grade { EnrollmentId = enrollments[1].Id, GradeTypeId = gradeTypes[1].Id, Score = 6.5m },
                    new Grade { EnrollmentId = enrollments[1].Id, GradeTypeId = gradeTypes[2].Id, Score = 6.0m },

                    // Học sinh 3 - Môn SQL
                    new Grade { EnrollmentId = enrollments[2].Id, GradeTypeId = gradeTypes[0].Id, Score = 5.0m },
                    new Grade { EnrollmentId = enrollments[2].Id, GradeTypeId = gradeTypes[1].Id, Score = 4.5m },
                    new Grade { EnrollmentId = enrollments[2].Id, GradeTypeId = gradeTypes[2].Id, Score = 4.0m }
                };
                await context.Grades.AddRangeAsync(grades);
                await context.SaveChangesAsync();
            }

            // Tự động tạo hoặc cập nhật Stored Procedure sp_SearchStudents khi app chạy
            await context.Database.ExecuteSqlRawAsync(@"
CREATE OR ALTER PROCEDURE sp_SearchStudents
    @Keyword NVARCHAR(100) = NULL,
    @ClassId INT = NULL,
    @AcademicPerformance NVARCHAR(50) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    WITH StudentGPA AS (
        SELECT 
            s.Id AS StudentId,
            s.StudentNo,
            p.FullName,
            p.Sex,
            c.ClassNo AS ClassName,
            c.Id AS ClassId,
            ISNULL(
                ROUND(
                    SUM(g.Score * gt.Weight) / NULLIF(SUM(gt.Weight), 0), 2
                ), 0
            ) AS AverageScore
        FROM Students s
        INNER JOIN Persons p ON s.Id = p.Id
        LEFT JOIN Student_Classes sc ON s.Id = sc.StudentId
        LEFT JOIN Classes c ON sc.ClassId = c.Id
        LEFT JOIN Enrollments e ON s.Id = e.StudentId
        LEFT JOIN Grades g ON e.Id = g.EnrollmentId
        LEFT JOIN Grade_Types gt ON g.GradeTypeId = gt.Id
        WHERE s.Status = 1
        GROUP BY s.Id, s.StudentNo, p.FullName, p.Sex, c.ClassNo, c.Id
    ),
    StudentRanked AS (
        SELECT 
            StudentId,
            StudentNo,
            FullName,
            Sex,
            ClassName,
            ClassId,
            AverageScore,
            CASE 
                WHEN AverageScore >= 8.0 THEN N'GIOI'
                WHEN AverageScore >= 6.5 THEN N'KHA'
                WHEN AverageScore >= 5.0 THEN N'TB'
                ELSE N'YEU'
            END AS AcademicPerformance
        FROM StudentGPA
    )
    SELECT TOP (
        CASE 
            WHEN (@Keyword IS NULL OR @Keyword = '') 
                 AND @ClassId IS NULL 
                 AND (@AcademicPerformance IS NULL OR @AcademicPerformance = '') 
            THEN 10 
            ELSE 1000000 
        END
    )
        StudentId,
        StudentNo,
        FullName,
        Sex,
        ISNULL(ClassName, N'Chưa phân lớp') AS ClassName,
        AverageScore,
        AcademicPerformance
    FROM StudentRanked
    WHERE (@Keyword IS NULL OR @Keyword = '' OR StudentNo LIKE '%' + @Keyword + '%' OR FullName LIKE '%' + @Keyword + '%')
      AND (@ClassId IS NULL OR ClassId = @ClassId)
      AND (@AcademicPerformance IS NULL OR @AcademicPerformance = '' OR AcademicPerformance = @AcademicPerformance)
    ORDER BY AverageScore DESC, StudentNo ASC;
END;
");

        }
    }
}
