using LibraryProject.DTOs.LoanItemDto;
using LibraryProject.Models;
using LibraryProject.Repositories.Implementation;
using LibraryProject.Services.Interface;

namespace LibraryProject.Services.Implementation
{
    public class LoanItemService : ILoanItemService
    {
        public void Create(LoanItemCreateDto loanItemCreateDto)
        {
            LoanItemRepository repository = new LoanItemRepository();
            BookRepository bookRepository = new BookRepository();
            var loanItem = new LoanItem
            {
                LoanId = loanItemCreateDto.LoanId,
                BookId = loanItemCreateDto.BookId,
                CreatedAt = DateTime.UtcNow.AddHours(4),
                UpdateAt = DateTime.UtcNow.AddHours(4),
            };


            var book = bookRepository.GetById(loanItemCreateDto.BookId);
            if (book != null)
            {

                repository.Add(loanItem);
                repository.Commit();
            }
        }

        public void Delete(int id)
        {
            LoanItemRepository repository = new LoanItemRepository();
            var loanItem = repository.GetById(id);
            if (loanItem != null)
            {
                repository.Remove(loanItem);
                repository.Commit();
            }
        }

        public List<LoanItemGetDto> GetAll()
        {
            LoanItemRepository repository = new LoanItemRepository();
            List<LoanItemGetDto> loanItems = repository.IGetAll();
            if (loanItems is null) throw new ArgumentException("List is empty");
            return loanItems;
        }

        public LoanItemGetDto GetByIdLoanItem(int id)
        {
            LoanItemRepository repository = new LoanItemRepository();
            LoanItemGetDto loanItem = repository.IGetById(id);
            return loanItem;
        }

        public void Update(int id, LoanItemUpdateDto loanItemUpdateDto)
        {
            LoanItemRepository repository = new LoanItemRepository();
            BookRepository bookRepository = new BookRepository();
            var loanItem = repository.GetById(loanItemUpdateDto.Id);
            if (loanItem != null)
            {
                loanItem.LoanId = loanItemUpdateDto.LoanId;
                loanItem.BookId = loanItemUpdateDto.BookId;
                loanItem.UpdateAt = DateTime.UtcNow.AddHours(4);


                repository.Commit();
            }
        }
    }
}
