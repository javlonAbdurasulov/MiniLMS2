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
    private readonly HttpClient _httpClient = new();
    private readonly HttpClient _httpClientServices;
    
    //private readonly IAppCache _cacheProvider; 

    public StudentController(HttpClient httpClient, Serilog.ILogger serilog,IDistributedCache redis, IStudentService studentService, IMapper mapper,IValidator<Student> validator)
    {
        _validator = validator;
        _studentService = studentService;
        _mapper = mapper;
        _redis = redis;
        _seriaLog = serilog;
        _httpClientServices = httpClient;
    }

    [HttpGet]
    public async Task<ResponseModel<Catfact>> GetAllCatFacts()
    {
        
        HttpResponseMessage catFacts = await _httpClient.GetAsync("https://catfact.ninja/fact");
        var facts = await catFacts.Content.ReadFromJsonAsync<Catfact>();
        //var facts = await _httpClient.GetFromJsonAsync<Catfact>("fact");
        return new(facts);
    }
    [HttpGet]
    public async Task<ResponseModel<Catfact>> GetAllCatFacts2()
    {
        
        HttpResponseMessage catFacts = await _httpClient.GetAsync(_httpClient.BaseAddress);
        var facts = await catFacts.Content.ReadFromJsonAsync<Catfact>();
        //var facts = await _httpClient.GetFromJsonAsync<Catfact>("fact");
        return new(facts);
    }

    //[HttpGet]
    //public async Task<ResponseModel<string>> GetBitcoins()
    //{

    //    return new();
    //}
    [HttpGet]
    public async Task<ResponseModel<Catfact>> BasketbalWithKeyRapid()
    {

        string apiUrl = "https://api-basketball.p.rapidapi.com/seasons";

        using (HttpClient client = new HttpClient())
        {
            // Задайте заголовки запроса, включая X-Rapidapi-Key и X-Rapidapi-Host
            client.DefaultRequestHeaders.Add("X-Rapidapi-Key", "cd69f3fef2msh13bfd14051d51e1p1c3eafjsnc4b6c0c39adc");
            client.DefaultRequestHeaders.Add("X-Rapidapi-Host", "api-basketball.p.rapidapi.com");

            try
            {
                // Выполните GET-запрос и получите ответ
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Прочитайте ответ в виде строки
                    string responseContent = await response.Content.ReadAsStringAsync();
                    return new(responseContent);
                }
                else
                {
                    return new("Ошибка при выполнении запроса. Код статуса:", response.StatusCode);
                    //Console.WriteLine("Ошибка при выполнении запроса. Код статуса: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                _seriaLog.Error(ex.Message);
            }
        }
        return new("s");

    }


    [HttpGet]
    public async Task<ResponseModel<IEnumerable<StudentGetDTO>>> GetAll()
    {
        _seriaLog.Information("Get All Student!");
            //Log.Fatal("Get All Student!");
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
        //IEnumerable<StudentGetDTO> students = 
        //    _mapper.Map<IEnumerable<StudentGetDTO>>(res);
        
        return new(students);
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
