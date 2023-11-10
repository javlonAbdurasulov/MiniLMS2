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
using System.Net;
using AutoMapper;

namespace MiniLMS.Application.Mediatr
{
    public class StudentGetById:IRequest<ResponseModel<StudentGetDTO>>
    {
        public int Id { get; set; }
    }
    public class StudentGetByIdHandler : IRequestHandler<StudentGetById, ResponseModel<StudentGetDTO>>
    {
        private readonly Serilog.ILogger _seriaLog;
        private readonly IMapper _mapper;
        private readonly IStudentService _studentService;
        public StudentGetByIdHandler(Serilog.ILogger serialog, IMapper mapper, IStudentService studentService)
        {
            _seriaLog = serialog;
            _mapper = mapper;
            _studentService = studentService;
        }
        public async Task<ResponseModel<StudentGetDTO>> Handle(StudentGetById request, CancellationToken cancellationToken)
        {
            _seriaLog.Information($"Run GetbyId id:{request.Id} !");
            Student studentEntity = await _studentService.GetByIdAsync(request.Id);
            _seriaLog.Debug("Get by Id executing....");
            StudentGetDTO studentDto = _mapper.Map<StudentGetDTO>(studentEntity);
            if (studentEntity == null)
            {
                _seriaLog.Warning($"Student with id: {request.Id} not found!");
                _seriaLog.Error("this error.. ");
                return new(studentDto, HttpStatusCode.NotFound);
            }
            return new(studentDto);
        }
    }
}
