using System;

namespace UrChat.Data.Models
{
    public interface IModelBase
    {
        long Id { get; set; }
        DateTime CreatedAt { get; set; }
        DateTime ModifiedAt { get; set; }
    }
}