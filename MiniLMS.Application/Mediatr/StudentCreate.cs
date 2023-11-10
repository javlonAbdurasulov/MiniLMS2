using MediatR;
using MiniLMS.Domain.Models.StudentDTO;
using MiniLMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniLMS.Application.Caching;
using MiniLMS.Application.Services;
using MiniLMS.Domain.Entities;
using StackExchange.Redis;
using AutoMapper;
using MiniLMS.Application.FluentValidation;
using Microsoft.Extensions.Caching.Distributed;

namespace MiniLMS.Application.Mediatr
{
    public class StudentCreate:IRequest<ResponseModel<StudentGetDTO>>
    {
        public StudentCreateDTO studentCreateDTO { get; set; }
    }
    public class StudentCreateHandler : IRequestHandler<StudentCreate, ResponseModel<StudentGetDTO>>
    {
        
        private readonly IMapper _mapper;
        private readonly StudentValidator _validator;
        private readonly IStudentService _studentService;
        private readonly IDistributedCache _redis;

        public StudentCreateHandler(IMapper mapper,
            StudentValidator validator, IStudentService studentService, IDistributedCache redis)
        {
            _mapper = mapper;
            _validator = validator;
            _studentService = studentService;
            _redis = redis;
        }
        public async Task<ResponseModel<StudentGetDTO>> Handle(StudentCreate request, CancellationToken cancellationToken)
        {
            
            Student mappedStudent = _mapper.Map<Student>(request.studentCreateDTO);
            var validResult = await _validator.ValidateAsync(mappedStudent);

            if (!validResult.IsValid)
                return new(validResult.IsValid.ToString());
            Student studentEntity = await _studentService.CreateAsync(mappedStudent);
            StudentGetDTO studentDto = _mapper.Map<StudentGetDTO>(mappedStudent);

            _redis.Remove(CacheKeys.Student);
            return new(studentDto);
        }
    }
}
