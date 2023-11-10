using MediatR;
using MiniLMS.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniLMS.Application.Mediatr
{
    public class StudentDelete:IRequest<string>
    {
        public int Id{ get; set; }
    }
    public class StudentDeleteHandler : IRequestHandler<StudentDelete, string>
    {
        private readonly Serilog.ILogger _seriaLog;
        private readonly IStudentService _studentService;
        public StudentDeleteHandler(Serilog.ILogger seriaLog, IStudentService studentService )
        {
            _seriaLog = seriaLog;
            _studentService = studentService;
        }
        public async Task<string> Handle(StudentDelete request, CancellationToken cancellationToken)
        {
            //_seriaLog.Information($"Delete Student id:{request.Id}!");
            bool result = await _studentService.DeleteAsync(request.Id);
            if (result == false)
            {
                _seriaLog.Warning($"Student with id: {request.Id} not found!");
            }
            string s = result ? "O'chirildi" : "Bunday id topilmadi";
            return s;
        }
    }
}
