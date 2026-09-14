using StudentManagement.Models;
using StudentManagement.Models.Entities;
using StudentManagement.Models.ViewModels;
using StudentManagement.Repositories.Interface;
using StudentManagement.Services.Interface;

namespace StudentManagement.Services.Implement
{
    public class GradeService : IGradeService
    {
        private readonly IGradeRepository _gradeRepository;

        public GradeService(IGradeRepository gradeRepository)
        {
            _gradeRepository = gradeRepository;
        }

        public async Task DeleteGradeAsync(int id)
        {
            await _gradeRepository.DeleteGradeAsync(id);
        }

        public async Task<IEnumerable<GradeItemDto>> GetGradesByEnrollmentAsync(int enrollmentId)
        {
            var existingGrade = await _gradeRepository.GetGradesByEnrollmentIdAsync(enrollmentId);

            var allGradeTypes = await _gradeRepository.GetAllGradeTypesAsync();

            var result = new List<GradeItemDto>();

            foreach (var gradeType in allGradeTypes)
            {
                var grade = existingGrade.FirstOrDefault(g => g.GradeTypeId == gradeType.Id);

                string typeName = gradeType.Type switch
                {
                    TypeEnum.Attendance => "Chuyên cần (10%)",
                    TypeEnum.Midterm => "Giữa kỳ (30%)",
                    TypeEnum.Final => "Cuối kỳ (60%)"
                };

                result.Add(new GradeItemDto
                {
                    Id = grade?.Id ?? 0,
                    EnrollmentId = enrollmentId,
                    GradeTypeId = gradeType.Id,
                    GradeTypeName = typeName,
                    Weight = gradeType.Weight,
                    Score = grade?.Score ?? 0,
                });

            }

            return result;
        }

        public async Task SaveGradeAsync(GradeSaveDto request)
        {
            if (request.Score < 0 || request.Score > 10)
            {
                throw new Exception("Điểm số không hợp lệ! Điểm phải nằm trong khoảng từ 0.0 đén 10.0");
            }

            var existingGrade = await _gradeRepository.GetGradeByEnrollmentAndTypeAsync(request.EnrollmentId, request.GradeTypeId);

            if (existingGrade != null)
            {
                existingGrade.Score = request.Score;
                await _gradeRepository.UpdateGradeAsync(existingGrade);
            }
            else
            {
                var newGrade = new Grade
                {
                    EnrollmentId = request.EnrollmentId,
                    GradeTypeId = request.GradeTypeId,
                    Score = request.Score,
                    CreatedDate = DateTime.UtcNow,
                };
                await _gradeRepository.AddGradeAsync(newGrade);
            }
        }
    }
}
