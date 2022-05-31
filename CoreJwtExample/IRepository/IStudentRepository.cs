using CoreJwtExample.Models;

namespace CoreJwtExample.IRepository
{
    public interface IStudentRepository
    {
        Task<List<Student>> Gets1();
        Task<List<Student>> Gets2();

    }
}
