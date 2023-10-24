using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MiniLMS.Application.Caching;
using MiniLMS.Application.Services;
using MiniLMS.Domain.Entities;
using MiniLMS.Domain.Models;
using MiniLMS.Domain.Models.StudentDTO;
using Newtonsoft.Json;
using System.Net;

namespace MiniLMS.API.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class StudentController : ControllerBase
{

    private readonly ILogger<StudentController> _logger;
    private readonly IStudentService _studentService;
    private readonly IMapper _mapper;
    private readonly IValidator<Student> _validator;
    private readonly IDistributedCache _redis;
    //private readonly IAppCache _cacheProvider; 

    public StudentController(IDistributedCache redis/*IAppCache cacheProvider*/,ILogger<StudentController> logger, IStudentService studentService, IMapper mapper,IValidator<Student> validator)
    {
        _validator = validator;
        _studentService = studentService;
        _mapper = mapper;
        _redis = redis;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ResponseModel<IEnumerable<StudentGetDTO>>> GetAll()
    {
        _logger.LogInformation("Get All Student!");
        string st = _redis.GetString(CacheKeys.Student);
        IEnumerable<Student> student;
        IEnumerable<StudentGetDTO> students;
        if (string.IsNullOrEmpty(st))
        {
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
            students = JsonConvert.DeserializeObject<IEnumerable<StudentGetDTO>>(st);
        }
        //IEnumerable<StudentGetDTO> students = 
        //    _mapper.Map<IEnumerable<StudentGetDTO>>(res);
        
        return new(students);
    }

    [HttpGet]
    public async Task<ResponseModel<StudentGetDTO>> GetById(int id)
    {
        _logger.LogInformation($"Run GetbyId id:{id} !");
        Student studentEntity = await _studentService.GetByIdAsync(id);
        _logger.LogDebug("Get by Id executing....");
        StudentGetDTO studentDto= _mapper.Map<StudentGetDTO>(studentEntity);
        if(studentEntity == null)
        {
            _logger.LogWarning($"Student with id: {id} not found!");
            _logger.LogError("this error.. ");
            return new(studentDto, HttpStatusCode.NotFound);
        }
        return new(studentDto);
    }
    [HttpPost]
    public async Task<ResponseModel<StudentGetDTO>> Create(StudentCreateDTO studentCreateDto)
    {
        _logger.LogInformation("Create Student!");
        Student mappedStudent = _mapper.Map<Student>(studentCreateDto);
        var validResult = await _validator.ValidateAsync(mappedStudent);

        if (!validResult.IsValid)
            return new(validResult.IsValid.ToString());
        Student studentEntity = await _studentService.CreateAsync(mappedStudent);
        StudentGetDTO studentDto = _mapper.Map<StudentGetDTO>(mappedStudent);

        _redis.Remove(CacheKeys.Student);
        return new(studentDto);
    }

    [HttpDelete]
    public async Task<string> Delete(int id)
    {
        _logger.LogInformation($"Delete Student id:{id}!");
        bool result = await _studentService.DeleteAsync(id);
        string s = result ? "O'chirildi" : "Bunday id topilmadi";
        return s;
    }

    [HttpPatch]
    public async Task<ResponseModel<StudentGetDTO>> Update(UpdateStudentDTO update)
    {
        _logger.LogInformation("Update Student!");
        Student Mylogin =await _studentService.GetByIdAsync(update.Id);
        Student mapped = _mapper.Map<Student>(update);
        mapped.Login = Mylogin.Login;
        await _studentService.UpdateAsync(mapped);
        StudentGetDTO studentDto = _mapper.Map<StudentGetDTO>(mapped);
        return new(studentDto);

    }
}
