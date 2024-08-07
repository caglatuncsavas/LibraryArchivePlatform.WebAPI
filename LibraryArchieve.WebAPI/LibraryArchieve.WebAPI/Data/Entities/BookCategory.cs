﻿using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryArchieve.WebAPI.Data.Entities;

public sealed class BookCategory
{
    [ForeignKey("Book")]
    public int BookId { get; set; }
    public Book? Book { get; set; } 

    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
