using System.Collections.Generic;

namespace UrChat.ViewModels
{
    public class PaginationViewModel<T>
    {
        public int TotalPages { get; set; }
        public IEnumerable<T> Page { get; set; }
    }
}