using MediatR;
using MiniLMS.Domain.Models.StudentDTO;
using MiniLMS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using MiniLMS.Application.Caching;
using MiniLMS.Application.Services;
using MiniLMS.Domain.Entities;
using Newtonsoft.Json;
using StackExchange.Redis;
using AutoMapper;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace MiniLMS.Application.Mediatr
{
    public class StudentGetAll:IRequest<ResponseModel<IEnumerable<StudentGetDTO>>>
    {
        public ResponseModel<IEnumerable<StudentGetDTO>> studentGetDTO{ get; set; }
    }
    
    public class StudentGetAllHandler : IRequestHandler<StudentGetAll, ResponseModel<IEnumerable<StudentGetDTO>>>
    {
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;
        private readonly IValidator<Student> _validator;
        private readonly IDistributedCache _redis;
        private readonly Serilog.ILogger _seriaLog;

        public StudentGetAllHandler(Serilog.ILogger serilog, IDistributedCache redis, IStudentService studentService, IMapper mapper, IValidator<Student> validator)
        {
            _validator = validator;
            _studentService = studentService;
            _mapper = mapper;
            _redis = redis;
            _seriaLog = serilog;
        }

        public async Task<ResponseModel<IEnumerable<StudentGetDTO>>> Handle(StudentGetAll request, CancellationToken cancellationToken)
        {
            _seriaLog.Information("Get All Student!");
            string st = _redis.GetString(CacheKeys.Student);
            IEnumerable<Student> student;
            IEnumerable<StudentGetDTO> students;
            if (string.IsNullOrEmpty(st))
            {
                _seriaLog.Information("get all from database");
                student = await _studentService.GetAllAsync();
                var cacheEntityOption = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                    SlidingExpiration = TimeSpan.FromSeconds(30)
                };
                students =
                    _mapper.Map<IEnumerable<StudentGetDTO>>(student);
                st = JsonConvert.SerializeObject(students);
                _redis.SetString(CacheKeys.Student, st, cacheEntityOption);

            }
            else
            {
                _seriaLog.Information("get all from cache");
                students = JsonConvert.DeserializeObject<IEnumerable<StudentGetDTO>>(st);
            }

            return new(students);
        }
    }
}
