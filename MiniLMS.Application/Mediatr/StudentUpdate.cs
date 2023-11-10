using MediatR;
using MiniLMS.Domain.Models.StudentDTO;
using MiniLMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniLMS.Application.Services;
using MiniLMS.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using AutoMapper;

namespace MiniLMS.Application.Mediatr
{
    public class StudentUpdate:IRequest<ResponseModel<StudentGetDTO>>
    {
        public UpdateStudentDTO update{ get; set; }
    }
    public class StudentUpdateHandler : IRequestHandler<StudentUpdate, ResponseModel<StudentGetDTO>>
    {
        private readonly Serilog.ILogger _seriaLog;
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        public StudentUpdateHandler(Serilog.ILogger seriaLog, IStudentService studentService, IMapper mapper)
        {
            _seriaLog = seriaLog;
            _studentService = studentService;
            _mapper = mapper;
        }
        public async Task<ResponseModel<StudentGetDTO>> Handle(StudentUpdate request, CancellationToken cancellationToken)
        {
            
            Student Mylogin = await _studentService.GetByIdAsync(request.update.Id);
            Student mapped = _mapper.Map<Student>(request.update);
            mapped.Login = Mylogin.Login;
            await _studentService.UpdateAsync(mapped);
            StudentGetDTO studentDto = _mapper.Map<StudentGetDTO>(mapped);
            return new(studentDto);
        }
    }
}
