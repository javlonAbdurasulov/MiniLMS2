using AutoMapper;
using LazyCache;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniLMS.Application.Caching;
using MiniLMS.Application.Services;
using MiniLMS.Domain.Entities;
using MiniLMS.Domain.Models;
using MiniLMS.Domain.Models.TeacherDTO;
using System.Net;

namespace MiniLMS.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TeacherController : ControllerBase
{
    private readonly ITeacherService _teacherService;
    private readonly IMapper _mapper;
    private readonly IAppCache _appCache;
    private readonly Serilog.ILogger _serilog;


    public TeacherController(IAppCache appCache,Serilog.ILogger serilog,ITeacherService teacherService, IMapper mapper)
    {
        _teacherService = teacherService;
        _mapper = mapper;
        _appCache = appCache;
        _serilog = serilog;
    }

    [HttpGet]
    public async Task<ResponseModel<IEnumerable<TeacherGetDTO>>> GetAll()
    {
        if (_appCache.TryGetValue(CacheKeys.Teacher, out IEnumerable<TeacherGetDTO> cachedTeachers))
        {
            _serilog.Information("Get all from cache");
            return new ResponseModel<IEnumerable<TeacherGetDTO>>(cachedTeachers);
        }
            _serilog.Information("Get all from Database");

        var getTeachers = await _teacherService.GetAllAsync();
        IEnumerable<TeacherGetDTO> teachers = _mapper.Map<IEnumerable<TeacherGetDTO>>(getTeachers);

        _appCache.Add(CacheKeys.Teacher, teachers, TimeSpan.FromMinutes(10));

        return new ResponseModel<IEnumerable<TeacherGetDTO>>(teachers);
    }
    [HttpGet]
    public async Task<ResponseModel<TeacherGetDTO>> GetById(int id)
    {
        Teacher teacherEntity = await _teacherService.GetByIdAsync(id);
        TeacherGetDTO teacherDto = _mapper.Map<TeacherGetDTO>(teacherEntity);
        if(teacherEntity == null)
        {
            _serilog.Warning($"Teacher with id:{id} not found!");
            return new(teacherDto, HttpStatusCode.NotFound);
        }
        return new(teacherDto);
    }
    [HttpPost]
    public async Task<ResponseModel<TeacherGetDTO>> Create(TeacherCreateDTO teacherCreateDto)
    {
        if(teacherCreateDto== null)
        {
            _serilog.Warning("teacher is null!");
        }
        _serilog.Debug("Create teacher executing...");
        Teacher mappedTeacher = _mapper.Map<Teacher>(teacherCreateDto);
        Teacher teacherEntity = await _teacherService.CreateAsync(mappedTeacher);

        TeacherGetDTO teacherDto = _mapper.Map<TeacherGetDTO>(teacherEntity);
        return new(teacherDto);
    }

    [HttpDelete]
    public async Task<ResponseModel<string>> DeleteAsync(int id)
    {
        bool resultDelete = await _teacherService.DeleteAsync(id);
        string res = resultDelete ? "O'chirildi" : "Bunday id topilmadi";
        if (resultDelete)
        {
            _serilog.Warning($"teacher not found id:{id}");
        }
        return new(res);
    }
    [HttpPut]
    public async Task<ResponseModel<TeacherGetDTO>> Update(TeacherCreateDTO createDTO)
    {
        Teacher teacher = _mapper.Map<Teacher>(createDTO);
        await _teacherService.UpdateAsync(teacher);
        TeacherGetDTO result = _mapper.Map<TeacherGetDTO>(teacher);
        return new(result);
    }
}
