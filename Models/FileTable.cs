using System;
using System.Collections.Generic;

namespace FileDetailsViewTable.Models;

public partial class FileTable
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public DateTime? DateModified { get; set; }

    public string? Type { get; set; }

    public long? Size { get; set; }

    public string? FilePath { get; set; }
}
