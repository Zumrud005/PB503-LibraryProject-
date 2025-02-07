using LibraryProject.DTOs.BorrowerDto;
using LibraryProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Services.Interface
{
    public interface IBorrowerService
    {
        BorrowerGetDto GetById(int id);
        List<BorrowerGetDto> GetAll();
        void Create(BorrowerCreateDto dto);
        void Update(int id, BorrowerUpdateDto dto);
        void Delete(int id);
        List<BorrowerBooksDto> GetAllBorrowedBooks();

    }
}
