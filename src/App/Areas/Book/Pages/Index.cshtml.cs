namespace BookRec.App.Areas.Book.Pages
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common;
    using EnsureThat;
    using Infrastructure.EntityFramework.Models;
    using Infrastructure.EntityFramework.Repositories;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IBookRepository repository;
        private readonly IUserBookRepository userBookRepository;

        public IndexModel(IBookRepository repository, IUserBookRepository userBookRepository)
        {
            EnsureArg.IsNotNull(repository);
            EnsureArg.IsNotNull(userBookRepository);

            this.userBookRepository = userBookRepository;
            this.repository = repository;
        }

        public IEnumerable<UserBook> UserBooks { get; set; }

        public async Task OnGet()
        {
            this.UserBooks = await this.userBookRepository.GetByUsernameAsync(this.User.Identity.Name).ConfigureAwait(false);
        }

        public async Task OnGetRatingAsync(string id, int stars)
        {
            await this.userBookRepository.UpdateStarAsync(id, this.User.Identity.Name, stars).ConfigureAwait(false);
        }

        public async Task OnGetAddAsync(string id)
        {
            EnsureArg.IsNotNullOrEmpty(id);
            var book = await this.userBookRepository.GetByBookIdAsync(id, this.User.Identity.Name).ConfigureAwait(false);
            if (book != null)
            {
                return;
            }

            var userBook = new UserBook()
            {
                BookId = id.ToGuid().Value,
                Username = this.User.Identity.Name,
                Rating = 1
            };

            await this.userBookRepository.InsertAsync(userBook).ConfigureAwait(false);
        }

        public async Task<IActionResult> OnGetFilterAsync(string value)
        {
            var list = await this.repository.GetListByTitleAsync(value).ConfigureAwait(false);
            return new JsonResult(list);
        }

        public async Task OnGetDeleteAsync(string id)
            => await this.userBookRepository.DeleteAsync(id.ToGuid().Value).ConfigureAwait(false);
    }
}