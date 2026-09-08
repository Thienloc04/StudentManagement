namespace StudentManagement.Models.ViewModels
{
    public class StudentSearchResultDto
    {
        public int StudentId { get; set; }
        public string StudentNo { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string GenderName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public decimal GPA { get; set; }
        public string AcademicRank { get; set; } = string.Empty;
    }
}

/* CREATE OR ALTER PROCEDURE sp_SearchStudents
    @StudentName NVARCHAR(100) = NULL,
    @ClassId INT = NULL,
    @AcademicRank NVARCHAR(20) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    WITH StudentGPA AS (
        SELECT 
            s.Id AS StudentId,
            s.StudentNo,
            p.FullName,
            p.Sex,
            p.DateOfBirth,
            c.Id AS ClassId,
            c.ClassNo AS ClassName,
            ISNULL(
                ROUND(
                    SUM(g.Score * gt.Weight) / NULLIF(SUM(gt.Weight), 0), 2
                ), 0
            ) AS GPA
        FROM Students s
        INNER JOIN Persons p ON s.Id = p.Id
        LEFT JOIN Student_Classes sc ON s.Id = sc.StudentId AND sc.EndDate IS NULL
        LEFT JOIN Classes c ON sc.ClassId = c.Id
        LEFT JOIN Enrollments e ON s.Id = e.StudentId
        LEFT JOIN Grades g ON e.Id = g.EnrollmentId
        LEFT JOIN GradeTypes gt ON g.GradeTypeId = gt.Id
        WHERE s.Status = 1
        GROUP BY s.Id, s.StudentNo, p.FullName, p.Sex, p.DateOfBirth, c.Id, c.ClassNo
    ),
    StudentRanked AS (
        SELECT 
            StudentId,
            StudentNo,
            FullName,
            Sex,
            DateOfBirth,
            ClassId,
            ClassName,
            GPA,
            CASE 
                WHEN GPA >= 8.0 THEN N'Giỏi'
                WHEN GPA >= 6.5 THEN N'Khá'
                WHEN GPA >= 5.0 THEN N'Trung bình'
                ELSE N'Yếu'
            END AS AcademicRank
        FROM StudentGPA
    )
    SELECT TOP (CASE WHEN @StudentName IS NULL AND @ClassId IS NULL AND @AcademicRank IS NULL THEN 10 ELSE 1000 END)
        StudentId,
        StudentNo,
        FullName,
        CASE WHEN Sex = 1 THEN N'Nam' ELSE N'Nữ' END AS GenderName,
        DateOfBirth,
        ISNULL(ClassName, N'Chưa xếp lớp') AS ClassName,
        GPA,
        AcademicRank
    FROM StudentRanked
    WHERE 
        (@StudentName IS NULL OR FullName LIKE '%' + @StudentName + '%')
        AND (@ClassId IS NULL OR ClassId = @ClassId)
        AND (@AcademicRank IS NULL OR AcademicRank = @AcademicRank)
    ORDER BY GPA DESC;
END;
 */