using Library.Data.Models;
using Library.Data.ViewsModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Data.Services
{
    public class BooksService
    {
        private AppDbContext _context;
        public BooksService(AppDbContext context)
        {
            _context = context;
        }

        public List<GetBookVM> GetAllBooks()
        {
            List<GetBookVM> _books = _context.Books
                .Select(i => new GetBookVM()
                {
                    Id = i.Id,
                    DateRead = i.DateRead,
                    CoverUrl = i.CoverUrl,
                    IsRead = i.IsRead,
                    Description = i.Description,
                    Genre = i.Genre,
                    Rate = i.Rate,
                    Title = i.Title,
                    Authors = i.Authors.Select(i => i.FullName).ToList(),
                    AuthorIds = i.Authors.Select(i => i.Id).ToList(),
                    Publisher = i.Publisher.Name,
                    PublisherId = i.PublisherId
                }).ToList<GetBookVM>();
            return _books;
        }

        public GetBookVM GetBookById(int id) 
        {
            GetBookVM _book = _context.Books.Where(i => i.Id == id)
                .Select(i => new GetBookVM()
                {
                    Id = i.Id,
                    DateRead = i.DateRead,
                    CoverUrl = i.CoverUrl,
                    IsRead = i.IsRead,
                    Description = i.Description,
                    Genre = i.Genre,
                    Rate = i.Rate,
                    Title = i.Title,
                    Authors = i.Authors.Select(i => i.FullName).ToList(),
                    AuthorIds = i.Authors.Select(i => i.Id).ToList(),
                    Publisher = i.Publisher.Name,
                    PublisherId = i.PublisherId
                }).FirstOrDefault();
            return _book;
        }

        public void AddBook(BookVM book)
        {
            var _book = new Book()
            {
                Title = book.Title,
                Description = book.Description,
                IsRead = book.IsRead,
                DateRead = book.IsRead ? book.DateRead.Value : null,
                Rate = book.IsRead ? book.Rate.Value : null,
                Genre = book.Genre,
                CoverUrl = book.CoverUrl,
                DateAdded = DateTime.Now,
                PublisherId = book.PublisherId,
                Authors = _context.Authors.Where(i => book.AuthorIds.Contains(i.Id)).ToHashSet<Author>()
            };
        }

        public Book UpdateBookById(int bookId, BookVM book)
        {
            var _book = _context.Books.FirstOrDefault(n => n.Id == bookId);
            if (_book != null)
            {
                _book.Title = book.Title;
                _book.Description = book.Description;
                _book.IsRead = book.IsRead;
                _book.DateRead = book.IsRead ? book.DateRead.Value : null;
                _book.Rate = book.IsRead ? book.Rate.Value : null;
                _book.Genre = book.Genre;
                _book.CoverUrl = book.CoverUrl;
                _book.DateAdded = DateTime.Now;
                _book.PublisherId = book.PublisherId;
                _book.Authors = _context.Authors.Where(i => book.AuthorIds.Contains(i.Id)).ToHashSet<Author>();
                _context.SaveChanges();
            }

            return _book;
        }

        public void DeleteBookById(int bookId)
        {
            var _book = _context.Books.FirstOrDefault(n => n.Id == bookId);
            if (_book != null)
            {
                _context.Books.Remove(_book);
                _context.SaveChanges();
            }
        }
    }
}
