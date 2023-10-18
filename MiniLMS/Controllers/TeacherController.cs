﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniLMS.Application.Services;
using MiniLMS.Domain.Entities;
using MiniLMS.Domain.Models;
using MiniLMS.Domain.Models.TeacherDTO;

namespace MiniLMS.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TeacherController : ControllerBase
{
    private readonly ITeacherService _teacherService;
    private readonly IMapper _mapper;

    public TeacherController(ITeacherService teacherService, IMapper mapper)
    {
        _teacherService = teacherService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ResponseModel<IEnumerable<TeacherGetDTO>>> GetAll()
    {
        var get = await _teacherService.GetAllAsync();
        IEnumerable<TeacherGetDTO> teachers = _mapper.Map<IEnumerable<TeacherGetDTO>>(get);

        return new(teachers);
    }
    [HttpGet]
    public async Task<ResponseModel<TeacherGetDTO>> GetById(int id)
    {
        Teacher teacherEntity = await _teacherService.GetByIdAsync(id);
        TeacherGetDTO teacherDto = _mapper.Map<TeacherGetDTO>(teacherEntity);
        return new(teacherDto);
    }
    [HttpPost]
    public async Task<ResponseModel<TeacherGetDTO>> Create(TeacherCreateDTO teacherCreateDto)
    {
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
