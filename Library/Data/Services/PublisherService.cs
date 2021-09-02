using Library.Data.Models;
using Library.Data.ViewsModel;
using NaturalSort.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Library.Data.Services
{
    public class PublishersService
    {
        const int _itemsOfPage = 6;


        private readonly AppDbContext _context;

        public PublishersService(AppDbContext context)
        {
            _context = context;
        }


        public GetPublishersVM GetAllPublishers(string sortBy, string searchString, int? pageNumber)
        {
            IEnumerable<Publisher> allPublishers = _context.Publishers.ToList().OrderBy(n => n.Name, new NaturalSortComparer(StringComparison.Ordinal));
            if (!string.IsNullOrEmpty(searchString))
            {
                allPublishers = allPublishers.Where(i => i.Name.ToUpper().Contains(searchString.ToUpper()));
            }
            if (sortBy?.ToUpper() == "DESC")
            {
                allPublishers = allPublishers.OrderByDescending(i => i.Name, new NaturalSortComparer(StringComparison.OrdinalIgnoreCase));
            }
            int count = allPublishers.Count();
            if (pageNumber.HasValue && pageNumber >= 0)
            {
                allPublishers = allPublishers.Skip((int)(pageNumber * _itemsOfPage)).Take(_itemsOfPage);
            }
            GetPublishersVM result = new GetPublishersVM() { Publishers = allPublishers.ToList(), Count = count };
            return result;
        }


        public Publisher AddPublisher(PublisherVM publisher)
        {
            var _publisher = new Publisher()
            {
                Name = publisher.Name
            };

            _context.Publishers.Add(_publisher);
            _context.SaveChanges();

            return _publisher;
        }

        public Publisher GetPublisherById(int id) => _context.Publishers.FirstOrDefault(n => n.Id == id);

        public PublisherWithBooksAndAuthorsVM GetPublisherData(int publisherId)
        {
            var _publisherData = _context.Publishers.Where(n => n.Id == publisherId)
                .Select(n => new PublisherWithBooksAndAuthorsVM()
                {
                    Name = n.Name,
                    BookAuthors = n.Books.Select(n => new BookAuthorVM()
                    {
                        BookName = n.Title,
                        BookAuthors = n.Authors.Select(n => n.FullName).ToList()
                    }).ToList()
                }).FirstOrDefault();
            return _publisherData;
        }

        public void DeletePublisherById(int id)
        {
            var _publisher = _context.Publishers.FirstOrDefault(n => n.Id == id);
            if (_publisher != null)
            {
                _context.Publishers.Remove(_publisher);
                _context.SaveChanges();
            }
            else
            {
                throw new Exception($"The publisher with id: {id} not found");
            }
        }
        public Publisher UpdatePublisherById(int id, PublisherVM publisher)
        {
            var _publisher = _context.Publishers.FirstOrDefault(n => n.Id == id);
            if (_publisher != null)
            {
                _publisher.Name = publisher.Name;
                _context.SaveChanges();
            }
            return _publisher;
        }
    }
}