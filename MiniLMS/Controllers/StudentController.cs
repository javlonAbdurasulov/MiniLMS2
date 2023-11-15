using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MiniLMS.Application.Caching;
using MiniLMS.Application.DelegatNotification;
using MiniLMS.Application.Mediatr;
using MiniLMS.Application.Mediatr.Notification;
using MiniLMS.Application.Services;
using MiniLMS.Domain.Entities;
using MiniLMS.Domain.Models;
using MiniLMS.Domain.Models.StudentDTO;
using System.Net;

namespace MiniLMS.API.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly IMediator _mediator;
    
    public StudentController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    public async Task<ResponseModel<IEnumerable<StudentGetDTO>>> GetAll()
    {
        var request = new StudentGetAll();
        var res = await _mediator.Send(request);
        //_mediator.Publish(new StudentNotification() { message= "Get All Student!" });
        EventPublisher publisher = new EventPublisher();
        EventSubscriber subscriber = new EventSubscriber();

        subscriber.Subscribe(publisher);

        publisher.RaiseEvent("Get All Student!");

        return res;
    }

    [HttpGet]
    public async Task<ResponseModel<StudentGetDTO>> GetById(int id)
    {
        var request = new StudentGetById() { Id=id };
        var res = await _mediator.Send(request);
        _mediator.Publish(new StudentNotification() { message = $"Run GetbyId id:{id} !\nStatusCode = {res.StatusCode}" });
        return res;
    }
    [HttpPost]
    public async Task<ResponseModel<StudentGetDTO>> Create(StudentCreateDTO _studentCreateDto)
    {
        var request = new StudentCreate() { studentCreateDTO= _studentCreateDto };
        var res = await _mediator.Send(request);
        _mediator.Publish(new StudentNotification() { message="Create Student!" });
        return res;
    }

    [HttpDelete]
    public async Task<string> Delete(int id)
    {
        var request = new StudentDelete() { Id=id };
        var res = await _mediator.Send(request);
        _mediator.Publish(new StudentNotification() { message = $"Delete Student id:{id}! {res}" });
        return res;
    }

    [HttpPatch]
    public async Task<ResponseModel<StudentGetDTO>> Update(UpdateStudentDTO _update)
    {
        var request = new StudentUpdate() { update = _update};
        var res = await _mediator.Send(request);
        _mediator.Publish(new StudentNotification() { message= "Update Student!" });
        return res;

    }
}
