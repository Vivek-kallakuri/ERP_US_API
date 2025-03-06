using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PDFUploader.Models;

public partial class FileMetadatum
{
    [Key]
    public int Id { get; set; }

    [StringLength(255)]
    public string FileName { get; set; } = null!;

    [StringLength(500)]
    public string FilePath { get; set; } = null!;

    [StringLength(100)]
    public string UploadedBy { get; set; } = null!;

    [Column(TypeName = "datetime")]
    public DateTime? DateUploaded { get; set; }
}
