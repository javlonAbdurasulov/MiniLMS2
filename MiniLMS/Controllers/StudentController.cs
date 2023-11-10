using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MiniLMS.Application.Caching;
using MiniLMS.Application.Mediatr;
using MiniLMS.Application.Services;
using MiniLMS.Domain.Entities;
using MiniLMS.Domain.Models;
using MiniLMS.Domain.Models.StudentDTO;
using Newtonsoft.Json;
using Serilog;
using System.Net;

namespace MiniLMS.API.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class StudentController : ControllerBase
{

    private readonly IStudentService _studentService;
    private readonly IMapper _mapper;
    private readonly IValidator<Student> _validator;
    private readonly IDistributedCache _redis;
    private readonly Serilog.ILogger _seriaLog;
    private readonly IMediator _mediator;
    
    public StudentController(IMediator mediator,Serilog.ILogger serilog,IDistributedCache redis, IStudentService studentService, IMapper mapper,IValidator<Student> validator)
    {
        _validator = validator;
        _studentService = studentService;
        _mapper = mapper;
        _redis = redis;
        _seriaLog = serilog;
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<ResponseModel<IEnumerable<StudentGetDTO>>> GetAll()
    {
        var request = new StudentGetAll();
        var res = await _mediator.Send(request);
        return res;
    }

    [HttpGet]
    public async Task<ResponseModel<StudentGetDTO>> GetById(int id)
    {
        _seriaLog.Information($"Run GetbyId id:{id} !");
        Student studentEntity = await _studentService.GetByIdAsync(id);
        _seriaLog.Debug("Get by Id executing....");
        StudentGetDTO studentDto= _mapper.Map<StudentGetDTO>(studentEntity);
        if(studentEntity == null)
        {
            _seriaLog.Warning($"Student with id: {id} not found!");
            _seriaLog.Error("this error.. ");
            return new(studentDto, HttpStatusCode.NotFound);
        }
        return new(studentDto);
    }
    [HttpPost]
    public async Task<ResponseModel<StudentGetDTO>> Create(StudentCreateDTO studentCreateDto)
    {
        _seriaLog.Information("Create Student!");
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
        _seriaLog.Information($"Delete Student id:{id}!");
        bool result = await _studentService.DeleteAsync(id);
        if (result == false)
        {
            _seriaLog.Warning($"Student with id: {id} not found!");
        }
        string s = result ? "O'chirildi" : "Bunday id topilmadi";
        return s;
    }

    [HttpPatch]
    public async Task<ResponseModel<StudentGetDTO>> Update(UpdateStudentDTO update)
    {
        _seriaLog.Information("Update Student!");
        Student Mylogin =await _studentService.GetByIdAsync(update.Id);
        Student mapped = _mapper.Map<Student>(update);
        mapped.Login = Mylogin.Login;
        await _studentService.UpdateAsync(mapped);
        StudentGetDTO studentDto = _mapper.Map<StudentGetDTO>(mapped);
        return new(studentDto);

    }
}
